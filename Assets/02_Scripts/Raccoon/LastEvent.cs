using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class LastEvent : MonoBehaviour
{
    public Light playerLight; 
    public Volume globalVolume;
    private Vignette vignette; 
    public Camera playerCamera; 
    public GameObject bed; 
    public GameObject mannequines;

    [SerializeField] float shakeDuration = 1f;
    [SerializeField] float shakeMagnitude = 0.5f;

    Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
        /*
        if (globalVolume != null && globalVolume.profile != null)
        {
            if (globalVolume.profile.TryGet(out vignette))
            {
                Debug.Log("Vignette 설정에 접근 성공");
            }
            else
            {
                Debug.LogError("Vignette 설정을 찾을 수 없습니다.");
            }
        }
        else
        {
            Debug.LogError("Global Volume 또는 Volume Profile이 설정되지 않았습니다.");
        }
        */
        initialPosition = playerCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void LastEventActive()
    {
        mannequines.SetActive(true); 
        bed.GetComponent<BedScript_Raccoon>().IsEventOn = true;

        Play();

        if (vignette != null)
        {
            vignette.color.value = Color.red;
        }
    }

    public void Play()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            playerCamera.transform.position = initialPosition + (Vector3)Random.insideUnitCircle * shakeMagnitude;
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        playerCamera.transform.position = initialPosition;
    }
}
