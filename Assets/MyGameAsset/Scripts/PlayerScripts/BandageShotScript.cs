using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageShotScript : MonoBehaviour
{
    [SerializeField]
    private PlayerPossessionScript playerPossessionScript;
    [SerializeField]
    private GameObject bandage;
    [SerializeField]
    private float destroyTime;
    [SerializeField]
    private GameObject shotPositionTarget;

    private int shotCount;

    void Update()
    {
        this.transform.position = new Vector3(shotPositionTarget.transform.position.x, 
             + shotPositionTarget.transform.position.y, shotPositionTarget.transform.position.z);

        if (Input.GetMouseButton(1) && Input.GetMouseButtonDown(0) && playerPossessionScript.GetPossession() > 0
            && GameManager.GameState.Play == GameManager.gameManager.GetGameState())
        {
            var bandageObject = Instantiate(bandage, this.transform.position ,Quaternion.identity);
            bandageObject.GetComponent<BandageBulletMoveScript>().Init(transform.forward);
            playerPossessionScript.SetPossession(1);
            Destroy(bandageObject,destroyTime);

            shotCount++;
        }
    }

    public int GetShotCount()
    {
        return shotCount;
    }
}
