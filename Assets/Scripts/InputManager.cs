using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager {

    public static bool Up()
    {
        return Input.GetKey(KeyCode.UpArrow);
    }
    public static bool Down()
    {
        return Input.GetKey(KeyCode.DownArrow);
    }
    public static bool Left()
    {
        return Input.GetKey(KeyCode.LeftArrow);
    }
    public static bool Right()
    {
        return Input.GetKey(KeyCode.RightArrow);
    }
    public static bool Space()
    {
        return Input.GetKeyDown(KeyCode.Space);
    }
}
