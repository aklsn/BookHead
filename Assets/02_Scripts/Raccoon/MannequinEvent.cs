using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MannequinEvent : MonoBehaviour
{
    //MannequinEvent
    public GameObject mannequinsRevealPosition;
    public GameObject Mannequin;
    public GameObject mainMannequin;
    public GameObject blackoutPanel;
    private Vector3 direction = new Vector3(20, 20, 20);
    public float rotateSpeed = 1f;
    public float mannequin_revealDelay = 0.3f;
    public float mannequin_RotateDelay = 2f;
    Transform main_head = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void mannequinEvent()
    {
        StartCoroutine(HeadRotationEvent());
    }

    IEnumerator HeadRotationEvent()
    {
        yield return new WaitForSeconds(mannequin_RotateDelay);

        main_head = GameObject.Find("mixamorig1:Head").transform;
        Vector3 currentRotation = main_head.eulerAngles;

        while (Vector3.Distance(currentRotation, direction) > 3f)
        {
            currentRotation = Vector3.Lerp(currentRotation, direction, rotateSpeed * Time.deltaTime);
            main_head.eulerAngles = currentRotation;

            yield return null;
        }

        main_head.eulerAngles = direction;

        StartCoroutine(mannequinRevealEvent());
    }


    IEnumerator mannequinRevealEvent()
    {
            blackoutPanel.SetActive(!blackoutPanel.activeSelf);
            yield return new WaitForSeconds(mannequin_revealDelay);
            Destroy(mainMannequin);
            Mannequin.transform.position = mannequinsRevealPosition.transform.position;
            blackoutPanel.SetActive(!blackoutPanel.activeSelf);
    }
}
