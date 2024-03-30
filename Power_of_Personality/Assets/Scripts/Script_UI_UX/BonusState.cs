using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusState : MonoBehaviour
{
    public Button[] starButtons; // 별 버튼들을 배열로 지정해주세요
    public Button plusButton; // + 버튼을 지정해주세요
    public Button minusButton; // - 버튼을 지정해주세요
    public Sprite filledStarSprite; // 채워진 별 이미지를 지정해주세요
    public Sprite emptyStarSprite; // 비어있는 별 이미지를 지정해주세요

    private int initialStat = 10;
    private int selectedStarIndex = -1; // 선택된 별의 인덱스
    private int totalStars = 4; // 별의 총 개수 (예시로 5로 설정)
    private TextMeshProUGUI StatText;

    void Start()
    {
        PlayerPrefs.SetInt("RemainStat", initialStat);
        StatText = GameObject.Find("StatPoint").GetComponent<TextMeshProUGUI>();
        EnableStarRating();
        EnablePlusMinusButtons();
        UpdateStatText();
    }
    private void UpdateStatText()
    {
        StatText.text = "남은 보너스 스텟 = " + PlayerPrefs.GetInt("RemainStat", initialStat);
    }
    

    // 별점 시스템 활성화
    private void EnableStarRating()
    {
        // 별 버튼들에 대한 이벤트 리스너 추가
        for (int i = 0; i < starButtons.Length; i++)
        {
            int starIndex = i; // 클로저 변수로 인덱스 보존
            starButtons[i].onClick.AddListener(() => OnStarButtonClick(starIndex));
        }
    }

    // +, - 버튼 활성화
    private void EnablePlusMinusButtons()
    {
        if (plusButton != null)
        {
            plusButton.onClick.AddListener(IncrementStarRating);
        }

        if (minusButton != null)
        {
            minusButton.onClick.AddListener(DecrementStarRating);
        }
    }

    // 별점 시스템 비활성화
    private void DisableStarRating()
    {
        // 별 버튼들에 대한 이벤트 리스너 삭제
        for (int i = 0; i < starButtons.Length; i++)
        {
            starButtons[i].onClick.RemoveAllListeners();
        }
    }

    // +, - 버튼 비활성화
    private void DisablePlusMinusButtons()
    {
        if (plusButton != null)
        {
            plusButton.onClick.RemoveAllListeners();
        }

        if (minusButton != null)
        {
            minusButton.onClick.RemoveAllListeners();
        }
    }

    // 별 버튼 클릭 시 호출되는 함수
    private void OnStarButtonClick(int clickedStarIndex)
    {
        if (selectedStarIndex == clickedStarIndex)
            return;

        selectedStarIndex = clickedStarIndex;

        for (int i = 0; i <= clickedStarIndex; i++)
        {
            Image starImage = starButtons[i].GetComponent<Image>();
            if (starImage != null)
            {
                starImage.sprite = filledStarSprite;
            }
        }

        for (int i = clickedStarIndex + 1; i < starButtons.Length; i++)
        {
            Image starImage = starButtons[i].GetComponent<Image>();
            if (starImage != null)
            {
                starImage.sprite = emptyStarSprite;
            }
        }
    }

        // + 버튼 클릭 시 호출되는 함수
    private void IncrementStarRating()
    {
        if (selectedStarIndex < totalStars - 1 && PlayerPrefs.GetInt("RemainStat") > 0)
        {
            selectedStarIndex++;
            UpdateStarImages();
            PlayerPrefs.SetInt("RemainStat", PlayerPrefs.GetInt("RemainStat") - 1);
            UpdateStatText();
        }
    }

    // - 버튼 클릭 시 호출되는 함수
    private void DecrementStarRating()
    {
        if (selectedStarIndex > -1 && PlayerPrefs.GetInt("RemainStat") < 10)
        {
            selectedStarIndex--;
            UpdateStarImages();
            PlayerPrefs.SetInt("RemainStat", PlayerPrefs.GetInt("RemainStat") + 1);
            UpdateStatText();
        }
    }

    private void UpdateStarImages()
    {
        for (int i = 0; i < starButtons.Length; i++)
        {
            Image starImage = starButtons[i].GetComponent<Image>();
            if (starImage != null)
            {
                if (i <= selectedStarIndex)
                {
                    starImage.sprite = filledStarSprite;
                }
                else
                {
                    starImage.sprite = emptyStarSprite;
                }
            }
        }
    }
}