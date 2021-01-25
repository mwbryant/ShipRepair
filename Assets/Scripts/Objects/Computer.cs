using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Object {
    private enum ComputerState {
        Built,
        PanelOpen,
        WiresCut, //after 30 seconds show blue screen
        WiresFixed,
    }
    private ComputerState state;
    new private SpriteRenderer renderer;
    private bool rebooted;

    public Manual manual;

    public float rebootTime = 5f;
    private float timeToReboot = 0;
    public Sprite working;
    public Sprite rebooting;
    public Sprite broken;

    public void Start() {
        renderer = GetComponent<SpriteRenderer>();

    }

    public void Update() {
        manual.manualEnabled = !isBroken;
        switch (state) {
            case ComputerState.Built:
                if (isBroken)
                    renderer.sprite = broken;
                else
                    renderer.sprite = working;
                break;
            case ComputerState.PanelOpen:
                renderer.sprite = broken;
                break;
            case ComputerState.WiresCut:
                renderer.sprite = broken;
                break;
            case ComputerState.WiresFixed:
                renderer.sprite = rebooting;
                break;
        }
        if (state == ComputerState.WiresCut) {
            timeToReboot -= Time.deltaTime;
            if (timeToReboot < 0) {
                state = ComputerState.WiresFixed;
            }
        }
    }
    public override bool Interact(Interaction interaction) {
        switch (state) {
            case ComputerState.Built:
                if (interaction == Interaction.Unscrew) {
                    state = ComputerState.PanelOpen;
                    isBroken = true;
                    return true;
                }
                break;
            case ComputerState.PanelOpen:
                if (interaction == Interaction.AddComponent) {
                    //take wires
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    InteractionManager im = player.GetComponent<InteractionManager>();
                    im.ConsumeTool();
                    state = ComputerState.WiresCut;
                    timeToReboot = rebootTime;
                    return true;
                }
                if (interaction == Interaction.Screw) {
                    state = ComputerState.Built;
                }
                break;
            case ComputerState.WiresCut:
                break;
            case ComputerState.WiresFixed:
                if (interaction == Interaction.Screw) {
                    state = ComputerState.Built;
                    isBroken = false;
                }
                break;
        }
        return false;
    }
}