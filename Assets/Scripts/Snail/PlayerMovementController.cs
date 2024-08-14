using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 velocity;

    private Vector2 previousMovement;
    private bool hasReleasedKey;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        previousMovement = Vector2.zero;
        hasReleasedKey = true;
    }

    void Update()
    {
        // Get input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Normalize diagonal movement to prevent faster diagonal speed
        if (movement.magnitude > 1)
        {
            movement.Normalize();
        }

        // Check if the player has released all movement keys
        if (movement == Vector2.zero && !hasReleasedKey)
        {
            StartCoroutine(ReleaseKeyDelay());
        }
    }

    void FixedUpdate()
    {
        // Check if the player has changed direction and has released the previous key
        if (movement != Vector2.zero && movement != previousMovement && hasReleasedKey)
        {
            // Stop the player briefly before changing direction to avoid glide
            velocity = Vector2.zero;
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

            // Update previous movement direction
            previousMovement = movement;

            // Reset the release key flag
            hasReleasedKey = false;
        }
        else if (movement != Vector2.zero)
        {
            // Accelerate if there's input
            velocity = Vector2.Lerp(velocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
        else
        {
            // Decelerate to stop
            velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    IEnumerator ReleaseKeyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        hasReleasedKey = true;
    }

    public Vector2 GetMovement()
    {
        return movement;
    }
}
