using UnityEngine;

[RequireComponent(typeof(CameraModeController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(PlayerOptions))]
public class Player : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void Update () {

	}
}
