using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberofHumansUIScript : MonoBehaviour
{
    [SerializeField]
    private Text humansCountText;

    private GameObject[] humans;

    void Update()
    {
        //タグが人間であるオブジェクトを見つけて表示させる
        humans = GameObject.FindGameObjectsWithTag("Human");
        humansCountText.text = humans.Length.ToString();
    }
}
