using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Pedestrian : MonoBehaviour
{
    public float health = 100f;
    public float wanderRadius = 10.0f;
    public float fleeDuration = 5.0f;
    public float runSpeed = 6.0f;
    public float fleeRefreshRate = 0.5f; // New variable for flee path refresh rate
    public Transform player;
    public Animator animator;
    public AudioClip screamClip; // �������� ���� � ������

    private NavMeshAgent agent;
    private bool isFleeing = false;
    private float walkSpeed;
    private Vector3 destination;
    private AudioSource audioSource; // ��������� AudioSource

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // �������� ��������� AudioSource
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        else
        {
            walkSpeed = agent.speed;
            SetNewDestination();
        }
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, destination) < 0.5f)
        {
            SetNewDestination();
        }

        if (agent.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(agent.velocity.normalized), 0.15f);
        }

        // ���� NPC ��������� �� ���� � ������ ��� ���� �� �����, ������������� ��������������� �����
        if ((!animator.GetBool("isRunning") || Time.timeScale == 0) && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1))
        {
            destination = hit.position;
            agent.SetDestination(destination);
            animator.SetBool("isRunning", isFleeing);
            agent.isStopped = false; // Resume movement
        }
    }

    public void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            StartCoroutine(DeathSequence());
        }
        else
        {
            Flee();
            Invoke("StopFleeing", fleeDuration);
            if (!audioSource.isPlaying) // ���������, ��� ���� �� ���������������
            {
                if (screamClip != null) // ���������, ��� screamClip �� ����� null
                {
                    audioSource.PlayOneShot(screamClip); // ������������� ���� �����
                }
                else
                {
                    Debug.LogError("Scream clip is not assigned in the inspector");
                }
            }
        }
    }

    IEnumerator DeathSequence()
    {
        animator.SetBool("isDead", true); // Assuming you have a death animation associated with this parameter
        agent.speed = 0; // Stop NPC movement
        audioSource.Stop(); // ������������� ��������������� �����
        yield return new WaitForSeconds(5.0f); // NPC will lie on the ground for 5 seconds before disappearing
        Destroy(gameObject);
    }


    void Flee()
    {
        isFleeing = true;
        agent.speed = runSpeed;
        Vector3 fleeDirection = (transform.position - player.position).normalized;
        Vector3 newDestination = transform.position + fleeDirection * wanderRadius * 5; // ����������� ���������� �������

        NavMeshHit hit;
        if (NavMesh.SamplePosition(newDestination, out hit, wanderRadius * 5, 1)) // ����������� ������ ������ �������
        {
            newDestination = hit.position;
            agent.SetDestination(newDestination);
            animator.SetBool("isRunning", true);
            agent.isStopped = false; // Resume movement
        }
        else
        {
            // If the new destination is not on the NavMesh, generate a new random destination
            SetNewDestination();
        }

        // Invoke SetNewDestination at a slower rate while fleeing
        InvokeRepeating("SetNewDestination", fleeRefreshRate * 10, fleeRefreshRate * 10); // ����������� ����� ���������� ����
    }

    void StopFleeing()
    {
        // Check the distance to the player
        if (Vector3.Distance(transform.position, player.position) > wanderRadius)
        {
            isFleeing = false;
            agent.speed = walkSpeed;
            animator.SetBool("isRunning", false);
            CancelInvoke("SetNewDestination"); // Stop the repeated invocation of SetNewDestination
            SetNewDestination(); // Generate a new path
        }
        else
        {
            // If the NPC is still too close to the player, continue fleeing
            Invoke("StopFleeing", fleeRefreshRate);
        }
    }
}
