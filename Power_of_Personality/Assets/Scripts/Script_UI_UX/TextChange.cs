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
    }
}
