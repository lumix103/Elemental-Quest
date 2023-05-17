using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamera : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isDebug = false;
    private Transform trans;
    void Start()
    {
        trans = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDebug)
        {
            trans.position = new Vector3(trans.position.x + Input.GetAxis("Horizontal") * Time.fixedDeltaTime,
            trans.position.y + Input.GetAxis("Vertical") * Time.fixedDeltaTime,
            trans.position.z
            );
        }
    }
}
