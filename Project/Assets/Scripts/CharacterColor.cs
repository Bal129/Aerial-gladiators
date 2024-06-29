using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterColor : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] GameObject enemyObject;
    [SerializeField] Material[] Materials;

    private System.Random randomiser;

    private void Start()
    {
        Debug.Log("Material length: " + Materials.Length);

        randomiser = new System.Random();
        int materialIndex = randomiser.Next(0, Materials.Length);

        enemyObject.GetComponent<Renderer>().material = Materials[materialIndex];
    }
}
