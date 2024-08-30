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

public class PhotonCallBack : MonoBehaviourPunCallbacks
{
    public Hashtable PlayerProperties;

    private Player[] PlayersList;

    // 마스터 서버 접속 성공시 자동 실행
    public override void OnConnectedToMaster() {
        SceneManager.LoadScene("1-1-1 (Multi Room Choice)");
        // 룸 접속 버튼을 활성화
        //joinButton.interactable = true;
        // 접속 정보 표시
        //connectionInfoText.text = "온라인 : 마스터 서버와 연결됨";
        //Debug.Log("온라인 : 마스터 서버와 연결됨");
    }

    // 마스터 서버 접속 실패시 자동 실행
    public override void OnDisconnected(DisconnectCause cause) {
        // 룸 접속 버튼을 비활성화
        //joinButton.interactable = false;
        // 접속 정보 표시
        //connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
        SceneManager.LoadScene("1 (Main)");
        // 마스터 서버로의 재접속 시도
        //PhotonNetwork.ConnectUsingSettings();
    }
        // // (빈 방이 없어)랜덤 룸 참가에 실패한 경우 자동 실행
    // public override void OnJoinRandomFailed(short returnCode, string message) {
    //     // 접속 상태 표시
    //     connectionInfoText.text = "빈 방이 없음, 새로운 방 생성... 로비코드: "+InputText.text;
    //     // 최대 4명을 수용 가능한 빈방을 생성
    //     PhotonNetwork.CreateRoom(InputText.text, new RoomOptions {MaxPlayers = 4});
    // }

    // 룸에 참가 완료된 경우 자동 실행
    public override void OnJoinedRoom() {
        // 접속 상태 표시
        //connectionInfoText.text = "방 참가 성공";
        // 모든 룸 참가자들이 Main 씬을 로드하게 함
        //PhotonNetwork.LoadLevel("Main");
        //플레이어가 1명이 아니면
        // if(PhotonNetwork.PlayerList.Length!=1){
        //         PlayerPropertiesAdd2();
        //     }
        // else{
        //     PlayerPropertiesAdd();
        // }
        LobbyManager.PlayerPropertiesAdd();
        PlayerProperties = LobbyManager.PlayerProperties;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties); //클래스 프로퍼티 설정
        //connectionInfoText.text = LobbyCode;
        //Debug.Log(PlayerProperties["PlayerClass"]);
        //현재 접속한 플레이어 리스트
        PlayersList = PhotonNetwork.PlayerList;
        SceneManager.LoadScene("1-2 (Multi Lobby)");
        Debug.Log(PhotonNetwork.LocalPlayer.ActorNumber);

    }

    // public override void OnCreatedRoom(){
    //     LobbyManager.PlayerPropertiesAdd();
    //     PlayerProperties = LobbyManager.PlayerProperties;
    //     PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties); //클래스 프로퍼티 설정
    //     PlayersList = PhotonNetwork.PlayerList;
    //     SceneManager.LoadScene("1-2 (Multi Lobby)");
    // }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        //connectionInfoText.text = "방 참가 실패";
        //Invoke("RoomCreate",2.0f); //지연시간 추가용 함수
        SceneManager.LoadScene("1-1-1 (Multi Room Choice)");
        Debug.Log("방이 존재하지 않음");
        
    }


    public override void OnLeftRoom(){
        SceneManager.LoadScene("1-1-1 (Multi Room Choice)");
    }
}
