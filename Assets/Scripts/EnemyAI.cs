
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 4f;
    public float detectionRange = 8f;
    public int damage = 10;
    public float attackCooldown = 1.2f;
    public int maxHealth = 60;

    Transform player;
    NavMeshAgent agent;
    float lastAttackTime;
    int health;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        agent = GetComponent<NavMeshAgent>();
        health = maxHealth;
        if (agent != null) agent.speed = patrolSpeed;
    }

    void Update()
    {
        if (player == null) return;
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= detectionRange)
        {
            if (agent != null)
            {
                agent.speed = chaseSpeed;
                agent.SetDestination(player.position);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            }

            if (dist <= 1.8f && Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;
                var pc = player.GetComponent<PlayerController>();
                if (pc != null) pc.TakeDamage(damage);
                var animator = GetComponent<Animator>();
                if (animator != null) animator.SetTrigger("Attack");
            }
        }
    }
}
