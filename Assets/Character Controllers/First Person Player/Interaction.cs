using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interaction : MonoBehaviour
{
    public string interactionPrompt;
    //public InteractRaycast interactRaycast;

    public abstract void Interact();

    public abstract void Select();
}
