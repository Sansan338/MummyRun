using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultScoreScript : MonoBehaviour
{
    [SerializeField]
    private PlayerPossessionScript playerPossessionScript;
    [SerializeField]
    private Text totalScoreText;

    [SerializeField]
    private int catchMagnification;
    [SerializeField]
    private int possessionMagnification;
    [SerializeField]
    private float timeMagnification;

    private float totalScore;

    private int possessionScore;

    private int catchScore;

    private float timeScore;

    void Start()
    {
        totalScore = 0;
    }

    void Update()
    {
        possessionScore = playerPossessionScript.GetPossession();
        timeScore = GameManager.gameManager.GetTimeLimit();
        catchScore = GameObject.FindGameObjectsWithTag("MummyAI").Length;
        totalScore = Mathf.Floor((catchScore * catchMagnification) + (possessionScore * possessionMagnification) * (1 + timeScore * timeMagnification));
        totalScoreText.text = totalScore.ToString();
    }
}
