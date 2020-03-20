using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Options : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;
    private AudioManager audioManager;

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio Manager").GetComponent<AudioManager>();
        musicSlider.value = audioManager.musicSource.volume;
        sfxSlider.value = audioManager.sfxSource.volume;
    }

    public void UpdateSoundValues()
    {
        audioManager.musicSource.volume = musicSlider.value;
        audioManager.sfxSource.volume = sfxSlider.value;
    }

    public void ReturnToMenu ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
