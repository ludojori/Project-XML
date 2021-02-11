using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public float speed = 1.0f;
    public float gravity = -9.81f;
    public float jumpHeight = 3.0f;

    private float horizontal = 1.0f;
    private float vertical = 1.0f;
    private bool isGrounded = false;

    private Vector3 velocity;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;

        velocity.y += gravity * Time.deltaTime;

        //controller.Move(movement * speed * Time.deltaTime);
        //controller.Move(velocity * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
