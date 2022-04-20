using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgentHUD : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI agentNameText;
    public Image healthBar;

    private Agent selectedAgent;

    public void ShowAgentInformation(Agent agent){
        if(!gameObject.activeSelf)
            gameObject.SetActive(true);

        selectedAgent = agent;
        agentNameText.text = agent.gameObject.name;
        SetHealthBar(agent.healthPoints, agent.maxHealthPoints);

        selectedAgent.onAgentHealthChanged += UpdateAgentHealth;
    }

    public void HideAgentInformation(){
        gameObject.SetActive(false);
        selectedAgent.onAgentHealthChanged -= UpdateAgentHealth;
        selectedAgent = null;
    }

    private void UpdateAgentHealth(){
        SetHealthBar(selectedAgent.healthPoints, selectedAgent.maxHealthPoints);
        if(selectedAgent.healthPoints <= 0)
            HideAgentInformation();
    }

    private void SetHealthBar(float healthPoints, float maxHealthPoints){
        float value = healthPoints / maxHealthPoints;
        healthBar.fillAmount = value;
    }
}
