using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerController : MonoBehaviour
{

    //Assignables
    public Transform playerCam;
    public Transform orientation;

    //Rigibody
    private Rigidbody rb;

    //Movement
    private float speed = 4500f;
    private bool grounded;
    public LayerMask WhatTheHellIsGround;
    private Vector3 normalVector = Vector3.up;
    private float gravity = 10f;

    private float treshold = 0.01f;
    private float counterMovement = 0.175f;
    private float maxSpeed = 20f;

    //Jumping
    private bool readyToJump = true;
    private float jumpForce = 550f;
    private float jumpColdown = 0.25f;
    private float maxSlopeAngle = 35f;

    //Crouch and slide
    private Vector3 playerScale;
    private Vector3 crouchScale = new Vector3(1,0.5f,1);
    private float slideForce = 400;
    private float slideCounterMov = 0.2f;

    //look and rotation
    private float xRotation;
    public float sensitivity = 50f;
    private float sensMultiplier = 1f;

    //Inputs 
    private float x, y;
    private bool jumping, crouching;

    //Being Damaged
    public LayerMask WhatTheHellIsEnemy;

    //------------------------------------------

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Look();
        myInputs();
    }

    private void FixedUpdate()
    {
        move();
    }

    //------------------------------------------

    private void myInputs()
    {
        x = InputManager.getHorizontal();
        y = InputManager.getVertical();
        jumping = InputManager.isJumping();
        crouching = InputManager.isCrouching();
        if (InputManager.keepCrouching())
        {
            StartCrouch();
        }
        if (InputManager.finishCrouching())
        {
            StopCrouch();
        }

    }

    //------------------------------------------

    private void move()
    {


        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * gravity);

        // Find actual velocity relative to where the palyer is
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;


        CounterMovement(x,y,mag);


        //jumping
        if (readyToJump && jumping) Jump();


        // more speed in slides
        float maxSpeed = this.maxSpeed;

        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }


        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        // some multipliers for being crouching or in the air


        float multiplier = 1f, multiplierV = 1f;

        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        if (grounded && crouching)
        {
            multiplierV = 0f;
        
        }

        rb.AddForce(orientation.forward * y * speed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.right * x * speed * Time.deltaTime * multiplier);

    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumping) return;

        if (crouching)
        {
            rb.AddForce(speed * Time.deltaTime * -rb.velocity.normalized * slideCounterMov);
            return;
        }

        if (Mathf.Abs(mag.x) > treshold && Mathf.Abs(x) < 0.05f || (mag.x < -treshold && x > 0) || (mag.x > treshold && x < 0))
        {
            rb.AddForce(speed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Mathf.Abs(mag.y) > treshold && Mathf.Abs(y) < 0.05f || (mag.y < -treshold && y > 0) || (mag.y > treshold && y < 0))
        {
            rb.AddForce(speed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }



        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }


    }

    private Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90  -  u ;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);



        return new Vector2(xMag, yMag);
    }

    //------------------------------------------

    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x,transform.position.y - 0.5f,transform.position.z);
        if (rb.velocity.magnitude > 0.5f)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    //------------------------------------------

    private void Jump()
    {

        if (readyToJump && grounded)
        {
            readyToJump = false;
        
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(vel.x, 0,vel.z);
            }else if (rb.velocity.y > 0f)
            {
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            }

            Invoke(nameof(ResetJump), jumpColdown);

        }
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private bool cancellingGrounded;
    private void OnCollisionStay(Collision collision)
    {
        int layer = collision.gameObject.layer;
        if (WhatTheHellIsGround != (WhatTheHellIsGround | (1 << layer))) return;
        //if (layer != 6) return;

        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.contacts[i].normal;
            if (IsFloor(normal))
            {
                grounded = true;
                normalVector = normal;
                cancellingGrounded = false;
                CancelInvoke(nameof(StopGrounded));

            }
        }


        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }

    }

    private void StopGrounded() {
        grounded = false;
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    //------------------------------------------

    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -45f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }


}