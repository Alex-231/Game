using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class testLoader : MonoBehaviour {

	// Use this for initialization
	void OnGUI()
    {

        Rect containerBoxRect = new Rect((Screen.width / 2) - 75, (Screen.height / 2) - 100, 150, 200);
        Rect button1Rect = new Rect((Screen.width / 2) - 70, (Screen.height / 2) - 70, 140, 20);

        GUI.Box(containerBoxRect, "Load a Scenario");
        if(GUI.Button(button1Rect, "PlayerControllerTest"))
            SceneManager.LoadScene("PlayerControllerTest");
    }
}
