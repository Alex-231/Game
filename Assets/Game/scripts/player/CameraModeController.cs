using UnityEngine;
using System.Collections;

public class CameraModeController : MonoBehaviour
{
    public GameObject camPoint;

    //how close the camera can be to directly overhead or underfoot.
    public float xAxisBuffer = 10f;

    public FirstPersonCameraSettings firstPersonCamSettings;
    public ThirdPersonCameraSettings thirdPersonCamSettings;

    public enum CameraModes
    {
        FirstPerson,
        ThirdPerson,
        TopDown,
        FlyCam,
        Static,
        Follow,
        DogsLife
    }

    public CameraModes seectedCameraMode = CameraModes.ThirdPerson;
    private CameraModes activeCamera;

    [System.Serializable]
    public class FirstPersonCameraSettings
    {
        public float lookSensitivity = 3f;
        public bool inverted = false;
    }

    [System.Serializable]
    public class ThirdPersonCameraSettings
    {
        public LayerMask transparent;
        public float lookSensetivity = 3f;
        public float minDistance = 5f;
        public float maxDistance = 15f;
        public float distanceMoveSpeed = 3f;
        public bool inverted = false;
    }

    Vector3 cameraStartingPoint;

    // Use this for initialization
    void Start()
    {
        activeCamera = seectedCameraMode;

        SwitchCameraMode();
    }

    //The method the player uses to change mode.
    void ChangeCameraMode()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if(seectedCameraMode == CameraModes.FirstPerson)
                seectedCameraMode = CameraModes.ThirdPerson;
            else
                seectedCameraMode = CameraModes.FirstPerson;
        }
    }

    /// <summary>
    /// Activates the camera specified in selectedCameraMode.
    /// </summary>
    void SwitchCameraMode()
    {
        RemoveCameraController();

        activeCamera = seectedCameraMode;

        //Might be a better way to do this, but it beats the old one.
        switch(seectedCameraMode)
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
            case CameraModes.DogsLife:
                gameObject.AddComponent<DogsLifeCameraController>();
                break;
        }
    }

    void RemoveCameraController()
    {
        //Remove script of type CameraController
        Destroy(GetComponent<CameraController>());
    }

    void FixedUpdate()
    {
        ChangeCameraMode();

        if (activeCamera != seectedCameraMode)
        {
            SwitchCameraMode();
        }
    }
}
