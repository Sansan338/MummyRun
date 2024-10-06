using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class HumanMoveScript : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent human;
    [SerializeField]
    private Animator humanAnimator;
    [SerializeField]
    private float wanderingSpeed;     //徘徊スピード
    [SerializeField]
    private float escapeSpeed;        //逃走スピード
    [SerializeField]
    private float searchSpeed;        //索敵時スピード
    [SerializeField]
    private float chaseSpeed;         //チェイススピード
    [SerializeField]
    private float stunTime;
    [SerializeField]
    private float cocoonTime;
    [SerializeField]
    private GameObject humanMesh;
    [SerializeField]
    private GameObject mummyMesh;
    [SerializeField]
    private GameObject cocoonMesh;
    [SerializeField]
    private GameObject escapePositionObject;
    [SerializeField]
    private GameObject threatObject;
    [SerializeField]
    private AIThreatRangeScript AIThreatRangeScript;
    [SerializeField]
    private GameObject[] patrolPosition;

    //エフェクト
    [SerializeField]
    private GameObject transformationEffect;
    [SerializeField]
    private GameObject bandageHitEffect;

    private Vector3 randomWanderingPosition;
    private Vector3 nextEscapePosition;
    private Vector3 nextPatrolPosition;
    private int randomPatrolPositionNumber;

    private float stunTimeCount;
    private bool isStun;

    public enum HumanState
    {
        Wandering,
        Escape,
        Cocoon,
        Search,
        Chase
    }

    private HumanState currentHumanState;

    void Start()
    {
        humanAnimator.SetBool("Grounded", true);
        humanMesh.SetActive(true);
        mummyMesh.SetActive(false);
        cocoonMesh.SetActive(false);
        //最初は人間なので脅威範囲は存在しない
        this.threatObject.SetActive(false);

        stunTimeCount = 0;
        isStun = false;

        currentHumanState = HumanState.Wandering;
    }


    void Update()
    {
        humanAnimator.SetFloat("MoveSpeed", human.speed);

        if (human.remainingDistance < 0.5f && currentHumanState == HumanState.Wandering)
        {
            human.speed = wanderingSpeed;
            WanderingNextPosition();
        }

        if (currentHumanState == HumanState.Escape)
        {
            human.speed = escapeSpeed;
            EscapeNextPosition();
        }

        if(currentHumanState == HumanState.Cocoon)
        {
            human.speed = 0;
        }

        if (currentHumanState == HumanState.Search)
        {
            human.speed = searchSpeed;
            PatrolNextPosition();
        }

        if (currentHumanState == HumanState.Chase)
        {
            human.speed = chaseSpeed;
            ChaseNextPosition();
        }

        if(isStun == true)
        {
            Stun();
        }
    }

    void WanderingNextPosition()
    {
        randomWanderingPosition = new Vector3(Random.Range(this.transform.position.x - 10, this.transform.position.x + 10), 0
            , Random.Range(this.transform.position.z - 10, this.transform.position.z + 10));
        human.destination = randomWanderingPosition;
    }

    void EscapeNextPosition()
    {
        nextEscapePosition = escapePositionObject.transform.position;
        human.destination = nextEscapePosition;
    }

    void Stun()
    {
        stunTimeCount += Time.deltaTime;

        if (stunTimeCount <= stunTime)
        {
            human.speed = 0;
        }
        else if (stunTimeCount >= stunTime)
        {
            human.speed = escapeSpeed;
            isStun = false;
            stunTimeCount = 0;
        }
    }

    void PatrolNextPosition()
    {
        nextPatrolPosition = patrolPosition[randomPatrolPositionNumber].transform.position;
        human.destination = nextPatrolPosition;

        if(human.remainingDistance < 0.5f)
        {
            //巡回の目標地点をランダムで指定
            randomPatrolPositionNumber = Random.Range(0, patrolPosition.Length);
        }
    }

    void ChaseNextPosition()
    {
        human.destination = AIThreatRangeScript.GetChaseHuman().transform.position;
        if(AIThreatRangeScript.GetChaseHuman().gameObject.tag == "MummyAI" && currentHumanState != HumanState.Cocoon)
        {
            this.SetHumanState(HumanState.Search);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        //プレイヤーミイラかAIミイラの脅威範囲内に入ると逃走状態に入る
        if(((collider.gameObject.tag == "ThreatRange" || collider.gameObject.tag == "AIThreatRange") && this.gameObject.tag == "Human") && currentHumanState != HumanState.Cocoon)
        {
            SetHumanState(HumanState.Escape);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        //脅威範囲から離れると逃走状態を解除
        if(((collider.gameObject.tag == "ThreatRange" || collider.gameObject.tag == "AIThreatRange") && this.gameObject.tag == "Human") && currentHumanState != HumanState.Cocoon)
        {
            SetHumanState(HumanState.Wandering);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //プレイヤーにミイラにされてしまったとき
        if(collision.gameObject.tag == "Player" && this.gameObject.tag == "Human")
        {
            TransformationMummy();

            //繭状態であれば解除する
            if (cocoonMesh != null)
            {
                cocoonMesh.SetActive(false);
            }
        }

        //ミイラAIにミイラにされてしまったとき
        if(collision.gameObject.tag == "MummyAI" && this.gameObject.tag == "Human")
        {
            SetHumanState(HumanState.Cocoon);
            TransformationCocoon();

            //ミイラが追いかけてくる処理をやめされる
            var tmp = collision.gameObject.GetComponent<HumanMoveScript>();
            tmp.SetHumanState(HumanState.Search);
        }

        //包帯弾が当たった時
        if(collision.gameObject.tag == "Bullet" && this.gameObject.tag == "Human")
        {
            isStun = true;
            Instantiate(bandageHitEffect,this.gameObject.transform);
        }
    }

    private void TransformationMummy()
    {
        //人間からミイラの姿に変身する
        Destroy(humanMesh);
        Instantiate(transformationEffect, new Vector3(this.transform.position.x,this.transform.position.y + 0.5f,this.transform.position.z )
            ,Quaternion.identity);
        mummyMesh.SetActive(true);
        this.tag = "MummyAI";
        this.threatObject.SetActive(true);

        //巡回の目標地点をランダムで指定
        randomPatrolPositionNumber = Random.Range(0, patrolPosition.Length);

        //ミイラになり、索敵状態に移行
        SetHumanState(HumanState.Search);
    }

    private void TransformationCocoon()
    {
        //繭状態にする
        Instantiate(transformationEffect, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z)
            , Quaternion.identity);
        cocoonMesh.SetActive(true);
    }

    //人間の状態を設定
    public void SetHumanState(HumanState humanState)
    {
        currentHumanState = humanState;
    }

    //人間の状態を取得
    public HumanState GetHumanState()
    {
        return currentHumanState;
    }
}
