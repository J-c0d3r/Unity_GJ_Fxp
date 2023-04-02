using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour
{
    [SerializeField] private float life;
    [SerializeField] private float veloc;
    [SerializeField] private int damage;

    private Animator anim;

    public Transform rightB;
    public Transform leftB;
    private Rigidbody2D rig;
    private SpriteRenderer sprite;
    private CapsuleCollider2D col;
    [SerializeField] private GameObject gameO;


    void Start()
    {
        anim = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        col = GetComponent<CapsuleCollider2D>();
    }

    private void FixedUpdate()
    {
        rig.velocity = new Vector2(veloc, 0f);
    }

    public void TakeDamage(int dmg)
    {
        life -= dmg;

        if (life <= 0)
        {
            anim.SetTrigger("death");
            veloc = 0f;
            Destroy(gameO, 0.9f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("barrierEnemy"))
        {
            veloc *= -1;
            sprite.flipX = !sprite.flipX;
            col.offset = new Vector2(col.offset.x * -1, col.offset.y);
        }
    }

    public int GetDamage()
    {
        return damage;
    }

}
