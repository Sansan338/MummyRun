using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    [SerializeField]
    GameObject teleporter;
    [SerializeField]
    GameObject player;
    [SerializeField]
    private int teleportTime;

    private float time;

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            time += Time.deltaTime;
        }

        if(time >= teleportTime)
        {
            Teleport();
            time = 0;
        }
    }

    private void Teleport()
    {
        player.transform.position = teleporter.transform.position;
    }
}
