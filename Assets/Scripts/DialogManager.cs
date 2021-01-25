using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Singleton
public class DialogManager {

    private static DialogManager instance;
    private Image box;
    private Text text;

    public static DialogManager Instance {
        get {
            if (instance == null) {
                instance = new DialogManager();
            }
            return instance;
        }
    }

    public void Setup(Image b, Text t) {
        box = b;
        text = t;
        box.enabled = false;
        text.enabled = false;
    }

    public bool isEmpty {
        get {
            return dialogBoxVisible;
        }
    }

    private Queue<Dialog> dialogBacklog;
    private bool dialogBoxVisible;
    private Dialog currentDialog;

    private DialogManager() {
        dialogBacklog = new Queue<Dialog>();
        currentDialog = new Dialog();
    }

    public void AddDialog(Dialog dialog, int delay = 0) {
        //TODO support delay
        dialogBacklog.Enqueue(dialog);
        if (!text.enabled) {
            AdvanceDialog();
        }
    }

    public void AdvanceDialog() {
        text.enabled = true;
        box.enabled = true;
        string toDisplay = currentDialog.NextSentence();
        while (toDisplay == "" && dialogBacklog.Count != 0) { //This is really gross, program better
            currentDialog = dialogBacklog.Dequeue();
            toDisplay = currentDialog.NextSentence();
        }
        if (toDisplay == "") {
            text.enabled = false;
            box.enabled = false;
        } else {
            Debug.Log(toDisplay);
            text.text = toDisplay;
        }
    }

}
