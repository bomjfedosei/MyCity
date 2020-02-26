using UnityEditor;
using UnityEngine;

public class Editor : MonoBehaviour
{
    public static void PreExport()
    {
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeRight;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        
        //PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7
        Debug.Log("TEST");
        Debug.Log(PlayerSettings.Android.targetArchitectures);
    }
}
