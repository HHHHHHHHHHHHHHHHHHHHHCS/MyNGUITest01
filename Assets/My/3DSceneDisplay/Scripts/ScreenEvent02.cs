using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenEvent02 : MonoBehaviour
{
    private Transform targetTS;
    private Camera targetCamera;
    #region Build Setting
    private const string tag_towerEmpty = "TowerEmpty";
    private const string tag_tower = "Tower";
    private int layer_clickMask;
    #endregion

    #region Map Setting
    private readonly Vector4 clampMapBorder
        = new Vector4(85f, 85f, 60, 40);
    private Vector3 minBorder, maxBorder;

    private readonly Vector2 clampZoomScale
        = new Vector2(40, 80);
    #endregion

    #region Mouse Setting
    private const float checkMoveDistance = 150f;
    private const float moveSpeed = 1.5f;
    private const float zoomScaleSpeed = 33f;
    private const float stepDistanceTime = 100f;
    private const float minDampingDistance = 0.001f;
    private const float cancelDampingDistance = 5f;
    private const float maxMoveDistance = 3f;

    private bool isPress, isTwoTouch, startDamping, isOverUI;
    private float moveDistance, currentTime;
    private Vector2 oldPos1, oldPos2;
    private Vector3 targetPos, currentVelocity;

    #endregion

    #region Nav Setting
    private bool isNav;
    private Vector3 navPos, currentPos;
    private const float navTime = 1.5f;
    #endregion

    private void Awake()
    {
        layer_clickMask = LayerMask.GetMask("Tower", "Cloud");
        targetTS = transform;
        targetCamera = targetTS.GetComponent<Camera>();
        maxBorder = targetTS.localPosition+ targetTS.right * clampMapBorder.x + targetTS.up * clampMapBorder.z;
        minBorder = targetTS.localPosition - targetTS.right * clampMapBorder.y - targetTS.up * clampMapBorder.w;
        Debug.Log(minBorder);
        Debug.Log(maxBorder);
    }


    private void Update()
    {
        CheckPress();
        ScrollDamping();
        UpdateNavigation();
    }

    private void CheckPress()
    {
        if (isNav)
            return;
        var havePress = Input.GetMouseButton(0) || Input.GetMouseButtonUp(0);
        if (!(Input.touchCount > 0 || havePress))
        {
            isOverUI = false;
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isOverUI = isOverUI || UICamera.isOverUI;
        }
        if (!isOverUI)
        {
            bool isFiger = Input.touchCount > 0;

            if (Input.touchCount == 1 || (!isFiger && havePress))
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
                    moveDistance = 0;
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
        isPress = true;
        moveDistance = 0;
        oldPos1 = nowPos;
    }

    private void OnePressKeep(Vector2 nowPos)
    {
        Vector2 movePos = nowPos - oldPos1;
        MoveTarget(new Vector3(movePos.x, 0, movePos.y));
        oldPos1 = nowPos;
    }

    private void OnePressUp(Vector2 nowPos)
    {
        if (moveDistance <= maxMoveDistance)
        {
            StartRayCast(nowPos);
        }
        isPress = false;
        moveDistance = 0;
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
        
        Vector3 pos = targetTS.localPosition + (movePos.x * -transform.right+ movePos.z * transform.up) * moveSpeed * Time.deltaTime;
        //边界用
        pos.x = Mathf.Clamp(pos.x, maxBorder.x, minBorder.x);
        pos.y = Mathf.Clamp(pos.y, minBorder.y, maxBorder.y);
        pos.z = Mathf.Clamp(pos.z, minBorder.z, maxBorder.z);
        targetPos = pos;
        var dis = Vector3.Distance(targetPos, transform.localPosition);
        moveDistance += dis;
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

    private void StartRayCast(Vector2 nowPos)
    {
        RaycastHit hitInfo;
        Ray ray = targetCamera.ScreenPointToRay(nowPos);
        if (Physics.Raycast(ray, out hitInfo, 1000f, layer_clickMask))
        {
            GameObject gameObj = hitInfo.collider.gameObject;
            //var towerBase = gameObj.GetComponent<IClickEvent>();
            //if (towerBase != null)
            //{
            //    towerBase.OnClick();
            //}
        }
    }

    public void NavigationBuild(Vector3 buildPos)
    {
        isNav = true;
        Transform cameraTS = targetCamera.transform;
        Vector3 angle = cameraTS.eulerAngles;
        cameraTS.eulerAngles = new Vector3(90, 0, 0);

        Vector3 centerViewPos = targetCamera.WorldToViewportPoint(buildPos);
        centerViewPos.y -= Mathf.Cos(angle.x * Mathf.Deg2Rad);
        var centerPos = targetCamera.ViewportToWorldPoint(centerViewPos);

        cameraTS.eulerAngles = angle;
        navPos = new Vector3(centerPos.x, targetCamera.transform.position.y, centerPos.z);

    }

    private void UpdateNavigation()
    {
        if (isNav)
        {
            targetCamera.transform.position = Vector3.SmoothDamp(targetCamera.transform.position, navPos, ref currentPos, navTime);
            if (Vector3.SqrMagnitude(targetCamera.transform.position - navPos) < 1f)
            {
                isNav = false;
            }
        }
    }

}

