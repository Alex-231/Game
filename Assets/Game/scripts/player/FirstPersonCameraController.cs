using UnityEngine;
using System.Collections;

public class FirstPersonCameraController : MonoBehaviour {

    [SerializeField]
    public GameObject camPoint;
    private GameObject cam;


    // Use this for initialization
    void Start () {

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
	
	}
}
