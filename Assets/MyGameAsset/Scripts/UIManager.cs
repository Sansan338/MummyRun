using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bandageExplanationUI;
    [SerializeField]
    private GameObject jumpPowerUI;
    [SerializeField]
    private GameObject possessionUI;
    [SerializeField]
    private GameObject humansCountUI;
    [SerializeField]
    private GameObject resultScoreUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject crosshair;

    [SerializeField]
    private BandageScript bandageScript;

    private bool bandagePossessionFlag;
    private bool deleteTextFlag;

    void Start()
    {
        deleteTextFlag = false;
        bandageExplanationUI.SetActive(false);
        jumpPowerUI.SetActive(false);
        possessionUI.SetActive(false);
        humansCountUI.SetActive(false);
        resultScoreUI.SetActive(false);
        pauseUI.SetActive(false);
        crosshair.SetActive(false);
    }

    void Update()
    {
        bandagePossessionFlag = bandageScript.GetPossessionFlag();

        if (bandagePossessionFlag == true && deleteTextFlag == false && bandageExplanationUI != null)
        {
            bandageExplanationUI.SetActive(true);
            possessionUI.SetActive(true);
            OnMouseDown();
        }

        if(GameManager.GameState.Play == GameManager.gameManager.GetGameState())
        {
            Destroy(bandageExplanationUI);
            humansCountUI.SetActive(true);
        }

        if(GameManager.GameState.GameOver == GameManager.gameManager.GetGameState() || GameManager.GameState.Clear == GameManager.gameManager.GetGameState())
        {
            resultScoreUI.SetActive(true);
            jumpPowerUI.SetActive(false);
            possessionUI.SetActive(false);
            humansCountUI.SetActive(false);
            if (bandageExplanationUI != null)
            {
                bandageExplanationUI.SetActive(false);
            }
        }

        //ポーズ画面
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale != 0 && GameManager.gameManager.GetGameState() == GameManager.GameState.Play)
            {
                pauseUI.SetActive(true);
                GameManager.gameManager.SetGameState(GameManager.GameState.Pause);
                Time.timeScale = 0;
            }
            else if(GameManager.gameManager.GetGameState() == GameManager.GameState.Pause)
            {
                pauseUI.SetActive(false);
                GameManager.gameManager.SetGameState(GameManager.GameState.Play);
                Time.timeScale = 1;
            }
        }

        //ジャンプパワーチャージ
        if(Input.GetKey(KeyCode.Space))
        {
            jumpPowerUI.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            Invoke(nameof(HiddenJumpPowerUI), 0.1f);
        }

        //クロスヘア
        if(Input.GetMouseButton(1) && GameManager.GameState.Play == GameManager.gameManager.GetGameState())
        {
            crosshair.SetActive(true);
        }
        if(!Input.GetMouseButton(1))
        {
            crosshair.SetActive(false);
        }
    }

    private void HiddenJumpPowerUI()
    {
        jumpPowerUI.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(bandageExplanationUI);
            deleteTextFlag = true;
        }
    }
}
