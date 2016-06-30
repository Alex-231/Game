using UnityEngine;

[RequireComponent(typeof(CharacterController))]
abstract public class CameraController : MonoBehaviour
{
    [SerializeField]
    public GameObject camPoint;
    public GameObject cam;

    public CameraModeController modeController;
    public CharacterController characterController;

    public void Start()
    {
        modeController = GetComponent<CameraModeController>();
        characterController = GetComponent<CharacterController>();

        //Wait for campoint available.
        while (camPoint == null)
        {

        }

        //Assign the camera.
        cam = camPoint.gameObject.transform.GetChild(0).gameObject;

        //Activate the camera.
        cam.SetActive(true);

        ResetCamera();
    }

    public void ResetCamera()
    {
        cam.transform.localEulerAngles = Vector3.zero;
        camPoint.transform.localEulerAngles = Vector3.zero;
        camPoint.transform.localPosition = Vector3.zero;
    }

}