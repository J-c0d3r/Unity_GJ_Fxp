using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{  
    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();    
    }


    public void DestroyYourSelf()
    {
        anim.SetTrigger("destroy");
        Destroy(gameObject, 1f);
    }
}
