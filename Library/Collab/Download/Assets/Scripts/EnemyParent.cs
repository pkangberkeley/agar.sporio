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

    #region Animation_components
    protected Animator anim;
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

    #region Element variables
    //dictionary storing weakness of each type
    protected static readonly Dictionary<string, List<string>> weaknesses = new Dictionary<string, List<string>>
    {
        { "Fire", new List<string> {"Water", "Air" }},
        { "Water", new List<string> { "Earth", "Lightning" } },
        { "Earth", new List<string> { "Fire"} },
        { "Air", new List<string> { "Lightning"} },
        { "Lightning", new List<string> { "Air"} }
    };

    public string element;
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

        anim = GetComponent<Animator>();
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
        Move();
    }
    #endregion

    #region Movement_functions
    protected void Move()
    {
        anim.SetBool("Moving", true);
        //calculate movement vector: player position - enemy position = direction of player relative to enemy
        Vector2 direction = player.position - transform.position;
        direction.Normalize();
        EnemyRB.MovePosition((Vector2)transform.position + (direction * movespeed * Time.deltaTime));
        currDirection = direction * movespeed;
        if ((currDirection.x < 0 && transform.localScale.x > 0) || (currDirection.x > 0 && transform.localScale.x < 0))
        {
            Vector3 flip = transform.localScale;
            flip.x *= -1;
            transform.localScale = flip;
        }
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
        anim.SetTrigger("AttackTrig");

        yield return new WaitForSeconds(hitboxtiming);
        UnityEngine.Debug.Log("Casting hitbox now");
        RaycastHit2D[] hits = Physics2D.BoxCastAll(EnemyRB.position + currDirection, new Vector2(3,3), 0f, Vector2.zero);

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
    public void TakeDamage(float value, string elementDamage)
    {
        //decrement health if correct element
        if (weaknesses[element].Contains(elementDamage))
        {
            currHealth -= value;
                    UnityEngine.Debug.Log("Health is now" + currHealth.ToString());
        }
        
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
