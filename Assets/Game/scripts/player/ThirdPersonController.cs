using UnityEngine;
using System.Collections;

public class ThirdPersonController : MonoBehaviour {

    [SerializeField]
    public GameObject cameraPoint;

    public void UpdateCamPoint (GameObject _camPoint)
    {
        cameraPoint = _camPoint;
    }

	// Use this for initialization
	void Start () {

        //Wait for campoint available.
	    while(cameraPoint == null)
        {

        }

        //Activate camera.
        cameraPoint.gameObject.transform.GetChild(0).gameObject.SetActive(true);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
