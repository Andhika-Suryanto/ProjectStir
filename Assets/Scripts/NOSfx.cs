using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class NOSfx : MonoBehaviour
{
    private Animator animator;
    public ParticleSystem particleSystem; // Public variable to assign the Particle System

    private ParticleSystem.EmissionModule emissionModule;
    private bool wasEmissionEnabled = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (particleSystem == null)
        {
            ///Debug.LogError("Particle System is not assigned.");
            return;
        }

        // Get the emission module from the particle system
        emissionModule = particleSystem.emission;
    }

    void Update()
    {
        // Check if the emission is enabled
        if (emissionModule.enabled && !wasEmissionEnabled)
        {
            animator.SetBool("isNOS", true);
            //Debug.Log("Emission is enabled");
            wasEmissionEnabled = true;
        }
        else if (!emissionModule.enabled && wasEmissionEnabled)
        {
            animator.SetBool("isNOS", false);
            wasEmissionEnabled = false;
        }
    }
}
