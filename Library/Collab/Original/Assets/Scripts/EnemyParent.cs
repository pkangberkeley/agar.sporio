using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParent : MonoBehaviour
{
    #region Movement_variables
    public float movespeed;
    #endregion

    #region Physics_components
    protected Rigidbody2D EnemyRB;
    #endregion

    #region Targeting_variables
    public Transform player;
    #endregion

    #region Attack_variables
    public float Damage;
    public float attackspeed = 1;
    protected float attackTimer;
    public float hitboxtiming;
    public float endanimationtiming;
    public bool isAttacking;
    protected Vector2 currDirection;
    public bool inAttackRange;
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
    }

    // Update is called once per frame
    protected virtual void Update()
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
    }

    private void FixedUpdate()
    {
        Move();
    }
    #endregion

    #region Movement_functions
    protected virtual void Move()
    {
        //calculate movement vector: player position - enemy position = direction of player relative to enemy
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        EnemyRB.MovePosition((Vector2)transform.position + (direction * movespeed * Time.deltaTime));
        currDirection = direction * movespeed;
        Debug.Log("Move Function activated");
        Debug.Log(EnemyRB.velocity);
    }
    #endregion

    #region Attack functions
    public virtual void Attack()
    {
        UnityEngine.Debug.Log("attacking_now");
        UnityEngine.Debug.Log(currDirection);
        attackTimer = attackspeed;
        // handles animations and hit boxes
        StartCoroutine(AttackRoutine());
    }
    protected IEnumerator AttackRoutine()
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
