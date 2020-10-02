using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuff : EnemyParent
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
        //clean this up later? stunning:
        if (numAttacks == 3)
        {
            stunTimer = stunTime;
            isStunned = true;
            stun = Instantiate(stunObj, transform.position + new Vector3(0f, 1.5f, 0), transform.rotation);
            numAttacks = 0;
        }
        if (isStunned)
        {
            anim.SetBool("Moving", false);
            UnityEngine.Debug.Log("I'm stunned!");
            if (stunTimer <= 0)
            {
                isStunned = false;
                Destroy(stun);
                anim.SetBool("Moving", true);
            }
            EnemyRB.velocity = Vector2.zero;
            stunTimer -= Time.deltaTime;
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
