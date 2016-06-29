using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    GameObject firstPersonCamPoint;

    [SerializeField]
    GameObject thirdPersonCamPoint;

    // Use this for initialization
    void Start () {
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
        firstPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        //Add the Third Person Controller, setup variables.
        this.gameObject.AddComponent<ThirdPersonCameraController>();
        ThirdPersonCameraController _thirdPersonCameraController = GetComponent<ThirdPersonCameraController>();
        _thirdPersonCameraController.camPoint = thirdPersonCamPoint;
    }

    void SwitchToFirstPerson()
    {
        //Remove the Third Person Controller if it's there.
        Destroy(GetComponent<ThirdPersonCameraController>());
        //Deactivate the third person camera.
        thirdPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        //Add the First Person Controller, setup variables.
        this.gameObject.AddComponent<FirstPersonCameraController>();
        FirstPersonCameraController _firstPersonCameraController = GetComponent<FirstPersonCameraController>();
        _firstPersonCameraController.camPoint = firstPersonCamPoint;
    }

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchCameraMode();
        }

    }
}
