using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVTrigger : MonoBehaviour
{
    public AudioSource sound1;
    public bool IsTv;

    private void Start()
    {
        IsTv = false;
    }

    private void Update()
    {
        if (!IsTv)
        {
            sound1.Stop();
        }
    }
}