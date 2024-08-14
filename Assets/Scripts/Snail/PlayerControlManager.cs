using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlManager : MonoBehaviour
{
    public List<PlayerMovementController> players; // List of all PlayerMovementManager scripts attached to the GameObjects
    private int currentIndex = 0; // Index of the currently controlled player

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

            // Activate the new current player
            ActivatePlayer(currentIndex);
        }
    }

    void ActivatePlayer(int index)
    {
        // Activate the player at the given index
        players[index].enabled = true;

        // Deactivate all other players
        for (int i = 0; i < players.Count; i++)
        {
            if (i != index)
            {
                players[i].enabled = false;
            }
        }
    }
}
