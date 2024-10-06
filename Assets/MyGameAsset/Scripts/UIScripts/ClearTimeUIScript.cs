using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClearTimeUIScript : MonoBehaviour
{
    [SerializeField]
    private Text timeCountText;

    private float timeCount;


    void Update()
    {
        timeCount = GameManager.gameManager.GetTime();
        var time = Mathf.Floor(timeCount);
        timeCountText.text = time.ToString();
    }
}
