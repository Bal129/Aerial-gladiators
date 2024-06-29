using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformType : MonoBehaviour
{
    public readonly static int Normal = 0;
    public readonly static int Saw = 1;
    public readonly static int Pithole = 2;
    public readonly static int DashReset = 3;
    public readonly static int Glue = 4;
    public readonly static int Spawner = 5;

    [Header("Details")]
    public int platformID;
    public string platformName;
    public GameObject platformPrefabModel;

    private Gladiator gladiator;

    /*
    public void DealWithPlayer(GameObject player)
    {
        gladiator = player.GetComponent<Gladiator>();

        switch(platformID)
        {
            case 1:
                SawBehaviour(gladiator);
                break;
            case 3:
                DashResetBehaviour(player);
                break;
            case 4:
                StartCoroutine(GlueBehaviour(player));
                break;
        }
    }

    void SawBehaviour(Gladiator gladiator)
    {
        gladiator.UpdateHealth(gladiator.currentHealth - 1); // MAY NEED TO CHANGE TO PROPER VALUE
        gladiator.StartIFrame();
    }

    void DashResetBehaviour(GameObject player)
    {

    }

    IEnumerator GlueBehaviour(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        yield return new WaitForSeconds(2.0f); // MAY NEED TO CHANGE TO PROPER VALUE
    }
    */
}
