using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonCameraController : MonoBehaviour {

    [SerializeField]
    public GameObject camPoint;
    private GameObject cam;

    CameraSwitch switcher;
    CharacterController characterController;

    // Use this for initialization
    void Start () {

        switcher = GetComponent<CameraSwitch>();
        characterController = GetComponent<CharacterController>();

        //Wait for campoint available.
        while (camPoint == null)
        {

        }

        //Assign the camera.
        cam = camPoint.gameObject.transform.GetChild(0).gameObject;

        //Activate the camera.
        cam.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
        //Calculate rotation as a 3D vector (turning around)
        float _yRot = Input.GetAxisRaw("Mouse X");

        Vector3 _rotation = new Vector3(0f, _yRot, 0f) * switcher.lookSensitivity;

        //Apply rotation
        characterController.transform.Rotate(_rotation);
    }
}
