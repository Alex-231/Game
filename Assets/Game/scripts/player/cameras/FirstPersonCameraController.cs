using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCameraController : CameraController
{
    FirstPersonCameraController()
    {
        base.pointStartingPos = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update () {
        RotatePlayer();
        RotateCamera();
        CorrectCameraRotation(cam.transform.rotation, true);
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
}
