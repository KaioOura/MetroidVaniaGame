using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ItemWorld : MonoBehaviour, IInteractable
{
    [FormerlySerializedAs("_itemData")] [SerializeField] private ItemData itemData;
    [SerializeField] private int quantity = 1;
    #region Temporary region
    [SerializeField] private GameObject interactObj;
    #endregion
    
    private SpriteRenderer spriteRenderer;
    
    private Item Item { get; set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Item = new Item(itemData, quantity);
        spriteRenderer.sprite = itemData.ItemIcon;
    }

    public void OnInteract(Transform transform)
    {
        if (!transform.TryGetComponent(out Character character)) return;
        if (!character.InventoryService.TryAddItem(Item, Item.Quantity)) return;
        
        Destroy(gameObject);
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void Select()
    {
        interactObj.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        interactObj.gameObject.SetActive(false);
    }

    public int GetPriority()
    {
        return InteractionPriority.ITEM;
    }
}
