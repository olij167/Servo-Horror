using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DialogueMaster : MonoBehaviour
{
    [SerializeField] private GameObject playerDialoguePrefab;
    [SerializeField] private GameObject listDialoguePanel;

    public bool inDialogue;
    public bool playerIsSpeaking;

    public NPCInfo npc;

    //Dialogue to display in npc text box
    public NPCDialogueOption npcDialogue;

    //Dialogue to display in the player text box
    public PlayerDialogueOption selectedDialogueOption;

    //Dialogue UI
    public TextMeshProUGUI npcNameText;
    //public TextMeshProUGUI npcMoodText;
    [SerializeField] private TextMeshProUGUI npcDialogueText;
    public TextMeshProUGUI playerDialogueText;

    //public bool playerResponseLockedIn;

    public GameObject continueButton, leaveButton, changeTopicButton;


    //Response Timer Variables
    private bool responseTimerActive;
    private float responseTimer = 5f;
    private float responseTimerReset;

    [SerializeField] private Slider responseTimerUI;   // if true the timer will automatically start during a time-limited response and pick a random option if the player doesn't begin viewing the dialogue options
                                                       // if false the timer won't start until the player has begun viewing the dialogue options
                                                       //Default player dialogue
    [SerializeField] private PlayerDialogue playerDialogue;

    [SerializeField] private GameObject dialogueUI;

    private void Update()
    {
        if (inDialogue)
        {
            if (npcDialogue.requiresResponse)
            {
                if (responseTimerActive)
                {
                    responseTimer -= Time.deltaTime;
                    responseTimerUI.value = responseTimer;

                    if (responseTimer <= 0f)
                    {
                        // select a random option if the player hasn't selected in time
                        selectedDialogueOption = listDialoguePanel.transform.GetChild(Random.Range(0, listDialoguePanel.transform.childCount)).GetComponent<DialogueListButton>().dialogueOption;
                        playerDialogueText.text = selectedDialogueOption.dialogue;

                        LockInResponse();
                    }
                }
            }
        }
        else
        {
            LeaveDialogue();
        }
    }


    // Update UI Dialogue Text
    public void SetNewDialogueText(NPCDialogueOption npcDialogueOption)
    {
        if (inDialogue)
        {
            npcDialogueText.text = npcDialogueOption.dialogue;
            npcDialogue = npcDialogueOption;

            if (npcDialogue.conditionalEvents.Count > 0)
            {
                InvokeNPCConditonalEvents();
            }

            DestroyOldDialogueOptions();

            if (!npcDialogue.requiresResponse)
            {
                playerIsSpeaking = false;
            }
            else
            {
                if (npcDialogue.playerResponses.Count <= 0)
                {
                    npcDialogue.playerResponses = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npcDialogue).playerResponses;
                }
            }

            SetResponseTimer();

            CreateDialogueOptions(npcDialogue);
        }
    }

    public void DestroyOldDialogueOptions()
    {
        for (int i = 0; i < listDialoguePanel.transform.childCount; i++)
        {
            if (i != 0 && i != 1 && i != 2)
            {
                Destroy(listDialoguePanel.transform.GetChild(i).gameObject);
            }
        }
    }

    public void CreateDialogueOptions(NPCDialogueOption npcDialogueOption)
    {
        DestroyOldDialogueOptions();

        if (npcDialogue.requiresResponse)
        {
            foreach (PlayerDialogueOption dialogueOption in npcDialogueOption.playerResponses)
            {
                if (dialogueOption == playerDialogue.continueDialogue)
                {
                    return;
                }

                GameObject newDialogue = Instantiate(playerDialoguePrefab, listDialoguePanel.transform.position, Quaternion.identity);
                newDialogue.GetComponentInChildren<TextMeshProUGUI>().text = dialogueOption.dialogue;
                newDialogue.GetComponent<DialogueListButton>().dialogueOption = dialogueOption;
                newDialogue.transform.SetParent(listDialoguePanel.transform);
            }
            continueButton.SetActive(false);
        }
        else
        {
            if (!npcDialogue.endOfConversation)
            {
                continueButton.SetActive(true);
            }
        }

        if (npcDialogueOption.playerCanChangeTopic)
        {
            leaveButton.SetActive(true);
            int rand = Random.Range(0, playerDialogue.goodbyeDialogue.Count);
            leaveButton.GetComponentInChildren<TextMeshProUGUI>().text = playerDialogue.goodbyeDialogue[rand].dialogue;
            leaveButton.GetComponent<DialogueListButton>().dialogueOption = playerDialogue.goodbyeDialogue[rand];

            if (!playerIsSpeaking)
            {
                //CreateChangeTopicListOption();
            }
        }
        else
        {
            for (int i = 0; i < listDialoguePanel.transform.childCount; i++)
            {

                if (playerDialogue.goodbyeDialogue.Contains(listDialoguePanel.transform.GetChild(i).GetComponent<DialogueListButton>().dialogueOption))
                {
                    Destroy(listDialoguePanel.transform.GetChild(i).gameObject);
                }

                if (playerDialogue.changeTopicDialogue.Contains(listDialoguePanel.transform.GetChild(i).GetComponent<DialogueListButton>().dialogueOption))
                {
                    Destroy(listDialoguePanel.transform.GetChild(i).gameObject);
                }
            }
        }

        if (listDialoguePanel.transform.childCount > 1)
        {
            CreateDialogueOptions(npcDialogueOption);
        }
    }

    

    //Lock in dialogue selection
    public void LockInResponse()
    {
        playerDialogueText.text = selectedDialogueOption.dialogue;

        responseTimer = responseTimerReset;
        responseTimerUI.value = responseTimer;
        responseTimerActive = false;

        npc.npcEmotions.emotion = selectedDialogueOption.AffectEmotionValues(npc.npcEmotions.emotion);
        npc.npcEmotions.SetMood();

        if (selectedDialogueOption.conditionalEvents.Count > 0)
        {
            InvokePlayerConditionalEvents();
        }

        if (playerDialogue.changeTopicDialogue.Contains(selectedDialogueOption) || npcDialogue.changeOfTopic)
        {
            if (!playerIsSpeaking)
            {
                ChangeTopic();
            }
        }

        if (selectedDialogueOption.isGoodbyeOption || npcDialogue.endOfConversation)
        {
            LeaveDialogue();
        }

        if (!npcDialogue.requiresResponse)
        {
            npcDialogue = npcDialogue.continuedDialogue;
        }
        else
        {
            npcDialogue = npc.RespondBasedOnMood(selectedDialogueOption);
        }

        SetNewDialogueText(npcDialogue);

    }

    public void InvokeNPCConditonalEvents()
    {
        //Invoke player dialogue conditional event

        foreach (UnityEvent conditional in npcDialogue.conditionalEvents)
        {
            if (conditional != null)
            {
                //conditional.SetNPC(npc);
                conditional.Invoke();
            }
        }

    }


    public void InvokePlayerConditionalEvents()
    {
        //Invoke player dialogue conditional event
        foreach (UnityEvent conditional in selectedDialogueOption.conditionalEvents)
        {
            if (conditional != null)
            {
                //conditional.SetNPC(npc);
                conditional.Invoke();
            }
        }

    }
    // set response timer values & activate it
    private void SetResponseTimer()
    {
        if (npcDialogue.limitedTime)
        {
            responseTimer = npcDialogue.timeLimit;

            responseTimerReset = responseTimer;
            responseTimerUI.maxValue = responseTimerReset;
            responseTimerUI.value = responseTimer;

            responseTimerActive = true;

        }
        else
        {
            responseTimerReset = responseTimer;
            responseTimerUI.maxValue = responseTimerReset;
            responseTimerUI.value = responseTimer;

            responseTimerActive = false;

        }
    }

    //Initiate Dialogue
    public void BeginDialogue()
    {
        inDialogue = true;
        dialogueUI.SetActive(true);

        playerIsSpeaking = true;

    }

    // Close Dialogue
    public void LeaveDialogue()
    {

        DestroyOldDialogueOptions();
        dialogueUI.SetActive(false);
        inDialogue = false;

        //playerMovement.enabled = true;
        //playerCam.enabled = true;

        foreach (NPCBrain npc in FindObjectsOfType<NPCBrain>())
        {
            npc.isSpeakingToPlayer = false;
        }

        Cursor.lockState = CursorLockMode.Locked;

        enabled = false;
    }

    // Return to initial dialogue options
    public void ChangeTopic()
    {
        // get stored inquiries depending on NPC
        npcDialogue = playerDialogue.SetPlayerDialogueBasedOnCurrentNPCAndDialogue(npc, npc.npcDialogue.changeTopicDialogue[Random.Range(0, npc.npcDialogue.changeTopicDialogue.Count)]);


        playerIsSpeaking = true;

        Debug.Log("Changing the topic");
    }
}
