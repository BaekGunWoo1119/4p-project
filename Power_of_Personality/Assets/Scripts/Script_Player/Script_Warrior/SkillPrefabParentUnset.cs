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

        if (RemainingTime > 0.7f)
        {
            gameObject.transform.SetParent(null);
        }
    }
}
