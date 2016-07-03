using UnityEngine;
using System.Collections;

public class ShoulderCameraController : CameraController {

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

        Vector3 _rotation = new Vector3(_xRot, 0f, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        _rotation = ApplyXBufferToRotation(cam.transform.eulerAngles, _rotation);

        //Apply rotation
        camPoint.transform.Rotate(_rotation);
    }
}
