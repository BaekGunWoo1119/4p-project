using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPrefabParentUnset : MonoBehaviour
{
    public float RemainingTime;

    // Update is called once per frame
    void Update()
    {
        RemainingTime += Time.deltaTime;

        if (RemainingTime > 1)
        {
            gameObject.transform.SetParent(null);
        }
    }
}
