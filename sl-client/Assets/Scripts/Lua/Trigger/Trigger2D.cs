using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger2D : MonoBehaviour
{
    string ColidedName;

    void OnTriggerEnter2D(Collider2D col)
    {
        ColidedName = col.name;
    }

    void OnTriggerStay2D(Collider2D col)
    {
        ColidedName = col.name;
    }

    void OnTriggerExit2D(Collider2D col)
    {
        ColidedName = "";
    }

    public string GetValue()
    {
        return ColidedName;
    }
}
