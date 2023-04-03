using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour
{
    public bool isRight;
    public float stopDistance;
    public float maxVision;
    private bool isPlayerInFront;
    private bool isRecovery;
    private bool isAttacking;
    private bool isStunning;

    [SerializeField] private float life;
    [SerializeField] private float veloc;
    [SerializeField] private float radius;
    [SerializeField] private int damageSword;


    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer sprite;
    private BoxCollider2D coll;
    private Vector2 direction;

    public Transform front;
    public Transform behind;

    [SerializeField] private GameObject attackP;

    [SerializeField] private Player player;

    [SerializeField] private DialogueTrigger dialogue;
  


    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim.SetInteger("transition", 0);

        if (isRight)
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.right;
            //attackP.transform.position *= Vector2.right;
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
            direction = Vector2.left;
            //attackP.transform.position *= Vector2.left;
        }
    }


    private void FixedUpdate()
    {
        if (!isStunning)
        {
            OnMove();
            FoundPlayer();
        }
    }


    void OnMove()
    {
        if (isPlayerInFront && !isAttacking)
        {
            anim.SetInteger("transition", 1);

            if (isRight)
            {
                transform.eulerAngles = new Vector2(0, 0);
                direction = Vector2.right;
                //attackP.transform.position *= Vector2.right;
                rig.velocity = new Vector2(veloc, rig.velocity.y);
            }
            else
            {
                transform.eulerAngles = new Vector2(0, 180);
                direction = Vector2.left;
                //attackP.transform.position *= Vector2.left;
                rig.velocity = new Vector2(-veloc, rig.velocity.y);
            }
        }
        else
        {
            rig.velocity = Vector2.zero;
        }
    }


    void FoundPlayer()
    {
        if (!isAttacking)
        {
            RaycastHit2D hitRight = Physics2D.Raycast(front.position, direction, maxVision);

            if (hitRight.collider != null)
            {
                if (hitRight.transform.CompareTag("Player"))
                {
                    isPlayerInFront = true;

                    float distance = Vector2.Distance(transform.position, hitRight.transform.position);

                    if (distance <= stopDistance && !isAttacking)
                    {
                        StartCoroutine(OnAttack());
                    }
                }
            }
            else
            {
                isPlayerInFront = false;
                rig.velocity = Vector2.zero;
                anim.SetInteger("transition", 0);
            }

            RaycastHit2D behindHit = Physics2D.Raycast(behind.position, -direction, maxVision);

            if (behindHit.collider != null)
            {
                if (behindHit.transform.CompareTag("Player"))
                {
                    isRight = !isRight;
                    isPlayerInFront = true;
                }
            }
        }
    }

    IEnumerator OnAttack()
    {
        isPlayerInFront = false;
        isAttacking = true;
        anim.SetInteger("transition", 2);
        yield return new WaitForSeconds(1.5f);
        isPlayerInFront = true;
        isAttacking = false;
        //yield return new WaitForSeconds(1f);

    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(front.position, direction * maxVision);
        Gizmos.DrawRay(behind.position, -direction * maxVision);
        Gizmos.DrawWireSphere(attackP.transform.position, radius);
    }


    public void TakeDamage(int dmg)
    {
        if (!isRecovery)
        {
            life -= dmg;

            if (life <= 0)
            {
                rig.gravityScale = 0f;
                rig.velocity = Vector2.zero;
                coll.enabled = false;

                player.isUnlockedDoubleJump = true;
                dialogue.isSkelletonBossDefeat = true;

                anim.SetTrigger("death");                
                veloc = 0f;
                Destroy(gameObject, 2.1f);
            }
            else
            {
                anim.SetInteger("transition", 3);
                StartCoroutine(OnTakeDamage());
            }
        }
    }

    IEnumerator OnTakeDamage()
    {
        isRecovery = true;
        isStunning = true;
        rig.gravityScale = 0f;
        rig.velocity = Vector2.zero;
        coll.enabled = false;

        yield return new WaitForSeconds(1.1f);

        coll.enabled = true;
        rig.gravityScale = 1f;
        isRecovery = false;
        isStunning = false;
    }


    public void BoxColliderOn()
    {
        //attackP.GetComponent<CircleCollider2D>().enabled = true;


        Collider2D[] hitList;

        hitList = Physics2D.OverlapCircleAll(attackP.transform.position, radius);

        foreach (Collider2D hit in hitList)
        {
            if (hit.GetComponent<Player>())
            {
                hit.GetComponent<Player>().TakeDamage(damageSword);
            }
        }
    }



    //public void BoxColliderOff()
    //{
    //    attackP.GetComponent<CircleCollider2D>().enabled = false;
    //}

}
