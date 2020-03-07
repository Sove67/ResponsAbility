using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    // Variables
    // UI Properties
    public int transitionThreshold;
    public bool screenLock;
    public float revertSpeed;
    private RectTransform panelContainer;

    // Calculations
    public int page = 2;
    public float oldPositionX;
    private float totalDist = 0;

    // Functions
    private void Start()
    {
        panelContainer = this.GetComponent<RectTransform>();
        oldPositionX = panelContainer.anchoredPosition.x;
        panelContainer.anchoredPosition = panelContainer.anchoredPosition;
    }

    public void SetLock(bool choice) // Locks the screen from transitioning
    { screenLock = choice; }

    public void Swipe(float dist, Touch touch) // Handles transitions using horizontal swipes
    {
        if (!screenLock)
        {
            panelContainer.anchoredPosition = new Vector2(panelContainer.anchoredPosition.x + dist, panelContainer.anchoredPosition.y);
            totalDist += dist;
            if (touch.phase == TouchPhase.Ended)
            {
                // Swipe Right
                if (oldPositionX - panelContainer.anchoredPosition.x > transitionThreshold && page + 1 < 4)
                {
                    StartCoroutine(MoveToXPosition(oldPositionX - panelContainer.rect.width));
                    page += 1;
                }

                // Swipe Left
                else if (oldPositionX - panelContainer.anchoredPosition.x < -transitionThreshold && page - 1 > 0)
                {
                    StartCoroutine(MoveToXPosition(oldPositionX + panelContainer.rect.width));
                    page += -1;
                }

                else
                { StartCoroutine(MoveToXPosition(oldPositionX));  }
                totalDist = 0;
            }
        }
    }

    public IEnumerator MoveToXPosition(float targetX)
    {
        float distance = targetX - panelContainer.anchoredPosition.x;

        while(distance < -revertSpeed || revertSpeed < distance)
        {
            // Move Left 
            if (distance > 0 && panelContainer.anchoredPosition.x + revertSpeed <= targetX)
            { panelContainer.anchoredPosition += new Vector2(revertSpeed, 0); }

            // Move Right
            else if (distance < 0 && panelContainer.anchoredPosition.x - revertSpeed >= targetX)
            { panelContainer.anchoredPosition += new Vector2(-revertSpeed, 0); }

            distance = targetX - panelContainer.anchoredPosition.x;
            yield return new WaitForSeconds(.01f);
        }

        panelContainer.anchoredPosition = new Vector2(targetX, panelContainer.anchoredPosition.y);
        oldPositionX = panelContainer.anchoredPosition.x;
        yield break;
    }
}
