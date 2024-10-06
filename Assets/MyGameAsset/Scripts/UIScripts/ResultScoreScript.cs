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

    private float totalScore;

    private int possessionScore;

    void Start()
    {
        totalScore = 0;
    }

    void Update()
    {
        possessionScore = playerPossessionScript.GetPossession();
        totalScore = possessionScore * 100;
        totalScoreText.text = totalScore.ToString();
    }
}
