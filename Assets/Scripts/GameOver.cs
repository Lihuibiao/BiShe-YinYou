using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public GameObject WinObj;
    public GameObject LoseObj;

    private void Start()
    {
        if (PlayerCntroller.IsWin)
        {
            WinObj.gameObject.SetActive(true);
            LoseObj.gameObject.SetActive(false);
        }
        else
        {
            WinObj.gameObject.SetActive(false);
            LoseObj.gameObject.SetActive(true);
        }
    }
}
