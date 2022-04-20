using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentSelector : MonoBehaviour
{
    [Header("UI")]
    public AgentHUD agentHUD;

    [Header("Indicators")]
    public Follower hoverIndicator;
    public Follower selectionIndicator;

    private GameObject hoveredObject;
    private GameObject selectedObject;

    private Camera cam;

    private void Start(){
        cam = GetComponent<Camera>();
    }

    private void Update(){
        GameObject foundObject = null;

        // Find Agent where mouse is placed
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.3f, 100);
        if(hits.Length > 0){
            foreach(RaycastHit hit in hits){
                if(hit.collider.gameObject.CompareTag("Agent")){
                    foundObject = hit.collider.gameObject;
                    break;
                }
            }
        }

        if(Mouse.current.leftButton.wasPressedThisFrame){
            if(foundObject)
                Select(foundObject);
            else
                Deselect();
        }else{
            Hover(foundObject);
        }
    }

    private void Hover(GameObject hoverObject){
        if(hoveredObject != null && hoveredObject != selectedObject && hoveredObject != hoverObject)
            Unhover();

        if(hoveredObject == hoverObject || hoverObject == selectedObject || hoverObject == null) return;

        hoveredObject = hoverObject;
        hoverIndicator.Follow(hoveredObject.transform);
    }

    private void Unhover(){
        if(hoveredObject == null) return;

        hoveredObject = null;
        hoverIndicator.Unfollow();
    }

    private void Select(GameObject objectToSelect){
        if(objectToSelect == selectedObject)
            return;
        else
            Deselect();

        Unhover();

        selectedObject = objectToSelect;
        selectionIndicator.Follow(objectToSelect.transform);

        Agent agent = selectedObject.GetComponent<Agent>();
        agentHUD.ShowAgentInformation(agent);
    }

    private void Deselect(){
        if(selectedObject == null) return;

        selectedObject = null;
        selectionIndicator.Unfollow();

        agentHUD.HideAgentInformation();
    }
}
