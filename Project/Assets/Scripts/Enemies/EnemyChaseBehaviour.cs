using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyChaseBehaviour : MonoBehaviour
{
    [Header("Configurations")]
    [SerializeField] GameObject enemyObject;
    private Enemy enemy;

    void Start()
    {
        enemy = enemyObject.GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enemy trigger enter");
        if (other.CompareTag("Player"))
        {
            enemyObject.transform.LookAt(other.gameObject.transform.position);
            enemyObject.GetComponent<Rigidbody>().AddForce(enemyObject.transform.forward * enemy.moveSpeed, ForceMode.VelocityChange);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Debug.Log("enemy trigger stay");
        if (other.CompareTag("Player"))
        {
            enemyObject.transform.LookAt(other.gameObject.transform.position);
            enemyObject.GetComponent<Rigidbody>().AddForce(enemyObject.transform.forward * enemy.moveSpeed, ForceMode.Acceleration);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemyObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }
}
