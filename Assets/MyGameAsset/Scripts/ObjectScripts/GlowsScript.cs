using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowsScript : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject startArea;

    void Start()
    {
        this.transform.position = startArea.transform.position;
    }

    
    void Update()
    {
        if(player != null && GameManager.gameManager.GetGameState() == GameManager.GameState.Play)
        {
            this.transform.position = player.transform.position;
        }
    }
}
