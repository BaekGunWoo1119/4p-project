using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_TimeCtrl : MonoBehaviour
{
    private ParticleSystem particleSystem;
    public float simulationSpeed = 1.5f;

    void Start()
    {
        particleSystem = this.GetComponent<ParticleSystem>();

        UpdateSimulationSpeed();
    }

    void UpdateSimulationSpeed()
    {
        var mainModule = particleSystem.main;
        mainModule.simulationSpeed = simulationSpeed;
    }
}