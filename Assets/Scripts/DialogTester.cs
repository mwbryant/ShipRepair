using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTester : MonoBehaviour {
    public Dialog[] testingDialogs;

    void Start() {
        foreach (Dialog dialog in testingDialogs) {
            DialogManager.Instance.AddDialog(dialog);
        }

    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DialogManager.Instance.AdvanceDialog();
        }
    }
}
