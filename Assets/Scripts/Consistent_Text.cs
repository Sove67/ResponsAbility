using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Consistent_Text : MonoBehaviour
{

    // The following code is a modified version of Gwom's accepted answer at: https://answers.unity.com/questions/1301310/how-to-build-a-bestfit-font-size-for-a-group-of-ui.html

    // Sizing Variables
    public Text[] m_TextList;
    public int m_Size = 1;

    // Objects To Activate
    public GameObject[] pop_ups;
    public RectMask2D[] rect_masks;

    void Start()
    {
        // ApplySizes() doesnt function unless the text objects used are active and not masked. This allows the script to function properly
        foreach (var obj in pop_ups)
        { obj.SetActive(true); }
        foreach (var obj in rect_masks)
        { obj.enabled = false; }

        foreach (var item in m_TextList)
        { item.resizeTextMaxSize = 10000; }

        StartCoroutine(WaitFrame());
    }

    // This Coroutine pauses the script for a frame, to allow the required data to be processed
    public IEnumerator WaitFrame()
    {
        yield return 0;
        ApplySizes();
    }

    public void ApplySizes()
    {
        Debug.Log("Group: " + name);
        // Get the first one
        if (m_TextList.Length > 0)
        { m_Size = m_TextList[0].cachedTextGenerator.fontSizeUsedForBestFit; }

        // See if any are smaller
        foreach (var item in m_TextList)
        {
            Debug.Log("Item Text Size: " + item.cachedTextGenerator.fontSizeUsedForBestFit);

            if (item.cachedTextGenerator.fontSizeUsedForBestFit < m_Size)
            { m_Size = item.cachedTextGenerator.fontSizeUsedForBestFit; }
        }

        Debug.Log("Final Size: " + m_Size);

        // Apply as max size to all of the images
        foreach (var item in m_TextList)
        { item.resizeTextMaxSize = m_Size; }

        // ApplySizes() doesnt function unless the text objects used are active and not masked. This hides them once finished.
        foreach (var window in pop_ups)
        { window.SetActive(false); }
        foreach (var obj in rect_masks)
        { obj.enabled = true; }
    }
}
