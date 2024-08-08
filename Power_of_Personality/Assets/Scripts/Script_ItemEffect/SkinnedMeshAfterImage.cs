using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class SkinnedMeshAfterImage : AfterImageBase
{
    private SkinnedMeshRenderer[] TargetSmrArray { get; set; }

    protected override void Init()
    {
        // 1. Target Meshes
        if (_containChildrenMeshes)
            TargetSmrArray = GetComponentsInChildren<SkinnedMeshRenderer>();
        else
            TargetSmrArray = new[] { GetComponent<SkinnedMeshRenderer>() };

        // 2. Queues
        FaderWaitQueue = new Queue<AfterImageFaderBase>();
        FaderRunningQueue = new Queue<AfterImageFaderBase>();

        // 3. Container
        _faderContainer = new GameObject($"{gameObject.name} AfterImage Container");
        _faderContainer.transform.SetPositionAndRotation(default, default);

        _data.Mat = _afterImageMaterial;
    }

    protected override void SetupFader(out AfterImageFaderBase fader)
    {
        GameObject faderGo = new GameObject($"{gameObject.name} AfterImage");
        faderGo.transform.SetParent(_faderContainer.transform);

        fader = faderGo.AddComponent<SkinnedMeshAfterImageFader>();
        fader.Setup(TargetSmrArray, _data, this);
    }
}