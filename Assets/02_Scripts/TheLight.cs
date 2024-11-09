using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLight : MonoBehaviour
{
    private Light _Light;

    private float targetIntensity;
    private float currentIntensity;

    void Start()
    {
        _Light = GetComponent<Light>();
        currentIntensity = _Light.intensity;
        targetIntensity = Random.Range(0.4f, .6f);
    }

    void Update()
    {
        // targetIntensity에 도달하지 않은 경우에만 조명 밝기를 조절
        if (Mathf.Abs(targetIntensity - currentIntensity) >= 0.01f)
        {
            if (targetIntensity > currentIntensity)
            {
                currentIntensity += Time.deltaTime * 1.2f;
            }
            else
            {
                currentIntensity -= Time.deltaTime * 1.2f;
            }

            // currentIntensity 값을 항상 조명에 반영
            _Light.intensity = currentIntensity;
            _Light.range = currentIntensity + 10;
        }
        else
        {
            // 목표 밝기에 도달하면 새로운 targetIntensity 설정
            targetIntensity = Random.Range(0.4f, .6f);
        }
    }
}
