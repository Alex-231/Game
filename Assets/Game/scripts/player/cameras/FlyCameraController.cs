using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FlyCameraController : CameraController
{
    MovementController movController;
    void Start()
    {
        base.Start();

        movController = GetComponent<MovementController>();
        movController.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        MoveCamera();
        CorrectCameraRotation();
    }

    void RotateCamera()
    {
        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _xRot = -Input.GetAxisRaw("Mouse Y");

        if (modeController.thirdPersonCamSettings.inverted)
        {
            //probably better syntax for this.
            _xRot = -_xRot;
        }

        Vector3 _camPointRotation = new Vector3(_xRot, _yRot, 0f) * modeController.thirdPersonCamSettings.lookSensetivity;

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
    }

    void MoveCamera()
    {
        float _movX = Input.GetAxis("Horizontal");
        float _movZ = Input.GetAxis("Vertical");

        camPoint.transform.Translate(new Vector3(_movX, 0, _movZ));
    }

    void OnDestroy()
    {
        movController.enabled = true;
    }
}
