using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyDownCtrl : MonoBehaviour
{
    public GameObject activeObj; // 활성화할 오브젝트
    public KeyCode ctrlKey;

    void Update()
    {
        if (Input.GetKeyDown(ctrlKey))
        {
            if (ctrlKey == KeyCode.Escape)
            {
                // Escape 키가 눌렸을 때 게임의 상태를 체크하고 일시 정지 혹은 재개
                if (GamePause.IsPaused == true)
                {
                    if(SceneManager.GetActiveScene().name != "Forest_Example_Multi"){
                        GamePause.ResumeGame();
                    }
                    UnSetActiveObj();
                }
                else
                {
                    if(SceneManager.GetActiveScene().name != "Forest_Example_Multi"){
                        GamePause.PauseGame();
                    }
                    SetActiveObj();
                }
            }
            else
            {
                SetActiveObj();
            }
        }
    }

    // 특정 키보드 키 클릭
    private void SetActiveObj()
    {
        if (activeObj != null)
        {
            // 오브젝트의 스케일을 초기 스케일로 변경
            activeObj.transform.localScale = new Vector3(1, 1, 1);
            GamePause.IsPaused = true;
        }
    }

    private void UnSetActiveObj()
    {
        if (activeObj != null)
        {
            // 오브젝트의 스케일을 초기 스케일로 변경
            activeObj.transform.localScale = new Vector3(0, 0, 0);
            GamePause.IsPaused = false;
        }
    }
}
