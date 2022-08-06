using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public CharacterController2D CharacterController;

    public float RunSpeed = 40f;

    float HorizontalMove = 0f;

    bool Jumping = false;

    void Update()
    {
        HorizontalMove = Input.GetAxis("Horizontal") * RunSpeed;

        if(Input.GetButtonDown("Jump"))
        {
            Jumping = true;
        }
    }

    void FixedUpdate()
    {
        CharacterController.Move(HorizontalMove * Time.fixedDeltaTime, false, Jumping);
        Jumping = false;
    }
}
