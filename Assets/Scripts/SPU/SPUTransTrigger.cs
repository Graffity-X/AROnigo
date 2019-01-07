using System;
using UniRx;
using UnityEngine;

namespace SPU {
    public class SPUTransTrigger : MonoBehaviour {
        private Subject<Unit> launchStream=new Subject<Unit>();
        public IObservable<Unit> LaunchStream {
            get { return launchStream; }
        }

        public void Launch() {
            launchStream.OnNext(Unit.Default);
        }
    }
}