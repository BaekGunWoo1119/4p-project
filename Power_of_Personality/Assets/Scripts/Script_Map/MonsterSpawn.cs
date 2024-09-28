using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{

	[System.Serializable]
    public class SpawnCases
    {
        public List<SpawnData> SpawnDatas;
    }

    // JSON 데이터를 저장할 클래스 정의
    [System.Serializable]
    public class SpawnData
    {
        public List<PlatformData> SpawnCase;
    }

    //플랫폼 종류, Vec3 데이터
    [System.Serializable]
    public class PlatformData
    {
        public float x;
    }


    // 선택된 맵 데이터
    private SpawnData selectedSpawnData;
    public GameObject Monster;
    public GameObject Monster_1;
    public GameObject Monster_2;
    public GameObject Monster_3;
    public List<SpawnData> platforms;
	public SpawnCases JSONSpawnCases;
	
    void Start()
    {

		string jsondata = Resources.Load<TextAsset>("JSON/Monsterdata"+PlayerPrefs.GetInt("Spawn")).text;
		// JSON 데이터를 SpawnCases 클래스로 Deserialize
		SpawnCases JSONSpawnCases = JsonUtility.FromJson<SpawnCases>(jsondata);
        // 선택된 스폰 데이터 가져오기
        platforms = JSONSpawnCases.SpawnDatas;

        SpawnMonster(0.5f);
        SpawnMonster(5f);
        SpawnMonster(9.5f);

    }

    void SpawnMonster(float y){
        // 1부터 6까지의 숫자 중에서 무작위로 선택
        int randomSpawnIndex = Random.Range(0, 6);
        selectedSpawnData = platforms[randomSpawnIndex];
        //Debug.Log(platforms.Count);
		//foreach(var SpawnCases in platforms){
        Debug.Log(selectedSpawnData);
        foreach (var MonsterData in selectedSpawnData.SpawnCase)
        {
            int randomSpawnMonster = Random.Range(0, 3);
            switch (randomSpawnMonster){
                case 0:
                    Monster = Monster_1;
                    break;
                case 1:
                    Monster = Monster_2;
                    break;
                case 2:
                    Monster = Monster_3;
                    break;
                default:
                    break;
            }
            // Instantiate를 이용하여 몹 생성
            GameObject monster = Instantiate(Monster);
            monster.transform.parent = this.transform;
            monster.transform.localPosition = new Vector3(MonsterData.x, y, 0);
            monster.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
