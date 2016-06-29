using UnityEngine;

[RequireComponent(typeof(PlayerMotor))]
public class PlayerController : MonoBehaviour {

    public movementAndRotationSettings movSettings;
    public jumpingAndFallingSettings jumpSettings;
    public cameraPointsForCameraControllers camPoints;

    [System.Serializable]
    public class movementAndRotationSettings
    {
        public float speed = 5f;
        public float lookSensitivity = 3f;
    }

    [System.Serializable]
    public class jumpingAndFallingSettings
    {
        public LayerMask ground;
        public float jumpVelocity = 20f;
        public float distToGrounded = 1.1f;
        public float downAcceleration = 0.75f;

        public float jumpCount = 3;
    }

    [System.Serializable]
    public class cameraPointsForCameraControllers
    {
        public GameObject firstPersonCamPoint;
        public GameObject thirdPersonCamPoint;
    }

    private PlayerMotor motor;
    Vector3 velocity = Vector3.zero;

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
    void Update () {

        if (Input.GetKeyDown(KeyCode.F))
        {
            SwitchCameraMode();
        }

    }

    void FixedUpdate()
    {
        UpdateMovement();

        Jump();

        //Apply movement
        motor.Move(velocity);

    }


    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, jumpSettings.distToGrounded, jumpSettings.ground);
    }

    void UpdateMovement()
    {
        //Calculate movement velocity as a 3D vector
        float _xMov = Input.GetAxisRaw("Horizontal");
        float _zMov = Input.GetAxisRaw("Vertical");

        velocity.x = _xMov * movSettings.speed;
        velocity.z = _zMov * movSettings.speed;
    }

    void Jump()
    {
        float _yMov = Input.GetAxisRaw("Jump");

        if (_yMov > 0 && IsGrounded())
        {
            //Jumping
            velocity.y = jumpSettings.jumpVelocity;
        }
        else if (_yMov == 0 && IsGrounded())
        {
            //Landed
            velocity.y = 0;
        }
        else
        {
            //Falling
            velocity.y -= jumpSettings.downAcceleration;
        }
    }
}
