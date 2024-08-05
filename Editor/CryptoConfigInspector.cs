using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using UnityEditor;
using Framework.Security;
[CustomEditor(typeof(CryptoConfig))]
public class CryptoConfigInspector : Editor
{
    CryptoConfig config;
    private void OnEnable()
    {
        config = (CryptoConfig)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // if (GUILayout.Button("Create"))
        // {
        //     CryptoRSA.CreateRSAKeyPair();
        // }
    }

}
