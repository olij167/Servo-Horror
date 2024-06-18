using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interaction
{
    private Animator animator;
    //private bool isInTrigger = false;

    public bool isLocked;
    public bool isOpen;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ToggleDoorState()
    {
        if (!isLocked)
        {
            isOpen = animator.GetBool("isOpen");

            animator.SetBool("isOpen", !isOpen);


            if (animator.GetBool("isOpen"))
            {
                transform.GetChild(0).GetComponent<Collider>().enabled = false;
            }
            else transform.GetChild(0).GetComponent<Collider>().enabled = true;
        }

    }

    public bool ToggleDoorLock()
    {
        return isLocked = !isLocked;
    }

    public override void Interact()
    {
        if (isLocked)
        {
            //check if player can unlock
            //ToggleDoorLock();
        }
        else
        {
            ToggleDoorState();
        }
    }

    public override void Select()
    {
        
    }
}
