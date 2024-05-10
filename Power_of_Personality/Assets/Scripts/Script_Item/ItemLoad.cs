using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoads : MonoBehaviour
{

    // JSON 데이터를 저장할 클래스 정의
    [System.Serializable]
    public class ItemList
    {
        public List<Item> Items;
    }

    //아이템
    [System.Serializable]
    public class Item
    {
        public string Name;
        public string Set; //세트
        public string Grade; // 등급
        public string Description; //설명
        public string ItemCode; //아이템 코드 이미지 불러올 때 사용
        public float FixedAD; //공격력 고정
        public float PercentAD; //공격력 비율
        public float FixedArmor; //방어력 고정
        public float PercentArmor; //방어력 비율
        public float FixedSpeed; //행동속도 고정
        public float PercentSpeed; //행동속도 비율
        public float FixedADC; //평타 데미지 고정
        public float PercentADC; // 평타 데미지 비율
        public float FixedAP; // 스킬 데미지 고정
        public float PercentAP; // 스킬 데미지 비율
        public float FixedCooltime; // 스킬 고정 쿨감
        public float PercentCooltime; // 스킬 퍼센트 쿨감
        public float FixedFire; // 화속성 데미지 고정
        public float PercentFire; // 화속성 데미지 비율
        public float FixedIce; // 빙속성 데미지 고정
        public float PercentIce; // 빙속성 데미지 비율

    }


	
    void Start()
    {
		string jsondata = Resources.Load<TextAsset>("JSON/Itemdata").text;
		// JSON 데이터를 MapCases 클래스로 Deserialize
		ItemList JSONItemList = JsonUtility.FromJson<ItemList>(jsondata);
        ItemUpdate(JSONItemList);
    }
    

    void ItemUpdate(ItemList JSONItemList){
        foreach (var item in JSONItemList.Items)
        {
            Debug.Log(item.Name + ", " + item.Set + ", " + item.Grade + ", " + item.Description);
        }
    }
}
