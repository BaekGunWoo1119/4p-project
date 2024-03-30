using UnityEngine;

public class Fairy_Attack : MonoBehaviour
{
    private Transform PlayerTr;
    private Rigidbody rb;
    public float ATK = 5.0f;
    void Awake()
    {
        PlayerTr = GameObject.FindWithTag("Player").transform;
        Vector3 targetPosition = PlayerTr.position + Vector3.up;
        rb = GetComponent<Rigidbody>();
        Vector3 Dir = targetPosition - transform.position;
        rb.AddForce(Dir.normalized * 10, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
