using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colour_Selector : MonoBehaviour
{
    public int chosen_colour;

    public void ChangeColour(int colour)
    {
        chosen_colour = colour;
    }
}