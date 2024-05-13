using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAttack
{
    //공격 함수 (기본 공격 1,2,3 점프공격 4,5,6)
    void Attack(int AttackNumber);
}
