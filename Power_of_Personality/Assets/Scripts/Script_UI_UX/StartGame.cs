using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Button startButton;
    public TMP_Text classText;
    public GameObject dangerWindow;

    void Start()
    {
        dangerWindow.transform.localScale = new Vector3(0, 0, 0);
        if (startButton != null)
        {
            startButton.onClick.AddListener(GameStart);
        }
    }

    private void GameStart()
    {
        if(classText != null && classText.text != null)
        {
            SceneManager.LoadScene("4-1");
        }
        else
        {
            dangerWindow.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
