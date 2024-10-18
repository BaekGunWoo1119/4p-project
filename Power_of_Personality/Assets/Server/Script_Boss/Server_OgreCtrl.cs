using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

public class Server_OgreCtrl : Server_BossCtrl
{
    #region 변수 선언
    public GameObject LArmSwing_Effect;
    public GameObject RArmSwing_Effect;
    public GameObject HandSmash_Effect;
    public GameObject Tentacle_Effect;
    public GameObject SpewVenom_Effect;
    public GameObject RoarofAnger_Effect;
    public GameObject RaiseTentacle_Effect;


    // 보스 공격 컨트롤
    private bool onRoar;

    private bool canTraceAttack;
    private bool isRangedAttack = true;
    private bool TraceOn;
    private Vector3 PrevPosition;
    #endregion

    #region Awake, Start, Update문
    protected override void Awake()
    {
        DEF = 100f;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        MoveSpeed = 7f;
        SetHP(2000f);
        //shopPortal.SetActive(false);
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
        SoundsManager.Change_Sounds("Sewer_Boss"); //소리 추가(08.31)
    }

    protected override void Update()
    {
        base.Update();
        SkillYRot = transform.localEulerAngles.y;
        Debug.Log(SkillYRot);
        //캔버스 뒤집어지는 오류 해결(08.29)
        if(GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y > 0 && GameObject.FindWithTag("MainCamera").transform.parent.transform.eulerAngles.y < 180)
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot + 180f, 0);
        else
            MonsterCanvas.transform.localRotation = Quaternion.Euler(0, SkillYRot, 0);
        if(photonview.IsMine){
            DistanceCheck();
        }
        if(isDie == true)
        {
            SoundsManager.Change_Sounds("Sewer"); //소리 추가(08.31)
        }
    }
    #endregion

    #region HP 관련
    protected override void SetHP(float amount) // Hp����
    {
        maxHP = amount;
        curHP = maxHP;
    }

    public override void CheckHP() // HP ����
    {
        base.CheckHP();
    }

    #endregion

    #region 보스 피격, 피해량 공식
    public override void OnTriggerEnter(Collider col)
    {
        base.OnTriggerEnter(col);
    }

    public override void OnTriggerStay(Collider col)
    {
        base.OnTriggerStay(col);
    }
    public override IEnumerator TakeDamage(float Damage,string Property)
    {
        yield return base.TakeDamage(Damage, Property);
    }
    #endregion

    #region 보스 패턴 관련
    protected override IEnumerator FindPlayer()     // 플레이어를 찾아서 할당해주는 함수
    {
        yield return base.FindPlayer();
    }
    protected override void DistanceCheck()
    {
        if(PlayerTr != null)
            Distance = Vector3.Distance(transform.position, PlayerTr.position);
        //여기서부턴 세부 구현, 각 스크립트에서 보스 패턴에 맞게 구현
        if(TraceOn == true)
        {
            StartCoroutine(Trace());
        }

    }
    public IEnumerator Trace()
    {
        // 플레이어를 향해 이동하는 로직
        Debug.Log("추적");
        Vector3 directionToPlayer = (PlayerTr.position - transform.position).normalized;
        Vector3 movement = new Vector3(directionToPlayer.x, 0, 0) * MoveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        photonview.RPC("SetAnim",RpcTarget.All,"doMove", true);

        if (Distance <= 3f)
        {
            int ranAction = Random.Range(0, 3);
            photonview.RPC("SetAnim",RpcTarget.All,"doMove", false);
            switch (ranAction)
            {
                case 0:
                    Debug.Log("근접 약공 선택됨");
                    photonview.RPC("RPCMeleeWeakAttack", RpcTarget.All);
                    TraceOn = false;
                    isRangedAttack = true;
                    break;
                case 1:
                    Debug.Log("근접 강공 선택됨");
                    photonview.RPC("RPCMeleeStrongAttack", RpcTarget.All);
                    TraceOn = false;
                    isRangedAttack = true;
                    break;
                case 2:
                    Debug.Log("스킬 1 선택됨");
                    photonview.RPC("RPCSkill_1", RpcTarget.All);
                    TraceOn = false;
                    isRangedAttack = true;
                    break;
            }
        }
        else if(Distance <= 12f && isRangedAttack)
        {
            int ranAction = Random.Range(0, 2);
            photonview.RPC("SetAnim",RpcTarget.All,"doMove", false);
            switch (ranAction)
            {
                case 0:
                    int chooseAttack = Random.Range(0, 3);
                    switch (chooseAttack)
                    {
                        case 0:
                            Debug.Log("추적 후_원거리 약공 선택됨");
                            photonview.RPC("RPCRangedWeakAttack", RpcTarget.All);
                            TraceOn = false;
                            break;
                        case 1:
                            Debug.Log("추적 후_원거리 강공 선택됨");
                            photonview.RPC("RPCRangedStrongAttack", RpcTarget.All);
                            TraceOn = false;
                            break;
                        case 2:
                            Debug.Log("추적 후_스킬 2 선택됨");
                            photonview.RPC("RPCSkill_2", RpcTarget.All);
                            TraceOn = false;
                            break;
                    }
                    break;
                case 1:
                    isRangedAttack = false;
                    break;
            }
        }
        else
        {
            transform.Translate(movement, Space.World);
            photonview.RPC("SetAnim",RpcTarget.All,"doMove", true);
        }
        yield return null;
    }

    protected override IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("생각");
        Debug.Log("Distance = " + Distance);
        Debug.Log("여기까지는 잘 돼요");
        if(Distance <= 12f)
        {
            Debug.Log("1번 들어옴");
            int ranAction = Random.Range(0, 2);
            Debug.Log("선택된 번호는 ? = " + ranAction);
            switch (ranAction)
            {
                case 0:
                    Debug.Log("Trace 선택됨");
                    TraceOn = true;
                    break;
                case 1:
                    int chooseAttack = Random.Range(0, 3);
                    switch (chooseAttack)
                    {
                        case 0:
                            Debug.Log("원거리 약공 선택됨");
                            photonview.RPC("RPCRangedWeakAttack", RpcTarget.All);
                            TraceOn = false;
                            break;
                        case 1:
                            Debug.Log("원거리 강공 선택됨");
                            photonview.RPC("RPCRangedStrongAttack", RpcTarget.All);
                            TraceOn = false;
                            break;
                        case 2:
                            Debug.Log("스킬 2 선택됨");
                            photonview.RPC("RPCSkill_2", RpcTarget.All);
                            TraceOn = false;
                            break;
                    }
                    break;
            }
        }
        else
        {
            TraceOn = true;
        }
    }

    // 공격 애니메이션 && 콜라이더 스크립트
    protected override IEnumerator MeleeWeakAttack() //첫번째 공격 애니메이션 2.1초 두번째 1초
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeWeakAttack");   // 애니메이션
        yield return new WaitForSeconds(1.1f);        //애니메이션 지속 시간(10.03 수정)
        atkAudio[0].PlayOneShot(atkAudio[0].clip);    //약 공격 (양손 휘두르기)소리 추가(10.03 수정)
        yield return new WaitForSeconds(2f);        //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(2f);        //다음 행동까지 걸리는 시간 
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
    }

    protected override IEnumerator MeleeStrongAttack() //공격 애니메이션 3초
    {
        isAttacking = true;
        anim.SetTrigger("doMeleeStrongAttack");     //애니메이션
        yield return new WaitForSeconds(1.5f);        //애니메이션 지속 시간(10.03 수정)
        atkAudio[1].PlayOneShot(atkAudio[1].clip);    //강 공격 (손뼉 치기)소리 추가(10.03 수정)
        yield return new WaitForSeconds(1.5f);        //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(3f);        //다음 행동까지 걸리는 시간
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
    }

    protected override IEnumerator RangedWeakAttack() //공격 애니메이션 3.6초
    {
        isAttacking = true;
        anim.SetTrigger("doRangedWeakAttack");
        yield return new WaitForSeconds(1.6f);        //애니메이션 지속 시간(10.03 수정)
        atkAudio[2].PlayOneShot(atkAudio[2].clip);    //약 공격 (촉수 찌르기)소리 추가(10.03 수정)
        yield return new WaitForSeconds(2f);        //애니메이션 지속 시간(10.03 수정)
        isAttacking = false;
        yield return new WaitForSeconds(2f);        //다음 행동까지 걸리는 시간 
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
    }

    protected override IEnumerator RangedStrongAttack() //공격 애니메이션 4.2초 
    {
        isAttacking = true;
        anim.SetTrigger("doRangedStrongAttack");    // 애니메이션
        yield return new WaitForSeconds(1.2f);        //애니메이션 지속 시간
        atkAudio[3].PlayOneShot(atkAudio[3].clip);    //강 공격 (토하기)소리 추가(10.03 수정)
        yield return new WaitForSeconds(3.0f);        //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(3f);        //다음 행동까지 걸리는 시간      
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
    }

    protected override IEnumerator Skill_1() //공격 애니메이션 3.1초
    {
        isAttacking = true;
        anim.SetTrigger("doSkill1");
        yield return new WaitForSeconds(0.4f);        //애니메이션 지속 시간
        atkAudio[4].PlayOneShot(atkAudio[4].clip);    //스킬 공격 (광란 팔 휘두르기)소리 추가(10.03 수정)
        yield return new WaitForSeconds(2.7f);        //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(3f);        //다음 행동까지 걸리는 시간      
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
    }

    protected override IEnumerator Skill_2() //공격 애니메이션 3.8초
    {
        isAttacking = true;
        anim.SetTrigger("doSkill2");
        yield return new WaitForSeconds(1.8f);        //애니메이션 지속 시간
        atkAudio[5].PlayOneShot(atkAudio[5].clip);    //스킬 공격 (솟아나는 촉수)소리 추가(10.03 수정)
        yield return new WaitForSeconds(2f);        //애니메이션 지속 시간
        isAttacking = false;
        yield return new WaitForSeconds(3f);        //다음 행동까지 걸리는 시간      
        if(photonview.IsMine){
            StartCoroutine(Think());
        }
    }
    #endregion

    #region 공격 이펙트 스크립트
    public void ArmSwing_1()
    {
        if (SkillYRot == -90 || SkillYRot == 270 || (SkillYRot > -100 && SkillYRot < -80))
        {
            SkillEffect = Instantiate(LArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, -90, 0));
        }
        else
        {
            SkillEffect = Instantiate(LArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, 90, 0));
        }
    }

    public void ArmSwing_2()
    {
        if (SkillYRot == -90 || SkillYRot == 270 || (SkillYRot > -100 && SkillYRot < -80))
        {
            SkillEffect = Instantiate(RArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, 90, 0));
        }
        else
        {
            SkillEffect = Instantiate(RArmSwing_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, -90, 0));
        }
    }
    public void HandSmash()
    {
        if (SkillYRot == -90 || SkillYRot == 270 || (SkillYRot > -100 && SkillYRot < -80))
        {
            SkillEffect = Instantiate(HandSmash_Effect, new Vector3(EffectGen.transform.position.x - 1.5f, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, 180, 0));
        }
        else
        {
            SkillEffect = Instantiate(HandSmash_Effect, new Vector3(EffectGen.transform.position.x + 1.5f, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, 0, 0));
        } 
    }

    public void Tentacle()
    {
        if (SkillYRot == -90 || SkillYRot == 270 || (SkillYRot > -100 && SkillYRot < -80))
        {
            SkillEffect = Instantiate(Tentacle_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, -90, 0));
        }
        else
        {
            SkillEffect = Instantiate(Tentacle_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(0, 90, 0));
        }
    }

    public void SpewVenom()
    {
        if (SkillYRot == -90 || SkillYRot == 270 || (SkillYRot > -100 && SkillYRot < -80))
        {
            SkillEffect = Instantiate(SpewVenom_Effect, new Vector3(EffectGen.transform.position.x - 7.5f, EffectGen.transform.position.y + 0.25f, EffectGen.transform.position.z), Quaternion.Euler(0, -90, 0));
        }
        else
        {
            SkillEffect = Instantiate(SpewVenom_Effect, new Vector3(EffectGen.transform.position.x + 7.5f, EffectGen.transform.position.y + 0.25f, EffectGen.transform.position.z), Quaternion.Euler(0, 90, 0));
        } 
    }

    public void RoarofAnger()
    {
        SkillEffect = Instantiate(RoarofAnger_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, 0, 0));
        SkillEffect.transform.SetParent(EffectGen.transform);
    }

    public void RaiseTentacle()
    {
        if (SkillYRot == -90 || SkillYRot == 270 || (SkillYRot > -100 && SkillYRot < -80))
        {
            SkillEffect = Instantiate(RaiseTentacle_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, -90, 0));    
        }
        else
        {
            SkillEffect = Instantiate(RaiseTentacle_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(0, 90, 0));
        }
    }
    #endregion
    public override void Settarget(){
        base.Settarget();
    }

    [PunRPC]
    public void RPCMeleeWeakAttack(){
        StartCoroutine(MeleeWeakAttack());
    }
    [PunRPC]
    public void RPCMeleeStrongAttack(){
        StartCoroutine(MeleeStrongAttack());
    }
    [PunRPC]
    public void RPCRangedWeakAttack(){
        StartCoroutine(RangedWeakAttack());
    }
    [PunRPC]
    public void RPCRangedStrongAttack(){
        StartCoroutine(RangedStrongAttack());
    }
    [PunRPC]
    public void RPCSkill_1(){
        StartCoroutine(Skill_1());
    }
    [PunRPC]
    public void RPCSkill_2(){
        StartCoroutine(Skill_2());
    }
    [PunRPC]
    public void SetAnim(string animstring, bool animbool){
        anim.SetBool(animstring,animbool);
    }
}
