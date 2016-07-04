using UnityEngine;
using System.Collections;

/// <summary>
/// A camera controller loosly based on the game Dog's Life for the PS2.
/// </summary>
public class DogsLifeCameraController : ThirdPersonCameraController
{

    //override position and rotation in construct.
    DogsLifeCameraController()
    {
        camStartingPos = new Vector3(0, 3, -5f);
        camStartingRot = new Vector3(30f, 0, 0);
        pointStartingPos = new Vector3(0, 2f, 0);
    }
    void Update()
    {
        UpdateCameraDistance();
        RotateCamera();
        LockCamZRotation();
        KeepCameraInsideWalls();
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
            _xRot = -_xRot;
        }

        Vector3 _camPointRotate = new Vector3(0, _yRot, 0f) * modeController.thirdPersonCamSettings.lookSensetivity;
        Vector3 _camRotate = new Vector3(_xRot, 0, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

        _camPointRotate = ApplyCameraPaddingToRotation(camPoint.transform.eulerAngles, _camPointRotate);
        KeepCameraInsideWalls(camPoint.transform.eulerAngles, _camPointRotate);

        _camRotate = ApplyXBufferToRotation(cam.transform.rotation.eulerAngles, _camRotate);
        KeepCameraInsideWalls(cam.transform.eulerAngles, _camRotate);

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotate);
        cam.transform.Rotate(_camRotate);
    }
}
