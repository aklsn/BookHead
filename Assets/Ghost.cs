using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public AudioSource scarysound;
    public LPTrigger lpTrigger; // LPTrigger를 직접 참조
    public float panSpeed = 1.0f;

    private void Start()
    {
        lpTrigger = gameObject.GetComponent<LPTrigger>();
    }

    private void Update()
    {
        Debug.Log(lpTrigger.IsDone);
        if (lpTrigger.IsDone)
        {
            gameObject.SetActive(true);
        }
        // lpTrigger가 존재할 경우에만 실행
        if (lpTrigger != null)
        {
            if (scarysound.isPlaying)
            {
                // 사운드의 패닝 효과 설정
                scarysound.panStereo = Mathf.PingPong(Time.time * panSpeed, 2f) - 1f;
            }
            else
            {
                // 오디오가 재생 중이지 않을 때 오브젝트 비활성화
                gameObject.SetActive(false);
            }
        }
    }
}