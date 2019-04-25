using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Consistent_Text : MonoBehaviour
{

    // The following code is taken from Gwom's accepted answer at: https://answers.unity.com/questions/1301310/how-to-build-a-bestfit-font-size-for-a-group-of-ui.html
    public Text[] m_TextList;
    private int m_Size = 1;
    
    void Start()
    {
        SetSizes();
    }

    public void SetSizes()
    {
        foreach (var item in m_TextList)
            item.resizeTextMaxSize = 1000;

        StartCoroutine(WaitFrame());
    }

    public IEnumerator WaitFrame()
    {
        // returning 0 will make it wait 1 frame
        // need to wait a frame in order for the layout to work out the size
        yield return 0;
        ApplySizes();
    }

    public void ApplySizes()
    {
        // Get the first one
        if (m_TextList.Length > 0)
            m_Size = m_TextList[0].cachedTextGenerator.fontSizeUsedForBestFit;

        // See if any are smaller
        foreach (var item in m_TextList)
        {
            if (item.fontSize < m_Size)
                m_Size = item.cachedTextGenerator.fontSizeUsedForBestFit;
        }

        // Apply as max size to all of the images
        foreach (var item in m_TextList)
            item.resizeTextMaxSize = m_Size;
    }
}
