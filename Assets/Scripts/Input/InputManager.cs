using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static float getHorizontal()
    {
        return Input.GetAxisRaw("Horizontal");
    }

    public static float getVertical()
    {
        return Input.GetAxisRaw("Vertical");
    }

    public static bool isJumping()
    {
        return Input.GetButton("Jump");
    }

    public static bool isCrouching()
    {
        return Input.GetKey(KeyCode.LeftControl);
    }

    public static bool keepCrouching()
    {
        return Input.GetKeyDown(KeyCode.LeftControl);
    }

    public static bool finishCrouching()
    {
        return Input.GetKeyUp(KeyCode.LeftControl);
    }

    public static bool hasShooted()
    {
        return Input.GetMouseButtonDown(0);
    }
}
