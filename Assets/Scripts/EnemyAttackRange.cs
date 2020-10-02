using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    //called when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check if coll is the player
        if (coll.gameObject.tag == "Player")
        {
            GetComponentInParent<EnemyParent>().inAttackRange = true;
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GetComponentInParent<EnemyParent>().inAttackRange = false;
        }
    }
}
