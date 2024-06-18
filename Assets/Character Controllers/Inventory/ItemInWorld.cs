using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Add this script to all item prefabs, item = the respective InventoryItem scriptable object
public class ItemInWorld : MonoBehaviour
{
    public Item item;

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

    //public List<ItemInteraction> itemInteractions;

    //public AudioClip itemCollectedAudio;



    //[System.Serializable]
    //public class ItemInteraction
    //{
    //    // what to do when the item is interacted with

    //}

}
