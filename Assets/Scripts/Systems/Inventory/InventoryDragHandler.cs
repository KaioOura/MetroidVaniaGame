using UnityEngine;
using UnityEngine.UI;

public class InventoryDragHandler : MonoBehaviour
{
    public static InventoryDragHandler Instance { get; private set; }

    [SerializeField] private Canvas canvas;
    [SerializeField] private Image dragImage;

    public int DraggingIndex { get; private set; } = -1;

    private void Awake()
    {
        Instance = this;
        dragImage.gameObject.SetActive(false);
    }

    public void StartDrag(Sprite icon, int index)
    {
        DraggingIndex = index;
        dragImage.sprite = icon;
        dragImage.transform.SetParent(canvas.transform);
        dragImage.transform.SetAsLastSibling();
        dragImage.gameObject.SetActive(true);
    }

    public void UpdatePosition(Vector2 position)
    {
        dragImage.transform.position = position;
    }

    public void EndDrag()
    {
        dragImage.gameObject.SetActive(false);
        DraggingIndex = -1;
    }
}