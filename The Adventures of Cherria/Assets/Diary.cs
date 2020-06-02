using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Diary : MonoBehaviour
{
    public GameObject popUpBox;
    public TMP_Text dialogText;
    public string dialog;
    public bool playerInRange;
    public string popUp;



    // Start is called before the first frame update
    void Start()
    {
        popUpBox.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && playerInRange)
        {
            if (popUpBox.activeInHierarchy)
            {
                popUpBox.SetActive(false);
            }
            else
            {
                popUpBox.SetActive(true);
                dialogText.text = dialog;
            }


        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            popUpBox.SetActive(false);
        }
    }

    public void Close()
    {
        popUpBox.SetActive(false);
    }
}
