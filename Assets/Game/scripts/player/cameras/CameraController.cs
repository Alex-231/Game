using UnityEngine;

abstract public class CameraController : MonoBehaviour
{
    //These values represent local positions and rotations!
    [Header("Camera Point")]
    public Vector3 pointStartingPos = Vector3.zero;
    public Vector3 pointStartingRot = Vector3.zero;
    [Header("Camera")]
    public Vector3 camStartingPos = Vector3.zero;
    public Vector3 camStartingRot = Vector3.zero;

    [SerializeField]
    public GameObject camPoint;
    public GameObject cam;

    public CameraModeController modeController;
    public Transform parent = null;

    public void Start()
    {
        modeController = GetComponent<CameraModeController>();

        modeController.ChangeCameraParent(parent);

        camPoint = modeController.camPoint;

        //Assign the camera.
        cam = camPoint.transform.GetChild(0).gameObject;

        //Activate the camera.
        cam.SetActive(true);

        ResetCamAndCamPointPosAndRot();
    }

    //Sets the position and rotation of the camera and camPoint to the starting values.
    public void ResetCamAndCamPointPosAndRot()
    {
        cam.transform.localPosition = camStartingPos;
        cam.transform.localEulerAngles = camStartingRot;
        camPoint.transform.localPosition = pointStartingPos;
        camPoint.transform.localEulerAngles = pointStartingRot;
    }

    public virtual Transform GetParent()
    {
        return null;
    }
}
