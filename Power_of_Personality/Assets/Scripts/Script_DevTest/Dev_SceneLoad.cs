using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Dev_SceneLoad : MonoBehaviour
{
    private Button goDev;

    void Start()
    {
        goDev = GameObject.Find("Go Dev").GetComponent<Button>();
        goDev.onClick.AddListener(Dev_Scene);
    }

    void Dev_Scene()
    {
        SceneManager.LoadScene("Forest_Example_Dev");
    }
}
