using UnityEngine;
using System.Collections;

public class PlayerMovementController : MonoBehaviour
{
    public enum ControlType
    {
        WASD,
        MouseClick
    }

    public ControlType controlType = ControlType.WASD; // Choose the type of control
    public float moveSpeed = 5f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    protected Vector2 movement;
    protected Vector2 velocity;
    protected Vector2 targetPosition;

    protected Vector2 previousMovement;
    protected bool hasReleasedKey;


    protected virtual void Start()
    {
        previousMovement = Vector2.zero;
        hasReleasedKey = true;
        targetPosition = transform.position;
    }

    protected virtual void Update()
    {
        if (controlType == ControlType.WASD)
        {
            // WASD movement
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
        else if (controlType == ControlType.MouseClick)
        {
            // Mouse click movement
            if (Input.GetMouseButtonDown(0))
            {
                targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            // Calculate direction towards target
            movement = (targetPosition - (Vector2)transform.position).normalized;
        }

    }

    protected virtual void FixedUpdate()
    {
        if (controlType == ControlType.WASD)
        {
            HandleWASDMovement();
        }
        else if (controlType == ControlType.MouseClick)
        {
            HandleMouseClickMovement();
        }
    }

    protected virtual void HandleWASDMovement()
    {
        Vector3 movementDelta = Vector3.zero;

        // Check if the player has changed direction and has released the previous key
        if (movement != Vector2.zero && movement != previousMovement && hasReleasedKey)
        {
            // Stop the player briefly before changing direction to avoid glide
            velocity = Vector2.zero;

            // Update previous movement direction
            previousMovement = movement;

            // Reset the release key flag
            hasReleasedKey = false;
        }
        else if (movement != Vector2.zero)
        {
            // Accelerate if there's input
            velocity = Vector2.Lerp(velocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
            movementDelta = velocity * Time.fixedDeltaTime;
            transform.position += movementDelta;
        }
        else
        {
            // Decelerate to stop
            velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            movementDelta = velocity * Time.fixedDeltaTime;
            transform.position += movementDelta;
        }
    }

    protected virtual void HandleMouseClickMovement()
    {
        Vector3 movementDelta = Vector3.zero;

        // Move towards the target position if not reached
        if (Vector2.Distance(transform.position, targetPosition) > 0.1f)
        {
            velocity = Vector2.Lerp(velocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
            movementDelta = velocity * Time.fixedDeltaTime;
            transform.position += movementDelta;
        }
        else
        {
            // Decelerate to stop when close to the target
            velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
            movementDelta = velocity * Time.fixedDeltaTime;
            transform.position += movementDelta;
        }
    }

    protected IEnumerator ReleaseKeyDelay()
    {
        yield return new WaitForSeconds(0.5f);
        hasReleasedKey = true;
    }

    public Vector2 GetMovement()
    {
        return movement;
    }
}