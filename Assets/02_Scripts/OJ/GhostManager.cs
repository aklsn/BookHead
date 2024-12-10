using System;
using UnityEngine;

public class GhostManager : MonoBehaviour
{
    public GameObject ghost1;
    public GameObject ghost2;
    public AudioSource scarysound1;
    public AudioSource scarysound2;
    public float panSpeed = 1.0f;

    private void Start()
    {
        scarysound1.loop = false;
        scarysound2.loop = false;
    }

    private void Update()
    {
        // 사운드 패닝 설정
        if (scarysound1.isPlaying)
        {
            scarysound1.panStereo = Mathf.PingPong(Time.time * panSpeed, 2f) - 1f;
        }
        else
        {
            ghost1.SetActive(false);
        }

        if (!scarysound2.isPlaying)
        {
            ghost2.SetActive(false);
        }
    }
}