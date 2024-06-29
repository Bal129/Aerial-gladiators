using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [Header("Attributes")]
    public float damage;
    public float rotateSpeed;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collidedObject = collision.gameObject;

        if (collidedObject.tag == "Player") // TO DO: also add for enemy
        {
            Gladiator gladiator = collidedObject.GetComponent<Gladiator>();
            gladiator.UpdateHealth(gladiator.currentHealth - 1);
            gladiator.StartIFrame();
        }
        else if (collidedObject.tag == "Enemy")
        {
            CharacterEffects effects = collidedObject.gameObject.GetComponent<CharacterEffects>();
            effects.PlayPopSound();
            effects.PlayDeathParticles();
            Destroy(collidedObject);
        }
    }
}
