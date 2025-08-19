using UnityEngine;
using System.Collections;

/// <summary>
/// Applies physics based movement to the player character.
/// Reads input from the InputReader and applies movement to the player.
/// Implements dash-sticking mechanic.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float sprintMultiplier = 1.5f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;

    [Header("Dash Settings")]
    [SerializeField] private float dashPower = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;

    // Debug variable to monitor current speed in Inspector
    [Header("Debug")]
    [SerializeField] private float debugCurrentSpeed;

    private Rigidbody2D rb;
    private InputReader inputReader;

    private bool isDashing = false;
    private bool canDash = true;
    private bool isStuckToWall = false; // Tracks if player is stuck to wall

    private float targetSpeed;
    private float currentSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = FindAnyObjectByType<InputReader>();
        currentSpeed = moveSpeed;
    }

    private void Update()
    {
        if (inputReader == null) return;

        // If stuck, only allow dash or shift to unstick
        if (isStuckToWall)
        {
            // Unstick by dashing (any direction)
            if (inputReader.DashPressed && inputReader.MoveInput != Vector2.zero)
            {
                UnstickFromWall();
                StartCoroutine(Dash(true)); // Pass true to skip cooldown
            }
            // Unstick by pressing Shift (sprint key)
            else if (inputReader.IsSprinting)
            {
                UnstickFromWall();
            }
            return; // Prevent normal movement while stuck
        }

        // Normal dash logic
        if (isDashing) return;

        if (inputReader.DashPressed && canDash && inputReader.MoveInput != Vector2.zero)
        {
            Debug.Log("Dash Pressed!");
            StartCoroutine(Dash(false)); // Normal dash with cooldown
        }
    }

    private void FixedUpdate()
    {
        if (isDashing || isStuckToWall) return; // Prevent movement while dashing or stuck

        // Determine target speed
        targetSpeed = inputReader.IsSprinting ? moveSpeed * sprintMultiplier : moveSpeed;

        // Accelerate or decelerate towards target speed
        float lerpRate = (targetSpeed > currentSpeed) ? acceleration : deceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, lerpRate * Time.fixedDeltaTime);

        rb.linearVelocity = inputReader.MoveInput * currentSpeed;

        // Update debug variable
        debugCurrentSpeed = currentSpeed;
    }

    /// <summary>
    /// Performs a dash in the direction of the current movement input.
    /// If skipCooldown is true, dash cooldown is ignored (used for unsticking from wall).
    /// </summary>
    private IEnumerator Dash(bool skipCooldown)
    {
        Debug.Log("Dashing!");
        canDash = false;
        isDashing = true;

        rb.linearVelocity = inputReader.MoveInput.normalized * dashPower;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;

        if (!skipCooldown)
        {
            yield return new WaitForSeconds(dashCooldown);
        }
        canDash = true;
        Debug.Log("Dashing Has Been Reset!");
    }

    /// <summary>
    /// Called when the player collides with a wall during a dash.
    /// </summary>
    private void StickToWall()
    {
        isStuckToWall = true;
        rb.linearVelocity = Vector2.zero; // Stop movement immediately
        rb.linearVelocity = Vector2.zero;
        Debug.Log("Player stuck to wall!");
    }

    /// <summary>
    /// Releases the player from the wall, restoring normal movement.
    /// </summary>
    private void UnstickFromWall()
    {
        isStuckToWall = false;
        Debug.Log("Player unstuck from wall!");
    }

    // Detect collision with wall during dash
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // You may want to use tags/layers for walls, e.g. "Wall"
        if (isDashing && collision.gameObject.CompareTag("Wall"))
        {
            StickToWall();
        }
    }
}
