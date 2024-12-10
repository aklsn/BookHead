using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1Event : MonoBehaviour
{
    public AudioSource Room1AudioSource;
    public AudioClip Room1AudioClip;
    public GameObject Delete_Mannequin;
    public GameObject ChangeImage;
    public Material Image;
    private bool isAudioPlay = false;

    public bool Room1EventActive = false;

    // Start is called before the first frame update
    void Start()
    {
        // 디버그용 로그 추가
        if (Room1AudioSource == null)
            Debug.LogWarning("AudioSource가 할당되지 않았습니다!");

        if (Room1AudioClip == null)
            Debug.LogWarning("AudioClip이 할당되지 않았습니다!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Room1_Event()
    {
        // 널 체크 추가
        if (ChangeImage != null)
        {
            MeshRenderer renderer = ChangeImage.GetComponent<MeshRenderer>();
            if (renderer != null && Image != null)
            {
                renderer.material = Image;
            }
        }

        if (Delete_Mannequin != null)
        {
            Destroy(Delete_Mannequin);
        }

        if (Room1AudioClip == null)
        {
            Debug.LogError("Room1AudioClip이 null입니다!");
            return;
        }

        // 클립 변경
        Room1AudioSource.clip = Room1AudioClip;

        if (Room1AudioSource != null && Room1AudioClip != null)
        {
            if (Room1AudioSource.isPlaying)
                Room1AudioSource.Stop();
            Room1AudioSource.PlayOneShot(Room1AudioClip);
        }
        else
        {
            Debug.LogWarning("오디오 소스나 클립이 할당되지 않았습니다!");
        }
    }
}
