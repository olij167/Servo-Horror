using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//To create: Right click in project window -> Create -> Inventory item
[CreateAssetMenu(menuName ="Item")]
public class Item : ScriptableObject
{
    //the inventory item model
    public GameObject prefab;
    public string itemName;
    public Sprite itemIcon;
    public Quaternion heldRotation;

    public float itemValue;

    public float weight;

    //public int numCarried;
    public int maxNumCarried;

    public bool canConsume;
    public bool isStackable;

    public bool canBreak;
    public List<GameObject> brokenPartsPrefabs;
    public Vector2 brokenPartsRange;


    public bool usesBatteries;
    public float maxBatteryCharge;

}
