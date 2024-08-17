using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UIPlayerMovementController : PlayerMovementController
{
    public RectTransform uiElement;  // The UI element you want to move
    private RectTransform canvasRectTransform;  // The RectTransform of the Canvas
    private bool isMoving = false;

    protected override void Start()
    {
        base.Start();
        // Get the RectTransform of the canvas to convert screen to UI space
        canvasRectTransform = uiElement.GetComponentInParent<Canvas>().GetComponent<RectTransform>();
    }

    protected override void Update()
    {
        if (controlType == ControlType.MouseClick)
        {
            // UI Click movement
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, Input.mousePosition, null, out localPoint);
                targetPosition = localPoint;
                isMoving = true;
            }

            // Calculate direction towards target in UI space
            movement = (targetPosition - (Vector2)uiElement.localPosition).normalized;
        }
        else
        {
            base.Update();
        }
    }

    protected override void FixedUpdate()
    {
        if (controlType == ControlType.MouseClick)
        {
            HandleUIClickMovement();
        }
        else
        {
            base.FixedUpdate();
        }
    }

    protected void HandleUIClickMovement()
    {
        if (isMoving)
        {
            Vector3 movementDelta = Vector3.zero;

            // Move towards the target position if not reached
            if (Vector2.Distance(uiElement.localPosition, targetPosition) > 0.1f)
            {
                velocity = Vector2.Lerp(velocity, movement * moveSpeed, acceleration * Time.fixedDeltaTime);
                movementDelta = velocity * Time.fixedDeltaTime;
                uiElement.localPosition += movementDelta;
            }
            else
            {
                // Decelerate to stop when close to the target
                velocity = Vector2.Lerp(velocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
                movementDelta = velocity * Time.fixedDeltaTime;
                uiElement.localPosition += movementDelta;
                isMoving = false;
            }
        }
    }

    // Helper function to check if the pointer is over a UI element
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
