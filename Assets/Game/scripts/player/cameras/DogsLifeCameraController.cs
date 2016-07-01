using UnityEngine;
using System.Collections;

public class DogsLifeCameraController : CameraController
{

    //override position and rotation in construct.
    DogsLifeCameraController()
    {
        camStartingPos = new Vector3(0, 2, -5f);
        camStartingRot = new Vector3(30f, 0, 0);
        pointStartingPos = new Vector3(0, 1f, 0);
        overrideWalking = true;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        LockCamZRotation();
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

        Vector3 _camPointRotation = new Vector3(0, _yRot, 0f) * modeController.thirdPersonCamSettings.lookSensetivity;
        Vector3 _camRotation = new Vector3(_xRot, 0, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

        _camRotation = ApplyXBufferToRotation(cam.transform.rotation.eulerAngles, _camRotation);

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
        cam.transform.Rotate(_camRotation);
    }

    void RotatePlayer(float _yRot)
    {
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }
}
