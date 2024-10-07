using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimitUIScript : MonoBehaviour
{
    [SerializeField]
    private Text timeLimitText;

    private float timeLimit;

    void Update()
    {
        timeLimit = Mathf.Floor(GameManager.gameManager.GetTimeLimit() + 1);
        timeLimitText.text = timeLimit.ToString();
    }
}
