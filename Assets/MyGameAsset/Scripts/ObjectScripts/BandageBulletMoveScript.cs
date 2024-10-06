using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageBulletMoveScript : MonoBehaviour
{

    [SerializeField]
    private Rigidbody bandageRigidbody;
    [SerializeField]
    private float shotPower;

    public void Init(Vector3 moveForward)
    {
        bandageRigidbody.AddForce(moveForward * shotPower, ForceMode.Impulse);
    }
}
