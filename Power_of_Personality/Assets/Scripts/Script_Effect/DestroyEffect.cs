using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    public float DelTime;
    void Update()
    {
        Destroy(this.gameObject, DelTime);
    }
}
