using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInteractionRaycast : MonoBehaviour
{
    public bool firstPerson;

    [SerializeField] private KeyCode selectInput = KeyCode.F;

    [SerializeField] private float reachDistance = 5f;
    [SerializeField] private float selectionSize = 1f;

    public GameObject selectedObject;

    public Vector3 hitPoint;
    public Transform hitTransform;

    public Image interactionAimIndicator;

    //public bool isDoor;
    [SerializeField] private bool isItem;
    [SerializeField] private bool isInteraction;

    [HideInInspector] public bool isItemInteracted;
    //[HideInInspector] public bool isConsumableInteracted;

    //[SerializeField] private TextPopUp popUpText;

    //private ToggleDoor door;

    public LayerMask uiLayer;

    public TextMeshProUGUI interactPromptText;
    //[SerializeField] private TextMeshProUGUI consumePromptText;

    [SerializeField] private Inventory inventorySystem;
    [SerializeField] private float delayTime = 1f;


    //public AudioSource audioSource;

    // public List<AudioClip> collectAudioList;
    //public List<AudioClip> consumeAudioList;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<Inventory>();
        interactPromptText.text = "";
    }

    private void Update()
    {
        //StartCoroutine(InteractionRaycast());

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.SphereCast(ray, selectionSize, out hit, reachDistance, ~uiLayer))
        {
            if (hit.transform.GetComponent<ItemInWorld>())
            {
                isItem = true;
                selectedObject = hit.transform.gameObject;

                interactPromptText.text = "Collect " + selectedObject.GetComponent<ItemInWorld>().item.itemName;
            }
            else
            {
                isItem = false;
            }

            //if (hit.transform.GetComponent<ToggleDoor>() && hit.transform.GetComponent<ToggleDoor>().enabled)
            //{
            //    isDoor = true;
            //    selectedObject = hit.transform.gameObject;

            //    if (!selectedObject.GetComponent<ToggleDoor>().isLocked)
            //    {
            //        if (selectedObject.GetComponent<ToggleDoor>().isOpen)
            //        {
            //            interactPromptText.text = "Close Door [" + selectInput + "]";
            //        }
            //        else
            //        {
            //            interactPromptText.text = "Open Door [" + selectInput + "]";
            //        }
            //    }
            //    else interactPromptText.text = "Locked";

            //}
            //else if (hit.transform.GetComponent<EnteranceDoor>() || hit.transform.GetComponentInChildren<EnteranceDoor>())
            //{
            //    isDoor = true;
            //    selectedObject = hit.transform.gameObject;

            //    for (int i = 0; i < DoorMaster.instance.connectedDoorsList.Count; i++)
            //    {
            //        if (hit.transform.GetComponent<EnteranceDoor>() == DoorMaster.instance.connectedDoorsList[i].exteriorDoor)
            //        {
            //            interactPromptText.text = "Enter [" + selectInput + "]";
            //            break;
            //        }
            //        else if (hit.transform.GetComponent<EnteranceDoor>() == DoorMaster.instance.connectedDoorsList[i].interiorDoor)
            //        {
            //            interactPromptText.text = "Exit [" + selectInput + "]";
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    isDoor = false;
            //}

            if (selectedObject != null && Input.GetKeyDown(selectInput))
            {

                if (isItem)
                {

                    isItemInteracted = true;

                    PickUpItem();
                }

                //StartCoroutine(CheckInventoryIndicator());

                //if (isDoor)
                //{
                //    if (selectedObject.GetComponent<ToggleDoor>())
                //    {
                //        door = selectedObject.GetComponent<ToggleDoor>();
                //        if (!door.isLocked)
                //        {
                //            door.ToggleDoorState();
                //        }
                //        else
                //        {
                //            //communicate locked door to player

                //            interactPromptText.text = "Locked";
                //        }
                //    }
                //    else if (selectedObject.GetComponent<EnteranceDoor>())
                //    {
                //        selectedObject.GetComponent<EnteranceDoor>().WarpEnter(FindObjectOfType<PlayerController>().gameObject);
                //    }
                //}

                if (isInteraction)
                {
                    //get interaction from selected object & perform

                }
                else
                {
                    isInteraction = false;
                }
            }

            if (hit.point != null)
            {
                hitTransform = hit.transform;
                hitPoint = hit.point;
            }

            if (isItem || /*isDoor ||*/ isInteraction)
            {
                interactionAimIndicator.color = Color.red;
            }
            else
            {
                hitTransform = null;
                hitPoint = Vector3.zero;

                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                //Debug.Log("Did not Hit");
                selectedObject = null;
                interactPromptText.text = "";

                interactionAimIndicator.color = Color.white;
            }
        }
        else
        {
            hitTransform = null;
            hitPoint = Vector3.zero;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            selectedObject = null;
            interactPromptText.text = "";

            interactionAimIndicator.color = Color.white;
        }

       
    }

    public void PickUpItem()
    {
        Item item = selectedObject.GetComponent<ItemInWorld>().item;

        inventorySystem.AddItemToInventory(item, selectedObject);
        

        StartCoroutine(DelaySettingFalseVariables());

    }

    public IEnumerator DelaySettingFalseVariables()
    {
        if (isItemInteracted)
        {
            yield return new WaitForSeconds(delayTime);

            isItemInteracted = false;
        }

        //if (isConsumableInteracted)
        //{
        //    yield return new WaitForSeconds(delayTime);

        //    isConsumableInteracted = false;
        //}

        //if (isBreakableInteracted)
        //{
        //    yield return new WaitForSeconds(delayTime);

        //    isBreakableInteracted = false;
        //}
    }

    public IEnumerator InteractionRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.SphereCast(ray, selectionSize, out hit, reachDistance, ~uiLayer))
        {
            if (hit.transform.GetComponent<ItemInWorld>())
            {
                isItem = true;
                selectedObject = hit.transform.gameObject;
                //Debug.Log("hit = " + selectedObject);
                //interactionPromptText.gameObject.SetActive(true);
                interactionAimIndicator.color = Color.red;

                interactPromptText.gameObject.SetActive(true);

                interactPromptText.text = "Collect " + selectedObject.GetComponent<ItemInWorld>().item.itemName;
            }
            else
            {
                isItem = false;
            }

            //if (hit.transform.GetComponent<ToggleDoor>() && hit.transform.GetComponent<ToggleDoor>().enabled)
            //{
            //    isDoor = true;
            //    selectedObject = hit.transform.gameObject;
            //    //interactionPromptText.gameObject.SetActive(true);
            //    interactionAimIndicator.color = Color.red;

            //    interactPromptText.gameObject.SetActive(true);


            //    if (!selectedObject.GetComponent<ToggleDoor>().isLocked)
            //    {
            //        if (selectedObject.GetComponent<ToggleDoor>().isOpen)
            //        {
            //            interactPromptText.text = "Close Door";
            //        }
            //        else
            //        {
            //            interactPromptText.text = "Open Door";
            //        }
            //    }
            //    else interactPromptText.text = "Locked";

            //}
            //else
            //{
            //    isDoor = false;
            //}

            if (selectedObject != null && Input.GetKeyDown(selectInput))
            {
                interactPromptText.gameObject.SetActive(false);

                if (isItem)
                {

                    isItemInteracted = true;

                    PickUpItem();
                }

                //StartCoroutine(CheckInventoryIndicator());

                //if (isDoor)
                //{
                //    door = selectedObject.GetComponent<ToggleDoor>();
                //    if (!door.isLocked)
                //    {
                //        if (door.GetComponent<Animator>().GetBool("isOpen"))
                //        {
                //            door.GetComponent<Animator>().SetBool("isOpen", !door.isOpen);
                //        }

                //        if (door.GetComponent<Animator>().GetBool("isOpen"))
                //        {
                //            transform.GetChild(0).GetComponent<Collider>().enabled = false;
                //        }
                //        else transform.GetChild(0).GetComponent<Collider>().enabled = true;
                //    }
                //    else
                //    {
                //        //communicate locked door to player

                //        interactPromptText.text = "Locked";
                //    }
                //}

                if (isInteraction)
                {
                    //get interaction from selected object & perform

                }
            }
        }
        else
        {

            interactPromptText.gameObject.SetActive(false);

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
            selectedObject = null;
            //interactPromptIndicator.SetActive(false);

            interactionAimIndicator.color = Color.white;
        }


        yield return null;
    }

}
