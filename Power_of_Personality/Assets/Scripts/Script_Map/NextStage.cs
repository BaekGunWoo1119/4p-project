using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public GameObject portal; //포탈 오브젝트
    public Vector3 portalPos = new Vector3(194f, 12.5f, 0f); // 포탈 생성 위치(추후 맵마다 변경해 둘 예정)
    // Start is called before the first frame update
    void Start()
    {
        //portalPos = new Vector3(194, 12.5, 0) // 추후 맵마다 변경해 둘 예정
        OpenPortal();
    }

    // Update is called once per frame
    void OpenPortal()
    {
        Instantiate(portal, portalPos, Quaternion.Euler(0, 80, 0));
    }
}
