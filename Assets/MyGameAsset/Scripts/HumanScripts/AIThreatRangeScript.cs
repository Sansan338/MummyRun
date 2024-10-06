using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIThreatRangeScript : MonoBehaviour
{
    [SerializeField]
    private HumanMoveScript humanMoveScript;
    [SerializeField]
    private GameObject mummyAI;

    private GameObject chaseHuman;
    public GameObject ChaseHuman
    {
        get { return chaseHuman; }
    }

    void Update()
    {
        this.transform.position = mummyAI.transform.position;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Human")
        {
            chaseHuman = collider.gameObject;
            humanMoveScript.SetHumanState(HumanMoveScript.HumanState.Chase);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "Human")
        {
            humanMoveScript.SetHumanState(HumanMoveScript.HumanState.Search);
        }
    }

    public GameObject GetChaseHuman()
    {
        return chaseHuman;
    }
}
