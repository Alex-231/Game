using UnityEngine;

[RequireComponent(typeof(AnimationController))]
[RequireComponent(typeof(CameraModeController))]
[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(PlayerOptions))]
public class Player : MonoBehaviour {

    public bool lockCursor = true;

	// Use this for initialization
	void Start () {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
	
	// Update is called once per frame
	void Update () {

	}
}
