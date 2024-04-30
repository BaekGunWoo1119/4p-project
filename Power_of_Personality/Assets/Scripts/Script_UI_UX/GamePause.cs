using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePause : MonoBehaviour
{
    public Button pauseButton;
    public Button resumeButton1;
    public Button resumeButton2;
    // Start is called before the first frame update
    void Start()
    {
        pauseButton.onClick.AddListener(PauseGame);
        resumeButton1.onClick.AddListener(ResumeGame);
        resumeButton2.onClick.AddListener(ResumeGame);
    }

    // Update is called once per frame
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }
}
