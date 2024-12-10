using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScuttleEvent : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Scuttle;
    public float ScuttleOpenAngle = -30f;
    public float rotationSpeed = 2f;
    public GameObject Mannequin;
    public AudioSource ScuttleAudioSource;
    public AudioClip ScuttleAudioClip;

    private bool isAudioPlay = false;

    [System.NonSerialized]
    public bool ScuttleEventActive = false;
    void Start()
    {
        ScuttleAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ScuttleEventActive == true)
        {
            Mannequin.SetActive(true);
            float currentAngle = Mathf.LerpAngle(Scuttle.transform.eulerAngles.x, ScuttleOpenAngle, Time.deltaTime * rotationSpeed);
            Scuttle.transform.localRotation = Quaternion.Euler(currentAngle, 0, 0);
            if (isAudioPlay == false)
            {
                if (ScuttleAudioSource.isPlaying)
                    ScuttleAudioSource.Stop();
                ScuttleAudioSource.PlayOneShot(ScuttleAudioClip);
                isAudioPlay = true;
            }
        }
        else
        {
            Mannequin.SetActive(false);
        }
    }
}
