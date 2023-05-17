using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    // Variables to control the bounce effect
    public float bounceHeight = 0.1f;
    public float bounceSpeed = 2f;

    // Initial position of the GameObject
    private Vector3 startPos;

    void Start()
    {
        // Store the initial position of the GameObject
        startPos = transform.position;
    }

    void Update()
    {
        // Calculate the new Y position based on a sine wave
        float newY = startPos.y + Mathf.Sin(Time.time * bounceSpeed) * bounceHeight;

        // Update the position of the GameObject
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

}