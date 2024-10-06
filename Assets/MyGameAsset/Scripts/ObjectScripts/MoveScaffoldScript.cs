using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScaffoldScript : MonoBehaviour
{
    [SerializeField]
    private Rigidbody scaffoldRigidBody;
    [SerializeField]
    private float movementWidth;
    [SerializeField]
    private float moveSpeed;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void FixedUpdate()
    {
        float positionZ = startPosition.z + Mathf.PingPong(Time.time * moveSpeed, movementWidth);

        this.transform.position = new Vector3(startPosition.x, startPosition.y, positionZ);
    }
}
