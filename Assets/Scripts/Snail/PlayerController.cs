using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerLeft;
    public GameObject playerRight;
    public GameObject playerFront;
    public GameObject playerBack;
    public float moveSpeed = 5f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 velocity;

    private GameObject lastActiveSprite;
    private Animator animator;
    private Vector2 previousMovement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastActiveSprite = playerFront; // Default sprite when idle
        previousMovement = Vector2.zero;
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

        // Update the player's sprite based on movement direction
        UpdatePlayerSprite();

        // Update the animator's "Move" parameter
        if (movement != Vector2.zero)
        {
            animator.SetBool("Move", true);
        }
        else
        {
            animator.SetBool("Move", false);
        }
    }

    void FixedUpdate()
    {
        // Check if the player has changed direction
        if (movement != Vector2.zero && movement != previousMovement)
        {
            // Trigger the "Turn" animation
            animator.SetTrigger("Turn");

            // Stop the player briefly before changing direction to avoid glide
            velocity = Vector2.zero;
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);

            // Update previous movement direction
            previousMovement = movement;
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

    void UpdatePlayerSprite()
    {
        if (movement.x > 0) // Moving right
        {
            ShowSprite(playerRight);
        }
        else if (movement.x < 0) // Moving left
        {
            ShowSprite(playerLeft);
        }
        else if (movement.y > 0) // Moving up
        {
            ShowSprite(playerBack);
        }
        else if (movement.y < 0) // Moving down
        {
            ShowSprite(playerFront);
        }
        else
        {
            // When not moving, keep the last active sprite visible
            ShowSprite(lastActiveSprite);
        }
    }

    void ShowSprite(GameObject spriteToShow)
    {
        // Hide all sprites
        playerLeft.SetActive(false);
        playerRight.SetActive(false);
        playerFront.SetActive(false);
        playerBack.SetActive(false);

        // Show the specified sprite
        spriteToShow.SetActive(true);
        lastActiveSprite = spriteToShow; // Store the last active sprite
    }
}
