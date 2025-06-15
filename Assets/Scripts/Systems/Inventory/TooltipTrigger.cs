using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField] private string tooltipContent;
    
    private ToolTipUiController _tooltipController;

    public void Initialize(ToolTipUiController tooltipController)
    {
        _tooltipController = tooltipController;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _tooltipController.ShowTooltip(tooltipContent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tooltipController.HideTooltip();
    }

    public void SetTooltipContent(string content)
    {
        tooltipContent = content;
    }

    public void HideTooltip()
    {
        _tooltipController.HideTooltip();
    }
}