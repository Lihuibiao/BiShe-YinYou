using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour
{
    public Camera Camera;

    private void Start()
    {
        Camera.enabled = false;
        StartCoroutine(delay2OpenCamera());
    }

    IEnumerator delay2OpenCamera()
    {
        yield return new WaitForSeconds(0.1f);
        Camera.enabled = true;
    }
}
