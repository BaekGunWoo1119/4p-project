using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextChange : MonoBehaviour
{
    public string[] innerText;
    //public string textType;
    public TMP_Text[] changeText;
    
    //범용성 있게 둘 다 public으로 지정해둠. 필요한 곳에 넣어서 사용 가능
    public void Start()
    {
        for(int i = 0; i < innerText.Length; i++)
        {
            innerText[i] = changeText[i].text;
        }
    }

    // 외부에서 텍스트 바꿀 때 이걸로 사용하면 됨
    public void ChangeText(string othertext, int Num)
    {
        changeText[Num].text = othertext;
    }

    public void ChangeTextColor(float red, float green, float blue, int Num)
    {
        changeText[Num].color = new Color(red / 255, green / 255, blue / 255, 1f);
    }

    public void ItemTextChange()
    {
        Item itemvalue = transform.parent.GetChild(this.transform.parent.childCount - 1).GetComponent<Item>();
        if(itemvalue != null)
        {
            ChangeText(itemvalue.Name, 0);
            ChangeText(itemvalue.Set, 1);
            ChangeText(itemvalue.Grade, 2);
            ChangeText(itemvalue.Description, 3);
        }
        
        if(this.transform.parent.gameObject.name == "TraitBox-E")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("E", 4);
            ChangeText("공격력", 5);
            ChangeText("고정 공격력", 6);
            ChangeText("공격력 증가 비율", 7);
            ChangeText(Status.TotalAD.ToString(), 8);
            ChangeText(Status.FixedAD.ToString(), 9);
            ChangeText(Status.PercentAD.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-I")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("I", 4);
            ChangeText("방어력", 5);
            ChangeText("고정 방어력", 6);
            ChangeText("방어력 증가 비율", 7);
            ChangeText(Status.TotalArmor.ToString(), 8);
            ChangeText(Status.FixedArmor.ToString(), 9);
            ChangeText(Status.PercentArmor.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-S")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("S", 4);
            ChangeText("공격속도", 5);
            ChangeText("고정 공격속도", 6);
            ChangeText("공격속도 증가 비율", 7);
            ChangeText(Status.TotalADC.ToString(), 8);
            ChangeText(Status.FixedADC.ToString(), 9);
            ChangeText(Status.PercentADC.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-N")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("N", 4);
            ChangeText("스킬 데미지", 5);
            ChangeText("고정 데미지", 6);
            ChangeText("데미지 증가 비율", 7);
            ChangeText(Status.TotalAP.ToString(), 8);
            ChangeText(Status.FixedAP.ToString(), 9);
            ChangeText(Status.PercentAP.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-F")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("F", 4);
            ChangeText("불 데미지", 5);
            ChangeText("고정 데미지", 6);
            ChangeText("데미지 증가 비율", 7);
            ChangeText(Status.TotalAP.ToString(), 8);
            ChangeText(Status.FixedAP.ToString(), 9);
            ChangeText(Status.PercentAP.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-T")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("T", 4);
            ChangeText("얼음 데미지", 5);
            ChangeText("고정 데미지", 6);
            ChangeText("데미지 증가 비율", 7);
            ChangeText(Status.TotalAP.ToString(), 8);
            ChangeText(Status.FixedAP.ToString(), 9);
            ChangeText(Status.PercentAP.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-J")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("J", 4);
            ChangeText("행동속도", 5);
            ChangeText("고정 행동속도", 6);
            ChangeText("행동속도 증가 비율", 7);
            ChangeText(Status.TotalAP.ToString(), 8);
            ChangeText(Status.FixedAP.ToString(), 9);
            ChangeText(Status.PercentAP.ToString(), 10);
        }
        else if(this.transform.parent.gameObject.name == "TraitBox-P")
        {
            ChangeText("", 0);
            ChangeText("", 1);
            ChangeText("", 2);
            ChangeText("", 3);
            ChangeText("P", 4);
            ChangeText("쿨타임", 5);
            ChangeText("고정 쿨타임", 6);
            ChangeText("쿨타임 증가 비율", 7);
            ChangeText(Status.TotalAP.ToString(), 8);
            ChangeText(Status.FixedAP.ToString(), 9);
            ChangeText(Status.PercentAP.ToString(), 10);
        }

        else
        {
            for(int i = 4; i <= 10; i++)
            {
                ChangeText("", i);
            }
        }
    }

    public void ItemTextReset()
    {
        Item itemvalue = transform.parent.GetChild(this.transform.parent.childCount - 1).GetComponent<Item>();
        ChangeText("이름", 0);
        ChangeText("세트", 1);
        ChangeText("그레이드", 2);
        ChangeText("특성 설명", 3);
    }
}
