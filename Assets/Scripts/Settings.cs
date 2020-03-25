using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    //Variables
    // Audio
    public AudioSource audioSource;
    public Toggle audioToggle;

    // Colour
    int lightColourIndex;
    int mediumColourIndex;
    int darkColourIndex;
    public Material lightColour;
    public Material mediumColour;
    public Material darkColour;
    public List<Material> lightColourOptions;
    public List<Material> mediumColourOptions;
    public List<Material> darkColourOptions;

    //Functions
    public void ToggleAudio()
    {
        if (audioToggle.isOn)
        { audioSource.volume = 1; }
        else
        { audioSource.volume = 0; }
    }

    // Palette Swap
    public void SetLightColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the light material
    {
        lightColourIndex = (lightColourIndex + mod + lightColourOptions.Count) % lightColourOptions.Count;
        lightColour.color = lightColourOptions[lightColourIndex].color;
    }

    public void SetMediumColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the medium material
    {
        mediumColourIndex = (mediumColourIndex + mod + mediumColourOptions.Count) % mediumColourOptions.Count;
        mediumColour.color = mediumColourOptions[mediumColourIndex].color;
    }

    public void SetDarkColour(int mod) // Move once through the choice of colours by a step size of "mod" and assign the new value to the dark material
    {
        darkColourIndex = (darkColourIndex + mod + darkColourOptions.Count) % darkColourOptions.Count;
        darkColour.color = darkColourOptions[darkColourIndex].color;
    }
}
