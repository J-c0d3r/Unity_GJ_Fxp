using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
