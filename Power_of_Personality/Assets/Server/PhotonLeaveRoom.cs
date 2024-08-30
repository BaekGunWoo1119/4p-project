using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Security.Cryptography;
using System.Linq;
using System.Text;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class PhotonLeaveRoom : MonoBehaviourPunCallbacks
{
    public override void OnLeftRoom(){
        SceneManager.LoadScene("1-1-1 (Multi Room Choice)");
    }

    public static void Leave(){
        PhotonNetwork.LeaveRoom();
    }
}
