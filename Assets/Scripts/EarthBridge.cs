using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class EarthBridge : MonoBehaviour
{
    // Start is called before the first frame update
    Tilemap tilemap;
    public int distance;
    public float speed = 1f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        // Save the original position of the tilemap
        originalPosition = tilemap.transform.position;
        // Set the initial target position
        targetPosition = originalPosition + Vector3.right * distance;
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition = Vector3.Lerp(tilemap.transform.position, targetPosition, Time.deltaTime * speed);
        // Move the tilemap to the new position
        tilemap.transform.position = newPosition;

        // Check if the tilemap has reached its target position
        if (Vector3.Distance(tilemap.transform.position, targetPosition) < 0.01f)
        {
            if (tilemap.transform.childCount == 0)
            {
                StartCoroutine(KillBridge());
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.transform.SetParent(transform);
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }

    IEnumerator KillBridge()
    {
        yield return new WaitForSeconds(6f);
        tilemap.transform.position = originalPosition;
        gameObject.SetActive(false);
    }
}
