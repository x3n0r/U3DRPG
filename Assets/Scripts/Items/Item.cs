using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Superclass for all items
/// </summary>
public abstract class Item : ScriptableObject, IMoveable, IDescribable
{
    /// <summary>
    /// Icon used when moving and placing the items
    /// </summary>
    [SerializeField]
    private Sprite icon;

    /// <summary>
    /// The size of the stack, less than 2 is not stackable
    /// </summary>
    [SerializeField]
    private int stackSize;

    /// <summary>
    /// The item's title
    /// </summary>
    [SerializeField]
    private string titel;

    /// <summary>
    /// The item's quality
    /// </summary>
    [SerializeField]
    private Quality quality;

    /// <summary>
    /// A reference to the slot that this item is sitting on
    /// </summary>
    private SlotScript slot;

    private CharButton charButton;

    [SerializeField]
    private int price;


    /// <summary>
    /// Property for accessing the icon
    /// </summary>
    public Sprite MyIcon
    {
        get
        {
            return icon;
        }
    }

    /// <summary>
    /// Property for accessing the stacksize
    /// </summary>
    public int MyStackSize
    {
        get
        {
            return stackSize;
        }
    }

    /// <summary>
    /// Proprty for accessing the slotscript
    /// </summary>
    public SlotScript MySlot
    {
        get
        {
            return slot;
        }

        set
        {
            slot = value;
        }
    }

    public Quality MyQuality
    {
        get
        {
            return quality;
        }
    }

    public string MyTitle
    {
        get
        {
            return titel;
        }
    }

    public CharButton MyCharButton
    {
        get
        {
            return charButton;
        }

        set
        {
            MySlot = null;
            charButton = value;
        }
    }

    public int MyPrice
    {
        get
        {
            return price;
        }
    }

    /// <summary>
    /// Returns a description of this specific item
    /// </summary>
    /// <returns></returns>
    public virtual string GetDescription()
    {
        return string.Format("<color={0}>{1}</color>", QualityColor.MyColors[MyQuality], MyTitle);
    }

    /// <summary>
    /// Removes the item from the inventory
    /// </summary>
    public void Remove()
    {
        if (MySlot != null)
        {
            MySlot.RemoveItem(this);

        }
    }
}

