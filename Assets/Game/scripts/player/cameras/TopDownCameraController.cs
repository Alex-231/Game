using UnityEngine;
using System.Collections;

public class TopDownCameraController : CameraController
{

    TopDownCameraController()
    {
        base.camStartingPos = new Vector3(0, 0, -5f);
        base.pointStartingRot = new Vector3(90f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        UpdateCameraRotation();
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

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }

    void UpdateCameraRotation()
    {
        camPoint.transform.eulerAngles = base.pointStartingRot;
    }
}
