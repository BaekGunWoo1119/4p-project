// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MapGen1 : MonoBehaviour
// {
//     // JSON 데이터를 저장할 클래스 정의
//     [System.Serializable]
//     public class MapData
//     {
//         public List<PlatformData> MapCase;
//     }

//     //플랫폼 종류, Vec3 데이터
//     [System.Serializable]
//     public class PlatformData
//     {
//         public string platform;
//         public float x;
//         public float y;
//         public float z;
//     }

//     // JSON 형식의 맵 데이터
//     private string mapJson = @"
//     {""MapCase"":[
// 		{
// 			""platform"": ""Long"",
// 			""x"": 0,
// 			""y"": 4.5,
// 			""z"": 0
// 		},
// 		{
// 			""platform"": ""Mid"",
// 			""x"": 11,
// 			""y"": 9,
// 			""z"": 0
// 		},
// 		{
// 			""platform"": ""Mid"",
// 			""x"": -11,
// 			""y"": 9,
// 			""z"": 0
// 		}
// 	]
// 	}
// ";

//     // 선택된 맵 데이터
//     private MapData selectedMapData;
//     public GameObject PlatformMid;
//     public GameObject PlatformShort;
//     public GameObject PlatformLong;
//     public GameObject PlatformCase;
// 	//public var loadedJson = Resources.Load<TextAsset>("JSON/Mapdata");
	
//     // 시작 시 실행되는 함수
//     void Start()
//     {
		
//         // JSON 데이터를 MapData 클래스로 Deserialize
//         selectedMapData = JsonUtility.FromJson<MapData>(mapJson);

//         // 1부터 5까지의 숫자 중에서 무작위로 선택
//         int randomMapIndex = Random.Range(1, 6);

//         // 선택된 맵 데이터 가져오기
//         List<PlatformData> platforms = selectedMapData.MapCase;

//         Debug.Log(platforms);
//         foreach (var platformData in platforms)
//         {
//             switch (platformData.platform){
//                 case "Mid":
//                     PlatformCase = PlatformMid;
//                     break;
//                 case "Long":
//                     PlatformCase = PlatformLong;
//                     break;
//                 case "Short":
//                     PlatformCase = PlatformShort;
//                     break;
//                 default:
//                     break;
//             }
//             // Instantiate를 이용하여 플랫폼 생성
//             GameObject platform = Instantiate(PlatformCase);
// 			platform.transform.parent = this.transform;
//             platform.transform.localPosition = new Vector3(platformData.x, platformData.y, platformData.z);
// 			platform.transform.localRotation = Quaternion.Euler(-90, 0, 90);
//         }
        
//     }
// }
