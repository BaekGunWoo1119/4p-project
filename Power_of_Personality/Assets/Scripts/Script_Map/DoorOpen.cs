using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnim;

    // 플레이어가 닿을 경우 문이 열림
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorAnim.SetBool("isOpen", true);
        }
    }

    void LateUpdate()
    {
        if(this.transform.position.x + 1 < GameObject.FindWithTag("Player").transform.position.x)
        {
            doorAnim.SetBool("isOpen", false);
        }
    }
}
