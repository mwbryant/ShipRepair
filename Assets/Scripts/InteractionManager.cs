using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO this needs a lot of refractoring, a lot of duped code
//TODO This belongs somewhere else I think maybe on the Object Base class
public enum Interaction {
    Unscrew,
    Screw,
    Crowbar,
    AddComponent,
    CutWires,
    FixWires,
    Dry,
    Drain,
    Wrench,
    Unwrench,
    RemovePiston, // Open hand operation
    ClosePanel, // Should the hands ones be more generic
    PlacePiston,
}

[System.Serializable]
public struct InteractionAction {
    public Interaction action;
    public AudioClip soundFx;

    public InteractionAction(Interaction action) {
        this.action = action;
        this.soundFx = null;
    }

}

public class InteractionManager : MonoBehaviour {
    //This is redundant because equipped == null is handsFree, whats the correct way to do this
    private bool handsFree = true;
    private Tool equipped; //Really the type here should be holdable or something

    public InteractionAction[] handsInteractions;
    public float reachDistance = 1f;
    public float useDistance = 1f;
    public float throwForce = 100f;

    public AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    //Tool needs to come from players equipped field, obj is from a click raycast
    void UseTool(Object obj, Tool tool) {
        InteractionAction[] actions;
        if (handsFree) {
            actions = handsInteractions;
        } else {
            actions = tool.interactionsPossible;
        }
        foreach (InteractionAction interaction in actions) {
            if (obj.Interact(interaction.action)) {
                if (handsFree) {
                    Debug.Log("Hands worked");
                } else {
                    Debug.Log("Tool worked");
                }

                if (interaction.soundFx != null) {
                    Debug.Log(interaction.soundFx);
                    audioSource.PlayOneShot(interaction.soundFx);
                }

                return;
            }
        }
    }

    void FixedUpdate() {
        if (!handsFree) {
            equipped.transform.rotation = Quaternion.identity;
            equipped.transform.position = transform.position + new Vector3(-.2f, -.25f, 0);
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
            TryManual();
        }
        if (Input.GetMouseButtonUp(1)) {
            Throw();
        }
    }

    void TryManual() {
        if (!handsFree) return;
        //TODO honestly refractor this whole sections, duplication from TryPickup
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D target = Physics2D.Raycast(ray, Vector2.zero);

        if (!target)
            return;

        Manual man = target.collider.gameObject.GetComponent<Manual>();
        if (man == null) return;

        if (Vector3.Distance(target.transform.position, transform.position) > useDistance) {
            Debug.Log("Manual out of range");
            return;
        }

        man.ShowManual(gameObject, useDistance);
    }

    void Throw() {
        if (equipped == null) return;
        handsFree = true;
        Vector2 mouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Rigidbody2D rb = equipped.gameObject.GetComponent<Rigidbody2D>();
        Vector2 force = (mouse - rb.position).normalized;
        rb.AddForce(force * throwForce);
        StartCoroutine(EnableAfterWait(equipped));
        equipped = null;
    }

    IEnumerator EnableAfterWait(Tool toenable) {
        yield return new WaitForSeconds(.2f);
        toenable.GetComponent<BoxCollider2D>().enabled = true;

    }

    public void ConsumeTool() {
        if (equipped != null) {
            Destroy(equipped.gameObject);
            equipped = null;
            handsFree = true;
        } else {
            Debug.LogError("Consumed nothing");
        }
    }

    public void Equip(Tool tool) {
        if (tool != null) {
            if (handsFree == false) {
                //Do something with the current tool
                //Why not put it where this tool was
                //Hope C# does this copy by value and not by ref... I'm sure it does but why
                equipped.gameObject.transform.position = tool.gameObject.transform.position;
                equipped.GetComponent<BoxCollider2D>().enabled = true;
            }
            handsFree = false;
            equipped = tool;
            equipped.GetComponent<BoxCollider2D>().enabled = false;
        }

    }

    void TryInteract() {
        //TODO honestly refractor this whole sections, duplication from TryPickup
        Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D target = Physics2D.Raycast(ray, Vector2.zero);
        if (!target)
            return;
        Object obj = target.collider.gameObject.GetComponent<Object>();
        //TODO this is copy and pasted, make a range check function
        if (obj != null) {
            //FIXME Need to find distance to closest point
            if (Vector3.Distance(target.transform.position, transform.position) > useDistance) {
                Debug.Log("Object out of range");
                return;
            }
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
            //if (target.collider.gameObject.tag == "Tool") { -- Is this better or worse
            Tool targetTool = target.collider.gameObject.GetComponent<Tool>();
            if (Vector3.Distance(target.transform.position, transform.position) > reachDistance) {
                Debug.Log("Tool out of range");
                return;
            }
            Equip(targetTool);
        }

    }
}
