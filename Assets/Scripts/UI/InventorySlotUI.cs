using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public event Action<int> OnSlotClicked;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI quantityText;
    protected int _index;

    protected InventoryUIController _uiController;

    public void Initialize(InventoryUIController uiController)
    {
        _uiController = uiController;
    }
    
    public void SetIndex(int index)
    {
        _index = index;
    }

    public void Render(InventoryItemSlot itemSlot)
    {
        if (itemSlot == null || itemSlot.IsEmpty)
        {
            icon.enabled = false;
            quantityText.text = "";
        }
        else
        {
            icon.enabled = true;
            icon.sprite = itemSlot.Item.Data.ItemIcon;
            quantityText.text = itemSlot.Item.Data.IsStackable ? itemSlot.Quantity.ToString() : "";
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnSlotClicked?.Invoke(_index);
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
        }
    }
    
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (!icon.enabled) return;
        InventoryDragHandler.Instance.StartDrag(icon.sprite, _index);
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        InventoryDragHandler.Instance.UpdatePosition(eventData.position);
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        InventoryDragHandler.Instance.EndDrag();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        int fromIndex = InventoryDragHandler.Instance.DraggingIndex;
        
        if (fromIndex != _index)
        {
            _uiController.SwapSlots(fromIndex, _index);
        }
    }
}