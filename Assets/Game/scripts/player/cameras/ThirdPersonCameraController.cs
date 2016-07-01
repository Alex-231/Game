using UnityEngine;
using System.Collections;

public class ThirdPersonCameraController : CameraController
{

    //override position and rotation in construct.
    ThirdPersonCameraController()
    {
        camStartingPos = new Vector3(0, 0, -5f);
        pointStartingPos = new Vector3(0, 1f, 0);
        overrideWalking = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        LockCamPointZRotation();
        UpdateCameraDistance();
    }

    void RotateCamera()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        float _xRot = Input.GetAxisRaw("Mouse Y");

        if (walking)
        {
            RotatePlayer(_yRot);
            _yRot = 0;
        }

        if (modeController.thirdPersonCamSettings.inverted)
        {
            //probably better syntax for this.
            _xRot = -_xRot;
        }

        Vector3 _camPointRotate = new Vector3(_xRot, _yRot, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

        _camPointRotate = ApplyXBufferToRotation(camPoint.transform.eulerAngles, _camPointRotate);

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotate);

    }

    void RotatePlayer(float _yRot)
    {
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }
}
