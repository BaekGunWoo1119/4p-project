using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalCtrl : MonoBehaviour
{
    public string sceneName;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerPrefs.SetString("NextScene_Name", sceneName);
            PlayerPrefs.SetInt("clearTime", (int)GameEnd.PlayTime);
            SceneManager.LoadScene("LoadingScene"); //포탈을 타면 로딩씬으로 넘어간 후 넘어감(09.23)
        }
    }

    public void SetSceneName(string newSceneName)
    {
        sceneName = newSceneName;
    }
}
