using UnityEngine;

public class Fairy_Attack : MonoBehaviour
{
    private Transform PlayerTr;
    private Rigidbody rb;
    public float ATK = 5.0f;
    private string fairyParent = "Monster(Script)"; // 찾고자 하는 부모 객체의 이름
    public MonsterCtrl monsterCtrl;
    void Awake()
    {
        PlayerTr = GameObject.FindWithTag("Player").transform;
        Vector3 targetPosition = PlayerTr.position + Vector3.up;
        rb = GetComponent<Rigidbody>();
        if(Status.Spell_TimeSlowdown_ON){
            rb.AddForce(transform.forward * 10 *0.3f, ForceMode.Impulse);
        }
        else{
            rb.AddForce(transform.forward * 10, ForceMode.Impulse);
        }
        Transform parent = this.transform;
        monsterCtrl = null;

        while (parent != null)
        {
            if (parent.name ==  fairyParent)
            {
                monsterCtrl = parent.GetComponent<MonsterCtrl>();
                break;
            }
            parent = parent.parent;
        }

        this.transform.SetParent(null);
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            if (monsterCtrl != null && Status.set2_3_Activated)
            {
                float reflectDamage = Status.TotalArmor;
                StartCoroutine(monsterCtrl.TakeDamage(reflectDamage));
            }
            this.transform.localPosition = new Vector3(0, -600f, 0);
            Destroy(gameObject,5.0f);
        }
    }

    public void Update()
    {
        //transform.rotation = Quaternion.identity; // 초기 회전으로 설정
    }
    
}
