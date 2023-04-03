using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    public List<Transform> checkPoints = new List<Transform>();

    public int point = -1;

    public Transform playerT;

    public void PlayerRespawn()
    {
            playerT.position = checkPoints[point].position;        
    }

}
