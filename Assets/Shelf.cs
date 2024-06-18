using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : Interaction
{
    private Inventory inventory;

    public List<Item> validItems;

    //Stock Shelf Interaction
    //check whether the player is holding a valid item / container of valid items
    //press input to stock shelf with item selected

    public string shelfType;

    public List<Slot> currentItems;

    [System.Serializable]
    public class Slot
    {
        public GameObject item;
        public Transform shelfPos;
    }

    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
    }

    public bool CheckValidStock(Item item)
    {
        if (validItems.Contains(item))
            return true;
        else
            return false;
    }

    public bool CheckShelfCapacity()
    {
        for (int i = 0; i < currentItems.Count; i++)
        {
            if (currentItems[i].item == null)
                return true;
        }
        return false;
    }

    public void SetPrompt()
    {
        if (inventory.selectedInventoryItem == null)
        {
            interactionPrompt = "You aren't holding anything to put here";

        }
        else if (CheckValidStock(inventory.selectedInventoryItem.item))
        {


            if (CheckShelfCapacity())
                interactionPrompt = "Stock " + shelfType + " with " + inventory.selectedInventoryItem.item.itemName;
            else interactionPrompt = "This " + shelfType + " is full";

        }
        else
        {
            interactionPrompt = "That doesn't go here";

        }
    }

    public override void Select()
    {
        SetPrompt();
    }

    public override void Interact()
    {
        if (inventory.selectedInventoryItem != null && CheckValidStock(inventory.selectedInventoryItem.item) && CheckShelfCapacity())
        {
                interactionPrompt = "Stock " + shelfType + " with " + inventory.selectedInventoryItem.item.itemName;
            for (int i = 0; i < currentItems.Count; i++)
            {
                if (currentItems[i].item == null)
                {
                    currentItems[i].item = inventory.selectedPhysicalItem;
                    inventory.PlaceHeldItemInWorld(inventory.selectedInventoryItem, currentItems[i].shelfPos.position);
                    break;
                }
            }

        }
    }

}
