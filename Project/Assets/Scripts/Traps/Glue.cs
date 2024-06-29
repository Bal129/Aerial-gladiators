using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glue : MonoBehaviour
{
    [Header("Attributes")]
    public float speedDecreaseMultiplier;
    public float speedDecreaseDuration;

    private Gladiator gladiator;
    private float gladiatorOriginalSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // ALSO CREATE FOR ENEMY
        { 
            gladiator = other.gameObject.GetComponent<Gladiator>();
            StartCoroutine(ApplyGlue());
        }
    }

    IEnumerator ApplyGlue()
    {
        gladiator.isGlued = true;
        GameObject gladiatorObject = gladiator.gameObject;
        gladiator.GetComponent<Rigidbody>().velocity = Vector3.zero;
        gladiatorObject.transform.position = transform.position;

        yield return new WaitForSeconds(speedDecreaseDuration);
        
        gladiator.isGlued = false;
    }
}
