using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrollBy3D : MonoBehaviour
{
    private Transform targetTS;
    private Camera targetCamera;

    private readonly Vector3 startCameraPos = new Vector3(-20,50,-13);
    private readonly Vector2 clampZoomScale = new Vector2(40, 80);
    private readonly Vector4 nessClampBorder = new Vector4(-45, 205, -100, 60);
    private readonly Vector4 clampClampBorder = new Vector4(-60, 220, -115, 75);

    private const float moveSpeed = 4, scrollDistance = 20, scrollNess = 100
        , zoomScaleSpeed = 33, borderNess = 100, borderNessTime = 0.2f, borderBackSpeed = 100;
    private const int queMaxNumber = 10;

    private Vector2 lastPos;
    private bool isStayTouch, isTwoTouch;
    private LinkedList<Vector2> vec2List;
    private Vector2 scrollAccele, nessScrollAccele, borderSpeed, nessBorderAccele, oldPos1, oldPos2;

    private void Awake()
    {
        targetTS = transform;
        targetCamera = targetTS.GetComponent<Camera>();
        targetTS.position = startCameraPos;
        vec2List = new LinkedList<Vector2>();
    }

    private void Update()
    {
        CheckTouch();
        UpdateScrollScreen();
    }

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
#endif
    }

    private void OnOneTouchEvent()
    {
        Vector2 nowPos = Input.mousePosition;
        if (Input.GetMouseButtonDown(0))
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

    private void OnePress(Vector2 nowPos)
    {
        lastPos = nowPos;
        isStayTouch = true;
        scrollAccele = Vector2.zero;
        nessScrollAccele = Vector2.zero;
        borderSpeed = Vector2.zero;
        vec2List.Clear();
        AddQueue(nowPos);
    }

    private void OneUp(Vector2 nowPos)
    {
        isStayTouch = false;
        AddQueue(nowPos);
        MoveDir(Vector2.zero, false, true);
        ScrollScreen();
    }

    private void OneStay(Vector2 nowPos)
    {
        if (isTwoTouch)
        {
            OnePress(nowPos);
            isTwoTouch = false;
        }
        AddQueue(nowPos);
        Vector2 newVec2 = nowPos - lastPos;
        MoveDir(newVec2);
        lastPos = nowPos;
    }

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

    private void AddQueue(Vector2 nowPos)
    {
        if (vec2List.Count >= queMaxNumber)
        {
            vec2List.RemoveFirst();
        }
        vec2List.AddLast(nowPos);
    }

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
                ||(targetTS.position.x >= nessClampBorder.x && targetTS.position.x <= nessClampBorder.y))
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

    private void MoveDir(Vector2 dir, bool isScroll = false, bool isUp = false)
    {
        var moveVec3 = -new Vector3(dir.x, 0, dir.y) * Time.deltaTime * moveSpeed;
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

        if (isUp|| isScroll)
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
                nessBorderAccele.x = (targetTS.position.x - nessClampBorder.y) / borderNessTime ;
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

    private void ZoomCamera(float offset)
    {
        float scaleFactor = offset / zoomScaleSpeed;
        float endView = Mathf.Clamp((targetCamera.fieldOfView - scaleFactor)
            , clampZoomScale.x, clampZoomScale.y);
        targetCamera.fieldOfView = endView;
    }
}
