using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    #endregion

    #region Physics_components
    Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float Damage;
    public float attackspeed = 1;
    float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    public bool isAttacking;
    Vector2 currDirection;
    public bool inAttackRange;
    private int numAttacks;
    private bool isStunned;
    private float stunTimer;
    public int stunTime;
    #endregion

    #region Health_variables
    public float maxHealth;
    float currHealth;
    #endregion

    #region Unity functions
    // Start is called before the first frame update
    void Start()
    {
        EnemyRB = GetComponent<Rigidbody2D>();

        currHealth = maxHealth;

        numAttacks = 0;
    }

    // Update is called once per frame
    void Update()
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
            numAttacks = 0;
        }
        if (isStunned)
        {
            UnityEngine.Debug.Log("I'm stunned!");
            if (stunTimer <= 0)
            {
                isStunned = false;
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

    #region Movement_functions
    private void Move()
    {
        //calculate movement vector: player position - enemy position = direction of player relative to enemy
        Vector2 direction = player.position - transform.position;

        EnemyRB.velocity = direction.normalized * movespeed;
        currDirection = EnemyRB.velocity;
    }
    #endregion

    #region Attack functions
    public void Attack()
    {
        numAttacks += 1;
        UnityEngine.Debug.Log("attacking_now");
        UnityEngine.Debug.Log(currDirection);
        attackTimer = attackspeed;
        // handles animations and hit boxes
        StartCoroutine(AttackRoutine());
    }
    IEnumerator AttackRoutine()
    {
        isAttacking = true;
        EnemyRB.velocity = Vector2.zero;

        //anim.SetTrigger("Attacktrig");

        yield return new WaitForSeconds(hitboxtiming);
        UnityEngine.Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(EnemyRB.position + currDirection, Vector2.one, 0f, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            UnityEngine.Debug.Log(hit.transform.name);
            if (hit.transform.CompareTag("Player"))
            {
                UnityEngine.Debug.Log("Tons of Damage");
                hit.transform.GetComponent<PlayerController>().TakeDamage(Damage);
            }
        }
        yield return new WaitForSeconds(hitboxtiming);
        isAttacking = false;

        yield return null;
    }
    #endregion

    #region Health_functions
    //enemy takes damage based on value param
    public void TakeDamage(float value)
    {
        //decrement health
        currHealth -= value;
        UnityEngine.Debug.Log("Health is now" + currHealth.ToString());

        //check for health
        if (currHealth <= 0)
        {
            Die();
        }
    }

    //destroys enemy object
    private void Die()
    {
        //Destroy game object
        Destroy(this.gameObject);
    }
    #endregion
}
