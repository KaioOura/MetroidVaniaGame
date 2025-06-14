using System;
using UnityEngine;

public class ItemWorld : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private int quantity = 1;
    private SpriteRenderer spriteRenderer;
    
    private Item Item { get; set; }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Item = new Item(_itemData, quantity);
        spriteRenderer.sprite = _itemData.ItemIcon;
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
        //throw new System.NotImplementedException();
    }

    public void Deselect()
    {
        //throw new System.NotImplementedException();
    }

    public int GetPriority()
    {
        return InteractionPriority.ITEM;
    }
}
