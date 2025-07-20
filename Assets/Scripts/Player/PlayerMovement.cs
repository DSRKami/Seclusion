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

    [Header("Dash Settings")]
    [SerializeField] private float dashPower = 15f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 1f;


    private Rigidbody2D rb;
    private InputReader inputReader;

    private bool isDashing = false;
    private bool canDash = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputReader = FindAnyObjectByType<InputReader>();
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
        if (isDashing) return; // Don't overwrite velocity during dash

        float currentSpeed = inputReader.IsSprinting ? moveSpeed * sprintMultiplier : moveSpeed;
        rb.linearVelocity = inputReader.MoveInput * currentSpeed;
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
