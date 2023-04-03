using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float movimentX;
    [SerializeField] private int life;
    [SerializeField] private int damageSword;
    [SerializeField] private float veloc;
    [SerializeField] private float jumpForce;
    [SerializeField] private float FallingThreshold;
    [SerializeField] private float radius;



    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;


    private bool isAlive = true;
    private bool isJumping;
    private bool doubleJump;
    private bool isRecovery;
    private bool isAttacking;
    private bool canMove = true;
    private bool canAttack = true;
    [SerializeField] private bool isLookingToRight;


    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer sprite;
    private CapsuleCollider2D coll;
    [SerializeField] private Transform atkPointRight;
    [SerializeField] private Transform atkPointLeft;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private CheckPoints checkP;

    public DialogueManager dialogue;


    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }


    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //dialogue.DisplayNextSentence();
        //}

        if (isAlive)
        {
            Move();
            Jump();
            Attack();
        }

    }

    private void FixedUpdate()
    {

    }

    private void Move()
    {
        if (isDashing)
        {
            return;
        }

        movimentX = Input.GetAxis("Horizontal");
        if (canMove)
        {
            rig.velocity = new Vector2(movimentX * veloc, rig.velocity.y);
        }
        else
        {
            rig.velocity = new Vector2(0f, 0f);
        }


        if (movimentX > 0)
        {
            sprite.flipX = false;
            isLookingToRight = true;

            if (!isJumping && !isAttacking && !isRecovery)
                anim.SetInteger("transition", 3);

        }

        if (movimentX < 0)
        {
            sprite.flipX = true;
            isLookingToRight = false;

            if (!isJumping && !isAttacking && !isRecovery)
                anim.SetInteger("transition", 3);

        }

        if (movimentX == 0 && !isJumping && !isAttacking && !isRecovery)
        {
            anim.SetInteger("transition", 0);
        }


        if (Input.GetButtonDown("Dash") && canDash)
        {
            StartCoroutine(OnDash());
        }

    }

    private void Jump()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetButtonDown("Jump") && !isAttacking && isAlive)
        {
            if (!isJumping)
            {
                anim.SetInteger("transition", 1);
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                isJumping = true;
                doubleJump = true;
                //play audio
            }
            else if (doubleJump)
            {
                anim.SetInteger("transition", 1);
                rig.velocity = new Vector2(rig.velocity.x, jumpForce);
                doubleJump = false;
                //play audio
            }
        }

        if (isJumping && rig.velocity.y < FallingThreshold)
        {
            anim.SetInteger("transition", 2);
        }

    }

    private void Attack()
    {
        if (isDashing)
        {
            return;
        }

        if (Input.GetButtonDown("Attack") && !isAttacking && !isJumping && canAttack && isAlive)
        {
            isAttacking = true;
            canMove = false;
            //play audio

            anim.SetInteger("transition", 4);

            Collider2D[] hitList;

            if (isLookingToRight)
            {
                hitList = Physics2D.OverlapCircleAll(atkPointRight.position, radius);
            }
            else
            {
                hitList = Physics2D.OverlapCircleAll(atkPointLeft.position, radius);
            }

            foreach (Collider2D hit in hitList)
            {
                if (hit.GetComponent<Skeleton>())
                {
                    hit.GetComponent<Skeleton>().TakeDamage(damageSword);
                }

                if (hit.GetComponent<Box>())
                {
                    hit.GetComponent<Box>().DestroyYourSelf();                   
                }

                if (hit.GetComponent<SkeletonBoss>())
                {
                    hit.GetComponent<SkeletonBoss>().TakeDamage(damageSword);
                }
            }


            StartCoroutine(OnAttack());
        }
    }

    IEnumerator OnAttack()
    {
        yield return new WaitForSeconds(0.28f);
        isAttacking = false;
        canMove = true;
    }

    public void TakeDamage(int dmg)
    {
        if (!isRecovery)
        {

            life -= dmg;

            if (life <= 0)
            {
                Die();
            }
            else
            {
                anim.SetInteger("transition", 7);
                StartCoroutine(OnRecover());
            }
        }
    }

    private void Die()
    {
        isAlive = false;
        anim.SetInteger("transition", 6);
        rig.velocity = Vector2.zero;
        rig.gravityScale = 0f;
        coll.enabled = false;
        //play audio
        //gameover
    }

    IEnumerator OnRecover()
    {
        isRecovery = true;
        yield return new WaitForSeconds(1f);
        isRecovery = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(atkPointRight.position, radius);
        Gizmos.DrawWireSphere(atkPointLeft.position, radius);
    }

    IEnumerator OnDash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rig.gravityScale;
        rig.gravityScale = 0f;

        anim.SetInteger("transition", 5);

        if (isLookingToRight)
        {
            rig.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        }
        else
        {
            rig.velocity = new Vector2(transform.localScale.x * -dashingPower, 0f);
        }

        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);

        tr.emitting = false;
        rig.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);

        canDash = true;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            isJumping = false;
            canAttack = true;
        }

        if (collision.gameObject.CompareTag("enemy"))
        {
            if (collision.gameObject.GetComponent<Skeleton>())
            {
                TakeDamage(collision.gameObject.GetComponent<Skeleton>().GetDamage());
            }
        }

        if (collision.gameObject.CompareTag("boss"))
        {
            TakeDamage(2);
        }

        if (collision.gameObject.CompareTag("spike"))
        {
            TakeDamage(1);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            canAttack = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("heart"))
        {
            life++;
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("deathArea"))
        {
            TakeDamage(1);
            checkP.PlayerRespawn();
        }

        if (collision.gameObject.CompareTag("checkPoint1"))
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            checkP.point++;
        }

        if (collision.gameObject.CompareTag("checkPoint2"))
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            checkP.point++;
        }

    }
}

