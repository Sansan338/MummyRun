using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private float timeCount;

    private int humansCount;

    public enum GameState
    {
        Tutorial,
        Pause,
        Play,
        GameOver,
        Clear
    }

    private GameState currentGameState;
    public static GameManager gameManager;

    void Start()
    {
        //フレームレートを固定
        Application.targetFrameRate = 60;

        timeCount = 0;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gameManager = this;
        SetGameState(GameState.Tutorial);
    }

    void Update()
    {
        if (GameManager.GameState.Play == GameManager.gameManager.GetGameState())
        {
            timeCount += Time.deltaTime;
        }

        humansCount = GameObject.FindGameObjectsWithTag("Human").Length;

        if(humansCount <= 0)
        {
            currentGameState = GameState.Clear;
        }

        if(currentGameState == GameState.GameOver)
        {
            var mummyAI = GameObject.FindGameObjectsWithTag("MummyAI");
            foreach(GameObject npc in mummyAI)
            {
                Destroy(npc);
            }
        }

        if (currentGameState == GameState.Clear)
        {
            Time.timeScale = 0;
        }
    }

    public void SetGameState(GameState gameState)
    {
        currentGameState = gameState;
    }

    public GameState GetGameState()
    {
        return currentGameState;
    }

    public float GetTime()
    {
        return timeCount;
    }
}
