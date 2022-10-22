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
    private float JumpForce = 5f;
    [SerializeField]
    private float GravityOnGround;
    private float MoveMul = 1;
    [SerializeField]
    private bool Grounded = false;
    [SerializeField]
    private LayerMask _LayerMask;
    [SerializeField]
    private Transform GroundCheck;

    float MoveX;
    float MoveZ;

    [SerializeField]
    private Transform PlayerBody;
    [SerializeField]
    private Rigidbody PlayerRigidbody;

    private void Start()
    {
        // Kill me later...
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 35;

        PlayerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        Grounded = Physics.CheckSphere(GroundCheck.position, 0.4f, _LayerMask);

        if (Input.GetKeyDown(KeyCode.Space) && Grounded)
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, JumpForce, PlayerRigidbody.velocity.z);

        if(Grounded)
            MoveMul = 1f;
        else
            MoveMul = InAirMultiplier;

        MouseLook();
    }

    void FixedUpdate ()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 Axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * MoveSpeed;
        Axis = Axis * MoveMul;

        Vector3 Forward = new Vector3(-Camera.main.transform.right.z, 0.0f, Camera.main.transform.right.x);

        Vector3 Direction = (Forward * Axis.x + Camera.main.transform.right * Axis.y + Vector3.up * PlayerRigidbody.velocity.y);

        PlayerRigidbody.velocity = Direction;
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
