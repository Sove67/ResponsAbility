using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //Variables
    public AudioSource audioSource;
    public Toggle audioToggle;


    //Functions
    public void ToggleAudio()
    {
        if (audioToggle.isOn)
        { audioSource.volume = 1; }
        else
        { audioSource.volume = 0; }
    }
}
