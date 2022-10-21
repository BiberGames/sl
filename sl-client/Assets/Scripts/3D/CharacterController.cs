using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Camera stuff")]
    public Transform CameraPivot;
    public float MouseSensitivity;
    public Vector2 MinMaXUpDown;

    private float MouseX = 0f;
    private float MouseY = 0f;
    private float Xrotation = 0f;

    [Header("Movement stuff")]
    public float MoveSpeed = 5f;
    public float InAirMultiplier = 0.45f;
    public float JumpForce = 5f;
    public float Gravity;
    public bool Grounded = false;
    public LayerMask _LayerMask;
    public Transform GroundCheck;

    float MoveX;
    float MoveZ;

    public Transform PlayerBody;
    private Rigidbody PlayerRigidbody;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 35;
        PlayerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    private void Update()
    {
        Grounded = Physics.CheckSphere(GroundCheck.position, 0.4f, _LayerMask);

        if (Input.GetKey(KeyCode.Space) && Grounded)
            PlayerRigidbody.velocity = new Vector3(PlayerRigidbody.velocity.x, Gravity, PlayerRigidbody.velocity.z);

        MouseLook();
    }
    void FixedUpdate ()
    {
        if(Grounded)
            MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 Axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * MoveSpeed;

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
