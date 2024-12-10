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
        // ����׿� �α� �߰�
        if (Room1AudioSource == null)
            Debug.LogWarning("AudioSource�� �Ҵ���� �ʾҽ��ϴ�!");

        if (Room1AudioClip == null)
            Debug.LogWarning("AudioClip�� �Ҵ���� �ʾҽ��ϴ�!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Room1_Event()
    {
        // �� üũ �߰�
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
            Debug.LogError("Room1AudioClip�� null�Դϴ�!");
            return;
        }

        // Ŭ�� ����
        Room1AudioSource.clip = Room1AudioClip;

        if (Room1AudioSource != null && Room1AudioClip != null)
        {
            if (Room1AudioSource.isPlaying)
                Room1AudioSource.Stop();
            Room1AudioSource.PlayOneShot(Room1AudioClip);
        }
        else
        {
            Debug.LogWarning("����� �ҽ��� Ŭ���� �Ҵ���� �ʾҽ��ϴ�!");
        }
    }
}
