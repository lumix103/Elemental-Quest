using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class OnKeyPickup : MonoBehaviour
{
    public GameObject associatedObject;
    public Tilemap associatedTilemap;
    public AudioSource pickUp;
    public AudioSource trapDoor;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the triggering object is the player
        if (other.gameObject.CompareTag("Player"))
        {
            // Deactivate the associated object and tilemap
            pickUp.Play();
            associatedObject.gameObject.SetActive(false);
            trapDoor.Play();
            associatedTilemap.gameObject.SetActive(false);
        }
    }
}