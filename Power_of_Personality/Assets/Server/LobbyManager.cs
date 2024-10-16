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

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks {
    //public string gameVersion = "1"; // 게임 버전
    //public Text PlayerCount;
    //public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    //public Text InputText; //로비코드 입력
    //public Button joinButton; // 룸 접속 버튼

    #region 플레이어 오브젝트
    public GameObject Player1;
    public GameObject Player1Ready;
    public GameObject Player1Wait;
    public GameObject Player2;
    public GameObject Player2Ready;
    public GameObject Player2Wait;
    public TextMeshProUGUI Player1Class;
    public TextMeshProUGUI Player2Class;
    public TextMeshProUGUI CurrentLobbyCode;
    public GameObject readywarning;
    public static LobbyManager LM;


    #endregion
    public static Hashtable PlayerProperties = new Hashtable(); //로컬플레이어 프로퍼티
    //private Hashtable PP; //로컬플레이어 프로퍼티 변수화

    private Player[] PlayersList; //현재 접속한 플레이어 리스트
    private string LobbyCode;
    private static string TempLobbyCode;

    public void Start(){
        LM = this;
        CurrentLobbyCode.text = TempLobbyCode;

            if(PhotonNetwork.InRoom == true)
            {
                PlayersList = PhotonNetwork.PlayerList;
                Debug.Log(PlayersList.Length);
                ResetSetting();
                Debug.Log((string)PhotonNetwork.PlayerList[0].CustomProperties["PlayerClass"]);
                Player1Class.text = (string)PhotonNetwork.PlayerList[0].CustomProperties["PlayerClass"];
                if(PhotonNetwork.PlayerList.Length>1)
                {
                    Player2Class.text = (string)PhotonNetwork.PlayerList[1].CustomProperties["PlayerClass"];
                    if((bool)PhotonNetwork.PlayerList[0].CustomProperties["IsReady"] == false || (bool)PhotonNetwork.PlayerList[0].CustomProperties["IsReady"] ==null){
                        Player1Ready.SetActive(false);
                        Player1Wait.SetActive(true);
                    }
                    else{
                        Player1Ready.SetActive(true);
                        Player1Wait.SetActive(false);
                    }
                    if((bool)PhotonNetwork.PlayerList[1].CustomProperties["IsReady"] == null || (bool)PhotonNetwork.PlayerList[1].CustomProperties["IsReady"] ==false)
                    {
                        Player2Ready.SetActive(false);
                        Player2Wait.SetActive(true);
                    }
                    else{
                        Player2Ready.SetActive(true);
                        Player2Wait.SetActive(false);
                    }
                }
                else{
                    Player1Ready.SetActive(false);
                    Player1Wait.SetActive(true);
                }
            }
    }
    
    public void Update(){
        //만약 룸에 들어가있다면
        if(PhotonNetwork.InRoom == true){
            //플레이어 프로퍼티 업데이트
            //PP = PhotonNetwork.LocalPlayer.CustomProperties;
            //현재 플레이어 수 체크
            //Debug.Log("현재 접속자 수" + PhotonNetwork.CurrentRoom.PlayerCount); 
            //만약 플레이어가 2명일 경우
            if(PhotonNetwork.PlayerList.Length>1){
                Player2.SetActive(true);
            }
            else{
                Player2.SetActive(false);
                
            }

            //Debug.Log((bool)PhotonNetwork.PlayerList[1].CustomProperties["IsReady"]);
        }
    }

#region 포톤함수
    //마스터 서버 접속 시도
    public static void SeverConnect() {
        // 접속에 필요한 정보(게임 버전) 설정
        //PhotonNetwork.GameVersion = gameVersion;
        // 설정한 정보를 가지고 마스터 서버 접속 시도
        Debug.Log(PhotonNetwork.NetworkingClient.State);
        if (PhotonNetwork.NetworkingClient.State == ClientState.ConnectingToNameServer)
        {
            PhotonNetwork.Disconnect();  // 연결 시도 중단
        }
        else if (PhotonNetwork.NetworkingClient.State == ClientState.PeerCreated)
        {
            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectUsingSettings();
        }
        else if (PhotonNetwork.NetworkingClient.State == ClientState.Disconnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
        else{
            PhotonNetwork.NetworkingClient.LoadBalancingPeer.StopThread();
        }
        
        // 룸 접속 버튼을 잠시 비활성화
        //joinButton.interactable = false;
        // 접속을 시도 중임을 텍스트로 표시
        //connectionInfoText.text = "마스터 서버에 접속중...";
    }

    // 룸 접속 시도
    public static void Connect(string LobbyCode) {
        // 중복 접속 시도를 막기 위해, 접속 버튼 잠시 비활성화
        //joinButton.interactable = false;

        // 마스터 서버에 접속중이라면
        if (PhotonNetwork.IsConnected)
        {
            // 룸 접속 실행
            //connectionInfoText.text = "룸에 접속...";
            PhotonNetwork.JoinRoom(LobbyCode);
            Debug.Log("LobbyCode:"+LobbyCode);
            TempLobbyCode = LobbyCode;
        }
        else
        {
            // 마스터 서버에 접속중이 아니라면, 마스터 서버에 접속 시도
            //connectionInfoText.text = "오프라인 : 마스터 서버와 연결되지 않음\n접속 재시도 중...";
            // 마스터 서버로의 재접속 시도
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public static void Leave(){
        PhotonNetwork.LeaveRoom();
    }
    public static void DisConnect(){
        PhotonNetwork.Disconnect();
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Player target = targetPlayer;
        Hashtable PP = target.CustomProperties;
        //만약 내가 1번 플레이어라면
        if(PhotonNetwork.PlayerList[0].ActorNumber==PhotonNetwork.LocalPlayer.ActorNumber){
            if(target.ActorNumber==PhotonNetwork.LocalPlayer.ActorNumber){
                Player1Class.text=(string)PP["PlayerClass"];
                if((bool)PP["IsReady"]==true){
                    Player1Ready.SetActive(true);
                    Player1Wait.SetActive(false);
                }
                else{
                    Player1Ready.SetActive(false);
                    Player1Wait.SetActive(true);
                }
            }
            else{
                Player2Class.text=(string)PP["PlayerClass"];
                if((bool)PP["IsReady"]==true){
                    Player2Ready.SetActive(true);
                    Player2Wait.SetActive(false);
                }
                else{
                    Player2Ready.SetActive(false);
                    Player2Wait.SetActive(true);
                }
            }
        }
        else{
            if(target.ActorNumber==PhotonNetwork.LocalPlayer.ActorNumber){
                Player2Class.text=(string)PP["PlayerClass"];
                if((bool)PP["IsReady"]==true){
                    Player2Ready.SetActive(true);
                    Player2Wait.SetActive(false);
                }
                else{
                    Player2Ready.SetActive(false);
                    Player2Wait.SetActive(true);
                }
            }
            else{
                Player1Class.text=(string)PP["PlayerClass"];
                if((bool)PP["IsReady"]==true){
                    Player1Ready.SetActive(true);
                    Player1Wait.SetActive(false);
                }
                else{
                    Player1Ready.SetActive(false);
                    Player1Wait.SetActive(true);
                }
                
            }
        }

        if(AllPlayerReady()){
            
            GameStart();
        }
        
    }
    public override void OnPlayerLeftRoom (Player otherPlayer){
        if((bool)PlayerProperties["IsReady"]== true){
            Ready();
        }
        Debug.Log("OnPlayerLeftRoom : "+(bool)otherPlayer.CustomProperties["IsReady"]);
    }
#endregion

#region 클래스 선택
    //아처 선택
    public static void ClassSelectArcher(){
        PlayerProperties["PlayerClass"]="Archer";
        PlayerProperties["IsReady"]=false;
        PlayerPrefs.SetString("PlayerClass","Archer");
        PhotonCallBack.IsSelectClass = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }
    //전사 선택
    public static void ClassSelectWarrior(){
        PlayerProperties["PlayerClass"]="Warrior";
        PlayerProperties["IsReady"]=false;
        PlayerPrefs.SetString("PlayerClass","Warrior");
        PhotonCallBack.IsSelectClass = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }
    //도적 선택
    public static void ClassSelectRogue(){
        PlayerProperties["PlayerClass"]="Rogue";
        PlayerProperties["IsReady"]=false;
        PlayerPrefs.SetString("PlayerClass","Rogue");
        PhotonCallBack.IsSelectClass = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }
    //법사 선택
    public static void ClassSelectWizard(){
        PlayerProperties["PlayerClass"]="Wizard";
        PlayerProperties["IsReady"]=false;
        PlayerPrefs.SetString("PlayerClass","Wizard");
        PhotonCallBack.IsSelectClass = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
    }

#endregion

#region 로비 관련
//로비 관련 코드
    public static void RoomCreate(){
        string LobbyCode=GetLobbyCode(6); //로비코드생성
        //connectionInfoText.text = "방을 생성합니다.";
        PhotonNetwork.CreateRoom(LobbyCode, new RoomOptions {MaxPlayers = 2});
        Debug.Log(LobbyCode);
        TempLobbyCode = LobbyCode;
    }
    public static void PlayerPropertiesAdd(){
        PlayerProperties["PlayerClass"]="Warrior";
        PlayerPrefs.SetString("PlayerClass","Warrior");
        PlayerProperties["IsReady"]=(bool)false;
    }

//레디
    public static void Ready(){
        if(PhotonCallBack.IsSelectClass == true){
            if((bool)PlayerProperties["IsReady"]==true){
            PlayerProperties["IsReady"]=(bool)false;
            }
            else{
                PlayerProperties["IsReady"]=(bool)true;
            }
            PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties);
        }
        else{
            LM.StartCoroutine(LM.ReadyWarning());
        } 
    }

//랜덤 로비코드 생성
    public static string GetLobbyCode(int numLength){

        string  strResult = "";
        System.Random rand = new System.Random();
        string strRandomChar = "qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM0123456789"; //랜덤으로 들어갈 문자 및 숫자 
        
        StringBuilder rs = new StringBuilder();  
        
        //매개변수로 받은 numLength만큼 데이터를 가져 올 수 있습니다.
        for(int i=0; i<numLength; i++){
            rs.Append(strRandomChar[(int)(rand.NextDouble() * strRandomChar.Length)]);  
        }
        strResult = rs.ToString();
        
        return strResult;
    }
//로비 코드 복사
    public void CopyLobbyCode(string code){

        GUIUtility.systemCopyBuffer = code;
    }

    public void CopyToClipBoard(){
        Debug.Log(LobbyCode);
        Debug.Log(TempLobbyCode);
        CopyLobbyCode(TempLobbyCode);
    }
 
    public void GameStart(){
        /*게임 시작한다고 알림 띄울 예정*/
        foreach(Player Player in PhotonNetwork.PlayerList){
            Player.CustomProperties["IsReady"] = false;
        }
        PhotonNetwork.LoadLevel("Forest_Example_Multi");
    }

    //플레이어 레디 체크
    public bool AllPlayerReady(){
        bool IsAllReady = true;
        foreach(Player Player in PhotonNetwork.PlayerList){
            if((bool)Player.CustomProperties["IsReady"] == false){
                IsAllReady = false;
            };
        }
        return IsAllReady;
    }

    public static void ResetSetting(){
        PlayerProperties["PlayerClass"] = PlayerPrefs.GetString("PlayerClass");
        PlayerProperties["IsReady"]=false;
        PlayerProperties["IsExitShop"]=false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(PlayerProperties); //클래스 프로퍼티 설정
    }

    public void OnPlayerEnteredRoom(){
        ResetSetting();
    }

    public IEnumerator ReadyWarning(){
        readywarning.SetActive(true);
        yield return new WaitForSeconds(3f);
        readywarning.SetActive(false);
    }
#endregion

}