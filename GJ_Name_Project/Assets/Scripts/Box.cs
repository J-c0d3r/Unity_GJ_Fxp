using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{  
    private Animator anim;
    private BoxCollider2D coll;
    
    void Start()
    {
        anim = GetComponent<Animator>();    
        coll = GetComponent<BoxCollider2D>();
    }


    public void DestroyYourSelf()
    {
        coll.enabled = false;
        anim.SetTrigger("destroy");
        Destroy(gameObject, 1f);
    }
}
