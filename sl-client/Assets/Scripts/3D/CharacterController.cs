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
    public float JumpForce = 5f;
    public float Gravity;
    public bool Grounded = false;

    float MoveX;
    float MoveZ;

    public Transform PlayerBody;
    private Rigidbody PlayerRigidbody;

    void Start()
    {
        PlayerRigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        MouseLook();
        Movement();
    }

    public void Movement()
    {
        MoveX = Input.GetAxis("Horizontal");
        MoveZ = Input.GetAxis("Vertical");

        Vector3 MovementVector = PlayerBody.transform.right * MoveX + PlayerBody.transform.forward * MoveZ;
        if(!Grounded)
            MovementVector.y += Gravity * Time.deltaTime;
        else
            MovementVector.y = 0f;

        PlayerRigidbody.velocity = (MovementVector * (MoveSpeed * 100)) * Time.deltaTime;
    }

    public void MouseLook()
    {
        MouseX = Input.GetAxis("Mouse X") * (MouseSensitivity * 100) * Time.deltaTime;
        MouseY = Input.GetAxis("Mouse Y") * (MouseSensitivity * 100) * Time.deltaTime;

        Xrotation -= MouseY;
        Xrotation = Mathf.Clamp(Xrotation, -MinMaXUpDown.x, MinMaXUpDown.y);

        CameraPivot.localRotation = Quaternion.Euler(Xrotation, 0f, 0f);
        PlayerBody.Rotate(Vector3.up * MouseX);
    }
}
