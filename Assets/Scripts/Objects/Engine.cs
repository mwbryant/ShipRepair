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

    public override bool Interact(Interaction interaction) {
        switch (state) {
            case EngineState.Built:
                if (interaction == Interaction.Unscrew) {
                    Debug.Log("Unscrewed Panel");
                    state = EngineState.PanelLoose;
                    return true;
                }
                break;
        }
        return false;
    }

    void Start() {

    }

    void Update() {

    }
}
