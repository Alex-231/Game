using UnityEngine;

[RequireComponent(typeof(CharacterController))]
abstract public class CameraController : MonoBehaviour
{
    //These values represent local positions and rotations!
    [Header("Camera Point")]
    public Vector3 pointStartingPos = Vector3.zero;
    public Vector3 pointStartingRot = Vector3.zero;
    [Header("Camera")]
    public Vector3 camStartingPos = Vector3.zero;
    public Vector3 camStartingRot = Vector3.zero;

    [SerializeField]
    public GameObject camPoint;
    public GameObject cam;

    public CameraModeController modeController;
    public CharacterController characterController;

    public void Start()
    {
        modeController = GetComponent<CameraModeController>();
        characterController = GetComponent<CharacterController>();

        camPoint = modeController.camPoint;

        //Assign the camera.
        cam = camPoint.gameObject.transform.GetChild(0).gameObject;

        //Activate the camera.
        cam.SetActive(true);

        ResetPositionAndRotation();
    }

    public void ResetPositionAndRotation()
    {
        cam.transform.localPosition = camStartingPos;
        cam.transform.localEulerAngles = camStartingRot;
        camPoint.transform.localPosition = pointStartingPos;
        camPoint.transform.localEulerAngles = pointStartingRot;
    }

    public void CorrectCameraRotation(Quaternion _transformRot, bool local)
    {
        //Prevents rotation on the Z axis.
        _transformRot.eulerAngles = new Vector3(_transformRot.eulerAngles.x, _transformRot.eulerAngles.y, 0);

        camPoint.transform.rotation = _transformRot;

        //applies X axis buffer.
        Vector3 _bufferedRot = camPoint.transform.eulerAngles;
        //if x > 90 && x < 270, if the player is looking down.
        if (_bufferedRot.x > 90 - modeController.thirdPersonCamSettings.xAxisBuffer && _bufferedRot.x < 270)
        {
            _bufferedRot.x = 90 - modeController.thirdPersonCamSettings.xAxisBuffer;
        }
        //if x < 270 && x > 90, if the player is looking up.
        else if (_bufferedRot.x < 270 + modeController.thirdPersonCamSettings.xAxisBuffer && _bufferedRot.x > 90)
        {
            _bufferedRot.x = 270 + modeController.thirdPersonCamSettings.xAxisBuffer;
        }

        if(local)
        {
            camPoint.transform.localEulerAngles = _bufferedRot;
        }
        else
        {
            camPoint.transform.eulerAngles = _bufferedRot;
        }
    }
}