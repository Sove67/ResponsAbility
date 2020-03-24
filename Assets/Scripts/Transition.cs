using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition : MonoBehaviour
{
    // Variables
    // UI Properties
    public int transitionThreshold;
    public float slideSpeed;
    private RectTransform panelContainer;
    public bool screenLock;

    // Calculations
    public int page = 2;
    public float oldPositionX;


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
            if (touch.phase == TouchPhase.Ended)
            {
                // Swipe Right
                if (oldPositionX - panelContainer.anchoredPosition.x > transitionThreshold && page == 0)
                {
                    StartCoroutine(MoveToXPosition(oldPositionX - panelContainer.rect.width));
                    page = 1;
                }

                // Swipe Left
                else if (oldPositionX - panelContainer.anchoredPosition.x < -transitionThreshold && page == 1)
                {
                    StartCoroutine(MoveToXPosition(oldPositionX + panelContainer.rect.width));
                    page = 0;
                }

                else
                { StartCoroutine(MoveToXPosition(oldPositionX)); }
            }
        }
    }

    public IEnumerator MoveToXPosition(float targetX) // Animates the panels sliding from the current position to the target position
    {
        float distance = targetX - panelContainer.anchoredPosition.x;

        while(distance < -slideSpeed || slideSpeed < distance)
        {
            // Move Left 
            if (distance > 0 && panelContainer.anchoredPosition.x + slideSpeed <= targetX)
            { panelContainer.anchoredPosition += new Vector2(slideSpeed, 0); }

            // Move Right
            else if (distance < 0 && panelContainer.anchoredPosition.x - slideSpeed >= targetX)
            { panelContainer.anchoredPosition += new Vector2(-slideSpeed, 0); }

            distance = targetX - panelContainer.anchoredPosition.x;
            yield return new WaitForSeconds(.01f);
        }

        panelContainer.anchoredPosition = new Vector2(targetX, panelContainer.anchoredPosition.y);
        oldPositionX = panelContainer.anchoredPosition.x;
        yield break;
    }
}