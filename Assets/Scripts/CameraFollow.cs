using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow Inst;
    public Transform target;
    public float smoothing;
    public Vector2 minPosition;
    public Vector2 maxPosition;
    [Header("黑场动画")]
    public GameObject heiChangAni;

    public Vector2 Offset;

    private void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // heiChangAni.gameObject.SetActive(true);
        this.Invoke("delayCloseHeiChang" , 1f);
    }

    void LateUpdate() 
    {
        if (target != null)
        {
            // if (transform.position != target.position)
            {
                Vector3 targetPos = target.position;
                targetPos.x = Mathf.Clamp(targetPos.x + Offset.x, minPosition.x, maxPosition.x);
                targetPos.y = Mathf.Clamp(targetPos.y + Offset.y, minPosition.y, maxPosition.y);
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }
    }
    
    public void SetCamPosLimit(Vector2 minPos, Vector2 maxPos)
    {
        minPosition = minPos;
        maxPosition = maxPos;
    }

    public Vector2 PlayerEnterPos = new Vector2(366 , 0);

    private void delayCloseHeiChang()
    {
        // heiChangAni.gameObject.SetActive(false);
    }
}