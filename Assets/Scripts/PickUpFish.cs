using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFish : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kitty;
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.CompareTag("Player"))
        {
            kitty.tag = "Player";
            gameObject.SetActive(false);
        }

    }
}
