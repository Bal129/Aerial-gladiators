using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [Header("Sounds")]
    public AudioSource popSound;
    public AudioSource hurtSound;
    public AudioSource tapSound;
    public AudioSource bowSound;

    public void PlayPopSound()
    {
        popSound.Play();
    }

    public void PlayHurtSound()
    {
        hurtSound.Play();
    }

    public void PlayTapSound()
    {
        tapSound.Play();
    }

    public void PlayBowSound()
    {
        bowSound.Play();
    }
}
