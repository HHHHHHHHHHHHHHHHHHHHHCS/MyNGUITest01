using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RayCheck3D : MonoBehaviour
{
    private Transform targetTS;
    private Camera targetCamera;
    #region Build Setting
    private const string towerEmpty = "TowerEmpty";
    private const string tower = "Tower";
    private int towerMask;
    #endregion

    #region Map Setting
    private readonly Vector4 clampMapBorder
        = new Vector4(-40, 40, -75, 40);

    private readonly Vector2 clampZoomScale
        = new Vector2(40, 80);
    #endregion

    #region Mouse Setting
    private const float checkMoveDistance = 150f;
    private const float moveSpeed = 5f;
    private const float zoomScaleSpeed = 33f;
    private const float stepDistanceTime = 100f;
    private const float minDampingDistance = 0.001f;
    private const float cancelDampingDistance = 5f;

    private bool isPress, isTwoTouch, startDamping, isOverUI;
    private float moveDistance, currentTime;
    private Vector2 oldPos1, oldPos2;
    private Vector3 targetPos, currentVelocity;

    #endregion


    private void Awake()
    {
        towerMask = LayerMask.GetMask("Tower");
        targetTS = transform;
        targetCamera = targetTS.GetComponent<Camera>();
    }


    private void Update()
    {
        CheckPress();
        ScrollDamping();
    }

    private void CheckPress()
    {
        if (!(Input.touchCount > 0 || Input.GetMouseButton(0)))
        {
            isOverUI = false;
            return;
        }

        if(Input.GetMouseButtonDown(0))
        {
            isOverUI = isOverUI||UICamera.isOverUI;
        }
        if (!isOverUI)
        {
            bool isFiger = Input.touchCount > 0;

            if (Input.touchCount == 1 || (!isFiger && Input.GetMouseButton(0)))
            {
                TouchPhase touchPhase = TouchPhase.Canceled;
                Vector2 nowPos;
                if (isFiger)
                {
                    Touch touch1 = Input.GetTouch(0);
                    touchPhase = touch1.phase;
                    nowPos = touch1.position;
                }
                else
                {
                    nowPos = Input.mousePosition;
                }

                if (isTwoTouch)
                {
                    oldPos1 = nowPos;
                    isTwoTouch = false;
                    return;
                }

                if (Input.GetMouseButtonDown(0) || touchPhase == TouchPhase.Began)
                {
                    OnePressDown(nowPos);
                }
                else if (Input.GetMouseButtonUp(0) || touchPhase == TouchPhase.Ended)
                {
                    OnePressUp(nowPos);
                }
                else if (isPress && (Input.GetMouseButton(0) || touchPhase == TouchPhase.Stationary
                    || touchPhase == TouchPhase.Moved))
                {
                    OnePressKeep(nowPos);
                }
            }
            else if (Input.touchCount >= 2)
            {
                TwoTouch();
            }
        }
    }

    private void OnePressDown(Vector2 nowPos)
    {
        if (!UICamera.isOverUI)
        {
            isPress = true;
            moveDistance = 0;
            oldPos1 = nowPos;
        }
    }

    private void OnePressKeep(Vector2 nowPos)
    {
        Vector2 movePos = nowPos - oldPos1;
        MoveTarget(new Vector3(movePos.x, 0, movePos.y));
        oldPos1 = nowPos;
    }

    private void OnePressUp(Vector2 nowPos)
    {
        isPress = false;
    }

    private void TwoTouch()
    {
        isTwoTouch = true;
        Touch newTouch1 = Input.GetTouch(0);
        Touch newTouch2 = Input.GetTouch(1);

        if (newTouch2.phase == TouchPhase.Began)
        {
            oldPos2 = newTouch2.position;
            oldPos1 = newTouch1.position;
            return;
        }

        float oldDistance = Vector2.Distance(oldPos1, oldPos2);
        float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);
        float offset = newDistance - oldDistance;

        ZoomCamera(offset);

        oldPos1 = newTouch1.position;
        oldPos2 = newTouch2.position;
    }

    private void MoveTarget(Vector3 movePos)
    {
        startDamping = false;

        Vector3 pos = targetTS.localPosition - movePos * moveSpeed * Time.deltaTime;
        //边界用
        //pos.x = Mathf.Clamp(pos.x, clampMapBorder.x, clampMapBorder.y);
        //pos.z = Mathf.Clamp(pos.z, clampMapBorder.z, clampMapBorder.w);
        targetPos = pos;
        var dis = Vector3.Distance(targetPos, transform.localPosition);

        if (dis < minDampingDistance * Time.deltaTime)
        {
            startDamping = false;
            transform.localPosition = targetPos;
        }
        else
        {
            currentTime = dis / stepDistanceTime;
            startDamping = true;
        }

    }

    private void ZoomCamera(float offset)
    {
        float scaleFactor = offset / zoomScaleSpeed;
        float endView = Mathf.Clamp((targetCamera.fieldOfView + scaleFactor)
            , clampZoomScale.x, clampZoomScale.y);
        targetCamera.fieldOfView = endView;
    }

    private void ScrollDamping()
    {
        if (startDamping)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPos, ref currentVelocity, currentTime);
            if (Vector3.Distance(transform.localPosition, targetPos) <= minDampingDistance)
            {
                startDamping = false;
            }
        }
    }
}

