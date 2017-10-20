using UnityEngine;
using XboxCtrlrInput;
public sealed class PlayerJump : MonoBehaviour
{
    public PlayerController controller;
    public KeyCode jumpKeyCode;
    public XboxButton jumpKeyCodeController;
    public float jumpImpulse = 10.0f;

    private void Start()
    {
        Command.jumpKey = jumpKeyCode;
        Command.jumpKeyXbox = jumpKeyCodeController;
    }
    private void Update()
    {
        if (controller.isGrounded && !controller.isStunned && Command.GetJump())
            controller.playerRigidbody.AddForce(new Vector2(0.0f, jumpImpulse), ForceMode2D.Impulse);
    }
}
