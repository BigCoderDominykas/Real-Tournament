using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;

    public float speed;

    Health health;
    NavMeshAgent agent;

    private void Start()
    {
        health = GetComponent<Health>();
        agent = GetComponent<NavMeshAgent>();
        if (!target) target = GameObject.FindWithTag("Player").transform;
        agent.speed = speed;
    }

    private void Update()
    {
        agent.destination = target.position;
    }
}
