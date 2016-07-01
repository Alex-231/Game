using UnityEngine;

[RequireComponent(typeof(CameraModeController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(PlayerOptions))]
public class Player : MonoBehaviour {

    public bool lockCamera = true;

	// Use this for initialization
	void Start () {
        if (lockCamera)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
