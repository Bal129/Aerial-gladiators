using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [Header("Effects")]
    public CharacterEffects effects;

    private void OnCollisionEnter(Collision collision)
    {
        effects.PlayDeathParticles();
        effects.PlayPopSound();
        Destroy(gameObject);
    }
}
