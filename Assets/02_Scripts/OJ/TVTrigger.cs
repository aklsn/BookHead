using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVTrigger : MonoBehaviour
{
    public AudioSource sound1;
    public GhostManager ghostManager;
    public bool IsTv;
    public bool IsDone;

    private void Start()
    {
        IsTv = false;
        IsDone = false;
    }

    private void Update()
    {
        if (!IsTv)
        {
            sound1.Stop();
        }

        if (IsDone)
        {
            ghostManager.ghost2.SetActive(true);
            IsDone = false;
        }
    }
}