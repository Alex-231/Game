using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class Player : MonoBehaviour {

    [SerializeField]
    GameObject firstPersonCamPoint;

    [SerializeField]
    GameObject thirdPersonCamPoint;

	// Use this for initialization
	void Start () {
        SwitchToThirdPerson();
	}

    void SwitchCameraMode()
    {
        if (GetComponent<ThirdPersonController>() != null)
        {
            SwitchToFirstPerson();
        }
        else
        {
            SwitchToThirdPerson();
        }
    }

    void SwitchToThirdPerson ()
    {
        //Remove the First Person Controller if it's there.
        Destroy(GetComponent<FirstPersonController>());
        //Deactivate the first person camera.
        firstPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        //Add the Third Person Controller, setup variables.
        this.gameObject.AddComponent<ThirdPersonController>();
        ThirdPersonController _thirdPersonController = GetComponent<ThirdPersonController>();
        _thirdPersonController.cameraPoint = thirdPersonCamPoint;
    }

    void SwitchToFirstPerson()
    {
        //Remove the Third Person Controller if it's there.
        Destroy(GetComponent<ThirdPersonController>());
        //Deactivate the third person camera.
        thirdPersonCamPoint.gameObject.transform.GetChild(0).gameObject.SetActive(false);

        //Add the First Person Controller, setup variables.
        this.gameObject.AddComponent<FirstPersonController>();
        FirstPersonController _firstPersonController = GetComponent<FirstPersonController>();
        _firstPersonController.cameraPoint = firstPersonCamPoint;
    }
	
	// Update is called once per frame
	void Update () {
	
        if(Input.GetKeyDown(KeyCode.F))
        {
            SwitchCameraMode();
        }

	}
}
