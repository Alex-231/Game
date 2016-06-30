using UnityEngine;
using System.Collections;

[System.Serializable]
public class CameraSwitch : MonoBehaviour
{
    public cameraPointsForCameraControllers camPoints;

    [System.Serializable]
    public class cameraPointsForCameraControllers
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
