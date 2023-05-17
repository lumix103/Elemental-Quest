using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadNext : MonoBehaviour
{
    // Start is called before the first frame update
    public string nextScene;
    void OnTriggerEnter2D(Collider2D col)
    {
        col.transform.SetParent(transform);
        if (gameObject.transform.childCount == 2)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        other.transform.SetParent(null);
    }
}
