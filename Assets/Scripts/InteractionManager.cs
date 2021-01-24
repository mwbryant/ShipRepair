using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Interaction {
    Unscrew,
    Screw,
    Crowbar,
    AddComponent,
    CutWires,
    FixWires,
    Dry,
    Drain,
}
//Singleton
public class InteractionManager : MonoBehaviour {
    //This is redundant because equipped == null is handsFree, whats the correct way to do this
    private bool handsFree = true;
    private Tool equipped; //Really the type here should be holdable or something

    public float reachDistance = 1f;

    void Start() {

    }

    //Tool needs to come from players equipped field, obj is from a click raycast
    void UseTool(Object obj, Tool tool) {
        foreach (Interaction interaction in tool.interactionsPossible) {
            if (obj.Interact(interaction)) {
                Debug.Log("Tool worked");
                return;
            }
        }
        Debug.Log("Tool failed");
    }

    void FixedUpdate() {
        if (!handsFree) {
            equipped.transform.position = transform.position;
        }
    }

    //Probably very gross to have input across multiple files
    //Maybe not though, this handles using tools and inventory, not the player
    //Maybe this should be attached to player
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            //Only one of these should work at a time... enforce?
            TryPickup();
            TryInteract();
        }
    }

    void TryInteract() {
        //TODO honestly refractor this whole sections, duplication from TryPickup
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D target = Physics2D.Raycast(ray, Vector2.zero);
        Object obj = target.collider.gameObject.GetComponent<Object>();
        //TODO this is copy and pasted, make a range check function
        if (Vector3.Distance(target.transform.position, transform.position) > reachDistance) {
            Debug.Log("Tool out of range");
            return;
        }
        if (obj != null) {
            Debug.Log("Found Object");
            UseTool(obj, equipped);
        }
    }

    void TryPickup() {
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Can click everything except tables
        int layerMask = ~(LayerMask.GetMask("Tables"));
        RaycastHit2D target = Physics2D.Raycast(ray, Vector2.zero, 1.0f, layerMask);

        if (target) {
            Debug.Log("We clicked " + target.collider.name);
            if (Vector3.Distance(target.transform.position, transform.position) > reachDistance) {
                Debug.Log("Tool out of range");
                return;
            }
            //if (target.collider.gameObject.tag == "Tool") { -- Is this better or worse
            Tool targetTool = target.collider.gameObject.GetComponent<Tool>();
            if (targetTool != null) {
                Debug.Log("And its a tool");
                if (handsFree == false) {
                    //Do something with the current tool
                    //Why not put it where this tool was
                    //Hope C# does this copy by value and not by ref... I'm sure it does but why
                    equipped.gameObject.transform.position = targetTool.gameObject.transform.position;
                }
                handsFree = false;
                equipped = targetTool;
            }
        }

    }
}
