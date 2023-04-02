using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class Platform : MonoBehaviour
{

    [SerializeField] private int state;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();

        anim.SetInteger("state", state);

    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.position.y >= (transform.position.y + 0.5f))
        {
            collision.transform.SetParent(transform);
        }
        else
        {
            collision.transform.SetParent(null);
        }

        //collision.transform.SetParent(transform);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.transform.SetParent(null);
    }


}
