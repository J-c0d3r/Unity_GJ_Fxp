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

    [SerializeField] private float life;
    [SerializeField] private float veloc;


    private Animator anim;
    private Rigidbody2D rig;
    private SpriteRenderer sprite;
    private BoxCollider2D col;
    private Vector2 direction;

    public Transform front;
    public Transform behind;

    [SerializeField] private GameObject attackP;




    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();

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
        OnMove();
        FoundPlayer();
    }


    void OnMove()
    {
        if (isPlayerInFront)
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
        RaycastHit2D hitRight = Physics2D.Raycast(front.position, direction, maxVision);

        if (hitRight.collider != null)
        {
            if (hitRight.transform.CompareTag("Player"))
            {
                isPlayerInFront = true;

                float distance = Vector2.Distance(transform.position, hitRight.transform.position);

                if (distance <= stopDistance)
                {
                    isPlayerInFront = false;

                    anim.SetInteger("transition", 2);
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


    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(front.position, direction * maxVision);
        Gizmos.DrawRay(behind.position, -direction * maxVision);
    }


    public void TakeDamage(int dmg)
    {
        if (!isRecovery)
        {
            life -= dmg;

            StartCoroutine(OnTakeDamage());

            if (life <= 0)
            {
                anim.SetTrigger("death");
                veloc = 0f;
                Destroy(gameObject, 2.1f);
            }
        }
    }

    IEnumerator OnTakeDamage()
    {
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;

        isRecovery = false;
    }


    public void BoxColliderOn()
    {
        attackP.GetComponent<BoxCollider2D>().enabled = true;
    }


    public void BoxColliderOff()
    {
        attackP.GetComponent<BoxCollider2D>().enabled = false;
    }

}
