using UnityEngine;
using System.Collections;

public class StaticCameraController : ThirdPersonCameraController
{
    Vector3 cameraPosition;
    Quaternion cameraRotation;

    StaticCameraController()
    {
        camStartingPos = new Vector3(0, 2f, 0);
    }

    void Start()
    {
        //Runs inherited method
        base.Start();

        //Declares the initial position.
        cameraPosition = cam.transform.position;
        cameraRotation = cam.transform.rotation;
    }

    void Update()
    {
        RotatePlayer();
        FreezeCameraPosAndRot();
    }

    void FreezeCameraPosAndRot()
    {
        cam.transform.position = cameraPosition;
        cam.transform.rotation = cameraRotation;
    }
}
