using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using static UnityEngine.GraphicsBuffer;

public class MannequinEvent : MonoBehaviour
{
    //MannequinEvent
    public GameObject[] Mannequin;
    public GameObject mainMannequin;
    public GameObject blackoutPanel;
    private int sequence = 0;
    private int final_sequence = 3;

    public Light[] _light;
    private float targetIntensity;
    private float currentIntensity;
    private float originalIntensity;
    private bool isLight = false;

    private Vector3 direction = new Vector3(20, 20, 20);
    public float rotateSpeed = 1f;
    public float mannequin_revealDelay = 0.3f;
    public float mannequin_RotateDelay = 2f;
    Transform main_head = null;

    public GameObject player;
    public float distanceInFront = 2.0f;
    public GameObject ControlDoor;

    // Start is called before the first frame update
    void Start()
    {
        currentIntensity = _light[0].intensity;
        originalIntensity = currentIntensity;
        targetIntensity = Random.Range(0.05f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLight)
        {
            if (Mathf.Abs(targetIntensity - currentIntensity) >= 0.01f)
            {
                if (targetIntensity > currentIntensity)
                {
                    currentIntensity += Time.deltaTime * 1.6f;
                }
                else
                {
                    currentIntensity -= Time.deltaTime * 1.6f;
                }

                // currentIntensity 값을 항상 조명에 반영
                for (int i = 0; i < _light.Length; i++)
                {
                    _light[i].intensity = currentIntensity;
                }
            }
            else
            {
                // 목표 밝기에 도달하면 새로운 targetIntensity 설정
                targetIntensity = Random.Range(0.05f, 0.2f);
            }
        }
        else
        {
            for (int i = 0; i < _light.Length; i++)
            {
                _light[i].intensity = originalIntensity;
            }
        }
    }

    public void mannequinEvent()
    {
        StartCoroutine(HeadRotationEvent());
    }

    IEnumerator HeadRotationEvent()
    {
        isLight = true;
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
        isLight = false;
        while (sequence < final_sequence)
        {
            if (sequence > 0)
            {
                Destroy(Mannequin[sequence - 1]);
            }
            blackoutPanel.SetActive(true);
            yield return new WaitForSeconds(mannequin_revealDelay);
            if (mainMannequin != null)
            {
                Destroy(mainMannequin);
            }
            Vector3 forwardDirection = player.transform.forward;
            Vector3 newPosition = player.transform.position + forwardDirection * (distanceInFront+final_sequence-sequence-1);
            Mannequin[sequence].transform.position = new Vector3(newPosition.x, 0, newPosition.z);
            Vector3 targetDirection = new Vector3(player.transform.position.x, Mannequin[sequence].transform.position.y, player.transform.position.z);
            Mannequin[sequence].transform.LookAt(targetDirection);
            blackoutPanel.SetActive(false);
            yield return new WaitForSeconds(mannequin_revealDelay);
            sequence++;
            if (sequence == final_sequence)
            {
                blackoutPanel.SetActive(true);
                Destroy(Mannequin[sequence - 1]);
                yield return new WaitForSeconds(mannequin_revealDelay);
                blackoutPanel.SetActive(false);
            }
        }
        ControlDoor.GetComponent<doorController>().CloseControl = false;
    }
  }