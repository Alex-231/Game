using UnityEngine;
using System.Collections;

public class FollowCameraController : CameraController
{

    //Need to turn the player around on A and D down, and override motion controller.

    //override position and rotation in construct.
    FollowCameraController()
    {
        camStartingPos = new Vector3(0, 0, -5f);
        pointStartingPos = new Vector3(0, 2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        RotatePlayer();
        UpdateCameraDistance();
    }

    void RotateCamera()
    {
        float _xRot = -Input.GetAxisRaw("Mouse Y");

        if (modeController.thirdPersonCamSettings.inverted)
        {
            _xRot = -_xRot;
        }

        Vector3 _camPointRotation = new Vector3(_xRot, 0, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

        _camPointRotation = ApplyXBufferToRotation(cam.transform.rotation.eulerAngles, _camPointRotation);

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
    }
}
