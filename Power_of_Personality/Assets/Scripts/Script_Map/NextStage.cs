using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public GameObject portal; //포탈 오브젝트
    public Vector3 portalPos = new Vector3(194f, 12.5f, 0f); // 포탈 생성 위치(추후 맵마다 변경해 둘 예정)
    public string sceneName; 
    // Start is called before the first frame update
    void Start()
    {
        //portalPos = new Vector3(194, 12.5, 0) // 추후 맵마다 변경해 둘 예정
        OpenPortal();
    }

    // Update is called once per frame
    void OpenPortal()
    {
        GameObject portalInstance = Instantiate(portal, portalPos, Quaternion.Euler(0, 80, 0));
        PortalCtrl portalCtrl = portalInstance.GetComponent<PortalCtrl>();

        if (portalCtrl != null)
        {
            // 프리팹의 스크립트에 변수를 설정
            portalCtrl.SetSceneName(sceneName);
        }
    }
}
