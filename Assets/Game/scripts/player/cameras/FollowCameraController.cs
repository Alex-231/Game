﻿using UnityEngine;
using System.Collections;

public class FollowCameraController : CameraController
{

    //override position and rotation in construct.
    FollowCameraController()
    {
        camStartingPos = new Vector3(0, 0, -5f);
        pointStartingPos = new Vector3(0, 1f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        RotatePlayer();
        LockZRotation();
        ApplyRotationBufferX(camPoint.transform, false);
        UpdateCameraDistance();
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
}
