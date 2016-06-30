using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCameraController : MonoBehaviour {

    [SerializeField]
    public GameObject camPoint;
    private GameObject cam;

    CameraController camController;
    CharacterController characterController;

    // Use this for initialization
    void Start()
    {

        camController = GetComponent<CameraController>();
        characterController = GetComponent<CharacterController>();

        //Wait for campoint available.
        while (camPoint == null)
        {

        }

        //Assign the camera.
        cam = camPoint.gameObject.transform.GetChild(0).gameObject;

        //Activate the camera.
        cam.SetActive(true);

        ResetRotation();

    }

    // Update is called once per frame
    void Update () {
        RotatePlayer();
        RotateCamera();
        CorrectCameraRotation();
    }

    void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * camController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }
    
    void RotateCamera()
    {
        //Looking up and down, needs to be inverted for some reason...
        float _xRot = -Input.GetAxisRaw("Mouse Y");

        //If the camera is set to inverted mode, invert the rotation.
        if(camController.firstPersonCamSettings.inverted)
        {
            _xRot = -_xRot;
        }

        Vector3 _rotation = new Vector3(_xRot, 0f, 0f) * camController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        cam.transform.Rotate(_rotation);
    }

    void CorrectCameraRotation()
    {
        //Prevents rotation on the Z axis.
        Quaternion _transformRot = cam.transform.rotation;
        _transformRot.eulerAngles = new Vector3(_transformRot.eulerAngles.x, _transformRot.eulerAngles.y, 0);

        cam.transform.rotation = _transformRot;

        //applies X axis buffer.
        Vector3 _bufferedRot = cam.transform.localEulerAngles;
        //if x > 90 && x < 270, if the player is looking down.
        if (_bufferedRot.x > 90 - camController.thirdPersonCamSettings.xAxisBuffer && _bufferedRot.x < 270)
        {
            _bufferedRot.x = 90 - camController.thirdPersonCamSettings.xAxisBuffer;
        }
        //if x < 270 && x > 90, if the player is looking up.
        else if (_bufferedRot.x < 270 + camController.thirdPersonCamSettings.xAxisBuffer && _bufferedRot.x > 90)
        {
            _bufferedRot.x = 270 + camController.thirdPersonCamSettings.xAxisBuffer;
        }
        cam.transform.localEulerAngles = _bufferedRot;
    }

    void ResetRotation()
    {
        cam.transform.localEulerAngles = Vector3.zero;
    }
}
