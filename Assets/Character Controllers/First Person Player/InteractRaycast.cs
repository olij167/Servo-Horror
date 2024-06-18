using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractRaycast : MonoBehaviour
{
    [SerializeField] private float reachDistance = 5f;
    [SerializeField] private float selectionSize = 1f;

    public Interaction selectedObject;

    public Vector3 hitPoint;
    //public Transform hitTransform;

    public Image interactionAimIndicator;

    public LayerMask uiLayer;

    public TextMeshProUGUI interactPromptText;


    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.SphereCast(ray, selectionSize, out hit, reachDistance, ~uiLayer))
        {
            if (hit.transform.GetComponent<Interaction>())
            {
                interactionAimIndicator.color = Color.red;

                //hit.transform.GetComponent<Interaction>().interactRaycast = this;

                selectedObject = hit.transform.GetComponent<Interaction>();

                selectedObject.Select();
            }
        }
        else
        {
            //hitTransform = null;
            hitPoint = Vector3.zero;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            selectedObject = null;
            interactPromptText.text = null;
            interactPromptText.gameObject.SetActive(false);

            interactionAimIndicator.color = Color.white;
        }

        if (selectedObject != null)
        {
            interactPromptText.text = selectedObject.interactionPrompt;
            interactPromptText.gameObject.SetActive(true);

            if (Input.GetButtonDown("Interact"))
            {
                selectedObject.Interact();
            }
        }
    }
}
