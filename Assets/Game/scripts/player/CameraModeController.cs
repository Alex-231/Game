using UnityEngine;
using System.Collections;

public class CameraModeController : MonoBehaviour
{
    public FirstPersonCameraSettings firstPersonCamSettings;
    public ThirdPersonCameraSettings thirdPersonCamSettings;

    public enum CameraModes
    {
        FirstPerson,
        ThirdPerson,
        TopDown,
        FlyCam,
        Static,
        Follow
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

    public GameObject camPoint;

    Vector3 cameraStartingPoint;

    // Use this for initialization
    void Start()
    {
        activeCamera = cameraMode;

        SwitchCameraMode();
    }

    //The method the player uses to change mode.
    void ChangeCameraMode()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(cameraMode == CameraModes.FirstPerson)
                cameraMode = CameraModes.ThirdPerson;
            else
                cameraMode = CameraModes.FirstPerson;
        }
    }

    void SwitchCameraMode()
    {
        DeactivateAllCameras();

        activeCamera = cameraMode;

        //Might be a better way to do this, but it beats the old one.
        switch(cameraMode)
        {
            case CameraModes.FirstPerson:
                gameObject.AddComponent<FirstPersonCameraController>();
                break;
            case CameraModes.ThirdPerson:
                gameObject.AddComponent<ThirdPersonCameraController>();
                break;
            case CameraModes.FlyCam:
                gameObject.AddComponent<FlyCameraController>();
                break;
            case CameraModes.TopDown:
                gameObject.AddComponent<TopDownCameraController>();
                break;
            case CameraModes.Static:
                gameObject.AddComponent<StaticCameraController>();
                break;
            case CameraModes.Follow:
                gameObject.AddComponent<FollowCameraController>();
                break;
        }
    }

    void DeactivateAllCameras()
    {
        //Remove script of type CameraControllerType
        Destroy(GetComponent<CameraController>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangeCameraMode();

        if (activeCamera != cameraMode)
        {
            SwitchCameraMode();
        }
    }
}
