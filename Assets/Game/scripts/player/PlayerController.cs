using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float lookSensitivity = 3f;

    [SerializeField]
    GameObject firstPersonCamPoint;

    [SerializeField]
    GameObject thirdPersonCamPoint;

    private PlayerMotor motor;

    // Use this for initialization
    void Start () {
        motor = GetComponent<PlayerMotor>();

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

        UpdateMovement();

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchCameraMode();
        }

    }

    void UpdateMovement()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        Vector3 _movHorizontal = transform.right * _xMov;
        Vector3 _movVertical = transform.forward * _zMov;

        // Final movement vector
        Vector3 _velocity = (_movHorizontal + _movVertical).normalized * speed;

        //Apply movement
        motor.Move(_velocity);
    }
}
