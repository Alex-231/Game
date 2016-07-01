using UnityEngine;
using System.Collections;

public class StaticCameraController : CameraController
{
    Vector3 cameraPosition;

    // Use this for initialization
    void Start()
    {
        //Runs inherited method
        base.Start();

        //Declares the initial position.
        cameraPosition = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        UpdateCameraPosition();
    }

    void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }

    void UpdateCameraPosition()
    {
        cam.transform.position = cameraPosition;
    }
}
