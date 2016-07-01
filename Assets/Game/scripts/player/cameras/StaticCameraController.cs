using UnityEngine;
using System.Collections;

public class StaticCameraController : CameraController
{
    Vector3 cameraPosition;
    Quaternion cameraRotation;

    // Use this for initialization
    void Start()
    {
        //Runs inherited method
        base.Start();

        //Declares the initial position.
        cameraPosition = cam.transform.position;
        cameraRotation = cam.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        UpdateCameraPosAndRot();
    }

    void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }

    void UpdateCameraPosAndRot()
    {
        cam.transform.position = cameraPosition;
        cam.transform.rotation = cameraRotation;
    }
}
