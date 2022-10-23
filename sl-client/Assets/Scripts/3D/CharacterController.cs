using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Camera stuff")]
    [SerializeField]
    private Transform CameraPivot;
    [SerializeField]
    private float MouseSensitivity;
    [SerializeField]
    private Vector2 MinMaXUpDown;

    private float MouseX = 0f;
    private float MouseY = 0f;
    private float Xrotation = 0f;

    [Header("Movement stuff")]
    [SerializeField]
    private float MoveSpeed = 5f;
    [SerializeField]
    private float InAirMultiplier = 0.45f;
    [SerializeField]
    private float NoclipSpeed = 5f;
    [SerializeField]
    private float JumpForce = 5f;
    [SerializeField]
    private float GravityOnGround;
    private float MoveMul = 1;
    private Vector2 Axis;
    [SerializeField]
    private bool Grounded = false;
    [SerializeField]
    private LayerMask _LayerMask;
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private State CharacterControllerState;

    [SerializeField]
    private Transform PlayerBody;
    [SerializeField]
    private Rigidbody PlayerRigidbody;

    public enum State
    {
        Crouching,
        Walking,
        Sprinting,
        Swimming,
        Climmbing,
        Noclip
    }

    private void Start()
    {
        // Kill me later...
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 35;

        PlayerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;

        CharacterControllerState = State.Walking;
    }

    // Update is called once per frame
    private void Update()
    {
        Grounded = Physics.CheckSphere(GroundCheck.position, 0.4f, _LayerMask);
        Axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * MoveSpeed;
        
        if (Grounded)
        {
            MoveMul = 1f;

            if(Input.GetKeyDown(KeyCode.Space))
                PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, JumpForce, PlayerRigidbody.velocity.z);
        }
        else
        {
            MoveMul = InAirMultiplier;
        }

        MouseLook();
    }

    void FixedUpdate ()
    {
        switch(CharacterControllerState)
        {
            case State.Crouching:
                NormalMovement();
                break;

            case State.Walking:
                NormalMovement();
                break;

            case State.Sprinting:
                NormalMovement();
                break;

            case State.Noclip:
                NoclipMovement();
                break;
        }
    }

    private void NormalMovement()
    {
        PlayerRigidbody.useGravity = true;
        PlayerBody.GetComponent<CapsuleCollider>().enabled = true;
        Axis = Axis * MoveMul;

        Vector3 Forward = new Vector3(-CameraPivot.transform.right.z, 0.0f, CameraPivot.transform.right.x);

        Vector3 Direction = (Forward * Axis.x + CameraPivot.transform.right * Axis.y + Vector3.up * PlayerRigidbody.velocity.y);

        PlayerRigidbody.velocity = Direction;
    }

    private void NoclipMovement()
    {
        PlayerRigidbody.useGravity = false;
        PlayerBody.GetComponent<CapsuleCollider>().enabled = false;

        Vector3 Direction = (CameraPivot.forward * Axis.x + CameraPivot.right * Axis.y) * (NoclipSpeed / 100);

        PlayerBody.position += Direction;//CameraPivot.forward + new Vector3(1,0,0) * Axis.y;
    }

    private void MouseLook()
    {
        MouseX = Input.GetAxis("Mouse X") * Time.deltaTime * MouseSensitivity;
        MouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSensitivity;

        Xrotation -= MouseY;
        Xrotation = Mathf.Clamp(Xrotation, -MinMaXUpDown.x, MinMaXUpDown.y);

        CameraPivot.localRotation = Quaternion.Euler(Xrotation, 0f, 0f);
        PlayerBody.Rotate(Vector3.up * MouseX);
    }
}
