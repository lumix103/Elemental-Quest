using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class EarthPowerTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject inputPromptE;
    public Tilemap tilemap;

    public void Start()
    {
        inputPromptE.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //print(" earth trigger stay");
        if (other.gameObject.CompareTag("Player") && (Input.GetKey(KeyCode.E)))
        {
            if (other.gameObject.GetComponent<PlayerController>().isCharacter == PlayerController.CharacterTypeState.TERRA)
            {
                if (tilemap == null)
                    print("ficl");
                tilemap.gameObject.SetActive(true);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //string wwww = 
        //soundPlayer.Play();
        if (other.gameObject.CompareTag("Player") && (other.gameObject.GetComponent<PlayerController>().isCharacter == PlayerController.CharacterTypeState.TERRA))
        {
            inputPromptE.SetActive(true);


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && (other.gameObject.GetComponent<PlayerController>().isCharacter == PlayerController.CharacterTypeState.TERRA))
        {
            inputPromptE.SetActive(false);


        }
    }

}
