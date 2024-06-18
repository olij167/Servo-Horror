using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : Interaction
{
    private Inventory inventorySystem;
    public Item item;

    //public string interactPrompt;

    private void Awake()
    {
        inventorySystem = FindObjectOfType<Inventory>();
        //interactRaycast = FindObjectOfType<InteractRaycast>();
    }

    public override void Interact()
    {
        inventorySystem.AddItemToInventory(item, gameObject);
    }

    public override void Select()
    {

    }

    public void BreakObject()
    {
        if (item.canBreak && item.brokenPartsPrefabs != null && item.brokenPartsPrefabs.Count > 0)
        {
            int brokenParts = (int)Random.Range(item.brokenPartsRange.x, item.brokenPartsRange.y);

            for (int i = 0; i < brokenParts; i++)
            {
                Instantiate(item.brokenPartsPrefabs[Random.Range(0, item.brokenPartsPrefabs.Count)], new Vector3(transform.position.x + Random.Range(-0.1f, 0.1f), transform.position.y + Random.Range(-0.1f, 0.1f), transform.position.z + Random.Range(-0.1f, 0.1f)), Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }
}
