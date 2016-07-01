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
        ApplyRotationBufferX(camPoint.transform, false);
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

        Vector3 _camPointRotation = new Vector3(_xRot, _yRot, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

        if(camPoint.transform.eulerAngles.x + _camPointRotation.x > 90 - modeController.thirdPersonCamSettings.xAxisBuffer && camPoint.transform.eulerAngles.x < 270)
        {
            _camPointRotation.x = (90 - modeController.thirdPersonCamSettings.xAxisBuffer) - camPoint.transform.eulerAngles.x;
        }
        else if (camPoint.transform.eulerAngles.x + _camPointRotation.x < 270 + modeController.thirdPersonCamSettings.xAxisBuffer && camPoint.transform.eulerAngles.x > 90)
        {
            _camPointRotation.x = (270 + modeController.thirdPersonCamSettings.xAxisBuffer) - camPoint.transform.eulerAngles.x;
        }

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);

    }

    void RotatePlayer(float _yRot)
    {
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }
}
