using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XboxCtrlrInput;

public class Command : MonoBehaviour
{
    public static KeyCode jumpKey;
    public static XboxButton jumpKeyXbox;

    public static bool GetJump()
    {
        return Input.GetKey(jumpKey) || XCI.GetButton(jumpKeyXbox);
    }
    
}
