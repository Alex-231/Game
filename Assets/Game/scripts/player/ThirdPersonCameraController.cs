using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonCameraController : MonoBehaviour
{
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
    void Update()
    {
        RotateCamera();
        CorrectCameraRotation();
        UpdateCameraDistance();
    }

    void UpdateCameraDistance()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float _proposedNewLocation = cam.transform.localPosition.z + Input.GetAxisRaw("Mouse ScrollWheel") * camController.thirdPersonCamSettings.distanceMoveSpeed;

            //Camera distances are negative because the camera is behind the player dingus.

            if (_proposedNewLocation < -camController.thirdPersonCamSettings.maxDistance)
            {
                ChangeCameraOffset(-camController.thirdPersonCamSettings.maxDistance);
            }
            else if (_proposedNewLocation > -camController.thirdPersonCamSettings.minDistance)
            {
                ChangeCameraOffset(-camController.thirdPersonCamSettings.minDistance);
            }
            else
            {
                ChangeCameraOffset(_proposedNewLocation);
            }
        }
    }

    void RotateCamera()
    {
        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _xRot = Input.GetAxisRaw("Mouse Y");

        if(camController.thirdPersonCamSettings.inverted)
        {
            //probably better syntax for this.
            _xRot = -_xRot;
        }

        Vector3 _camPointRotation = new Vector3(_xRot, _yRot, 0f) * camController.thirdPersonCamSettings.lookSensetivity;

        //Apply rotation
        camPoint.transform.Rotate(_camPointRotation);
    }

    void CorrectCameraRotation()
    {
        //Prevents rotation on the Z axis.
        Quaternion _transformRot = camPoint.transform.rotation;
        _transformRot.eulerAngles = new Vector3(_transformRot.eulerAngles.x, _transformRot.eulerAngles.y, 0);

        camPoint.transform.rotation = _transformRot;

        //applies X axis buffer.
        Vector3 _bufferedRot = camPoint.transform.eulerAngles;
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

        camPoint.transform.eulerAngles = _bufferedRot;
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }

    void ResetRotation()
    {
        camPoint.transform.localEulerAngles = Vector3.zero;
    }
}
