using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public List<PlayerMovementController> players; // List of all PlayerMovementController scripts attached to the GameObjects
    private int currentIndex = 0; // Index of the currently controlled player

    // Define the delegate and event for player switching
    public delegate void PlayerSwitchEvent(int newPlayerIndex);
    public static event PlayerSwitchEvent OnPlayerSwitch;

    void Start()
    {
        // Ensure only the first player in the list is active at the start
        ActivatePlayer(currentIndex);
    }

    void Update()
    {
        // Check if the "C" key is pressed to switch control to the next player
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Deactivate the current player
            players[currentIndex].enabled = false;

            // Move to the next player in the list
            currentIndex = (currentIndex + 1) % players.Count;

            // Activate the new current player and set the Animator's "Indoor" bool
            ActivatePlayer(currentIndex);

            // Trigger the player switch event
            OnPlayerSwitch?.Invoke(currentIndex);
        }
    }

    void ActivatePlayer(int index)
    {
        // Activate the player at the given index
        players[index].enabled = true;

        // Apply movement logic based on current player index
        if (index == 0)
        {
            // If controlling Player 0, disable Player 1's movement
            players[1].enabled = false;
        }
        else if (index == 1)
        {
            // If controlling Player 1, enable Player 0's movement
            players[0].enabled = true;
        }

        // Get the Animator component of the currently active player
        Animator animator = GetComponent<Animator>();

        if (animator != null)
        {
            // Set the "Indoor" bool to true if the index is 1, otherwise false
            animator.SetBool("Indoor", index == 1);
        }
    }
}
