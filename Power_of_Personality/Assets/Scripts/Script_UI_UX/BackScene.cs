using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackScene : MonoBehaviour
{
    string previousSceneName; // 이전 Scene의 이름을 저장

    public Button backButton;
    void Start()
    {
        // 이전 Scene의 이름을 저장
        previousSceneName = PlayerPrefs.GetString("PreviousScene", "DefaultSceneName");

        // 현재 Scene의 이름을 저장 (이전 Scene으로 돌아갈 때 사용)
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBackToPreviousScene);
        }
    }

    public void GoBackToPreviousScene()
    {
        if (!string.IsNullOrEmpty(previousSceneName))
        {
            SceneManager.LoadScene(previousSceneName);
        }
        else
        {
            Debug.LogWarning("PreviousSceneName is not found or invalid!");
        }
    }
}