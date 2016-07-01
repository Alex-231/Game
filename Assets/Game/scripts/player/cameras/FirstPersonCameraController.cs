﻿using UnityEngine;
using System.Collections;

public class FirstPersonCameraController : CameraController
{
    FirstPersonCameraController()
    {
        pointStartingPos = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update () {
        RotatePlayer();
        RotateCamera();
        CorrectCameraRotation(cam.transform.rotation, true);
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
