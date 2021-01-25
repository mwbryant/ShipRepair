using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Object {
    private enum EngineState {
        Built,
        PanelLoose,
        PanelOpen,
        PistonUnscrewed,
        MissingPiston,
    }
    private EngineState state = EngineState.Built;
    private bool pistonGood = true;

    public Transform piston;
    public Transform badPiston;
    public Sprite built;
    public Sprite broken;
    public Sprite panelLoose;
    public Sprite panelLooseBroken;
    public Sprite panelOpen;
    public Sprite pistonUnscrewed;
    public Sprite panelOpenBroken;
    public Sprite pistonUnscrewedBroken;
    public Sprite missingPiston;

    new private SpriteRenderer renderer;
    public void Start() {
        renderer = GetComponent<SpriteRenderer>();
    }
    public void Update() {
        switch (state) {
            case EngineState.Built:
                if (isBroken)
                    renderer.sprite = broken;
                else
                    renderer.sprite = built;
                break;
            case EngineState.PanelLoose:
                if (pistonGood)
                    renderer.sprite = panelLoose;
                else
                    renderer.sprite = panelLooseBroken;
                break;
            case EngineState.PanelOpen:
                if (pistonGood)
                    renderer.sprite = panelOpen;
                else
                    renderer.sprite = panelOpenBroken;
                break;
            case EngineState.PistonUnscrewed:
                if (pistonGood)
                    renderer.sprite = pistonUnscrewed;
                else
                    renderer.sprite = pistonUnscrewedBroken;
                break;
            case EngineState.MissingPiston:
                renderer.sprite = missingPiston;
                break;
        }
    }

    public void BreakPiston() {
        pistonGood = false;
        isBroken = true;
    }

    public override bool Interact(Interaction interaction) {
        switch (state) {
            //TODO is there a better way to create this state machine
            //TODO the dream to have the manual generate from the state machine
            case EngineState.Built:
                if (interaction == Interaction.Unscrew) {
                    Debug.Log("Unscrewed Panel");
                    state = EngineState.PanelLoose;
                    isBroken = true;
                    return true;
                }
                break;
            case EngineState.PanelLoose:
                //So here I have made it impossible to have tool be able to screw and crowbar
                //This seems fragile
                if (interaction == Interaction.Screw) {
                    Debug.Log("Screwed Panel");
                    state = EngineState.Built;
                    if (pistonGood) isBroken = false;
                    return true;
                }
                if (interaction == Interaction.Crowbar) {
                    Debug.Log("Removed Panel");
                    state = EngineState.PanelOpen;
                    return true;
                }
                break;
            case EngineState.PanelOpen:
                if (interaction == Interaction.Unscrew) {
                    Debug.Log("Unscrewed Piston");
                    state = EngineState.PistonUnscrewed;
                    return true;
                }
                if (interaction == Interaction.ClosePanel) {
                    Debug.Log("Closed Panel");
                    state = EngineState.PanelLoose;
                    return true;
                }
                break;
            case EngineState.PistonUnscrewed:
                if (interaction == Interaction.RemovePiston) {
                    Debug.Log("Remove Piston");
                    //Give player piston
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    InteractionManager im = player.GetComponent<InteractionManager>();
                    if (pistonGood) {
                        im.Equip(Instantiate(piston, player.gameObject.transform.position, Quaternion.identity).GetComponent<Tool>());
                    } else {
                        im.Equip(Instantiate(badPiston, player.gameObject.transform.position, Quaternion.identity).GetComponent<Tool>());
                    }
                    state = EngineState.MissingPiston;
                    return true;
                }
                if (interaction == Interaction.Screw) {
                    Debug.Log("Screwed in Piston");
                    state = EngineState.PanelOpen;
                    return true;
                }
                break;
            case EngineState.MissingPiston:
                if (interaction == Interaction.PlacePiston) {
                    Debug.Log("Piston Replaced");
                    pistonGood = true;
                    //take piston
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    InteractionManager im = player.GetComponent<InteractionManager>();
                    im.ConsumeTool();
                    state = EngineState.PistonUnscrewed;
                    return true;
                }
                break;
        }
        return false;
    }

}
