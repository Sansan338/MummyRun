using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WallTraptionScript : MonoBehaviour
{
    [SerializeField]
    private BandageScript bandageScript;
    [SerializeField]
    private float wallMoveSpeed;
    [SerializeField]
    private float maxWallHeight;

    private bool bandagePossessionFlag;

    void Update()
    {
        bandagePossessionFlag = bandageScript.GetPossessionFlag();
        if(bandagePossessionFlag == true && this.transform.position.y <= maxWallHeight)
        {
            this.transform.position += Vector3.up * wallMoveSpeed * Time.deltaTime;
        }

        if(this.transform.position.y >= maxWallHeight)
        {
            Destroy(this.gameObject);
        }
    }
}
