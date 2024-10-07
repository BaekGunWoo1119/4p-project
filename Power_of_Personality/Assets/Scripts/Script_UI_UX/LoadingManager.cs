using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{
    private string nextScene;
    public Slider loadingBar;
    public AsyncOperation op;

    // Start is called before the first frame update
    void Start()
    {
        nextScene = PlayerPrefs.GetString("NextScene_Name");
        op = SceneManager.LoadSceneAsync(nextScene);
        StartCoroutine(LoadScene());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator LoadScene()
    {
        yield return null;
        op.allowSceneActivation = false;
        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                loadingBar.value = Mathf.Lerp(loadingBar.value , op.progress, timer);
                if (loadingBar.value >= op.progress)
                {
                    timer = 0f;
                }
            }
            else
            {
                if(timer == 0)
                    timer = 0.5f;
                    
                loadingBar.value = Mathf.Lerp(loadingBar.value, 1f, timer);
                if (loadingBar.value == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        //무한로딩 해결을 위해 추가
        if(op.isDone)
        {
            loadingBar.value = 1f;
            SceneManager.LoadScene(nextScene);
        }
    }
}
