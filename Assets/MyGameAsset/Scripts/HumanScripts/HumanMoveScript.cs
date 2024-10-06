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
    private float wanderingSpeed;     //�p�j�X�s�[�h
    [SerializeField]
    private float escapeSpeed;        //�����X�s�[�h
    [SerializeField]
    private float searchSpeed;        //���G���X�s�[�h
    [SerializeField]
    private float chaseSpeed;         //�`�F�C�X�X�s�[�h
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

    //�G�t�F�N�g
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
        //�ŏ��͐l�ԂȂ̂ŋ��Д͈͂͑��݂��Ȃ�
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
            //����̖ڕW�n�_�������_���Ŏw��
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
        //�v���C���[�~�C����AI�~�C���̋��Д͈͓��ɓ���Ɠ�����Ԃɓ���
        if(((collider.gameObject.tag == "ThreatRange" || collider.gameObject.tag == "AIThreatRange") && this.gameObject.tag == "Human") && currentHumanState != HumanState.Cocoon)
        {
            SetHumanState(HumanState.Escape);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        //���Д͈͂��痣���Ɠ�����Ԃ�����
        if(((collider.gameObject.tag == "ThreatRange" || collider.gameObject.tag == "AIThreatRange") && this.gameObject.tag == "Human") && currentHumanState != HumanState.Cocoon)
        {
            SetHumanState(HumanState.Wandering);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //�v���C���[�Ƀ~�C���ɂ���Ă��܂����Ƃ�
        if(collision.gameObject.tag == "Player" && this.gameObject.tag == "Human")
        {
            TransformationMummy();

            //����Ԃł���Ή�������
            if (cocoonMesh != null)
            {
                cocoonMesh.SetActive(false);
            }
        }

        //�~�C��AI�Ƀ~�C���ɂ���Ă��܂����Ƃ�
        if(collision.gameObject.tag == "MummyAI" && this.gameObject.tag == "Human")
        {
            SetHumanState(HumanState.Cocoon);
            TransformationCocoon();

            //�~�C�����ǂ������Ă��鏈������߂����
            var tmp = collision.gameObject.GetComponent<HumanMoveScript>();
            tmp.SetHumanState(HumanState.Search);
        }

        //��ђe������������
        if(collision.gameObject.tag == "Bullet" && this.gameObject.tag == "Human")
        {
            isStun = true;
            Instantiate(bandageHitEffect,this.gameObject.transform);
        }
    }

    private void TransformationMummy()
    {
        //�l�Ԃ���~�C���̎p�ɕϐg����
        Destroy(humanMesh);
        Instantiate(transformationEffect, new Vector3(this.transform.position.x,this.transform.position.y + 0.5f,this.transform.position.z )
            ,Quaternion.identity);
        mummyMesh.SetActive(true);
        this.tag = "MummyAI";
        this.threatObject.SetActive(true);

        //����̖ڕW�n�_�������_���Ŏw��
        randomPatrolPositionNumber = Random.Range(0, patrolPosition.Length);

        //�~�C���ɂȂ�A���G��ԂɈڍs
        SetHumanState(HumanState.Search);
    }

    private void TransformationCocoon()
    {
        //����Ԃɂ���
        Instantiate(transformationEffect, new Vector3(this.transform.position.x, this.transform.position.y + 0.5f, this.transform.position.z)
            , Quaternion.identity);
        cocoonMesh.SetActive(true);
    }

    //�l�Ԃ̏�Ԃ�ݒ�
    public void SetHumanState(HumanState humanState)
    {
        currentHumanState = humanState;
    }

    //�l�Ԃ̏�Ԃ��擾
    public HumanState GetHumanState()
    {
        return currentHumanState;
    }
}
