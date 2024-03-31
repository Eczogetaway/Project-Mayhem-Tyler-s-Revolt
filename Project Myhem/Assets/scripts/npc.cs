using UnityEngine;
using System.Collections;

public class NPC : MonoBehaviour
{
    public float speed = 3.0f;
    private Vector3 targetPosition;
    private float changeTargetSqrDistance = 0.1f; // Минимальное расстояние до цели, при котором выбирается новая цель

    void Start()
    {
        // Задаем случайную цель в пределах области
        targetPosition = GetRandomPosition();


    }

    void Update()
    {
        // Перемещаем NPC к цели
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        // Если NPC достиг цели, задаем новую цель
        if ((transform.position - targetPosition).sqrMagnitude < changeTargetSqrDistance)
        {
            StartCoroutine(ChangeTarget());
        }

        // Проверяем, не упал ли NP C под пол
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }

    IEnumerator ChangeTarget()
    {
        yield return new WaitForSeconds(1); // Задержка в 1 секунду перед выбором новой цели
        targetPosition = GetRandomPosition();
    }

    Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-10.0f, 10.0f), 0, Random.Range(-10.0f, 10.0f));
    }
}