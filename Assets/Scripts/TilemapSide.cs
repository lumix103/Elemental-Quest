using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSide : MonoBehaviour
{
    public Tilemap tilemap;
    public float distance = 1f;
    public float speed = 1f;

    private Vector3 originalPosition;
    private Vector3 targetPosition;
    private bool movingRight = true;

    void Start()
    {
        // Save the original position of the tilemap
        originalPosition = tilemap.transform.position;
        // Set the initial target position
        targetPosition = originalPosition + Vector3.right * distance;
    }

    void Update()
    {
        // Calculate the new position of the tilemap
        Vector3 newPosition = Vector3.Lerp(tilemap.transform.position, targetPosition, Time.deltaTime * speed);
        // Move the tilemap to the new position
        tilemap.transform.position = newPosition;

        // Check if the tilemap has reached its target position
        if (Vector3.Distance(tilemap.transform.position, targetPosition) < 0.01f)
        {
            // If it has, switch the target position
            if (movingRight)
            {
                targetPosition = originalPosition;
            }
            else
            {
                targetPosition = originalPosition + Vector3.right * distance;
            }
            // Invert the movingRight flag
            movingRight = !movingRight;
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
}

