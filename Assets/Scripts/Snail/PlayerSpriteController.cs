using UnityEngine;

public class PlayerSpriteController : MonoBehaviour
{
    public GameObject playerLeft;
    public GameObject playerRight;
    public GameObject playerFront;
    public GameObject playerBack;

    private GameObject lastActiveSprite;
    private Animator animator;
    private PlayerMovementController movementController;

    void Start()
    {
        animator = GetComponent<Animator>();
        movementController = GetComponent<PlayerMovementController>();
        lastActiveSprite = playerFront; // Default sprite when idle
    }

    void Update()
    {
        Vector2 movement = movementController.GetMovement();

        // Update the player's sprite based on movement direction
        UpdatePlayerSprite(movement);

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

    void UpdatePlayerSprite(Vector2 movement)
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
