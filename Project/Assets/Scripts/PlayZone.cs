using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //if (LevelManager.levelSetUp)
        //{
            if (other.tag == "Player")
            {
                other.gameObject.GetComponent<Gladiator>().UpdateHealth(0);
                // Destroy(other.gameObject);
                // other.gameObject.GetComponent<Gladiator>().isAlive = false;
            }

            if (other.tag == "Enemy")
            {
                CharacterEffects effects = other.gameObject.GetComponent<CharacterEffects>();
                effects.PlayPopSound();
                effects.PlayDeathParticles();
                Destroy(other.gameObject);
            }
        //}
    }
}
