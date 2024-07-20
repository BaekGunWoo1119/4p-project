using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리


public class MultiGameManager : MonoBehaviourPunCallbacks
{
    public int WaveTime; //웨이브 시간
    public float CurrentTime; //현재 시간
    public int CurrentWave; //현재 웨이브
    public int MonsterCount; //현재 몬스터 수
    public int Spawned; //생성한 몬스터 수
    public int TargetSpawn; //목표 몬스터 수
    public float RespawnTime = 3.0f; //몬스터 리스폰 시간
    public float TempTime; //몬스터 저장용 시간
    public float sec; //타이머용 시간
    public int min; //타이머용 시간
    public GameObject SpawnPoint; //몬스터 스폰 포인트 (몇개 넣으지 고민중)

    // Start is called before the first frame update
    void Start()
    {
        TempTime = 0f;
        CurrentWave = 1;
        sec = 0f;
        min = 0;

    }


    void Update()
    {
        #region 인게임 타이머
        sec += Time.deltaTime;
            if (sec >= 60f)
            {
                min += 1;
                sec = 0;
            }

        //인게임에 UI 넣어야됨 고민해야 될 내용 
        //gameTime.text = string.Format("{0:D2}:{1:D2}", min, (int)sec);
        #endregion
        CurrentTime += Time.deltaTime;
        TempTime+= Time.deltaTime;
        MonsterCount = GameObject.FindGameObjectsWithTag("Monster").Length;  //몬스터 수 카운트
        //웨이브 시간 종료
        if(CurrentTime>WaveTime){
            //몬스터가 0명이라면
            if(GameObject.FindGameObjectsWithTag("Monster").Length<1){
                EndWave();
            }
        }

        else{
            //목표 몬스터 채울때까지 생성
            if(Spawned<TargetSpawn&& RespawnTime<TempTime){
            SpawnMonster();
            }
        }
    }

    //웨이브 종료 후 상점으로 이동
    void EndWave(){
        //체력 회복
        Status.HP = Status.MaxHP;
        CurrentWave += 1;
        //상점으로 이동
        //GoShop();
    }

    //몬스터 스폰
    void SpawnMonster(){
        if (PhotonNetwork.IsMasterClient){
            //PhotonNetwork.Instantiate("Server/Monster/Prefab_Cave/Server_Spider",SpawnPoint1.position,SpawnPoint1.rotation,0);

        }
        TempTime = 0f; //리스폰 대기시간 초기화

    }
}
