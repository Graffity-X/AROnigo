using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace SPU {
	public class SPUManager : MonoBehaviour {
		[SerializeField] private Canvas defoultCanvas;

		[SerializeField] private List<RelitionBox> relitionBoxs;

		private Canvas currentCanvas;
		private Canvas[] allCanvas;
		
		
		private void Awake () {
			var all_=new List<Canvas>();
			foreach (var item in relitionBoxs) {
				if (item.here == null) {
					try {
						item.here = GetRootCanvas(item.trigger.gameObject);
					}
					catch (Exception e) {
						continue;
					}
				}

				item.trigger.LaunchStream
					.Where(n=>item.here==currentCanvas)
					.Subscribe(n => TransCanvas(item.destination, currentCanvas))
					.AddTo(this);
				
				all_.Add(item.here);
				all_.Add(item.destination);
			}

			allCanvas = all_.Distinct().ToArray();

			foreach (var item in allCanvas) {
				item.gameObject.SetActive(false);
			}
			defoultCanvas.gameObject.SetActive(true);
			currentCanvas = defoultCanvas;
		}

		private Canvas GetRootCanvas(GameObject go) {
			Canvas res = null;
			var candidate = go.transform;
			
			while ((res=candidate.GetComponent<Canvas>())==null) {
				candidate = candidate.parent;
				if (candidate == null) {
					throw new Exception("Missing "+go.name+"'s root canvas.");
				}
			}

			return res;
		}

		private void TransCanvas(Canvas destination,Canvas current) {
			foreach (var item in GetRecievers(current)) {
				item.OnCloseCanvas();
			}
			current.gameObject.SetActive(false);
			destination.gameObject.SetActive(true);
			foreach (var item in GetRecievers(destination)) {
				item.OnOpenCanvas();
			}
			currentCanvas = destination;
		}

		private IRecieveSPUNotification[] GetRecievers(Canvas canvas) {
			return canvas.transform.GetComponentsInChildren<IRecieveSPUNotification>();
		}
		
		[Serializable]
		private class RelitionBox {
			public SPUTransTrigger trigger;
			public Canvas destination;

			public Canvas here;
		}
	}	
}
