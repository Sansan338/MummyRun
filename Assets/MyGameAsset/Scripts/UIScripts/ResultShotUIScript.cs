using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultShotUIScript : MonoBehaviour
{
    [SerializeField]
    BandageShotScript bandageShotScript;
    [SerializeField]
    private Text shotCoutText;

    private int shotCount;

    void Update()
    {
        shotCount = bandageShotScript.GetShotCount();
        shotCoutText.text = shotCount.ToString();
    }
}
