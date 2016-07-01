using UnityEngine;

[RequireComponent(typeof(CharacterController))]
abstract public class CameraController : MonoBehaviour
{
    //If the walking boolean needs to be updated, set this to true.
    public bool overrideWalking = false;
    public bool walking = false;

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

    public void LockCamPointZRotation()
    {
        Vector3 _camPointCurrentRot = camPoint.transform.eulerAngles;
        camPoint.transform.eulerAngles = new Vector3(_camPointCurrentRot.x, _camPointCurrentRot.y, 0);
    }

    public void LockCamZRotation()
    {
        Vector3 _camCurrentRot = cam.transform.eulerAngles;
        cam.transform.eulerAngles = new Vector3(_camCurrentRot.x, _camCurrentRot.y, 0);
    }

    public void LockCamPointYRotation()
    {
        Vector3 _camPointCurrentRot = camPoint.transform.eulerAngles;
        camPoint.transform.localEulerAngles = new Vector3(_camPointCurrentRot.x, 0, _camPointCurrentRot.z);
    }
    /// <summary>
    /// Applies the configured camera buffered value to the current rotation.
    /// </summary>
    /// <param name="_transform">The transform to apply the buffer to.</param>
    /// <param name="local">Apply to localEulerAngles?</param>
    public void ApplyRotationBufferX(Transform _transform, bool local)
    {
        Vector3 _transformRot;

        if(local)
        {
            _transformRot = _transform.localEulerAngles;
        }
        else
        {
            _transformRot = _transform.rotation.eulerAngles;
        }
        //if x > 90 && x < 270, if the player is looking down.
        if (_transformRot.x > 90 - modeController.thirdPersonCamSettings.xAxisBuffer && _transformRot.x < 270)
        {
            _transformRot.x = 90 - modeController.thirdPersonCamSettings.xAxisBuffer;
        }
        //if x < 270 && x > 90, if the player is looking up.
        else if (_transformRot.x < 270 + modeController.thirdPersonCamSettings.xAxisBuffer && _transformRot.x > 90)
        {
            _transformRot.x = 270 + modeController.thirdPersonCamSettings.xAxisBuffer;
        }

        if (local)
        {
            _transform.localEulerAngles = _transformRot;
        }
        else
        {
            _transform.eulerAngles = _transformRot;
        }
    }

    public void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }


    public void CenterRotation()
    {
        camPoint.transform.localEulerAngles = new Vector3(camPoint.transform.localEulerAngles.x, 0, 0);
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }

    public void UpdateCameraDistance()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float _proposedNewLocation = cam.transform.localPosition.z + Input.GetAxisRaw("Mouse ScrollWheel") * modeController.thirdPersonCamSettings.distanceMoveSpeed;

            //Camera distances are negative because the camera is behind the player dingus.

            if (_proposedNewLocation < -modeController.thirdPersonCamSettings.maxDistance)
            {
                ChangeCameraOffset(-modeController.thirdPersonCamSettings.maxDistance);
            }
            else if (_proposedNewLocation > -modeController.thirdPersonCamSettings.minDistance)
            {
                ChangeCameraOffset(-modeController.thirdPersonCamSettings.minDistance);
            }
            else
            {
                ChangeCameraOffset(_proposedNewLocation);
            }
        }
    }
}