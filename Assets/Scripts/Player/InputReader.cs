using UnityEngine;

/// <summary>
/// This script is responsible for reading player inputs and processing them.
/// Outputs a normalised 2D vector representing the player's movement direction.
/// The vector is normalized to ensure consistent movement speed regardless of direction.
/// </summary>
public class InputReader : MonoBehaviour
{
    /// <summary>
    /// normalised 2D vector representing the player's movement direction.
    /// </summary>
    public Vector2 MoveInput { get; private set; }

    /// <summary>
    /// True if player is holding sprint key (Left Shift).
    /// </summary>
    public bool IsSprinting { get; private set; }

    /// <summary>
    /// True if player has pressed the dash key (Space).
    /// </summary>
    public bool DashPressed { get; private set; }

    void Update()
    {
        // Get input from the horizontal and vertical axes
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // Normalise to ensure diagonal movement is not faster
        MoveInput = new Vector2(x, y).normalized;

        IsSprinting = Input.GetKey(KeyCode.LeftShift);
        DashPressed = Input.GetKeyDown(KeyCode.Space);
    }
}
