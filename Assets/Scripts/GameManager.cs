using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Singleton
public class GameManager : MonoBehaviour {
    public Engine engine;
    public Computer computer;

    public Text text;
    public Image box;

    public Dialog engineDialog;
    public Dialog computerTutorialDialog;
    public Dialog engineFixedDialog;
    public Dialog computerDialog;
    public Dialog winDialog;

    public bool hasUsedComputer = false;
    public float engineBreakTime = 5;
    public float computerTutorialTime = 10;

    private bool hasEngineBeenFixed = false;
    private bool engineEventStarted = false;
    private bool computerEventStarted = false;

    void Start() {
        DialogManager.Instance.Setup(box, text);
        StartCoroutine(BreakEngineInXSeconds(engineBreakTime));
        StartCoroutine(CheckComputerUsed(computerTutorialTime));

    }

    IEnumerator BreakEngineInXSeconds(float seconds) {
        yield return new WaitForSeconds(seconds);
        Debug.Log("BREAKING");
        DialogManager.Instance.AddDialog(engineDialog);
        engine.BreakPiston();
        engineEventStarted = true;
    }

    IEnumerator CheckComputerUsed(float seconds) {
        yield return new WaitForSeconds(seconds);
        if (!hasUsedComputer)
            DialogManager.Instance.AddDialog(computerTutorialDialog);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            DialogManager.Instance.AdvanceDialog();
        }
        if (!hasEngineBeenFixed && engineEventStarted) {
            if (!engine.isBroken && !computerEventStarted) {
                DialogManager.Instance.AddDialog(engineFixedDialog);
                StartCoroutine(BreakComputer(5));
            }
        }
        if (computerEventStarted && !computer.isBroken) {
            DialogManager.Instance.AddDialog(winDialog);
        }
    }

    IEnumerator BreakComputer(float seconds) {
        yield return new WaitForSeconds(seconds);
        Debug.Log("BREAKING");
        DialogManager.Instance.AddDialog(computerDialog);
        computer.isBroken = true;
        computerEventStarted = true;
    }
}
