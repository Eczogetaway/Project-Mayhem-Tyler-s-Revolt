using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
    public float speed = 3.0f;
    private Vector3 targetPosition;
    private float changeTargetSqrDistance = 0.1f; // ����������� ���������� �� ����, ��� ������� ���������� ����� ����

    void Start()
    {
        // ������ ��������� ���� � �������� �������
        targetPosition = GetRandomPosition();


    }

    void Update()
    {
        // ���������� NPC � ����
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // ���� NPC ������ ����, ������ ����� ����
        if ((transform.position - targetPosition).sqrMagnitude < changeTargetSqrDistance)
        {
            StartCoroutine(ChangeTarget());
        }

        // ���������, �� ���� �� NP C ��� ���
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    IEnumerator ChangeTarget()
    {
        yield return new WaitForSeconds(1); // �������� � 1 ������� ����� ������� ����� ����
        targetPosition = GetRandomPosition();
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    }
}