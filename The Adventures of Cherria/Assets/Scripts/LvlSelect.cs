using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LvlSelect : MonoBehaviour
{

    public Button[] lvlbuttons;
     void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt", 5);

        for (int i =0; i < lvlbuttons.Length; i++)
        {
            if (i + 5 > levelAt)
            {
                lvlbuttons[i].interactable = false;
            }
        }
    }
    public void Return()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }
    public void ToLvl1()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
    }

    public void ToLvl2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
    }
    public void ToLvl3()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 4);
    }
    public void ToLvl4()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 5);
    }
}
