using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    public float health = 100f; // Здоровье цели
    public float wanderRadius = 10.0f; // Радиус блуждания
    public float fleeDuration = 5.0f; // Время, в течение которого NPC будет убегать
    public float runSpeed = 6.0f; // Скорость бега NPC
    public Transform player; // ссылка на игрока

    private NavMeshAgent agent;
    private bool isFleeing = false;
    private float walkSpeed; // Скорость ходьбы NPC

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        else
        {
            walkSpeed = agent.speed; // Сохраняем исходную скорость ходьбы NPC
            SetNewDestination();
        }
    }

    void Update()
    {
        if (!isFleeing && agent.remainingDistance < 0.5f)
        {
            SetNewDestination();
        }
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        Vector3 finalPosition = hit.position;

        agent.SetDestination(finalPosition);
    }

    public void TakeDamage(float amount)
    {
        health -= amount; // Уменьшаем здоровье цели на величину урона

        if (health <= 0f) // Если здоровье цели стало меньше или равно нулю
        {
            Die(); // Цель умирает
        }
        else
        {
            Flee();
            Invoke("StopFleeing", fleeDuration); // NPC перестанет убегать через заданное время
        }
    }

    void Die()
    {
        Destroy(gameObject); // Уничтожаем объект
    }

    void Flee()
    {
        isFleeing = true;
        agent.speed = runSpeed; // Устанавливаем скорость бега NPC
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 newDestination = transform.position + fleeDirection * wanderRadius;

        NavMeshHit hit;
        NavMesh.SamplePosition(newDestination, out hit, wanderRadius, 1);
        newDestination = hit.position;

        agent.SetDestination(newDestination);
    }

    void StopFleeing()
    {
        isFleeing = false;
        agent.speed = walkSpeed; // Возвращаем исходную скорость ходьбы NPC
    }
}
