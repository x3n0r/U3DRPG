using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{

    /// <summary>
    /// Prefab for creating slots
    /// </summary>
    [SerializeField]
    private GameObject slotPrefab;

    /// <summary>
    /// A canvasgroup for hiding and showing the bag
    /// </summary>
    private CanvasGroup canvasGroup;

    /// <summary>
    /// A list of all the slots the belongs to the bag
    /// </summary>
    private List<SlotScript> slots = new List<SlotScript>();

    /// <summary>
    /// Indicates if this bag is open or closed
    /// </summary>
    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    public List<SlotScript> MySlots
    {
        get
        {
            return slots;
        }
    }

    public int MyEmptySlotCount
    {
        get
        {
            int count = 0;

            foreach (SlotScript slot in MySlots)
            {
                if (slot.IsEmpty)
                {
                    count++;
                }
            }

            return count;
        }
    }

   

    private void Awake()
    {
        //Creates a reference to the canvasgroup
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public List<Item> GetItems()
    {
        List<Item> items = new List<Item>();

        foreach (SlotScript slot in slots)
        {
            if (!slot.IsEmpty)
            {
                foreach (Item item in slot.MyItems)
                {
                    items.Add(item);
                }
            }
        }

        return items;
    }

    /// <summary>
    /// Creates slots for this bag
    /// </summary>
    /// <param name="slotCount">Amount of slots to create</param>
    public void AddSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slot.MyBag = this;
            MySlots.Add(slot);
        }
    }

    /// <summary>
    /// Adds an item to the bag
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(Item item)
    {
        foreach (SlotScript slot in MySlots)//Checks all slots
        {
            if (slot.IsEmpty) //If the slot is empty then we add the item
            {
                slot.AddItem(item); //Adds the item

                return true; //Success
            }
        }
        //Failure
        return false;
    }

    /// <summary>
    /// Opens or closes bag
    /// </summary>
    public void OpenClose()
    {
        //Changes the alpaha to open or closed
        canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

        //Blocks or removes raycast blocking
        canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
    }

    public void Clear()
    {
        foreach (SlotScript slot in slots)
        {
            slot.Clear();
        }
    }
}
