using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NGUIScrollCamera : MonoBehaviour
{
    [SerializeField]
    private Transform scrollViewBox;
    [SerializeField]
    private float posByPixel = 0.05f;

    private readonly Vector2 clampZoomScale = new Vector2(40, 80);
    private const float zoomScaleSpeed = 33f;

    private BoxCollider box;
    private UIPanel uiPanel;
    private UIScrollView view;
    private Camera targetCamera;
    private Vector3 startPos;
    private Vector2 oldPos1, oldPos2;


    private void Awake()
    {
        startPos = transform.position;
        targetCamera = GetComponent<Camera>();
        box = scrollViewBox.GetComponent<BoxCollider>();
        uiPanel = box.transform.GetChild(0).GetComponent<UIPanel>();
        view = uiPanel.GetComponent<UIScrollView>();
    }

    private void Update()
    {
        OnUpdateState();
    }

    private void OnUpdateState()
    {
        if (Input.touchCount >= 2)
        {
            view.enabled = false;
            TwoTouch();
        }

        if (!view.enabled && Input.touchCount ==0)
        {
            view.enabled = true;
        }

        if (view.enabled)
        {
            transform.position = new Vector3(startPos.x + uiPanel.clipOffset.x * posByPixel
                , startPos.y, startPos.z + uiPanel.clipOffset.y * posByPixel);
        }
    }


    private void TwoTouch()
    {

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
        float endView = Mathf.Clamp((targetCamera.fieldOfView + scaleFactor)
            , clampZoomScale.x, clampZoomScale.y);
        targetCamera.fieldOfView = endView;
    }

}
