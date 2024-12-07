using UnityEngine;

public class ObjectPlacerBetweenPoints : MonoBehaviour
{
    public GameObject objectToPlace; // 배치할 오브젝트
    public GameObject startPoint; // 시작 지점
    public GameObject endPoint; // 끝 지점
    public int numberOfObjects = 10; // 배치할 오브젝트 개수
    public float rotationAngle = 0f; // 배치되는 오브젝트의 Y축 회전 각도

    void Start()
    {
        PlaceObjects(); // 게임 시작 시 자동으로 오브젝트 배치
    }

    void PlaceObjects()
    {
        if (objectToPlace == null || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("배치할 오브젝트 또는 시작/끝 지점을 설정하세요!");
            return;
        }

        // 시작 지점과 끝 지점의 위치
        Vector3 startPos = startPoint.transform.position;
        Vector3 endPos = endPoint.transform.position;

        // 간격 계산
        Vector3 direction = (endPos - startPos).normalized; // 방향 벡터
        float distance = Vector3.Distance(startPos, endPos); // 거리
        float spacing = distance / (numberOfObjects - 1); // 오브젝트 간 간격

        // 오브젝트 배치
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 position = startPos + direction * spacing * i; // 위치 계산
            Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0); // Y축 회전 적용
            GameObject newObject = Instantiate(objectToPlace, position, rotation, transform);
            newObject.name = $"{objectToPlace.name}_{i}";
        }
    }
}
