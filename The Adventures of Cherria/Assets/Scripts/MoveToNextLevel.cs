using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MoveToNextLevel : MonoBehaviour
{
    public int nextsceneload;
    void Start()
    {
        nextsceneload = SceneManager.GetActiveScene().buildIndex;

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(4);

            if(nextsceneload > PlayerPrefs.GetInt("levelAt"))
            {
                PlayerPrefs.SetInt("levelAt", nextsceneload);
            }
        }
    }

}
