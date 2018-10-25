using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
    /// <summary>
    /// A stack for all items on this slot
    /// </summary>
    private ObservableStack<Item> items = new ObservableStack<Item>();

    //A a reference to the slot's icon
    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackSize;

    /// <summary>
    /// A reference to the bag that this slot belong to
    /// </summary>
    public BagScript MyBag { get; set; }

    /// <summary>
    /// Checks if the item is empty
    /// </summary>
    public bool IsEmpty
    {
        get
        {
            return MyItems.Count == 0;
        }
    }

    /// <summary>
    /// Indicates if the slot is full
    /// </summary>
    public bool IsFull
    {
        get
        {
            if (IsEmpty || MyCount < MyItem.MyStackSize)
            {
                return false;
            }

            return true;
        }
    }

    /// <summary>
    /// The current item on the slot
    /// </summary>
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return MyItems.Peek();
            }

            return null;
        }
    }

    /// <summary>
    /// The icon on the slot
    /// </summary>
    public Image MyIcon
    {
        get
        {
            return icon;
        }

        set
        {
            icon = value;
        }
    }

    /// <summary>
    /// The item count on the slot
    /// </summary>
    public int MyCount
    {
        get {return MyItems.Count; }
    }

    /// <summary>
    /// The stack text
    /// </summary>
    public Text MyStackText
    {
        get
        {
           return stackSize;
        }
    }

    public ObservableStack<Item> MyItems
    {
        get
        {
            return items;
        }
    }

    private void Awake()
    {
        //Assigns all the event on our observable stack to the updateSlot function
        MyItems.OnPop += new UpdateStackEvent(UpdateSlot);
        MyItems.OnPush += new UpdateStackEvent(UpdateSlot);
        MyItems.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    /// <summary>
    /// Whem the slot is clicked
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (InventoryScript.MyInstance.FromSlot == null && !IsEmpty) //If we don't have something to move
            {
                if (HandScript.MyInstance.MyMoveable != null )
                {
                    if (HandScript.MyInstance.MyMoveable is Bag)
                    {
                        if (MyItem is Bag)
                        {
                            InventoryScript.MyInstance.SwapBags(HandScript.MyInstance.MyMoveable as Bag, MyItem as Bag);
                        }
                    }
                    else if (HandScript.MyInstance.MyMoveable is Armor)
                    {
                        if (MyItem is Armor && (MyItem as Armor).MyArmorType == (HandScript.MyInstance.MyMoveable as Armor).MyArmorType)
                        {
                            (MyItem as Armor).Equip();
                            
                            HandScript.MyInstance.Drop();
                        }
                    }
    
                }
                else
                {
                    HandScript.MyInstance.TakeMoveable(MyItem as IMoveable);
                    InventoryScript.MyInstance.FromSlot = this;
                }
        
            }
            else if (InventoryScript.MyInstance.FromSlot == null && IsEmpty)
            {
                if (HandScript.MyInstance.MyMoveable is Bag)
                {
                    //Dequips a bag from the inventory
                    Bag bag = (Bag)HandScript.MyInstance.MyMoveable;

                    //Makes sure we cant dequip it into itself and that we have enough space for the items from the dequipped bag
                    if (bag.MyBagScript != MyBag && InventoryScript.MyInstance.MyEmptySlotCount - bag.Slots > 0)
                    {
                        AddItem(bag);
                        bag.MyBagButton.RemoveBag();
                        HandScript.MyInstance.Drop();
                    }
                }
                else if (HandScript.MyInstance.MyMoveable is Armor)
                {
                    Armor armor = (Armor)HandScript.MyInstance.MyMoveable;
                    AddItem(armor);
                    CharacterPanel.MyInstance.MySlectedButton.DequipArmor();
                    HandScript.MyInstance.Drop();
                }


            }
            else if (InventoryScript.MyInstance.FromSlot != null)//If we have something to move
            {
                //We will try to do diffrent things to place the item back into the inventory
                if (PutItemBack() || MergeItems(InventoryScript.MyInstance.FromSlot) ||SwapItems(InventoryScript.MyInstance.FromSlot) ||AddItems(InventoryScript.MyInstance.FromSlot.MyItems))
                {
                    HandScript.MyInstance.Drop();
                    InventoryScript.MyInstance.FromSlot = null;
                }
            }
      
        }
        if (eventData.button == PointerEventData.InputButton.Right && HandScript.MyInstance.MyMoveable == null)//If we rightclick on the slot
        {
            UseItem();
        }
    }

    /// <summary>
    /// Adds an item to the slot
    /// </summary>
    /// <param name="item">the item to add</param>
    /// <returns>returns true if the item was added</returns>
    public bool AddItem(Item item)
    {
        MyItems.Push(item);
        icon.sprite = item.MyIcon;
        icon.color = Color.white;
        item.MySlot = this;
        return true;
    }

    /// <summary>
    /// Adds a stack of items to the slot
    /// </summary>
    /// <param name="newItems">stack to add</param>
    /// <returns></returns>
    public bool AddItems(ObservableStack<Item> newItems)
    {
        if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
        {
            int count = newItems.Count;

            for (int i = 0; i < count; i++)
            {
                if (IsFull)
                {
                    return false;
                }

                AddItem(newItems.Pop());
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Removes the item from the slot
    /// </summary>
    /// <param name="item"></param>
    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
        }
    }

    public void Clear()
    {
        if (MyItems.Count > 0)
        {
            InventoryScript.MyInstance.OnItemCountChanged(MyItems.Pop());
            MyItems.Clear();
        }
    }

    /// <summary>
    /// Uses the item if it is useable
    /// </summary>
    public void UseItem()
    {
        if (MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
        else if (MyItem is Armor)
        {
            (MyItem as Armor).Equip();
        }
      
    }

    /// <summary>
    /// Stacks two items
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && MyItems.Count < MyItem.MyStackSize)
        {
            MyItems.Push(item);
            item.MySlot = this;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Puts the item back in the inventory
    /// </summary>
    /// <returns></returns>
    private bool PutItemBack()
    {
        if (InventoryScript.MyInstance.FromSlot == this)
        {
            InventoryScript.MyInstance.FromSlot.MyIcon.color = Color.white;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Swaps two items in the inventory
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    private bool SwapItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() != MyItem.GetType() || from.MyCount+MyCount > MyItem.MyStackSize)
        {
            //Copy all the items we need to swap from A
            ObservableStack<Item> tmpFrom = new ObservableStack<Item>(from.MyItems);

            //Clear Slot a
            from.MyItems.Clear();
            //All items from slot b and copy them into A
            from.AddItems(MyItems);

            //Clear B
            MyItems.Clear();
            //Move the items from ACopy to B
            AddItems(tmpFrom);

            return true;
        }

        return false;
    }

    /// <summary>
    /// Merges two identical stacks of items
    /// </summary>
    /// <param name="from">Slot to merge from</param>
    /// <returns></returns>
    private bool MergeItems(SlotScript from)
    {
        if (IsEmpty)
        {
            return false;
        }
        if (from.MyItem.GetType() == MyItem.GetType() && !IsFull)
        {
            //How many free slots do we have in the stack
            int free = MyItem.MyStackSize - MyCount;

            for (int i = 0; i < free; i++)
            {
                AddItem(from.MyItems.Pop());
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Updates the the slot
    /// </summary>
    private void UpdateSlot()
    {
        UIManager.MyInstance.UpdateStackSize(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //We need to show tooltip
        if (!IsEmpty)
        {
            UIManager.MyInstance.ShowTooltip(new Vector2(1, 0),transform.position, MyItem);
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.MyInstance.HideTooltip();
    }
}
