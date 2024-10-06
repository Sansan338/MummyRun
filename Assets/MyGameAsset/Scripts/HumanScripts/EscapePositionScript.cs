using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePositionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject mummy;
    [SerializeField]
    private GameObject escapePositionObject;

    private Vector3 escapePosition;
    private Vector3 startMummyPosition;

    void Start()
    {
        startMummyPosition = mummy.transform.position;
    }

    void Update()
    {
        if (mummy != null)
        {
            escapePosition = (this.transform.position - mummy.transform.position);
            escapePositionObject.transform.position = mummy.transform.position + (escapePosition * 1.1f);
        }
    }
}
