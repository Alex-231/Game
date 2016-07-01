using UnityEngine;
using System.Collections;

public class TopDownCameraController : CameraController
{

    TopDownCameraController()
    {
        camStartingPos = new Vector3(0, 0, -5f);
        pointStartingRot = new Vector3(90f, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayer();
        UpdateCameraRotation();
        UpdateCameraDistance();
    }

    void UpdateCameraRotation()
    {
        camPoint.transform.eulerAngles = base.pointStartingRot;
    }
}
