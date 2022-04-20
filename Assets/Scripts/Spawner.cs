using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Objects")]
    public GameObject agentPrefab;
    public Floor floor;

    [Header("Spawn Settings")]
    public float minSpawnCooldown;
    public float maxSpawnCooldown;
    public float spawnHeight;

    [Header("Agents Amount")]
    public int startAgents;
    public int maxAgents;

    private int spawnedAgents;
    private int totalSpawnedAgents;

    private void Start(){
        for(int i = 0; i < startAgents; i++){
            if(i < startAgents-1)
                SpawnAgent(false);
            else
                SpawnAgent(true);
        }
    }

    private void SpawnAgent(bool cooldown){
        Vector3 spawnTarget = floor.GetRandomPosition();
        spawnTarget.y = spawnHeight;

        GameObject newAgentObject = Instantiate(agentPrefab, spawnTarget, Quaternion.identity);
        newAgentObject.name = "Agent " + ++totalSpawnedAgents;

        Agent newAgent = newAgentObject.GetComponent<Agent>();
        newAgent.Floor = floor;
        newAgent.Spawner = this;
        RandomizeAgent(newAgentObject);

        spawnedAgents++;

        if(cooldown)
            StartCoroutine(SpawnCooldown());
    }

    public void RemoveAgent(){
        spawnedAgents--;
    }

    private IEnumerator SpawnCooldown(){
        while(spawnedAgents == maxAgents)
            yield return null;

        float cooldownSeconds = Random.Range(minSpawnCooldown, maxSpawnCooldown);
        yield return new WaitForSeconds(cooldownSeconds);

        SpawnAgent(true);
    }

    private void RandomizeAgent(GameObject agent){
        agent.name = RandomName.GetName();
        agent.GetComponentInChildren<SkinnedMeshRenderer>().materials[1].color = RandomColor.GetColor();
    }
}
