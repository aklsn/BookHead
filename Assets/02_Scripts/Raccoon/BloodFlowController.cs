using UnityEngine;

public class BloodFlowController : MonoBehaviour
{
    public Material bloodMaterial; 
    private Vector2 tiling = new Vector2(1, 0); 
    private float flowSpeed = 0.5f; 
    private bool EventOn = false;
    public void BloodEvent()
    {
        EventOn = true;
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (EventOn)
        {
            tiling.y += Time.deltaTime * flowSpeed;
            if (tiling.y > 1f)
            {
                tiling.y = 1f;
            }

            bloodMaterial.mainTextureScale = tiling;
        }
    }
}
