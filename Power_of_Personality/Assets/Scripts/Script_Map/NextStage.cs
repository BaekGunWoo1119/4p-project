using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextStage : MonoBehaviour
{
    public GameObject portal; //포탈 오브젝트
    public GameObject portalSpawner; //포탈 소환할 상위 오브젝트
    private Vector3 portalPos; //포탈 위치
    private GameObject portalInstance; //포탈 생성
    public string sceneName; 
    // Start is called before the first frame update
    void Start()
    {
        OpenPortal();
        if(portalInstance != null)
            portalInstance.transform.localPosition = portalPos;
    }

    // Update is called once per frame
    void OpenPortal()
    {
        if(sceneName == "Hidden_Shop" && PlayerPrefs.GetString("Hidden_Shop_Spawn_Scene") == PlayerPrefs.GetString("Before_Scene_Name"))
        {
        }
        else
        {
            portalInstance = Instantiate(portal, portalPos, Quaternion.identity, portalSpawner.transform);
            portalInstance.transform.localRotation = Quaternion.Euler(0, 80, 0);
            PortalCtrl portalCtrl = portalInstance.GetComponent<PortalCtrl>();

            if (portalCtrl != null)
            {
                // 프리팹의 스크립트에 변수를 설정
                portalCtrl.SetSceneName(sceneName);
            }
        }
    }
}
