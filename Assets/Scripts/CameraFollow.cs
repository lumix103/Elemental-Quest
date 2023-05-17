using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player1;
    public Transform player2;
    private Transform trans;
    private Camera cam;
    void Start()
    {
        trans = GetComponent<Transform>();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float midPointX = (player1.position.x + player2.position.x) / 2.0f;
        float midPointY = (player1.position.y + player2.position.y) / 2.0f;
        float distance = Vector3.Distance(player1.position, player2.position);
        trans.localPosition = new Vector3(midPointX, midPointY, trans.position.z);

        float cameraSize = Mathf.Clamp(distance * 2f, 5.0f, 15.0f);
        cam.orthographicSize = cameraSize;
    }
}

