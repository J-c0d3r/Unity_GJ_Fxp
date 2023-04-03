using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class GameManager : MonoBehaviour
{
    private int qtyHeart = 3;

    [SerializeField] private GameObject gameover;
    [SerializeField] private List<GameObject> lifesList = new List<GameObject>();
    //[SerializeField] private GameObject lightGlobal;



    //public void IncreaseLightGlobal(float intensite)
    //{
    //    lightGlobal.GetComponent<Light2DBase>().
    //}

    public void DecreaseHeart()
    {
        qtyHeart--;

        if (qtyHeart >= 0)
        {
            lifesList[qtyHeart + 1].SetActive(false);
        }
    }

    public void IncreaseHeart()
    {
        qtyHeart++;

        if (qtyHeart <= 3)
        {
            lifesList[qtyHeart].SetActive(true);
        }
    }



    public void GameOver()
    {
        StartCoroutine(GameOverScreeen());
    }

    IEnumerator GameOverScreeen()
    {
        yield return new WaitForSeconds(1.2f);
        gameover.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

}
