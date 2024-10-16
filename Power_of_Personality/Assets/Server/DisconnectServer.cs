using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리

public class DisconnectServer : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        // 마스터 서버에 접속중이라면

    PhotonNetwork.Disconnect();
    Time.timeScale = 1;


    }

}
