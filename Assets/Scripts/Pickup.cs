using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the triggering object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Deactivate the pickup object
            gameObject.SetActive(false);
            
            // Add your desired pickup behavior here
            // For example, you can increment a score, play a sound, etc.
        }
    }
}