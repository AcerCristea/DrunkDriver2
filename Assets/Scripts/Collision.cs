using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {

            Debug.Log("Collision");
            // Access the StateManager to decrease lives
            StateManager stateManager = FindObjectOfType<StateManager>();
            if (stateManager != null)
            {
                stateManager.LoseLife();
            }
        }
    }
}
