using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public CanvasGroup[] panels = new CanvasGroup[4];

    public void ActivateMainPanel()
    {
        foreach(CanvasGroup elem in panels)
        {
            elem.alpha = 0;
            elem.blocksRaycasts = false;
            elem.interactable = false;
        }
        panels[0].alpha = 1;
        panels[0].blocksRaycasts = true;
        panels[0].interactable = true;
    }
    public void ActivatePlayPanel()
    {
        foreach (CanvasGroup elem in panels)
        {
            elem.alpha = 0;
            elem.blocksRaycasts = false;
            elem.interactable = false;
        }
        panels[1].alpha = 1;
        panels[1].blocksRaycasts = true;
        panels[1].interactable = true;
    }
    public void ActivateOptionsPanel()
    {
        foreach (CanvasGroup elem in panels)
        {
            elem.alpha = 0;
            elem.blocksRaycasts = false;
            elem.interactable = false;
        }
        panels[2].alpha = 1;
        panels[2].blocksRaycasts = true;
        panels[2].interactable = true;
    }
    public void ActivateCreditsPanel()
    {
        foreach (CanvasGroup elem in panels)
        {
            elem.alpha = 0;
            elem.blocksRaycasts = false;
            elem.interactable = false;
        }
        panels[3].alpha = 1;
        panels[3].blocksRaycasts = true;
        panels[3].interactable = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
