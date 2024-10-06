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
        //�^�O���l�Ԃł���I�u�W�F�N�g�������ĕ\��������
        humans = GameObject.FindGameObjectsWithTag("Human");
        humansCountText.text = humans.Length.ToString();
    }
}
