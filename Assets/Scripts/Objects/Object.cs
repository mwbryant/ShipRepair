using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Object : MonoBehaviour {
    public bool isBroken;
    public abstract bool Interact(Interaction interaction);
}
