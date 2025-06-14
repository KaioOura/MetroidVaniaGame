using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "ItemData", menuName = "Scriptable Objects/ItemData")]
public class ItemData : ScriptableObject
{
    public string ItemName => itemName;
    public ItemType ItemType => itemType;
    public Sprite ItemIcon => itemIcon;
    public int MaxStack => maxStack;
    public bool IsStackable => isStackable;
    public int BuyValue => buyValue;
    public int SellValue => sellValue;
    public string Description => description;

    [SerializeField] private string itemName;
    [SerializeField] private ItemType itemType;
    [SerializeField] private Sprite itemIcon;
    [FormerlySerializedAs("maxStacks")] [SerializeField] private int maxStack;
    [SerializeField] private bool isStackable;
    [SerializeField] private int buyValue;
    [SerializeField] private int sellValue;
    [SerializeField, TextArea] private string description;
}

public enum ItemType
{
    None,
    Usable,
    Equippable,
    Mission
}