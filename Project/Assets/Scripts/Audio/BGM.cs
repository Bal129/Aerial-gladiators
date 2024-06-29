using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] AudioSource bgm;

    private GameObject otherBGMs; // to check any duplication

    private void Awake()
    {
        GameObject[] otherBGMs = GameObject.FindGameObjectsWithTag("BGM");
        if (otherBGMs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void StartMusic()
    {
        bgm.Play();
    }

    public void StopMusic()
    {
        bgm.Stop();
    }
}
