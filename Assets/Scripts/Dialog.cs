using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog {
    [TextArea(2, 5)]
    public string[] sentences = { };
    private int currentSentence = 0; //probably another queue tbh

    public string NextSentence() {
        if (currentSentence < sentences.Length) {
            return sentences[currentSentence++];
        }
        return "";
    }
}
