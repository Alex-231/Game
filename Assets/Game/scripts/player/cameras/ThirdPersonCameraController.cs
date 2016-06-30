using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCameraController : CameraController
{

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        CorrectCameraRotation();
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
        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _xRot = Input.GetAxisRaw("Mouse Y");

        if(modeController.thirdPersonCamSettings.inverted)
        {
            //probably better syntax for this.
            _xRot = -_xRot;
        }

        Vector3 _camPointRotation = new Vector3(_xRot, _yRot, 0f) * modeController.thirdPersonCamSettings.lookSensetivity;

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
    }

    void CorrectCameraRotation()
    {
        //Prevents rotation on the Z axis.
        Quaternion _transformRot = camPoint.transform.rotation;
        _transformRot.eulerAngles = new Vector3(_transformRot.eulerAngles.x, _transformRot.eulerAngles.y, 0);

        camPoint.transform.rotation = _transformRot;

        //applies X axis buffer.
        Vector3 _bufferedRot = camPoint.transform.eulerAngles;
        //if x > 90 && x < 270, if the player is looking down.
        if (_bufferedRot.x > 90 - modeController.thirdPersonCamSettings.xAxisBuffer && _bufferedRot.x < 270)
        {
            _bufferedRot.x = 90 - modeController.thirdPersonCamSettings.xAxisBuffer;
        }
        //if x < 270 && x > 90, if the player is looking up.
        else if (_bufferedRot.x < 270 + modeController.thirdPersonCamSettings.xAxisBuffer && _bufferedRot.x > 90)
        {
            _bufferedRot.x = 270 + modeController.thirdPersonCamSettings.xAxisBuffer;
        }

        camPoint.transform.eulerAngles = _bufferedRot;
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }
}
