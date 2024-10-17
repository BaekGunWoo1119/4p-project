using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.ComponentModel;

public class TabBtn : MonoBehaviour
{
    public Button button; // ????
    public GameObject innerImg;
    public Color fireColor; 
    public Color iceColor;
    public Sprite fireImage;
    public Sprite iceImage;
    //��ų �̹����� �޾ƿ��� ���� �迭
    public Sprite[] skillImage;

    public GameObject fireHit;
    public GameObject iceHit;

    private bool isIce;
    private bool isFire;

    void Start()
    {
        PlayerPrefs.SetString("property", "Ice");
        isIce = true;
    }

    private void SetFire() 
    {
        if (button != null)
        {
            //Debug.Log("??? ????");
            ColorBlock colors = button.colors;
            colors.normalColor = fireColor;
            button.colors = colors;
            Image btnImage = innerImg.GetComponent<Image>();
            btnImage.sprite = fireImage;
            //��ų �̹��� ����
            Image skillImageQ = GameObject.Find("SkillImg-Q").GetComponent<Image>();
            skillImageQ.sprite = skillImage[0];
            Image skillImageW = GameObject.Find("SkillImg-W").GetComponent<Image>();
            skillImageW.sprite = skillImage[1];
            Image skillImageE = GameObject.Find("SkillImg-E").GetComponent<Image>();
            skillImageE.sprite = skillImage[2];
            button.onClick.RemoveListener(SetFire);
            PlayerPrefs.SetString("property", "Fire");
            isFire = true;
        }
    }

    private void SetIce()
    {
        if (button != null)
        {
            //Debug.Log("???????? ????");
            ColorBlock colors = button.colors;
            colors.normalColor = iceColor;
            button.colors = colors;
            Image btnImage = innerImg.GetComponent<Image>();
            btnImage.sprite = iceImage;
            //��ų �̹��� ����
            Image skillImageQ = GameObject.Find("SkillImg-Q").GetComponent<Image>();
            skillImageQ.sprite = skillImage[3];
            Image skillImageW = GameObject.Find("SkillImg-W").GetComponent<Image>();
            skillImageW.sprite = skillImage[4];
            Image skillImageE = GameObject.Find("SkillImg-E").GetComponent<Image>();
            skillImageE.sprite = skillImage[5];
            button.onClick.RemoveListener(SetIce);
            PlayerPrefs.SetString("property", "Ice");
            isIce = true;
        }
    }
    void Update()
    {
        ColorBlock colors = button.colors;
        Color normalColor = colors.normalColor;
        if (button != null)
        {
            if (normalColor == iceColor)
            {
                button.onClick.AddListener(SetFire);
            }
            else if (normalColor == fireColor)
            {
                button.onClick.AddListener(SetIce);
            }
            else
            {
                normalColor = iceColor;
                button.onClick.AddListener(SetFire);
            }
        }

        if (Input.GetKeyDown(KeyCode.Tab) && button != null)
        {
            button.onClick.Invoke();
        }

        if(PlayerPrefs.GetString("property") == "Ice" && isIce == true)
        {
            isIce = false;
            StartCoroutine(Effect("Ice"));
        }
        else if(PlayerPrefs.GetString("property") == "Fire" && isFire == true)
        {
            isFire = false;
            StartCoroutine(Effect("Fire"));
        }
    }

    IEnumerator Effect(string EffectType)
    {
        
        if(EffectType == "Ice")
        {
            isIce = false;
            fireHit.SetActive(false);
            iceHit.SetActive(true);
            yield return new WaitForSeconds(1f);
            iceHit.SetActive(false);
        }
        else if(EffectType == "Fire")
        {
            isFire = false;
            iceHit.SetActive(false);
            fireHit.SetActive(true);
            yield return new WaitForSeconds(2f);
            fireHit.SetActive(false);
        }
    }
}
