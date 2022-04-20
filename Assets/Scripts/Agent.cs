using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Agent : MonoBehaviour
{
    [Header("Statistics")]
    public int maxHealthPoints;
    public int healthPoints;

    [Header("Settings")]
    public float minMovingTime;
    public float maxMovingTime;
    public float fallDuration;
    public float knockback;
    
    private AgentState agentState;

    public Floor Floor { get; set; }
    public Spawner Spawner { get; set; }

    private NavMeshAgent navMeshAgent;
    private Rigidbody rig;
    private Animator animator;

    public delegate void OnAgentHealthChanged();
    public event OnAgentHealthChanged onAgentHealthChanged;
    
    private void Start(){
        Initialize();
        MoveToRandomPosition();
    }

    private void Initialize(){
        healthPoints = maxHealthPoints;
        navMeshAgent = GetComponent<NavMeshAgent>();
        rig = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        agentState = AgentState.Standing;
    }

    private void Update(){
        float velocity = navMeshAgent.velocity.magnitude/navMeshAgent.speed;
        animator.SetFloat("Velocity", velocity);
    }

    private void MoveToRandomPosition(){
        Vector3 target = Floor.GetRandomPosition();
        navMeshAgent.SetDestination(target);

        Invoke("MoveToRandomPosition", Random.Range(minMovingTime, maxMovingTime));
    }

    public void GetHurt(){
        healthPoints--;
        if(onAgentHealthChanged != null)
            onAgentHealthChanged.Invoke();
            
        if(healthPoints <= 0)
            Die();
    }

    private void Die(){
        Spawner.RemoveAgent();
        agentState = AgentState.Dead;
        Destroy(gameObject);
    }

    public void Fall(GameObject attacker, bool initiatedByAnotherAgent = false){
        if(agentState == AgentState.Dead) return;

        Agent attackingAgent = attacker.GetComponent<Agent>();
        if(attackingAgent.agentState == AgentState.Standing || initiatedByAnotherAgent){
            transform.LookAt(attacker.transform);
            animator.SetTrigger("FallOnBack");
            StartCoroutine(Fall(AgentState.FallenOnBack));
            
            // Since both agents were standing, make sure the other agent also falls on back
            if(!initiatedByAnotherAgent){
                attackingAgent.GetHurt();
                attackingAgent.Fall(gameObject, true);
            }

        }else{
            animator.SetTrigger("FallOnFront");
            StartCoroutine(Fall(AgentState.FallenOnFront));
        }
    }

    private IEnumerator Fall(AgentState state){
        // NavMeshAgent needs to be stopped and kinematic rigidbody disabled in order to be able to add force
        agentState = state;
        navMeshAgent.isStopped = true;
        rig.isKinematic = false;

        Vector3 fallDirection = state == AgentState.FallenOnBack ? transform.forward * -1 : transform.forward;
        rig.AddForce((Vector3.up + fallDirection) * knockback, ForceMode.Impulse);

        yield return new WaitForSeconds(fallDuration);

        animator.SetTrigger("StandUp");
        agentState = AgentState.Standing;
        navMeshAgent.isStopped = false;
        rig.isKinematic = true;
    }

    private void OnTriggerEnter(Collider collider){
        if(collider.gameObject.CompareTag("Agent")){
            if(agentState != AgentState.Standing) return;

            GetHurt();
            Fall(collider.gameObject);
        }
    }
}

public enum AgentState{
    Standing, FallenOnBack, FallenOnFront, Dead
}
