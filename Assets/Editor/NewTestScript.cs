using System;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Systems;

public class NewTestScript {

    [Test]
    public void NewTestScriptSimplePasses() {
        int a = 2;
        string b = "abcd";
        var temp = ByteTranslater.CipherIntToByte(new int[] {a, Convert.ToInt32(b)});
        var hoo = ByteTranslater.DecodeByteToInt(temp);
        Debug.Log(hoo[0]);
        Debug.Log(Convert.ToString(hoo[1]));
    }
    


    // A UnityTest behaves like a coroutine in PlayMode
    // and allows you to yield null to skip a frame in EditMode
    [UnityTest]
    public IEnumerator NewTestScriptWithEnumeratorPasses() {
        // Use the Assert class to test conditions.
        // yield to skip a frame
        yield return null;
    }
}
