using UnityEngine;

public class roomControl : MonoBehaviour
{
    public GameObject[] roomObjects; // 방 내부 오브젝트 배열

    public void SetRoomObjectsState(bool state)
    {
        foreach (GameObject obj in roomObjects)
        {
            obj.SetActive(state); // 활성화/비활성화
        }
    }
}
