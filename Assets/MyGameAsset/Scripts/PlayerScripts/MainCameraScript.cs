using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainCameraScript : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private GameObject target;
    [SerializeField]
    private GameObject shotPositionTarget;
    [SerializeField]
    private Transform shotPosition;

    private Vector3 mainCamera;

    private Vector3 currentPosition;
    private Vector3 pastPosition;

    private Vector3 difference;

    private float upperLimit = 50.0f;
    private float underLimit = -40.0f;

    private Vector3 wallHitPosition;
    private RaycastHit wallHit;
    private float distance;

    [SerializeField]
    private LayerMask wallLayer;

    void Start()
    {
        mainCamera = cameraTransform.localEulerAngles;
        pastPosition = target.transform.position;
        distance = (this.transform.position - target.transform.position).sqrMagnitude;
    }

    void Update()
    {
        if (GameManager.GameState.GameOver != GameManager.gameManager.GetGameState() && target != null && GameManager.GameState.Pause != GameManager.gameManager.GetGameState())
        {
            currentPosition = target.transform.position;
            difference = currentPosition - pastPosition;
            transform.position = Vector3.Lerp(transform.position, transform.position + difference, 1.0f);
            pastPosition = currentPosition;

            WallCheak();

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            var x = mainCamera.y - mouseY;

            if (Mathf.Abs(mouseX) > 0.01f)
            {
                transform.RotateAround(target.transform.position, Vector3.up, mouseX);
                //包帯発射ポジションの回転(横)
                shotPosition.RotateAround(shotPositionTarget.transform.position,Vector3.up, mouseX);
            }
            if (Mathf.Abs(mouseY) > 0.01f && x <= upperLimit && x >= underLimit)
            {
                mainCamera.y = x;
                transform.RotateAround(target.transform.position, transform.right, -mouseY);
                //包帯発射ポジションの回転(縦)
                shotPosition.RotateAround(shotPositionTarget.transform.position,transform.right, -mouseY);
            }
        }
    }

    private bool WallCheak()
    {
        if(Physics.Raycast(target.transform.position, this.transform.position - target.transform.position, out wallHit, distance/2, wallLayer))
        {
            wallHitPosition = wallHit.point;
            transform.position = wallHitPosition;
            return true;
        }
        else
        {
            return false;
        }
    }
}
