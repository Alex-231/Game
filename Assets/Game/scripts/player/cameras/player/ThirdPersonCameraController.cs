using UnityEngine;
using System.Collections;

public class ThirdPersonCameraController : PlayerCameraController
{
    /*
    CREATED A BUG:
    When the KeepCameraWithinWalls method is called, the camera position moves.
    Possible Solution: use base.startingpos in constructors.
                       update KeepCameraWithinWalls !hitWall code.
    */

    //If override walking is enabled, 
    public bool overrideWalking = true;

    //If the player is walking and overrideWalking is set to true, this will be true.
    public bool walking = false;

    //This variable is used for cameras using the UpdateCameraDistance method.
    public float chosenCamDistance;

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
        LockCamPointZRotation();
        UpdateCameraDistance();
    }

    //override position and rotation in construct.
    public ThirdPersonCameraController()
    {
        camStartingPos = new Vector3(0, 0, -5f);
        pointStartingPos = new Vector3(0, 2f, 0);
    }

    //Add chosenCamDistance assignment to the inherited start method.
    void Start()
    {
        base.Start();

        chosenCamDistance = cam.transform.localPosition.z;
    }

    public void RotateCamera()
    {
        float _yRot = Input.GetAxisRaw("Mouse X");
        float _xRot = Input.GetAxisRaw("Mouse Y");

        if (_xRot != 0 || _yRot != 0 || walking)
        {
            if (walking)
            {
                RotatePlayer(_yRot);
                _yRot = 0;
            }

            if (modeController.thirdPersonCamSettings.inverted)
            {
                _xRot = -_xRot;
            }

            Vector3 _camPointRotate = new Vector3(_xRot, _yRot, 0) * modeController.thirdPersonCamSettings.lookSensetivity;

            _camPointRotate = ApplyXBufferToRotation(camPoint.transform.eulerAngles, _camPointRotate);
            _camPointRotate = KeepCamerWithinPadding(camPoint.transform.eulerAngles, _camPointRotate);
            KeepCameraInsideWalls(camPoint.transform.eulerAngles, _camPointRotate);

            //Apply rotation
            camPoint.transform.Rotate(_camPointRotate);
        }
    }

    public void RotatePlayer(float _yRot)
    {
        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * modeController.firstPersonCamSettings.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }


    public void KeepCameraInsideWalls(Vector3 _currentRotation, Vector3 _rotation)
    {
        #region prepare desired game objects
        //Create a couple of empty gameobjects for calculations.
        GameObject _desiredCamPoint = new GameObject("_desiredCamPoint");
        GameObject _desiredCam = new GameObject("_desiredCam");

        //Change the parents.
        _desiredCamPoint.transform.parent = playerTransform;
        _desiredCam.transform.parent = _desiredCamPoint.transform;

        //Read the rotation and position from actual gameobjects.
        _desiredCamPoint.transform.position = camPoint.transform.position;
        _desiredCamPoint.transform.rotation = camPoint.transform.rotation;
        _desiredCam.transform.position = cam.transform.position;
        _desiredCam.transform.rotation = cam.transform.rotation;

        //Rotate the empty gameobject by the desired amount.
        _desiredCamPoint.transform.Rotate(_rotation);
        #endregion

        #region Raycast and correct camera distance.
        RaycastHit objectHitInfo = new RaycastHit();
        float cameraDistance = Vector3.Distance(_desiredCamPoint.transform.position, _desiredCam.transform.position);

        bool hitWall = Physics.Raycast(_desiredCamPoint.transform.position, (_desiredCam.transform.position - _desiredCamPoint.transform.position).normalized, out objectHitInfo, -chosenCamDistance, ~modeController.thirdPersonCamSettings.transparent);
        if (hitWall)
        {
            ChangeCameraOffset(_desiredCam.transform.localPosition.z - (objectHitInfo.distance - cameraDistance) + modeController.thirdPersonCamSettings.cameraPadding);
        }
        else if (cam.transform.localPosition.z != chosenCamDistance)
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, chosenCamDistance);
        }
        #endregion

        Destroy(_desiredCamPoint);
    }

    public Vector3 KeepCamerWithinPadding(Vector3 _currentRotation, Vector3 _rotation)
    {
        #region prepare desired game objects
        //Create a couple of empty gameobjects for calculations.
        GameObject _desiredCamPoint = new GameObject("_desiredCamPoint");
        GameObject _desiredCam = new GameObject("_desiredCam");

        //Change the parents.
        _desiredCamPoint.transform.parent = playerTransform;
        _desiredCam.transform.parent = _desiredCamPoint.transform;

        //Read the rotation and position from actual gameobjects.
        _desiredCamPoint.transform.position = camPoint.transform.position;
        _desiredCamPoint.transform.rotation = camPoint.transform.rotation;
        _desiredCam.transform.position = cam.transform.position;
        _desiredCam.transform.rotation = cam.transform.rotation;

        //Rotate the empty gameobject by the desired amount.
        _desiredCamPoint.transform.Rotate(_rotation);
        #endregion

        #region Overlap sphere and correct rotation.
        float cameraDistance = Vector3.Distance(_desiredCamPoint.transform.position, _desiredCam.transform.position);

        //Overlap sphere onto desired camera position.
        Collider[] collisions = Physics.OverlapSphere(_desiredCam.transform.position, modeController.thirdPersonCamSettings.cameraPadding, ~modeController.thirdPersonCamSettings.transparent);
        foreach (Collider collision in collisions)
        {
            Vector3 collisionDirectionFromCamera = collision.ClosestPointOnBounds(_desiredCam.transform.position) - _desiredCam.transform.position;
            float collisionDistanceFromCamera = Vector3.Distance(collision.ClosestPointOnBounds(_desiredCam.transform.position), _desiredCam.transform.position);
            float collisionDistanceFromPadding = modeController.thirdPersonCamSettings.cameraPadding - collisionDistanceFromCamera;

            //Draws lines at collision points.
            //Debug.DrawLine(_desiredCam.transform.position, collision.ClosestPointOnBounds(_desiredCam.transform.position), Color.red);

            _rotation = Vector3.Scale(_rotation, -collisionDirectionFromCamera);
        }
        #endregion

        Destroy(_desiredCamPoint);

        return _rotation;
    }

    //Centers the campoint on Axis Y.
    public void CenterCamPointAxisY()
    {
        camPoint.transform.localEulerAngles = new Vector3(camPoint.transform.localEulerAngles.x, 0, 0);
    }

    void ChangeCameraOffset(float newLocation)
    {
        cam.transform.localPosition = new Vector3(0, 0, newLocation);
    }

    //Moves the camera forwards or backwards, within the min and max boundries, when the player scrolls.
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
}
