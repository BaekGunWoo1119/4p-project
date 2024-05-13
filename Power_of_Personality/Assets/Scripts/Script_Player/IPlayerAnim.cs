using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerAnim
{
    // Update is called once per frame
    void PlayAnim(string AnimationName);
    
    void StopAnim(string AnimationName);

    void AnimState();
}
