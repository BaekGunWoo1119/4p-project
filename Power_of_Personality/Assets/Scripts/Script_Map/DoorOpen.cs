using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen : MonoBehaviour
{
    public Animator doorAnim;
    public GameObject nextplatform;
    private AudioSource[] audio; 

    void Start()
    {
        audio = this.GetComponents<AudioSource>();
    }

    // 플레이어가 닿을 경우 문이 열림
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            doorAnim.SetBool("isOpen", true);
            if(nextplatform != null)
            {
                col.gameObject.transform.SetParent(nextplatform.transform);
            }
            
            audio[0].PlayOneShot(audio[0].clip);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            audio[1].PlayOneShot(audio[1].clip);
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
