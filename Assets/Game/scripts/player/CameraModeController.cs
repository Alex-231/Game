using UnityEngine;
using System.Collections;

public class CameraModeController : MonoBehaviour
{
    public CameraPointsForCameraControllers camPoints;
    public FirstPersonCameraSettings firstPersonCamSettings;
    public ThirdPersonCameraSettings thirdPersonCamSettings;

    public enum CameraModes
    {
        FirstPerson,
        ThirdPerson,
        TopDown,
        FlyCam,
        Static //Remains in place.
    }

    public CameraModes cameraMode = CameraModes.ThirdPerson;
    private CameraModes activeCamera;

    [System.Serializable]
    public class FirstPersonCameraSettings
    {
        public float lookSensitivity = 3f;
        public bool inverted = false;
        public float xAxisBuffer = 10f;
    }

    [System.Serializable]
    public class ThirdPersonCameraSettings
    {
        public float lookSensetivity = 3f;
        public float minDistance = 5f;
        public float maxDistance = 15f;
        //prevents the camera from rotating overhead or below feet.
        public float xAxisBuffer = 10f;
        public float distanceMoveSpeed = 3f;
        public bool inverted = false;
    }

    [System.Serializable]
    public class CameraPointsForCameraControllers
    {
        public GameObject firstPersonCamPoint;
        public GameObject thirdPersonCamPoint;
    }

    Vector3 cameraStartingPoint;

    // Use this for initialization
    void Start()
    {
        activeCamera = cameraMode;

        //This is important to make sure the scripts and cameras are setup.
        SwitchCameraMode();
    }

    void SwitchCameraMode()
    {
        DeactivateAllCameras();

        //Most used at the top.
        if (cameraMode == CameraModes.ThirdPerson)
        {
            activeCamera = CameraModes.ThirdPerson;
            SwitchToThirdPerson();
        }
        else if (cameraMode == CameraModes.FirstPerson)
        {
            activeCamera = CameraModes.FirstPerson;
            SwitchToFirstPerson();
        }
        else if (cameraMode == CameraModes.Static)
        {
            activeCamera = CameraModes.Static;
            SwitchToStatic();
        }
    }

    void DeactivateAllCameras()
    {
        //Remove script of type CameraControllerType
        Destroy(GetComponent<CameraController>());

        //Deactivate cam points.
        camPoints.firstPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);
        camPoints.thirdPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);
    }

    void SwitchToThirdPerson()
    {
        //Add the Third Person Controller, setup variables.
        this.gameObject.AddComponent<ThirdPersonCameraController>();
        ThirdPersonCameraController _thirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
        _thirdPersonCameraController.camPoint = camPoints.thirdPersonCamPoint;
    }

    void SwitchToFirstPerson()
    {
        //Add the First Person Controller, setup variables.
        this.gameObject.AddComponent<FirstPersonCameraController>();
        FirstPersonCameraController _firstPersonCameraController = GetComponent<FirstPersonCameraController>();
        _firstPersonCameraController.camPoint = camPoints.firstPersonCamPoint;
    }

    void SwitchToStatic()
    {
        //Add the First Person Controller, setup variables.
        this.gameObject.AddComponent<StaticCameraController>();
        StaticCameraController _firstPersonCameraController = GetComponent<StaticCameraController>();
        _firstPersonCameraController.camPoint = camPoints.firstPersonCamPoint;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (activeCamera != cameraMode)
        {
            SwitchCameraMode();
        }

    }
}
