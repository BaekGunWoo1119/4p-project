using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusState : MonoBehaviour
{
    public Button[] starButtons; // �� ��ư���� �迭�� �������ּ���
    public Button plusButton; // + ��ư�� �������ּ���
    public Button minusButton; // - ��ư�� �������ּ���
    public Sprite filledStarSprite; // ä���� �� �̹����� �������ּ���
    public Sprite emptyStarSprite; // ����ִ� �� �̹����� �������ּ���

    private int initialStat = 10;
    private int selectedStarIndex = -1; // ���õ� ���� �ε���
    private int totalStars = 4; // ���� �� ���� (���÷� 5�� ����)
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
        StatText.text = "���� ���ʽ� ���� = " + PlayerPrefs.GetInt("RemainStat", initialStat);
    }
    

    // ���� �ý��� Ȱ��ȭ
    private void EnableStarRating()
    {
        // �� ��ư�鿡 ���� �̺�Ʈ ������ �߰�
        for (int i = 0; i < starButtons.Length; i++)
        {
            int starIndex = i; // Ŭ���� ������ �ε��� ����
            starButtons[i].onClick.AddListener(() => OnStarButtonClick(starIndex));
        }
    }

    // +, - ��ư Ȱ��ȭ
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

    // ���� �ý��� ��Ȱ��ȭ
    private void DisableStarRating()
    {
        // �� ��ư�鿡 ���� �̺�Ʈ ������ ����
        for (int i = 0; i < starButtons.Length; i++)
        {
            starButtons[i].onClick.RemoveAllListeners();
        }
    }

    // +, - ��ư ��Ȱ��ȭ
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

    // �� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
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

        // + ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
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

    // - ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
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