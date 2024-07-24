using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData : MonoBehaviour
{

    // JSON 데이터를 저장할 클래스 정의
    [System.Serializable]
    public class WaveDatas
    {
        public List<Wave> WaveData;
    }

    [System.Serializable]
    public class Wave
    {
        public int Quantity;  //몬스터 수
        public string Monster1; //등장 몬스터 1
        public string Monster2; //등장 몬스터 2

    }


	
    void Start()
    {
		string jsondata = Resources.Load<TextAsset>("JSON/WaveData").text;
		// JSON 데이터를 MapCases 클래스로 Deserialize
		WaveDatas JSONWaveList = JsonUtility.FromJson<WaveDatas>(jsondata);
        ItemUpdate(JSONWaveList);
    }
    

    void ItemUpdate(WaveDatas JSONWaveList){
        foreach (var wave in JSONWaveList.WaveData)
        {
            Debug.Log(wave.Quantity + ", " + wave.Monster1 + ", " + wave.Monster2);
        }
    }
}
