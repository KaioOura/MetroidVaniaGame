using TMPro;
using UnityEngine;

public class ToolTipUiController : MonoBehaviour
{
    [SerializeField] private RectTransform  toolTip;
    [SerializeField] private TextMeshProUGUI toolTipText;
    

    [Header("Offset")]
    [SerializeField] private Vector2 offset = new Vector2(15f, -15f);

    private void Awake()
    {
        HideTooltip();
    }

    private void Update()
    {
        if (toolTip.gameObject.activeSelf)
        {
            Vector2 mousePos = Input.mousePosition;
            toolTip.position = mousePos + offset;
        }
    }

    public void ShowTooltip(string content)
    {
        toolTipText.text = content;
        toolTip.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        toolTip.gameObject.SetActive(false);
    }
}
