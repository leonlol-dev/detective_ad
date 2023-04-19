using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("Ground Mask")]
    public float playerHeight;
    public float groundDrag;
    public LayerMask groundLayer;
    bool grounded;
    
    public bool canMove { get; set; }

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("Steps")]
    //steps variables
    public GameObject stepRayHeight;
    public GameObject stepRayLower;
    public GameObject playerCapsule;


    public Transform camTransform;

    public float stepSmooth = 0.1f;



    private void Start()
    {
        rb  = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        canMove = true;
    }

    private void Update()
    {
        //Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        GetInput();
        SpeedControl();

        //Drag
        if(grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }

        //Trying to clamp camera rotate to match player capsule. Currently rotates the player, but not fitting with camera
        //float playerRotate = camTransform.transform.rotation.y;
        //transform.Rotate(0.0f, playerRotate, 0.0f);

    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
            Steps();
        }
        
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void Move()
    { 
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    void Steps()
    {
        //Steps section, player can step up when ray hits lower cast, but not upper
        RaycastHit hitLower;
        Debug.DrawRay(stepRayLower.transform.position, (transform.TransformDirection(-orientation.forward)*0.5f),Color.green);
        if (Physics.Raycast(stepRayLower.transform.position, transform.TransformDirection(-orientation.forward), out hitLower, 0.5f))
        {
            RaycastHit hitHigher;
            Debug.DrawRay(stepRayHeight.transform.position, (transform.TransformDirection(-orientation.forward) * 1f), Color.red);
            if (!Physics.Raycast(stepRayHeight.transform.position, transform.TransformDirection(-orientation.forward), out hitHigher, 1f))
            {
                rb.position -= new Vector3(0f, -stepSmooth, 0f);
            }
        }
    }
    
}
