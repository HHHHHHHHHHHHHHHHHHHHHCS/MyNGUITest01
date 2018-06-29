using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 3D世界摄像机事件
/// 包括移动和点击射线
/// </summary>
public class CameraScrollBy3D : MonoBehaviour
{
    private Transform targetTS;//要移动的目标
    private Camera targetCamera;//摄像机 用来做射线检测

    private readonly Vector3 startCameraPos = new Vector3(-20, 50, -13);//摄像机初始位置
    private readonly Vector3 startCameraRot = new Vector3(60, 0, 0);//摄像机初始的旋转
    private readonly Vector2 clampZoomScale = new Vector2(40, 80);//限制摄像机的缩放大小  最小最大
    private readonly Vector4 nessClampBorder = new Vector4(-45, 205, -100, 60);//滚动的时候边界 左右下上
    private readonly Vector4 clampClampBorder = new Vector4(-60, 220, -115, 75);//移动的时候边界 左右下上


    private const float moveSpeed = 4//摄像机的移动速度
        , scrollDistance = 20//滚动开检测的距离
        , scrollNess = 100//阻止滚动的力量大小
        , zoomScaleSpeed = 33//缩放的速度
        , borderNessTime = 0.25f//超过了阻尼边界返回的时间
        , borderBackSpeed = 25//边界返回的速度
        , moveMaxDistance = 10//如果不小心移动了,射线检测的最大距离
        , baseZoomScale = 60;//摄像机基础的缩放大小
    private const int queMaxNumber = 10;//队列最大数量

    private GameObject clickObj;//点击的建筑物体
    private int layermask;//射线检测的tower layermask
    private float moveDistance;//当前移动的距离
    private Vector2 lastPos;//最后的位置
    private bool isStayTouch, isTwoTouch;//是否一直按着,是否两个手指按着
    private LinkedList<Vector2> vec2List;//记录鼠标移动的位置
    private Vector2 scrollAccele, nessScrollAccele, borderSpeed, nessBorderAccele, oldPos1, oldPos2;

    private void Awake()
    {
        targetTS = transform;
        layermask = LayerMask.GetMask("Tower");
        targetCamera = targetTS.GetComponent<Camera>();
        targetTS.position = startCameraPos;
        targetTS.eulerAngles = startCameraRot;
        vec2List = new LinkedList<Vector2>();
    }

    private void Update()
    {
        CheckTouch();
        UpdateScrollScreen();
    }


    /// <summary>
    /// 检测touch  是鼠标还是手指  手指是几根
    /// </summary>
    private void CheckTouch()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        OnOneTouchEvent();
#else
        if (Input.touchCount >= 2)
        {
            OnTwoTouchEvent();
        }
        else if (Input.touchCount == 1)
        {
            OnOneTouchEvent();
        }
        else if (Input.touchCount == 1)
        {
        isTwoTouch=false;
        }
#endif
    }

    /// <summary>
    /// 一个点击事件的时候执行
    /// </summary>
    private void OnOneTouchEvent()
    {
        if (UICamera.isOverUI)
        {
            return;
        }

        Vector2 nowPos = Input.mousePosition;
        if (isTwoTouch)
        {
            OnePress(nowPos);
            isTwoTouch = false;
        }
        else if (Input.GetMouseButtonDown(0))
        {
            OnePress(nowPos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            OneUp(nowPos);
        }
        else if (Input.GetMouseButton(0))
        {
            OneStay(nowPos);
        }
    }

    /// <summary>
    /// 一个手指按下的时候
    /// </summary>
    /// <param name="nowPos"></param>
    private void OnePress(Vector2 nowPos)
    {
        lastPos = nowPos;
        isStayTouch = true;
        scrollAccele = Vector2.zero;
        nessScrollAccele = Vector2.zero;
        borderSpeed = Vector2.zero;
        moveDistance = 0;
        vec2List.Clear();
        AddQueue(nowPos);
        StartRayCast(nowPos, true);
    }

    /// <summary>
    /// 一个手指抬起的时候
    /// </summary>
    /// <param name="nowPos"></param>
    private void OneUp(Vector2 nowPos)
    {
        isStayTouch = false;
        AddQueue(nowPos);
        MoveDir(Vector2.zero, false, true);
        if (!StartRayCast(nowPos, false))
        {
            ScrollScreen();
        }
    }

    /// <summary>
    /// 一个手指持续在屏幕上的时候
    /// </summary>
    /// <param name="nowPos"></param>
    private void OneStay(Vector2 nowPos)
    {
        AddQueue(nowPos);
        Vector2 newVec2 = nowPos - lastPos;
        MoveDir(newVec2);
        lastPos = nowPos;
    }

    /// <summary>
    /// 滚动屏幕
    /// </summary>
    private void ScrollScreen()
    {
        if (vec2List.Count > 1)
        {
            Vector2 dir = vec2List.Last.Value - vec2List.First.Value;
            float distance = Vector2.Distance(dir, Vector2.zero);
            if (distance >= scrollDistance)
            {
                scrollAccele = dir / Time.deltaTime / queMaxNumber / 50;
                nessScrollAccele = -scrollAccele.normalized * scrollNess;
            }
        }
    }

    /// <summary>
    /// 记录鼠标的位置的队列
    /// </summary>
    /// <param name="nowPos"></param>
    private void AddQueue(Vector2 nowPos)
    {
        if (vec2List.Count >= queMaxNumber)
        {
            vec2List.RemoveFirst();
        }
        vec2List.AddLast(nowPos);
    }

    /// <summary>
    /// 执行屏幕滚动
    /// </summary>
    private void UpdateScrollScreen()
    {
        if (scrollAccele != Vector2.zero)
        {
            scrollAccele += nessScrollAccele * Time.deltaTime;
            if (Vector2.Dot(scrollAccele, nessScrollAccele) > 0)
            {
                scrollAccele = Vector2.zero;
            }
            else
            {
                MoveDir(scrollAccele, true);
            }
        }
        if (borderSpeed != Vector2.zero)
        {
            float tempX = borderSpeed.x + nessBorderAccele.x * Time.deltaTime;
            if (tempX * borderSpeed.x < 0
                || (targetTS.position.x >= nessClampBorder.x && targetTS.position.x <= nessClampBorder.y))
            {
                borderSpeed.x = 0;
            }
            else
            {
                borderSpeed.x = tempX;
            }

            float tempY = borderSpeed.y + nessBorderAccele.y * Time.deltaTime;
            if (tempY * borderSpeed.y < 0
                || (targetTS.position.z >= nessClampBorder.z && targetTS.position.z <= nessClampBorder.w))
            {
                borderSpeed.y = 0;
            }
            else
            {
                borderSpeed.y = tempY;
            }

            MoveDir(-borderSpeed);
        }
    }

    /// <summary>
    /// 移动执行方法
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="isScroll"></param>
    /// <param name="isUp"></param>
    private void MoveDir(Vector2 dir, bool isScroll = false, bool isUp = false)
    {
        var moveVec3 = -new Vector3(dir.x, 0, dir.y) * Time.deltaTime * moveSpeed * (targetCamera.fieldOfView / baseZoomScale);
        moveDistance += moveVec3.magnitude;
        Vector4 clampVec4;
        if (isScroll && (targetTS.position.x >= nessClampBorder.x && targetTS.position.x <= nessClampBorder.y
            && targetTS.position.z >= nessClampBorder.z && targetTS.position.z <= nessClampBorder.w))
        {
            clampVec4 = nessClampBorder;
        }
        else
        {
            clampVec4 = clampClampBorder;
        }

        moveVec3.x = Mathf.Clamp(targetTS.position.x + moveVec3.x, clampVec4.x, clampVec4.y);
        moveVec3.y = targetTS.position.y;
        moveVec3.z = Mathf.Clamp(targetTS.position.z + moveVec3.z, clampVec4.z, clampVec4.w);

        targetTS.position = moveVec3;

        if (isUp || isScroll)
        {
            if (targetTS.position.x < nessClampBorder.x)
            {
                scrollAccele.x = 0;
                nessScrollAccele.x = 0;
                borderSpeed.x = borderBackSpeed;
                nessBorderAccele.x = (targetTS.position.x - nessClampBorder.x) / borderNessTime;
            }
            else if (targetTS.position.x > nessClampBorder.y)
            {
                scrollAccele.x = 0;
                nessScrollAccele.x = 0;
                borderSpeed.x = -borderBackSpeed;
                nessBorderAccele.x = (targetTS.position.x - nessClampBorder.y) / borderNessTime;
            }

            if (targetTS.position.z < nessClampBorder.z)
            {
                scrollAccele.y = 0;
                nessScrollAccele.y = 0;
                borderSpeed.y = borderBackSpeed;
                nessBorderAccele.y = (targetTS.position.z - nessClampBorder.z) / borderNessTime / 2;
            }
            else if (targetTS.position.z > nessClampBorder.w)
            {
                scrollAccele.y = 0;
                nessScrollAccele.y = 0;
                borderSpeed.y = -borderBackSpeed;
                nessBorderAccele.y = (targetTS.position.z - nessClampBorder.w) / borderNessTime / 2;
            }
        }
    }

    /// <summary>
    /// 两个手指进行缩放的时候的方法
    /// </summary>
    private void OnTwoTouchEvent()
    {
        if (scrollAccele != Vector2.zero)
        {
            scrollAccele = Vector2.zero;
        }
        if (borderSpeed != Vector2.zero)
        {
            borderSpeed = Vector2.zero;
        }
        if (!isTwoTouch)
        {
            isTwoTouch = true;
        }

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

    /// <summary>
    /// 屏幕缩放执行方法
    /// </summary>
    /// <param name="offset"></param>
    private void ZoomCamera(float offset)
    {
        float scaleFactor = offset / zoomScaleSpeed;
        float endView = Mathf.Clamp((targetCamera.fieldOfView - scaleFactor)
            , clampZoomScale.x, clampZoomScale.y);
        targetCamera.fieldOfView = endView;
    }

    /// <summary>
    /// 射线检测方法
    /// </summary>
    /// <param name="nowPos"></param>
    /// <param name="isFirst"></param>
    /// <returns></returns>
    private bool StartRayCast(Vector2 nowPos, bool isFirst)
    {
        if (moveDistance <= moveMaxDistance)
        {
            RaycastHit hitInfo;
            Ray ray = targetCamera.ScreenPointToRay(nowPos);
            if (Physics.Raycast(ray, out hitInfo, 10000f, layermask))
            {
                var buildObj = hitInfo.collider.gameObject;
                if (isFirst)
                {
                    clickObj = buildObj;
                }
                else if (clickObj == buildObj)
                {
                    Debug.Log(clickObj);
                }


                return true;
            }
            return false;
        }
        return false;
    }
}
