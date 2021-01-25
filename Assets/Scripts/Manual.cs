using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manual : MonoBehaviour {
    public Image box;
    public Text text;

    public string[] tableOfContents;
    [TextArea(2, 12)]
    public string[] manualEntries;
    public GameManager manager;

    private GameObject currentUser;
    private float range;
    private bool onTOC;
    private bool thisManualOpen = false;

    public void ShowManual(GameObject user, float range) {
        manager.hasUsedComputer = true;
        currentUser = user;
        this.range = range;
        if (box.enabled == true) return;
        thisManualOpen = true;

        box.enabled = true;
        text.enabled = true;

        onTOC = true;

        text.text = "Choose an entry by pressing the correct number: \n";
        int i = 1;
        foreach (string entry in tableOfContents) {
            text.text += " " + i + ". " + entry + "\n";
            i++;
        }
    }

    public void HideManual() {
        box.enabled = false;
        text.enabled = false;
        thisManualOpen = false;
        onTOC = false;
    }

    void CheckInput() {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1)) {
            InputRecieved(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2)) {
            InputRecieved(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3)) {
            InputRecieved(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4)) {
            InputRecieved(4);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5)) {
            InputRecieved(5);
        }
    }

    void InputRecieved(int i) {
        Debug.Log(i + " Pressed");
        if (i > manualEntries.Length) return;
        onTOC = false;
        text.text = manualEntries[i - 1];
    }

    void CheckClosed() {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape)) {
            HideManual();
        }
    }
    void Update() {
        if (thisManualOpen == false) return;
        if (Vector3.Distance(currentUser.transform.position, transform.position) > range) {
            Debug.Log("Manual out of range");
            HideManual();
            return;
        }
        CheckClosed();
        if (onTOC) {
            CheckInput();
        }

    }

}
