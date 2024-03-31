using UnityEngine;
using UnityEngine.AI;

public class Pedestrian : MonoBehaviour
{
    public float health = 100f; // �������� ����
    public float wanderRadius = 10.0f; // ������ ���������
    public float fleeDuration = 5.0f; // �����, � ������� �������� NPC ����� �������
    public float runSpeed = 6.0f; // �������� ���� NPC
    public Transform player; // ������ �� ������

    private NavMeshAgent agent;
    private bool isFleeing = false;
    private float walkSpeed; // �������� ������ NPC

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        else
        {
            walkSpeed = agent.speed; // ��������� �������� �������� ������ NPC
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
        health -= amount; // ��������� �������� ���� �� �������� �����

        if (health <= 0f) // ���� �������� ���� ����� ������ ��� ����� ����
        {
            Die(); // ���� �������
        }
        else
        {
            Flee();
            Invoke("StopFleeing", fleeDuration); // NPC ���������� ������� ����� �������� �����
        }
    }

    void Die()
    {
        Destroy(gameObject); // ���������� ������
    }

    void Flee()
    {
        isFleeing = true;
        agent.speed = runSpeed; // ������������� �������� ���� NPC
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
        agent.speed = walkSpeed; // ���������� �������� �������� ������ NPC
    }
}
