using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    [SerializeField]
    private Rigidbody playerRigidbody;
    [SerializeField]
    private Animator playerAnimator;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float sprintSpeed;
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float minJumpPower;
    [SerializeField]
    private float chargeMultiplier;
    [SerializeField]
    private float maxCharge;
    [SerializeField]
    private Transform moveParent;
    [SerializeField]
    private GameObject deathEffect;

    private float moveSpeed;

    private bool isGround;

    private Vector3 moveFoward;

    private float currentJumpPower;

    private float time;

    private void Start()
    {
        currentJumpPower = minJumpPower;
    }

    void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime;

        Vector3 cameraFoward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        moveFoward = cameraFoward * moveZ + Camera.main.transform.right * moveX;

        if (moveX != 0 || moveZ != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = sprintSpeed;
            }
            else
            {
                moveSpeed = walkSpeed;
            }
        }
        else
        {
            moveSpeed = 0;
        }

        playerRigidbody.velocity = moveFoward.normalized * moveSpeed + new Vector3(0, playerRigidbody.velocity.y, 0);
    }

    void Update ()
    {
        if (moveFoward != Vector3.zero)
        {
            transform.forward = Vector3.Slerp(transform.forward, moveFoward, rotateSpeed * Time.deltaTime);
        }

        //ジャンプ
        if (isGround == true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                time += Time.deltaTime;
                if (time < maxCharge)
                {
                    currentJumpPower += (time * chargeMultiplier * Time.deltaTime);
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isGround = false;
                playerRigidbody.AddForce(Vector3.up * currentJumpPower, ForceMode.Impulse);
                currentJumpPower = minJumpPower;
                time = 0;
            }
        }

        //移動スピードに合わせてアニメーションを変化
        playerAnimator.SetFloat("Speed", moveSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }

        //ステージ外に行くと死亡
        if (collision.gameObject.tag == "DeathZone")
        {
            Instantiate(deathEffect, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "GameStartArea")
        {
            GameManager.gameManager.SetGameState(GameManager.GameState.Play);
        }
        if (collider.gameObject.tag == "Ground" || collider.gameObject.tag == "MoveObject")
        {
            isGround = true;
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.tag == "MoveObject")
        {
            this.transform.SetParent(moveParent);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if(collider.gameObject.tag == "MoveObject")
        {
            this.transform.SetParent(null);
        }
    }

    public float GetChargeTime()
    {
        return time;
    }

    public float GetMaxCharge()
    {
        return maxCharge;
    }
}
