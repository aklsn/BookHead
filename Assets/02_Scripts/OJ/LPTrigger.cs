using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LPTrigger : MonoBehaviour
{
    public AudioSource sound1;
    public bool IsLp;

    private void Start()
    {
        IsLp = false;
    }

    private void Update()
    {
        if (!IsLp)  
        {
            sound1.Stop();
        }
    }
}
