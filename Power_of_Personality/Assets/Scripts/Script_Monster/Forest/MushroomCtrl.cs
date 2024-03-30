using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MushroomCtrl : MonoBehaviour
{
    private Animator anim;  // �ִϸ�����
    public Slider HpBar;    // HP�� �����̴�

    // ���� �������ͽ�
    private float curHP;    // ���� ü��
    private float maxHP;    // �ִ� ü��
    public float ATK = 10;      // ���ݷ�
    private float DEF;      // ����
    private float MoveSpeed = 2;    // �̵��ӵ�
    private float FireATTDEF;   // �ҼӼ� ����
    private float IceATTDEF;    // ���� �Ӽ� ����
    private float Damage = 10f;   // ���� ���ط�

    //Ÿ�� ���׸���(Ÿ�� �� �Ӿ����°� ���� �Ϸ��ٰ� ������)
    private SkinnedMeshRenderer matObj;
    public GameObject targetObj;

    private Transform PlayerTr;  // �÷��̾��� Transform
    private float Distance;     // �÷��̾�� ���� ������ �Ÿ�
    private float TraceRadius = 10.0f;
    private float attackRadius = 3.0f; // ������ ���� �ݰ�
    private float AttackCoolTime; // ������ ���� ��Ÿ��

    public GameObject AttackCollider;   // 몬스터 공격 콜라이더
    public GameObject Coin;     //몬스터 죽이면 드랍되는 코인
    private bool isDie;     //몬스터 사망체크
    private bool isHit;     //몬스터 피격체크
    private void Start()
    {
        AttackCollider.SetActive(false);
        SetHp(100);
        CheckHp();
        anim = GetComponent<Animator>();
        matObj = targetObj.GetComponent<SkinnedMeshRenderer>();
        PlayerTr = this.transform;
        StartCoroutine(FindPlayer());
    }

    IEnumerator FindPlayer()
    {
        yield return new WaitForSeconds(0.1f);
        PlayerTr = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        if (!isDie)
        {
            DistanceCheck();
        }

        AttackCoolTime += Time.deltaTime;
        if (this.transform.position.x - PlayerTr.transform.position.x < 0)
        {
            this.transform.rotation = Quaternion.Euler(0, 90, 0);
        }
        else if (this.transform.position.x - PlayerTr.transform.position.x > 0)
        {
            this.transform.rotation = Quaternion.Euler(0, -90, 0);
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.tag == "WarriorAttack1")
        {
            isHit = true;
            Damage = 10;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillQ")
        {
            isHit = true;
            Damage = 20;
            StartCoroutine(TakeDamage());
        }

        if (col.tag == "WarriorSkillW")
        {
            isHit = true;
            Damage = 20;
            StartCoroutine(TakeDamage());
        }
    }


    IEnumerator TakeDamage()
    {
        if (maxHP != 0 || curHP > 0)
        {
            Material[] materials = matObj.materials;
            curHP -= Damage;
            CheckHp(); // ü�� ����
            anim.SetBool("TakeDamage", true);
            foreach (Material material in materials)
            {
                material.color = Color.red;
            }
            yield return new WaitForSeconds(0.5f);
            anim.SetBool("TakeDamage", false);
            yield return new WaitForSeconds(0.5f);
            if (anim.GetBool("TakeDamage") == false)
            {
                isHit = false;
            }
            foreach (Material material in materials)
            {
                material.color = Color.white;
            }
        }

        if (curHP <= 0) // ü���� 0�϶�
        {
            isDie = true;
            anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            Vector3 CoinPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.5f, gameObject.transform.position.z);
            Instantiate(Coin, CoinPosition, gameObject.transform.rotation);
            Destroy(this.gameObject); // ü���� 0 ���϶� ����
            Destroy(HpBar.gameObject);
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

    public void DistanceCheck()
    {
        Distance = Vector3.Distance(transform.position, PlayerTr.position);

        if (Distance <= TraceRadius && Distance > attackRadius && !isDie && !isHit)
        {
            StartCoroutine(Trace());
        }

        if (Distance <= attackRadius && AttackCoolTime >= 3.0f && !isDie && !isHit)
        {
            StartCoroutine(Attack());
        }
    }


    IEnumerator Attack()
    {
        AttackCoolTime = 0;
        AttackCollider.SetActive(true);
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("isAttack", false);
        AttackCollider.SetActive(false);
        AttackCoolTime = 0;
    }
    IEnumerator Trace()
    {
        // 플레이어를 향해 이동하는 로직
        Vector3 directionToPlayer = (PlayerTr.position - transform.position).normalized;
        Vector3 movement = new Vector3(directionToPlayer.x, 0, 0) * MoveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);
        anim.SetBool("isMove", true);

        Vector3 hpBarPosition = transform.position + Vector3.up * 3.5f; // 몬스터의 상단으로 설정
        HpBar.transform.position = hpBarPosition;

        yield return null;
    }
}
