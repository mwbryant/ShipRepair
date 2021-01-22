using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Singleton
public class DialogManager {

    private static DialogManager instance;
    public static DialogManager Instance {
        get {
            if (instance == null) {
                instance = new DialogManager();
            }
            return instance;
        }
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
    }

    public void AdvanceDialog() {
        string toDisplay = currentDialog.NextSentence();
        while (toDisplay == "" && dialogBacklog.Count != 0) { //This is really gross, program better
            currentDialog = dialogBacklog.Dequeue();
            toDisplay = currentDialog.NextSentence();
        }
        if (toDisplay == "") {
            Debug.Log("Nothing to display");
            dialogBoxVisible = false;
            //TODO remove box ui
        } else {
            Debug.Log(toDisplay);
        }
    }

}
