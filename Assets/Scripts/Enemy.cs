using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;

    Health health;
    NavMeshAgent agent;

    private void Start()
    {
        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        if (!target) target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        agent.destination = target.position;
    }
}
