using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    public Button pauseButton;
    public Button resumeButton;
    public GameObject pauseWindow;
    private Vector3 originalScale;
    // Start is called before the first frame update
    void Start()
    {
        originalScale = new Vector3(1, 1, 1);
        pauseWindow.transform.localScale = new Vector3(0, 0, 0);
        if(pauseButton != null)
            pauseButton.onClick.AddListener(PauseWindowOpen);
        if(resumeButton != null)
        resumeButton.onClick.AddListener(ResumeGame);
    }

    public void PauseWindowOpen()
    {
        pauseWindow.transform.localScale = originalScale;
        GamePause.PauseGame();
    }

    public void PauseWindowClose()
    {
        pauseWindow.transform.localScale = originalScale;
        GamePause.ResumeGame();
    }

    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void MultiPauseWindowOpen()
    {
        pauseWindow.transform.localScale = originalScale;
    }

    public void MultiPauseWindowClose()
    {
        pauseWindow.transform.localScale = originalScale;
    }
}