using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassRecommend : MonoBehaviour
{
    private TMP_Text RogueText;
    private TMP_Text WarriorText;
    private TMP_Text ArcherText;
    private TMP_Text WizardText;

    void Start()
    {
        RogueText = GameObject.Find("RogueRecommend").GetComponent<TMP_Text>();
        WarriorText = GameObject.Find("WarriorRecommend").GetComponent<TMP_Text>();
        ArcherText = GameObject.Find("ArcherRecommend").GetComponent<TMP_Text>();
        WizardText = GameObject.Find("WizardRecommend").GetComponent<TMP_Text>();
        RogueText.enabled = false;
        WarriorText.enabled = false;
        ArcherText.enabled = false;
        WizardText.enabled = false;
        string RecommendedClass = PlayerPrefs.GetString("RecommendedClass");

        if (RecommendedClass != null)
        {
            ChangeTextColor(RecommendedClass);
        }
    }

    void ChangeTextColor(string RecommendedClass)
    {
        //MBTI�� ���� ��õ ǥ��
        switch (RecommendedClass)
        {
            case "Rogue":
                RogueText.enabled = true;
                break;
            case "Warrior":
                WarriorText.enabled = true;
                break;
            case "Archer":
                ArcherText.enabled = true;
                break;
            case "Wizard":
                WizardText.enabled = true;
                break;
            default:
                break;
        }
    }
}