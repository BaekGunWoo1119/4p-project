using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using System.IO;
using UnityEngine;

public class SaveAndLoad : MonoBehaviour
{
    public Inventory Inv;
    public Item[] items;

    public void Start()
    {
        Inv = GetComponent<Inventory>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            Save();
        }
        else if (Input.GetKeyDown(KeyCode.K))
        {
            Load();
        }
    }
    void Save()
    {
        List<ItemLoad> itemsToLoad = new List<ItemLoad>();
        for(int i = 0; i < Inv.slots.Length; i++)
        {
            Slot z = Inv.slots[i];
            if (z.slotsItem)
            {
                ItemLoad h = new ItemLoad(z.slotsItem.itemID, z.slotsItem.SlotIndex, z.slotsItem.Name, z.slotsItem.Set, z.slotsItem.Grade, z.slotsItem.Description, z.slotsItem.FixedAD,
                    z.slotsItem.PercentAD, z.slotsItem.FixedArmor, z.slotsItem.PercentArmor, z.slotsItem.FixedSpeed, z.slotsItem.PercentSpeed, z.slotsItem.FixedADC, z.slotsItem.PercentADC,
                    z.slotsItem.FixedAP, z.slotsItem.PercentAP, z.slotsItem.FixedCooltime, z.slotsItem.PercentCooltime, z.slotsItem.FixedFire, z.slotsItem.PercentFire, z.slotsItem.FixedIce, z.slotsItem.PercentIce);
                itemsToLoad.Add(h);
            }
        }

        string json = CustomJSON.ToJson(itemsToLoad);
        Debug.Log(json);
        File.WriteAllText(Application.persistentDataPath + transform.name, json);
        Debug.Log("저장 중...");
    }

    void Load()
    {
        Debug.Log("로딩 중...");
        List<ItemLoad> itemsToLoad = CustomJSON.FromJson<ItemLoad>(File.ReadAllText(Application.persistentDataPath + transform.name));


    
        for(int i = itemsToLoad[0].slotIndex; i < Inv.slots.Length; i++)
        {
            foreach (ItemLoad z in itemsToLoad)
            {
                if (i == z.slotIndex)
                {
                    
                    Debug.Log(Inv.slots[i].transform);

                    Item b = Instantiate(items[z.Id], Inv.slots[i].transform).GetComponent<Item>();
                    b.itemID =z.Id;
                    b.SlotIndex = z.slotIndex;
                    b.Name = z.Name;
                    b.Set = z.Set;
                    b.Grade = z.Grade;
                    b.Description = z.Description;
                    b.FixedAD = z.FixedAD;
                    b.PercentAD = z.PercentAD;
                    b.FixedArmor = z.FixedArmor;
                    b.PercentArmor = z.PercentArmor;
                    b.FixedSpeed = z.FixedSpeed;
                    b.PercentSpeed = z.PercentSpeed;
                    b.FixedADC = z.FixedADC;
                    b.PercentADC = z.PercentADC;
                    b.FixedAP = z.FixedAP;
                    b.PercentAP = z.PercentAP;
                    b.FixedCooltime = z.FixedCooltime;
                    b.PercentCooltime = z.PercentCooltime;
                    b.FixedFire = z.FixedFire;
                    b.PercentFire = z.PercentFire;
                    b.FixedIce = z.FixedIce;
                    b.PercentIce = z.PercentIce;
                    break;
                }
            }
        }
    }
}


[System.Serializable]
public class ItemLoad
{
    public int Id;
    public int slotIndex;
    public string Name;
    public string Set; //세트
    public string Grade; // 등급
    public string Description; //설명
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

    public ItemLoad(int ID, int SlotIndex, string NAME, string SET, string GRADE, string DESCRIPTION, float FIXEDAD, float PERCENTAD, float FIXEDARMOR, float PERCENTARMOR, float FIXEDSPEED, 
        float PERCENTSPEED, float FIXEDADC, float PERCENTADC, float FIXEDAP, float PERCENTAP, float FIXEDCOOLTIME, float PERCENTCOOLTIME, float FIXEDFIRE, float PERCENTFIRE, float FIXEDICE, float PERCENTICE)
    {
        Id = ID;
        slotIndex = SlotIndex;
        Name = NAME;
        Set = SET;
        Grade = GRADE;
        Description = DESCRIPTION;
        FixedAD = FIXEDAD;
        PercentAD = PERCENTAD;
        FixedArmor = FIXEDARMOR;
        PercentArmor = PERCENTARMOR;
        FixedSpeed = FIXEDSPEED;
        PercentSpeed = PERCENTSPEED;
        FixedADC = FIXEDADC;
        PercentADC = PERCENTADC;
        FixedAP = FIXEDAP;
        PercentAP = PERCENTAP;
        FixedCooltime = FIXEDCOOLTIME;
        PercentCooltime = PERCENTCOOLTIME;
        FixedFire = FIXEDFIRE;
        PercentFire = PERCENTFIRE;
        FixedIce = FIXEDICE;
        PercentIce = PERCENTICE;
    }
}
