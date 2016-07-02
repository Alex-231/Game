using UnityEngine;

[RequireComponent(typeof(CharacterController))]
abstract public class CameraController : MonoBehaviour
{
    /*
    TODO:
    Add Camera Padding.
    */



    /// <summary>
    /// If the camera needs to react to the player walking, set this to true.
    /// </summary>
    public bool overrideWalking = false;
    /// <summary>
    /// If the player is walking and overrideWalking is set to true, this will be true.
    /// </summary>
    public bool walking = false;

    //This variable is used for cameras using the UpdateCameraDistance method.
    public float chosenCamDistance;

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
        cam = camPoint.transform.GetChild(0).gameObject;

        //Activate the camera.
        cam.SetActive(true);

        ResetCamAndCamPointPosAndRot();

        chosenCamDistance = cam.transform.localPosition.z;
    }

    /// <summary>
    /// Sets the position and rotation of the camera and camPoint to the starting values.
    /// </summary>
    public void ResetCamAndCamPointPosAndRot()
    {
        cam.transform.localPosition = camStartingPos;
        cam.transform.localEulerAngles = camStartingRot;
        camPoint.transform.localPosition = pointStartingPos;
        camPoint.transform.localEulerAngles = pointStartingRot;
    }


    /// <summary>
    /// Sets the z rotation of camPoint to 0.
    /// </summary>
    public void LockCamPointZRotation()
    {
        Vector3 _camPointCurrentRot = camPoint.transform.eulerAngles;
        camPoint.transform.eulerAngles = new Vector3(_camPointCurrentRot.x, _camPointCurrentRot.y, 0);
    }

    /// <summary>
    /// Sets the z value of cam to 0.
    /// </summary>
    public void LockCamZRotation()
    {
        Vector3 _camCurrentRot = cam.transform.eulerAngles;
        cam.transform.eulerAngles = new Vector3(_camCurrentRot.x, _camCurrentRot.y, 0);
    }

    /// <summary>
    /// Sets the y value of camPoint to 0.
    /// </summary>
    public void LockCamPointYRotation()
    {
        Vector3 _camPointCurrentRot = camPoint.transform.eulerAngles;
        camPoint.transform.localEulerAngles = new Vector3(_camPointCurrentRot.x, 0, _camPointCurrentRot.z);
    }

    public void RotatePlayer()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }

    /// <summary>
    /// Centers the campoint on Axis Y.
    /// </summary>
    public void CenterCamPointAxisY()
    {
        camPoint.transform.localEulerAngles = new Vector3(camPoint.transform.localEulerAngles.x, 0, 0);
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }

    /// <summary>
    /// Moves the camera forwards or backwards, within the min and max boundries, when the player scrolls.
    /// </summary>
    public void UpdateCameraDistance()
    {
        if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
        {
            float _proposedNewLocation = cam.transform.localPosition.z + Input.GetAxisRaw("Mouse ScrollWheel") * modeController.thirdPersonCamSettings.distanceMoveSpeed;

            //Camera distances are negative because the camera is behind the player dingus.

            if (_proposedNewLocation < -modeController.thirdPersonCamSettings.maxDistance)
            {
                chosenCamDistance = -modeController.thirdPersonCamSettings.maxDistance;
            }
            else if (_proposedNewLocation > -modeController.thirdPersonCamSettings.minDistance)
            {
                chosenCamDistance = -modeController.thirdPersonCamSettings.minDistance;
            }
            else
            {
                chosenCamDistance = _proposedNewLocation;
            }

            ChangeCameraOffset(chosenCamDistance);
        }
    }

    /// <summary>
    /// Prevents the a camera rotation from flipping to the back of the player.
    /// </summary>
    /// <param name="_Rotation">The current rotation of the camera or point.</param>
    /// <param name="_Rotate">The proposed rotate of the camera or point.</param>
    /// <returns>Returns the corrected rotation.</returns>
    public Vector3 ApplyXBufferToRotation(Vector3 _currentRotation, Vector3 _rotate)
    {
        if (_currentRotation.x + _rotate.x > 90 - modeController.xAxisBuffer && _currentRotation.x < 270)
        {
            _rotate.x = (90 - modeController.xAxisBuffer) - _currentRotation.x;
        }
        else if (_currentRotation.x + _rotate.x < 270 + modeController.xAxisBuffer && _currentRotation.x > 90)
        {
            _rotate.x = (270 + modeController.xAxisBuffer) - _currentRotation.x;
        }
        return _rotate;
    }

    public void KeepCameraInsideWalls()
    {
        RaycastHit objectHitInfo = new RaycastHit();
        float cameraDistance = Vector3.Distance(transform.position, cam.transform.position);

        bool hitWall = Physics.Raycast(transform.position, (cam.transform.position - transform.position).normalized, out objectHitInfo, -chosenCamDistance, ~modeController.thirdPersonCamSettings.transparent);
        //Debug.DrawLine(transform.position, cam.transform.position, Color.red);
        //Debug.DrawRay(transform.position, (cam.transform.position - transform.position).normalized, Color.white);
        if (hitWall)
        {
            ChangeCameraOffset(cam.transform.localPosition.z - (objectHitInfo.distance - cameraDistance));
            //Debug.Log("The camera linecast hit a wall.");
        }
        else if (cam.transform.localPosition.z != chosenCamDistance)
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, chosenCamDistance);
        }
    }
    public Vector3 KeepCamerWithinPadding(Vector3 _currentRotation, Vector3 _rotation)
    {
        //Create a couple of empty gameobjects for calculations.
        GameObject _desiredCamPoint = new GameObject("_desiredCamPoint");
        GameObject _desiredCam = new GameObject("_desiredCam");

        //Change the parents.
        _desiredCamPoint.transform.parent = gameObject.transform;
        _desiredCam.transform.parent = _desiredCamPoint.transform;

        //Read the rotation and position from actual gameobjects.
        _desiredCamPoint.transform.position = camPoint.transform.position;
        _desiredCamPoint.transform.rotation = camPoint.transform.rotation;
        _desiredCam.transform.position = cam.transform.position;
        _desiredCam.transform.rotation = cam.transform.rotation;

        _desiredCamPoint.transform.Rotate(_rotation);

        debugPosition = _desiredCam.transform.position;

        Collider[] collisions = Physics.OverlapSphere(_desiredCam.transform.position, modeController.thirdPersonCamSettings.cameraPadding, ~modeController.thirdPersonCamSettings.transparent);

        foreach (Collider collision in collisions)
        {
            Vector3 collisionDirection = (collision.ClosestPointOnBounds(_desiredCam.transform.position) - _desiredCam.transform.position).normalized;
            float collisionDistance = Vector3.Distance(_desiredCam.transform.position, collision.ClosestPointOnBounds(_desiredCam.transform.position));
            if (collisionDistance == 0)
                collisionDistance = 1;
            Vector3 compensationRotation = -(collisionDirection * collisionDistance);

            compensationRotation = new Vector3(compensationRotation.y, compensationRotation.z, compensationRotation.x);

            Debug.DrawRay(_desiredCam.transform.position, collisionDirection, Color.red);

            _rotation += compensationRotation;
        }

        Destroy(_desiredCamPoint);

        return _rotation;
    }

    Vector3 debugPosition;

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(debugPosition, modeController.thirdPersonCamSettings.cameraPadding);
    }
}