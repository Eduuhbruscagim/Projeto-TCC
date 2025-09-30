using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem DustTrail;
    [Range(0, 10)]
    [SerializeField] int occurAfterVelocity;
    [Range(0, 0.2f)]
    [SerializeField] float dustFormatioPeriod;

    [SerializeField] Rigidbody2D rbPersonagem;
    [SerializeField] Personagem personagem; // referência direta ao script Personagem

    float Counter;

    private void Update()
    {
        Counter += Time.deltaTime;

        if (personagem.EstaNoChao && Mathf.Abs(rbPersonagem.velocity.x) > occurAfterVelocity)
        {
            if (Counter > dustFormatioPeriod)
            {
                DustTrail.Play();
                Counter = 0;
            }
        }
    }
}
