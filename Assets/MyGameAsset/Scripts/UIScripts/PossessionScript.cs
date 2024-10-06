using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PossessionScript : MonoBehaviour
{
    [SerializeField]
    private PlayerPossessionScript playerPossessionScript;
    [SerializeField]
    private Text possessionText;

    private int possession;

    void Update()
    {
        possession = playerPossessionScript.GetPossession();
        possessionText.text = possession.ToString();
    }
}
