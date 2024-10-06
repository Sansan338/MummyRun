using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPossessionScript : MonoBehaviour
{
    private int totalPossession;
    private int possession;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bandage" && (GameManager.GameState.Tutorial == GameManager.gameManager.GetGameState() 
            || GameManager.GameState.Play == GameManager.gameManager.GetGameState()))
        {
            possession += 1;
            totalPossession += 1;
        }
    }

    public int GetPossession()
    {
        return possession;
    }

    public int GetTotalPossession()
    {
        return totalPossession;
    }

    public void SetPossession(int minus)
    {
        possession -= minus;
    }
}
