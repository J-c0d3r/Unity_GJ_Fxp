using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss_Sword : MonoBehaviour
{

    [SerializeField] private int damageSword;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(damageSword);
        }
    }
}
