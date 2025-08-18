using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCController : MonoBehaviour
{
    public enum NPCState
    {
        Patrol,
        Chat,
        Combat
    }

    public NPCState currentState = NPCState.Patrol;
    public NavMeshAgent agent;
    public Transform[] patrolPoints;
    public float waitTime = 2f;
    public float chatDuration = 3f;
    public float detectionRadius = 5f;
    public LayerMask targetMask;

    private int currentPatrolIndex;
    private float stateTimer;
    private Transform combatTarget;

    void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        SetNextPatrolPoint();
    }

    void Update()
    {
        switch (currentState)
        {
            case NPCState.Patrol:
                UpdatePatrol();
                break;
            case NPCState.Chat:
                UpdateChat();
                break;
            case NPCState.Combat:
                UpdateCombat();
                break;
        }
    }

    void UpdatePatrol()
    {
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            if (!agent.pathPending && agent.remainingDistance <= 0.5f)
            {
                stateTimer += Time.deltaTime;
                if (stateTimer >= waitTime)
                {
                    SetNextPatrolPoint();
                    stateTimer = 0f;
                }
            }
        }

        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, targetMask);
        if (hits.Length > 0)
        {
            combatTarget = hits[0].transform;
            currentState = NPCState.Combat;
        }
    }

    void UpdateChat()
    {
        agent.isStopped = true;
        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            agent.isStopped = false;
            currentState = NPCState.Patrol;
            SetNextPatrolPoint();
        }
    }

    void UpdateCombat()
    {
        if (combatTarget == null)
        {
            currentState = NPCState.Patrol;
            SetNextPatrolPoint();
            return;
        }

        agent.isStopped = false;
        agent.SetDestination(combatTarget.position);

        float distance = Vector3.Distance(transform.position, combatTarget.position);
        if (distance > detectionRadius * 2f)
        {
            combatTarget = null;
            currentState = NPCState.Patrol;
            SetNextPatrolPoint();
        }
    }

    void SetNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;
        agent.destination = patrolPoints[currentPatrolIndex].position;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
    }

    public void StartChat(float duration)
    {
        currentState = NPCState.Chat;
        stateTimer = duration;
    }

    public void StartCombat(Transform target)
    {
        combatTarget = target;
        currentState = NPCState.Combat;
    }
}

