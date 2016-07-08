using UnityEngine;
using System.Collections;

public class CameraControllerDebug : MonoBehaviour {

	// Use this for initialization
	void OnGUI()
    {
        string selectedCameraMode = GameObject.Find("CameraPoint").GetComponent<CameraModeController>().selectedCameraMode.ToString();

        Rect labelRect = new Rect(0, 0, Screen.width, Screen.height);
        GUI.Label(labelRect, string.Format(
            "Project Raider\n" +
            "branch: alex_test\n" +
            "test: Camera Controllers\n" +
            "2bUnity 2016\n" +
            "DO NOT REDISTRIBUTE\n\n" +
            "Selected Camera Mode: {0}\n" +
            "Press F to switch." 
            , selectedCameraMode));
    }
}
