using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickedScoreScript : MonoBehaviour
{
    [SerializeField]
    private Text pickedCountText;
    [SerializeField]
    PlayerPossessionScript playerPossessionScript;

    private int pickedCount;

    void Update()
    {
        pickedCount = playerPossessionScript.GetTotalPossession();
        pickedCountText.text = pickedCount.ToString();
    }
}
