using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BonusState : MonoBehaviour
{
    public Button[] starButtons; // ?? ??????? ????? ???????????
    public Button plusButton; // + ????? ???????????
    public Button minusButton; // - ????? ???????????
    public Sprite filledStarSprite; // ????? ?? ??????? ???????????
    public Sprite emptyStarSprite; // ?????? ?? ??????? ???????????
    private int bonusstat;

    private int initialStat = 10;
    private int selectedStarIndex = -1; // ????? ???? ??????
    private int totalStars = 4; // ???? ?? ???? (?????? 5?? ????)
    private TextMeshProUGUI StatText;

    void Start()
    {
        if(PlayerPrefs.GetInt("IsFirst") == 0){
            PlayerPrefs.SetInt("BonusStat", 0);
            PlayerPrefs.SetInt("IsFirst", 1);
        }
        bonusstat = PlayerPrefs.GetInt("BonusStat");
        PlayerPrefs.SetInt("RemainStat", bonusstat);
        StatText = GameObject.Find("StatPoint").GetComponent<TextMeshProUGUI>();
        EnableStarRating();
        EnablePlusMinusButtons();
        UpdateStatText();

        PlayerPrefs.SetInt("Drop", 0);
        PlayerPrefs.SetInt("Shop", 0);
        PlayerPrefs.SetInt("Spawn",0);
        PlayerPrefs.SetInt("Stat", 0);
    }
    private void UpdateStatText()
    {
        StatText.text = "???? ????? ???? = " + PlayerPrefs.GetInt("RemainStat", bonusstat);
    }
    

    // ???? ????? ????
    private void EnableStarRating()
    {
        // ?? ????? ???? ???? ?????? ???
        for (int i = 0; i < starButtons.Length; i++)
        {
            int starIndex = i; // ????? ?????? ?????? ????
            starButtons[i].onClick.AddListener(() => OnStarButtonClick(starIndex));
        }
    }

    // +, - ??? ????
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

    // ???? ????? ??????
    private void DisableStarRating()
    {
        // ?? ????? ???? ???? ?????? ????
        for (int i = 0; i < starButtons.Length; i++)
        {
            starButtons[i].onClick.RemoveAllListeners();
        }
    }

    // +, - ??? ??????
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

    // ?? ??? ??? ?? ????? ???
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

        // + ??? ??? ?? ????? ???
    private void IncrementStarRating()
    {
        if (selectedStarIndex < totalStars - 1 && PlayerPrefs.GetInt("RemainStat") > 0)
        {
            selectedStarIndex++;
            UpdateStarImages();
            PlayerPrefs.SetInt("RemainStat", PlayerPrefs.GetInt("RemainStat") - 1);
            UpdateStatText();
            string stattype = this.transform.parent.name;
            PlayerPrefs.SetInt(stattype, selectedStarIndex+1);
            Debug.Log(PlayerPrefs.GetInt(stattype));
        }
    }

    // - ??? ??? ?? ????? ???
    private void DecrementStarRating()
    {
        if (selectedStarIndex > -1 && PlayerPrefs.GetInt("RemainStat") < 10)
        {
            selectedStarIndex--;
            UpdateStarImages();
            PlayerPrefs.SetInt("RemainStat", PlayerPrefs.GetInt("RemainStat") + 1);
            UpdateStatText();
            string stattype = this.transform.parent.name;
            PlayerPrefs.SetInt(stattype, selectedStarIndex+1);
            Debug.Log(PlayerPrefs.GetInt(stattype));
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