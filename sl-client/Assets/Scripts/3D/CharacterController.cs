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
    private float InWaterMultiplier = 0.45f;
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
        Grounded = CheckForGround();//Grounded = Physics.OverlapSphere(GroundCheck.position, 0.25f, _LayerMask);
        Axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * MoveSpeed;
        
        if (Grounded)
        {
            MoveMul = 1f;

            if(CharacterControllerState == State.Walking || CharacterControllerState == State.Sprinting)
            {
                if(Input.GetKeyDown(KeyCode.LeftControl))
                    MoveMul = 1.2f;
            }
        }
        else
        {
            MoveMul = InAirMultiplier;
        }

        if(Input.GetKeyDown(KeyCode.V))
            ToggleNoclip();

        MouseLook();
    }

    private bool CheckForGround()
    {
        Collider[] hitColliders = Physics.OverlapSphere(GroundCheck.position, 0.25f, _LayerMask);
        for(int i = 0; i < hitColliders.Length; i++)
        {
            if(hitColliders.Length > 0)
                return true;
        }
        return false;
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

            case State.Swimming:
                SwimmMovement();
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

        if(Input.GetKeyDown(KeyCode.Space) && Grounded)
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, JumpForce, PlayerRigidbody.velocity.z);
    }

    private void SwimmMovement()
    {
        PlayerRigidbody.useGravity = true;
        PlayerBody.GetComponent<CapsuleCollider>().enabled = true;
        Axis = Axis * (MoveMul * InWaterMultiplier);

        Vector3 Forward = new Vector3(-CameraPivot.transform.right.z, 0.0f, CameraPivot.transform.right.x);

        Vector3 Direction = (Forward * Axis.x + CameraPivot.transform.right * Axis.y + Vector3.up * PlayerRigidbody.velocity.y);

        PlayerRigidbody.velocity = Direction;

        if(Input.GetKey(KeyCode.Space) && Grounded)
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, JumpForce * InWaterMultiplier, PlayerRigidbody.velocity.z);
        else
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, -InWaterMultiplier, PlayerRigidbody.velocity.z);
    }

    private void NoclipMovement()
    {
        PlayerRigidbody.useGravity = false;
        PlayerBody.GetComponent<CapsuleCollider>().enabled = false;

        Vector3 Direction = (CameraPivot.forward * Axis.x + CameraPivot.right * Axis.y) * (NoclipSpeed / 100f);

        PlayerBody.position += Direction;
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

    public void ToggleNoclip()
    {
        PlayerRigidbody.velocity = new Vector3(0f, 0f, 0f);
        if(CharacterControllerState == State.Noclip)
            CharacterControllerState = State.Walking;
        else
            CharacterControllerState = State.Noclip;
    }
}
