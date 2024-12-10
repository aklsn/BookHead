using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public AudioSource scarysound;
    public GameObject LP;
    
    public float panSpeed = 1.0f;

    private void Start()
    {
        LP = gameObject.GetComponent<LPTrigger>();
    }

    private void Update()
    {
        if(LP)
        if (scarysound.isPlaying)
        {
            scarysound.panStereo = Mathf.PingPong(Time.time * panSpeed, 2f) - 1f;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
