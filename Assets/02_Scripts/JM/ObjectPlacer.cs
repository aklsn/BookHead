using UnityEngine;

public class ObjectPlacerBetweenPoints : MonoBehaviour
{
    public GameObject objectToPlace; // ��ġ�� ������Ʈ
    public GameObject startPoint; // ���� ����
    public GameObject endPoint; // �� ����
    public int numberOfObjects = 10; // ��ġ�� ������Ʈ ����
    public float rotationAngle = 0f; // ��ġ�Ǵ� ������Ʈ�� Y�� ȸ�� ����

    void Start()
    {
        PlaceObjects(); // ���� ���� �� �ڵ����� ������Ʈ ��ġ
    }

    void PlaceObjects()
    {
        if (objectToPlace == null || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("��ġ�� ������Ʈ �Ǵ� ����/�� ������ �����ϼ���!");
            return;
        }

        // ���� ������ �� ������ ��ġ
        Vector3 startPos = startPoint.transform.position;
        Vector3 endPos = endPoint.transform.position;

        // ���� ���
        Vector3 direction = (endPos - startPos).normalized; // ���� ����
        float distance = Vector3.Distance(startPos, endPos); // �Ÿ�
        float spacing = distance / (numberOfObjects - 1); // ������Ʈ �� ����

        // ������Ʈ ��ġ
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 position = startPos + direction * spacing * i; // ��ġ ���
            Quaternion rotation = Quaternion.Euler(0, rotationAngle, 0); // Y�� ȸ�� ����
            GameObject newObject = Instantiate(objectToPlace, position, rotation, transform);
            newObject.name = $"{objectToPlace.name}_{i}";
        }
    }
}
