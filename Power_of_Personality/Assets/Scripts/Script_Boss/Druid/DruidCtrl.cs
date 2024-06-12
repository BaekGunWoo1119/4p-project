using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DruidCtrl : MonoBehaviour
{
    private float curHP;
    private float maxHP;
    private float Damage;
    public BoxCollider ownCollider;

    public GameObject EffectGen;
    public GameObject Scratch_Effect;
    private GameObject Scratch_Collider;

    public GameObject GroundStrike_Effect;
    private GameObject GroundStrike_Collider_S;
    private GameObject GroundStrike_Collider_M;
    private GameObject GroundStrike_Collider_L;

    public GameObject Projectile_Effect;
    public GameObject Projectile_Collider;

    public GameObject Vine_Effect;
    private GameObject Vine_Collider;
    private float Vine_xGrowthRate = 8f;
    private bool isVine = false;

    public GameObject ToxicPortal_Effect;
    private GameObject ToxicPortal_Collider;

    private GameObject SkillEffect;
    private GameObject SkillCollider;
    private float SkillYRot;

    Animator anim;
    public Slider HpBar;

    void Awake()
    {
        ownCollider = GetComponent<BoxCollider>();
        SkillYRot = transform.eulerAngles.y;
        Scratch_Collider = GameObject.Find("Scratch");
        GroundStrike_Collider_S = GameObject.Find("GroundStrike_S");
        GroundStrike_Collider_M = GameObject.Find("GroundStrike_M");
        GroundStrike_Collider_L = GameObject.Find("GroundStrike_L");
        Vine_Collider = GameObject.Find("Vine");
        ToxicPortal_Collider = GameObject.Find("ToxicPortal");
        StartCoroutine(Think());
    }

    void Start()
    {
        SetHp(100);
        CheckHp();
        Scratch_Collider.SetActive(false);
        GroundStrike_Collider_S.SetActive(false);
        GroundStrike_Collider_M.SetActive(false);
        GroundStrike_Collider_L.SetActive(false);
        Vine_Collider.SetActive(false);
        ToxicPortal_Collider.SetActive(false);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(isVine == true && Vine_Collider.transform.localScale.x <= 12.0)
        {
            Vector3 newScale = Vine_Collider.transform.localScale;
            newScale.x += Vine_xGrowthRate * Time.deltaTime;
            Vine_Collider.transform.localScale = newScale;
        }
    }
    public void SetHp(float amount) // Hp����
    {
        maxHP = amount;
        curHP = maxHP;
    }

    public void CheckHp() // HP ����
    {
        if (HpBar != null)
            HpBar.value = curHP / maxHP;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "WarriorAttack1")
        {
            Damage = 10;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillQ")
        {
            Damage = 20;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillW")
        {
            Damage = 20;
            StartCoroutine(TakeDamage());
        }
    }

    IEnumerator TakeDamage()
    {
        if (maxHP != 0 || curHP > 0)
        {
            //Material[] materials = matObj.materials;
            curHP -= Damage;
            Debug.Log(curHP);
            CheckHp();
            //anim.SetBool("TakeDamage", true);
            /*foreach (Material material in materials)
            {
                material.color = Color.red;
            }*/
            yield return new WaitForSeconds(0.5f);
           // anim.SetBool("TakeDamage", false);
            /*foreach (Material material in materials)
            {
                material.color = Color.white;
            }*/
        }

        if (curHP <= 0)
        {
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        int ranAction = Random.Range(0,6);

        switch (ranAction)
        {
            case 0: //팔을 휘두름(손톱처럼 3줄로 대각선 횡베기 이펙트) - 근접 약공
                StartCoroutine(MeleeWeakAttack());
                break;
            case 1: //지팡이로 바닥을 찍음 (바닥에 원형으로 이펙트) - 근접 강공
                StartCoroutine(MeleeStrongAttack());
                break;
            case 2: //지팡이를 휘두름(초록색 이펙트가 일직선으로 날아감) - 원거리 약공
                StartCoroutine(RangedWeakAttack());
                break;
            case 3: //팔을 휘두르고 공중으로 도약 후 지팡이로 바닥을 찍음(팔을 휘두를 때 도약할 때 원거리 투사체, 바닥을 찍을 때 식물 덩굴 발사) - 원거리 강공
                StartCoroutine(RangedStrongAttack());
                break;
            case 4: //바닥을 지팡이로 세번 내리찍음 (점점 범위가 넓어지는 원형 이펙트)
                StartCoroutine(Skill_1());
                break;
            case 5: //허공으로 손을 휘저음 (범위가 넓어지는 독 구름)
                StartCoroutine(Skill_2());
                break;
        }
    }

    // 공격 애니메이션 && 콜라이더 스크립트
    IEnumerator MeleeWeakAttack()
    {
        Debug.Log("실행 근접평타");
        anim.SetTrigger("doMeleeWeakAttack");   // 애니메이션

        yield return new WaitForSeconds(0.75f); // 스킬 콜라이더 ~~
        Scratch_Collider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Scratch_Collider.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        Scratch_Collider.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Scratch_Collider.SetActive(false);
        yield return new WaitForSeconds(4f);    // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    IEnumerator MeleeStrongAttack()
    {
        Debug.Log("실행 근접강공");
        anim.SetTrigger("doMeleeStrongAttack");     //애니메이션

        yield return new WaitForSeconds(1f);        // 스킬 콜라이더 ~~
        GroundStrike_Collider_S.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        GroundStrike_Collider_S.SetActive(false);
        GroundStrike_Collider_M.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        GroundStrike_Collider_M.SetActive(false);
        GroundStrike_Collider_L.SetActive(true);
        yield return new WaitForSeconds(1f);
        GroundStrike_Collider_L.SetActive(false);
        yield return new WaitForSeconds(3f);        // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    IEnumerator RangedWeakAttack()
    {
        Debug.Log("실행 원거리평타");
        anim.SetTrigger("doRangedWeakAttack");      // 애니메이션
        yield return new WaitForSeconds(4f);

        StartCoroutine(Think());
    }

    IEnumerator RangedStrongAttack()
    {
        Debug.Log("실행 원거리강공");
        anim.SetTrigger("doRangedStrongAttack");    // 애니메이션

        yield return new WaitForSeconds(2.5f);      // 스킬 콜라이더 ~~
        Vine_Collider.SetActive(true);
        isVine = true;
        yield return new WaitForSeconds(4f);
        isVine = false;
        Vine_Collider.SetActive(false);
        Vine_Collider.transform.localScale = new Vector3(1, 1, 1);
        yield return new WaitForSeconds(1.5f);      // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    IEnumerator Skill_1()
    {
        Debug.Log("실행 스킬1");
        anim.SetTrigger("doSkill1");


        yield return new WaitForSeconds(1f);          // 스킬 콜라이더 ~~
        GroundStrike_Collider_S.SetActive(true);        // 1-1 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_S.SetActive(false);       // 1-1 Off
        GroundStrike_Collider_M.SetActive(true);        // 1-2 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_M.SetActive(false);       // 1-2 Off
        GroundStrike_Collider_L.SetActive(true);        // 1-3 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_L.SetActive(false);       // 1-3 Off

        yield return new WaitForSeconds(0.1f);          // 0.1초 대기

        GroundStrike_Collider_S.SetActive(true);        // 2-1 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_S.SetActive(false);       // 2-1 Off
        GroundStrike_Collider_M.SetActive(true);        // 2-2 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_M.SetActive(false);       // 2-2 Off
        GroundStrike_Collider_L.SetActive(true);        // 2-3 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_L.SetActive(false);       // 2-3 Off

        yield return new WaitForSeconds(0.75f);          // 0.1초 대기

        GroundStrike_Collider_S.SetActive(true);        // 2-1 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_S.SetActive(false);       // 2-1 Off
        GroundStrike_Collider_M.SetActive(true);        // 2-2 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_M.SetActive(false);       // 2-2 Off
        GroundStrike_Collider_L.SetActive(true);        // 2-3 On
        yield return new WaitForSeconds(0.25f);         // 0.25초 대기
        GroundStrike_Collider_L.SetActive(false);       // 2-3 Off

        yield return new WaitForSeconds(3f);            // ~~ 스킬 콜라이더

        StartCoroutine(Think());
    }

    IEnumerator Skill_2()
    {
        Debug.Log("실행 스킬2");
        anim.SetTrigger("doSkill2");
        yield return new WaitForSeconds(0.75f);
        ToxicPortal_Collider.SetActive(true);
        yield return new WaitForSeconds(2.75f);
        ToxicPortal_Collider.SetActive(false);

        StartCoroutine(Think());
    }

    // 공격 이펙트 스크립트
    public void Scratch_1()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x + 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30, -30, 90));
        }
        else
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x - 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(30, 150, 90));
        }
    }

    public void Scratch_2()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x + 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(-30, -30, 90));
        }
        else
        {
            SkillEffect = Instantiate(Scratch_Effect, new Vector3(EffectGen.transform.position.x - 2, EffectGen.transform.position.y, EffectGen.transform.position.z), Quaternion.Euler(-30, 150, 90));
        }
    }
    public void GroundStrike()
    {
        SkillEffect = Instantiate(GroundStrike_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 1f, EffectGen.transform.position.z), Quaternion.Euler(0, 0, 0));
    }

    public void Projectile()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Projectile_Effect, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
            SkillCollider = Instantiate(Projectile_Collider, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
        }
        else
        {
            SkillEffect = Instantiate(Projectile_Effect, EffectGen.transform.position, Quaternion.Euler(0, 180, 0));
            SkillCollider = Instantiate(Projectile_Collider, EffectGen.transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    public void Vine()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(Vine_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(90, 0, 0));
        }
        else
        {
            SkillEffect = Instantiate(Vine_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y - 0.5f, EffectGen.transform.position.z), Quaternion.Euler(90, 180, 0));
        }
    }

    public void ToxicPortal()
    {
        if (SkillYRot == 90 || (SkillYRot < 92 && SkillYRot > 88))
        {
            SkillEffect = Instantiate(ToxicPortal_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(90, 90, 0));
        }
        else
        {
            SkillEffect = Instantiate(ToxicPortal_Effect, new Vector3(EffectGen.transform.position.x, EffectGen.transform.position.y + 2f, EffectGen.transform.position.z), Quaternion.Euler(90, -90, 0));
        }
    }
}
