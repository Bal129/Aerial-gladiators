using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffects : MonoBehaviour
{
    [Header("Particle")]
    public ParticleSystem deathParticle;

    [Header("Pop sound")]
    public GameObject popSoundObject;

    /*
    [Header("Status")]
    public CharacterStatus status;
    */

    /*
    private void OnDestroy()
    {
        if (LevelManager.levelSetUp)
        {
            //popSound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().popSound;
            //popSound.Play();
            PlayDeathParticles();
        }
    }
    */

    /*
    private void Start()
    {
        popSound = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<SoundManager>().popSound;
    }

    private void FixedUpdate()
    {
        if (!status.isAlive && LevelManager.levelSetUp)
        {
            //popSound.Play();
            PlayDeathParticles();
            Destroy(this.gameObject);
            //status.isAlive = true;
        }
    }
    */

    public void PlayPopSound()
    {
        // set the parent to world space
        // otherwise get error because the parent is already destroyed 
        popSoundObject.transform.SetParent(null);

        // play sound
        AudioSource popSound = popSoundObject.GetComponent<AudioSource>();
        popSound.Play();

        Destroy(popSoundObject, 2f);
    }

    public void PlayDeathParticles()
    {
        // set the parent to the world space
        // otherwise get error because the parent is already destroyed 
        deathParticle.transform.SetParent(null);

        // change color for particle according to the main object color
        var main = deathParticle.gameObject.GetComponent<ParticleSystem>().main;
        main.startColor = gameObject.GetComponent<Renderer>().material.color;

        // play and destroy after done animation
        deathParticle.Play();
        Destroy(deathParticle, 3f);
    }
}
