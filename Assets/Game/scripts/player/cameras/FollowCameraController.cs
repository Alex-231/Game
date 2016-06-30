using UnityEngine;
using System.Collections;

public class FollowCameraController : CameraController
{

    //override position and rotation in construct.
    FollowCameraController()
    {
        base.camStartingPos = new Vector3(0, 0, -5f);
        base.pointStartingPos = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        RotatePlayer();
        CorrectCameraRotation(camPoint.transform.rotation, false);
        UpdateCameraDistance();
    }

    void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
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
        float _xRot = -Input.GetAxisRaw("Mouse Y");

        if (modeController.thirdPersonCamSettings.inverted)
        {
            //probably better syntax for this.
            _xRot = -_xRot;
        }

        Vector3 _camPointRotation = new Vector3(_xRot, 0, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }
}
