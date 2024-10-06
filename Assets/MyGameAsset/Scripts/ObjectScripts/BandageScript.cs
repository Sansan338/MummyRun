using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageScript : MonoBehaviour
{
    [SerializeField]
    private GameObject getEffect;

    private bool possessionFlag;

    public bool PossessionFlag
    {
        get { return possessionFlag; }
    }

    private void Start()
    {
        possessionFlag = false;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //Žæ“¾‚µ‚½‚±‚Æ‚ª‚È‚¯‚ê‚Îtrue‚ð•Ô‚·
            if(possessionFlag == false)
            {
                possessionFlag = true;
            }

            Instantiate(getEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    public bool GetPossessionFlag()
    {
        return possessionFlag;
    }
}
