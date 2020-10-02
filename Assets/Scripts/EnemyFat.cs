using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFat : EnemyParent
{
    #region Stun variables
    private int numAttacks = 0;
    private bool isStunned;
    private float stunTimer;
    public int stunTime;
    public GameObject stunObj;
    private GameObject stun;
    #endregion

    #region Unity functions
    // Update is called once per frame
    protected override void Update()
    {
        //check to see if we know where player is
        if (player == null)
        {
            return;
        }
        if (isAttacking)
        {
            return;
        }
        if (inAttackRange)
        {
            Attack();
        }
        Move();
    }
    #endregion

    #region Attack functions
    public override void Attack()
    {
        numAttacks += 1;
        UnityEngine.Debug.Log("attacking_now");
        UnityEngine.Debug.Log(currDirection);
        attackTimer = attackspeed;
        // handles animations and hit boxes
        StartCoroutine(AttackRoutine());
    }
    #endregion
}
