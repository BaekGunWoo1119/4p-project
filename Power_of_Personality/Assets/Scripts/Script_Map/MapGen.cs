using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{

	[System.Serializable]
    public class MapCases
    {
        public List<MapData> MapDatas;
    }

    // JSON 데이터를 저장할 클래스 정의
    [System.Serializable]
    public class MapData
    {
        public List<PlatformData> MapCase;
    }

    //플랫폼 종류, Vec3 데이터
    [System.Serializable]
    public class PlatformData
    {
        public string platform;
        public float x;
        public float y;
        public float z;
    }


    // 선택된 맵 데이터
    private MapData selectedMapData;
    public GameObject[] PlatformMid;
    public GameObject[] PlatformShort;
    public GameObject[] PlatformLong;
    public GameObject PlatformCase;

	public MapCases JSONMapCases;
	
    void Start()
    {
		string jsondata = Resources.Load<TextAsset>("JSON/Mapdata").text;
		// JSON 데이터를 MapCases 클래스로 Deserialize
		MapCases JSONMapCases = JsonUtility.FromJson<MapCases>(jsondata);


        // 1부터 5까지의 숫자 중에서 무작위로 선택
        int randomMapIndex = Random.Range(0, 5);

        // 선택된 맵 데이터 가져오기
        List<MapData> platforms = JSONMapCases.MapDatas;
		selectedMapData = platforms[randomMapIndex];
        //Debug.Log(platforms);
		//foreach(var MapCases in platforms){
			//Debug.Log(MapCases);
			foreach (var platformData in selectedMapData.MapCase)
			{
                int randomInt = Random.Range(1, 6);
				switch (platformData.platform){
					case "Mid":
						PlatformCase = PlatformMid[randomInt-1];
						break;
					case "Long":
						PlatformCase = PlatformLong[randomInt-1];
						break;
					case "Short":
						PlatformCase = PlatformShort[randomInt-1];
						break;
					default:
						break;
				}
				// Instantiate를 이용하여 플랫폼 생성
				GameObject platform = Instantiate(PlatformCase);
				platform.transform.parent = this.transform;
				platform.transform.localPosition = new Vector3(platformData.x, platformData.y, platformData.z);
				platform.transform.localRotation = Quaternion.Euler(-90, 0, 90);
			}

    }
}
