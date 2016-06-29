using UnityEngine;
using System.Collections;

public class FirstPersonController : MonoBehaviour {

    [SerializeField]
    public GameObject cameraPoint;

    // Use this for initialization
    void Start () {

        //Wait for campoint available.
        while (cameraPoint == null)
        {

        }

        //Activate camera.
        cameraPoint.gameObject.transform.GetChild(0).gameObject.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
	
	}
}
