using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class MultiPlayStart : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Transform SpawnPoint = GameObject.Find("SpawnPoint").GetComponent<Transform>();
        PhotonNetwork.Instantiate("Server_Player_warrior",SpawnPoint.position,SpawnPoint.rotation,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
