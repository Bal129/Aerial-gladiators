using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Attributes")]
    public float moveSpeed;
    public int score;

    [Header("Effects")]
    public CharacterEffects effects;

    /*
    private void Start()
    {
        status.isAlive = true;
    }
    */

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.tag == "Player")
        {
            Gladiator player = collidedObject.GetComponent<Gladiator>();
            if (collision.relativeVelocity.magnitude >= player.offensiveVelocity)

            {
                effects.PlayPopSound();
                effects.PlayDeathParticles();
                Destroy(transform.parent.gameObject);
            }
        }
    }
}
