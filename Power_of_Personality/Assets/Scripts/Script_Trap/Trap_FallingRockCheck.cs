using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_FallingRockCheck : MonoBehaviour
{
    public GameObject Rock;
    private GameObject SkillEffect;
    private bool canSpawnRock = true; 
    private float spawnCooldown = 5f;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player" && canSpawnRock)
        {
            SkillEffect = Instantiate(Rock, new Vector3(this.transform.position.x, this.transform.position.y + 12f, this.transform.position.z), Quaternion.Euler(90, 0, 0));
            canSpawnRock = false; 
            StartCoroutine(RockSpawnCooldown());  
        }
    }

    IEnumerator RockSpawnCooldown()
    {
        yield return new WaitForSeconds(spawnCooldown);  
        canSpawnRock = true;  
    }
}
