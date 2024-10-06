using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreatRangeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mummy;

    void Update()
    {
        if(mummy != null)
        {
            this.transform.position = mummy.transform.position;
        }
    }
}
