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

    [System.NonSerialized]
    public bool Room1EventActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Room1_Event()
    {
        Debug.Log("ตส");
        ChangeImage.GetComponent<MeshRenderer>().material = Image;
        Destroy(Delete_Mannequin);
        StartCoroutine(StopAudioWithDelay(.3f));
    }

    private IEnumerator StopAudioWithDelay(float delay)
    {
        Room1AudioSource.clip = Room1AudioClip;
        isAudioPlay = true;
        Room1AudioSource.Play();
        yield return new WaitForSeconds(delay);
        Room1AudioSource.Stop();
    }
}
