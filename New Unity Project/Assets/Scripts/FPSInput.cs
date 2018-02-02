using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Control Script/FPS Input")]

public class FPSInput : MonoBehaviour {

    public float speed = 6.0f;
    public float gravity = -9.8f;
    public float jumpSpeed = 10.0f;
    public float vectorLengthDown = 1.5f;
    public float jumpDistance = 50.0f; 

    private CharacterController _charController; 

	// Use this for initialization
	void Start () {
        _charController = GetComponent<CharacterController>(); 
	}
	
	// Update is called once per frame
	void Update () {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        float deltaZ = Input.GetAxis("Vertical") * speed;
        Vector3 movement = new Vector3(deltaX, 0, deltaZ);
        movement = Vector3.ClampMagnitude(movement, speed);

        Vector3 down = transform.TransformDirection(Vector3.down);

        bool onGround = Physics.Raycast(transform.position, down, vectorLengthDown);
        bool overJumpHeight = !Physics.Raycast(transform.position, down, vectorLengthDown + jumpDistance);

        //print(overJumpHeight);
        print("jumpPressed = " + Input.GetButton("Jump"));

        print(!overJumpHeight && Input.GetButton("Jump"));

        //if there is ground beneath me, then i can jump
        if (!overJumpHeight && Input.GetButton("Jump"))
        {//1.1f is the best option apparently
            print("There something below the player!");
            movement.y = jumpSpeed; 
        }
        else//if player reaches jump height from the ground
        {
            movement.y = gravity;

        }


        movement *= Time.deltaTime;
        movement = transform.TransformDirection(movement);
        _charController.Move(movement); 
		
	}
}
