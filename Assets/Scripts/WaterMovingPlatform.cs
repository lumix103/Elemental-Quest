using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterMovingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;
    public GameObject button; // Reference to the button GameObject
    public BoxCollider2D buttonCollider; // Reference to the button's BoxCollider2D component
    private bool isMoving;
    private int i;

    private void Start()
    {
        transform.localPosition = points[startingPoint].localPosition;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E) && IsPlayerInRange())
        {
            ActivateMovement();
        }
        else
        {
            DeactivateMovement();
        }

        if (isMoving)
        {
            if (Vector2.Distance(transform.localPosition, points[i].localPosition) < 0.2f)
            {
                i++;
                if (i == points.Length)
                {
                    i = 0;
                }
            }

            transform.localPosition = Vector2.MoveTowards(transform.localPosition, points[i].localPosition, speed * Time.deltaTime);
        }
    }

    private bool IsPlayerInRange()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(buttonCollider.bounds.center, buttonCollider.bounds.size, 0f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }

    private void ActivateMovement()
    {
        if (!isMoving)
        {
            isMoving = true;
            button.SetActive(false); // Disable the button GameObject when the platform is moving
        }
    }

    private void DeactivateMovement()
    {
        if (isMoving)
        {
            isMoving = false;
            button.SetActive(true); // Enable the button GameObject when the platform is not moving
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(buttonCollider, collision, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(buttonCollider, collision, false);
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