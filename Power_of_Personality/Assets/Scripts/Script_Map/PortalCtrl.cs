using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCtrl : MonoBehaviour
{
    public string sceneName;
    void OnCollisionEnter(Collision collision)
    {
        if(sceneName == "Hidden_Shop")
        {
        
            //현재 플레이어 위치 및 현재 스테이지 저장 후 씬 넘기기
            PlayerPrefs.SetFloat("PlayerXPos", 0f);
            PlayerPrefs.SetFloat("PlayerYPos", 0f);
            PlayerPrefs.SetFloat("PlayerZPos", 0f);

            PlayerPrefs.SetString("Before_Scene_Name", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetString("Hidden_Shop_Spawn_Scene", SceneManager.GetActiveScene().name);
        }

        else if(sceneName == "Normal_Shop")
        {
            PlayerPrefs.SetString("Before_Scene_Name", SceneManager.GetActiveScene().name);
        }

        SceneManager.LoadScene(sceneName);
    }

    public void SetSceneName(string newSceneName)
    {
        sceneName = newSceneName;
    }
}
