using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
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
        UpdateCameraPosition();
    }

    void UpdateCameraPosition()
    {
        cam.transform.position = cameraPosition;
    }
}
