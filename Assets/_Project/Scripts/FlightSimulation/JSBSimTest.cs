using System.Runtime.InteropServices;
using UnityEngine;

public class JSBSimTest : MonoBehaviour
{
    [DllImport("JSBSimUnityWrapper")]
    private static extern void JSBSim_Init();

    void Start()
    {
        JSBSim_Init();
        Debug.Log("JSBSim initialized");
    }
}