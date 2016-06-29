using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    //How far the player can be from the ground to jump.
    [SerializeField]
    private float GroundDistance;

    private Vector3 velocity = Vector3.zero;
    private Vector3 rotation = Vector3.zero;

    private Rigidbody rb;

    private PlayerController controller;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<PlayerController>();
    }

    // Gets a movement vector
    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Jump()
    {
        if(IsGrounded())
        {
            velocity.y = controller.jumpVelocity;
        }
    }

    //Perform movement based on velocity variable
    void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

    }

    // Update is called once per frame
    void Update () {
        PerformMovement();
	}

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, -Vector3.up, GroundDistance + 0.1f);
    }
}
