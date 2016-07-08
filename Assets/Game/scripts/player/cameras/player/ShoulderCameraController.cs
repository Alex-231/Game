using UnityEngine;
using System.Collections;

public class ShoulderCameraController : ThirdPersonCameraController {

    ShoulderCameraController()
    {
        pointStartingPos = new Vector3(0, 2f, 0);
        camStartingPos = new Vector3(0.3f, -0.2f, -1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        RotateCamera();
        LockCamPointZRotation();
        LockCamPointYRotation();
        KeepCameraInPosition();
        //KeepCameraInsideWalls();
    }

    void RotateCamera()
    {
        //Looking up and down, needs to be inverted for some reason...
        float _xRot = -Input.GetAxisRaw("Mouse Y");

        //If the camera is set to inverted mode, invert the rotation.
        if (modeController.firstPersonCamSettings.inverted)
        {
            _xRot = -_xRot;
        }

        Vector3 _camPointRotate = new Vector3(_xRot, 0f, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        _camPointRotate = ApplyXBufferToRotation(camPoint.transform.eulerAngles, _camPointRotate);
        KeepCameraRotationWithinWalls(camPoint.transform.eulerAngles, _camPointRotate);

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotate);
    }

    void KeepCameraInPosition()
    {
        cam.transform.localPosition = camStartingPos;
    }
}
