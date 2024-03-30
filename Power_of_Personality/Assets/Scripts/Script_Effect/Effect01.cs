using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect01 : MonoBehaviour
{
    public GameObject PlayerCenter;
    private Vector3 PlayerCenterPos;
    private Vector3 Offset; public float Radius;   
    public float Speed;
    public float OffsetDelay;
    public float OffsetLerp;
    private Vector3 ToPos;      
    void Update()
    {
        PlayerCenterPos = PlayerCenter.transform.position;
        Offset = new Vector3(
            Mathf.Cos(Time.timeSinceLevelLoad * Speed + OffsetDelay*(float)Mathf.PI*2 ),            
            0,
            Mathf.Sin(Time.timeSinceLevelLoad * Speed + OffsetDelay * (float)Mathf.PI * 2)
            );         
        Offset *= Radius;   
        ToPos = PlayerCenterPos + Offset;
        transform.position = Vector3.Lerp(transform.position, ToPos, OffsetLerp);
    }
}