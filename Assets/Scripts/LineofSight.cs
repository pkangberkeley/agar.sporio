using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineofSight : MonoBehaviour
{
    //called when something enters the trigger collider
    private void OnTriggerEnter2D(Collider2D coll)
    {
        //check if coll is the player
        if (coll.CompareTag("Player"))
        {
            GetComponentInParent<EnemyParent>().player = coll.transform;
            UnityEngine.Debug.Log("SEE PLAYER RUN AT PLAYER");
        }
    }
    /*
    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            //GetComponentInParent<EnemyParent>().player = 
        }
    }
    */
}
