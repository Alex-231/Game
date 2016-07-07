using UnityEngine;
using System.Collections;

public class ThirdPersonCameraController : PlayerCameraController
{
    /*
    TODO:
    The KeepCameraWithinPadding method needs to change the offset to allow more rotation.
    Possible Solution: run method, if resulting rotation = Vector3.zero, decrease camera distance and try again.
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
        //KeepCameraInsideWalls();
    }

    //override position and rotation in construct.
    public ThirdPersonCameraController()
    {
        camStartingPos = new Vector3(0, 0, -5f);
        pointStartingPos = new Vector3(0, 2f, 0);
    }

    //Add chosenCamDistance assignment to the inherited start method.
    public void Start()
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
            _camPointRotate = ApplyCameraPaddingToRotation(camPoint.transform.eulerAngles, _camPointRotate);
            //KeepCameraInsideWalls(_camPointRotate);

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


    //Some camera controllers aren't very friendly, and the KeepCameraInsideWalls rotation method just isn't enough.
    //This method works though, so if a controller is clipping through a wall every frame, add this to the update. Should fix it.
    public void KeepCameraInsideWalls(Vector3 _castToPos)
    {
        RaycastHit objectHitInfo = new RaycastHit();
        float castDistance = Vector3.Distance(transform.position, _castToPos);

        bool hitWall = Physics.Raycast(transform.position, (_castToPos - transform.position).normalized, out objectHitInfo, castDistance, ~modeController.thirdPersonCamSettings.transparent);
        float newCamDistance = cam.transform.localPosition.z - (objectHitInfo.distance - castDistance);
        if (hitWall)
        {
            ChangeCameraOffset(cam.transform.localPosition.z - (objectHitInfo.distance - castDistance));
        }
        else
        {
            hitWall = Physics.Raycast(transform.position, transform.position - _castToPos, out objectHitInfo, ~modeController.thirdPersonCamSettings.transparent);
            newCamDistance = cam.transform.localPosition.z - (objectHitInfo.distance - castDistance);
            if (hitWall)
            {
                if (newCamDistance < chosenCamDistance)
                {
                    ChangeCameraOffset(chosenCamDistance);
                }
                else
                {
                    ChangeCameraOffset(newCamDistance);
                }
            }
        }
    }

    public Vector3 ApplyCameraPaddingToRotation(Vector3 _currentRotation, Vector3 _rotation)
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
        //float cameraDistance = Vector3.Distance(_desiredCamPoint.transform.position, _desiredCam.transform.position);

        //Overlap sphere onto desired camera position.
        Collider[] collisions = Physics.OverlapSphere(_desiredCam.transform.position, modeController.thirdPersonCamSettings.cameraPadding, ~modeController.thirdPersonCamSettings.transparent);
        if (collisions.Length > 0)
        {
            foreach (Collider collision in collisions)
            {
                Debug.DrawLine(collision.ClosestPointOnBounds(_desiredCam.transform.position), _desiredCam.transform.position, Color.red);
                KeepCameraInsideWalls(collision.ClosestPointOnBounds(_desiredCam.transform.position));
            }
        }
        else
        {
            KeepCameraInsideWalls(_desiredCam.transform.position);
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
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, newLocation);
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
