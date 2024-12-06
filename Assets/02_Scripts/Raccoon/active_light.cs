using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class active_light : MonoBehaviour
{
    public Light _Light;

    public float targetIntensity = 0.3f;
    private bool LightOn = false;

    // Start is called before the first frame update
    void Start()
    {
        _Light.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (LightOn)
        {
            _Light.intensity = targetIntensity;
        }
        else
        {
            _Light.intensity = 0;
        }
    }

    public void ChangeLightState()
    {
        LightOn = !LightOn;
    }
}
