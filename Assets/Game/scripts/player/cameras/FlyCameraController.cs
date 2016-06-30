using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FlyCameraController : CameraController {

    // Update is called once per frame
    void Update () {
        RotatePlayer();
        RotateCamera();
        CorrectCameraRotation();
    }

    void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }
    
    void RotateCamera()
    {
        //Looking up and down, needs to be inverted for some reason...
        float _xRot = -Input.GetAxisRaw("Mouse Y");

        //If the camera is set to inverted mode, invert the rotation.
        if(modeController.firstPersonCamSettings.inverted)
        {
            _xRot = -_xRot;
        }

        Vector3 _rotation = new Vector3(_xRot, 0f, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        cam.transform.Rotate(_rotation);
    }

    void CorrectCameraRotation()
    {
        //Prevents rotation on the Z axis.
        Quaternion _transformRot = cam.transform.rotation;
        _transformRot.eulerAngles = new Vector3(_transformRot.eulerAngles.x, _transformRot.eulerAngles.y, 0);

        cam.transform.rotation = _transformRot;

        //applies X axis buffer.
        Vector3 _bufferedRot = cam.transform.localEulerAngles;
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
        cam.transform.localEulerAngles = _bufferedRot;
    }
}
