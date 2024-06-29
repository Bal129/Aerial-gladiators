using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashReset : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Trigger Entered!");
            other.gameObject.GetComponent<Gladiator>().isPowerDashing = false;
        }
        
    }
}
