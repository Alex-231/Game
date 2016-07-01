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
        CorrectCameraRotation(camPoint.transform.rotation, false);
        UpdateCameraDistance();
    }

    void UpdateCameraDistance()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float _proposedNewLocation = cam.transform.localPosition.z + Input.GetAxisRaw("Mouse ScrollWheel") * modeController.thirdPersonCamSettings.distanceMoveSpeed;

            //Camera distances are negative because the camera is behind the player dingus.

            if (_proposedNewLocation < -modeController.thirdPersonCamSettings.maxDistance)
            {
                ChangeCameraOffset(-modeController.thirdPersonCamSettings.maxDistance);
            }
            else if (_proposedNewLocation > -modeController.thirdPersonCamSettings.minDistance)
            {
                ChangeCameraOffset(-modeController.thirdPersonCamSettings.minDistance);
            }
            else
            {
                ChangeCameraOffset(_proposedNewLocation);
            }
        }
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

        Vector3 _camPointRotation = new Vector3(_xRot, _yRot, 0f) * modeController.thirdPersonCamSettings.lookSensetivity;

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
    }

    void RotatePlayer(float _yRot)
    {
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }
}
