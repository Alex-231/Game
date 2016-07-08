using UnityEngine;
using System.Collections;

public class ShoulderCameraController : ThirdPersonCameraController {

    ShoulderCameraController()
    {
        pointStartingPos = new Vector3(0, 2f, 0);
        camStartingPos = new Vector3(0.3f, -0.2f, -1.5f);
    }

    void Start()
    {
        base.Start();
        chosenCamDistance = camStartingPos.z;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        LockCamPointZRotation();
    }
}
