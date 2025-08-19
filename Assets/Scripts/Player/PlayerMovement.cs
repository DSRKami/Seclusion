using UnityEngine;
using System.Collections;

/// <summary>
/// Applies physics based movement to the player character.
/// Reads input from the InputReader and applies movement to the player.
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
        if (isDashing || inputReader == null) return;

        if (inputReader.DashPressed && canDash && inputReader.MoveInput != Vector2.zero)
        {
            Debug.Log("Dash Pressed!");
            StartCoroutine(Dash());
        }
    }


    private void FixedUpdate()
    {
        if (isDashing) return;

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
    /// </summary>
    private IEnumerator Dash()
    {
        Debug.Log("Dashing!");
        canDash = false;
        isDashing = true;

        rb.linearVelocity = new Vector2(inputReader.MoveInput.x * dashPower, inputReader.MoveInput.y * dashPower);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
        Debug.Log("Dashing Has Been Reset!");
    }
}
