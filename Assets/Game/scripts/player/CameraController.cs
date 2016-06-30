using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public CameraPointsForCameraControllers camPoints;
    public FirstPersonCameraSettings firstPersonCamSettings;
    public ThirdPersonCameraSettings thirdPersonCamSettings;

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

    // Use this for initialization
    void Start()
    {
        //This is important to make sure the scripts and cameras are setup.
        SwitchToThirdPerson();
    }

    void SwitchCameraMode()
    {
        if (GetComponent<ThirdPersonCameraController>() != null)
        {
            SwitchToFirstPerson();
        }
        else
        {
            SwitchToThirdPerson();
        }
    }

    void SwitchToThirdPerson()
    {
        //Remove the First Person Controller if it's there.
        Destroy(GetComponent<FirstPersonCameraController>());
        //Deactivate the first person camera.
        camPoints.firstPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        //Add the Third Person Controller, setup variables.
        this.gameObject.AddComponent<ThirdPersonCameraController>();
        ThirdPersonCameraController _thirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
        _thirdPersonCameraController.camPoint = camPoints.thirdPersonCamPoint;
    }

    void SwitchToFirstPerson()
    {
        //Remove the Third Person Controller if it's there.
        Destroy(GetComponent<ThirdPersonCameraController>());
        //Deactivate the third person camera.
        camPoints.thirdPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        //Add the First Person Controller, setup variables.
        this.gameObject.AddComponent<FirstPersonCameraController>();
        FirstPersonCameraController _firstPersonCameraController = GetComponent<FirstPersonCameraController>();
        _firstPersonCameraController.camPoint = camPoints.firstPersonCamPoint;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchCameraMode();
        }

    }
}
