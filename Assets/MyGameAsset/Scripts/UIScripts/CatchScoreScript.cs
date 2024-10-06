using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CatchScoreScript : MonoBehaviour
{
    [SerializeField]
    private Text catchCountText;

    private int catchCount;

    void Update()
    {
        //catchCount = 
        catchCountText.text = catchCount.ToString();
    }
}
