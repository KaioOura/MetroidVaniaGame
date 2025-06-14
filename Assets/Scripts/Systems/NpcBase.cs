using TMPro;
using UnityEngine;

public class NpcBase : MonoBehaviour, IInteractable
{
    [SerializeField] private NpcData npcData;

    #region Temporary region
    //This region, all it`s content and dependencies should be removed when dialogue system its done
    [SerializeField] private GameObject dialogueObj;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;

    #endregion

    public void Deselect()
    {
        //Do some visual changing
        dialogueObj.gameObject.SetActive(false);
    }

    public int GetPriority()
    {
        return InteractionPriority.NPC;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void OnInteract(Transform transform)
    {
        dialogueObj.gameObject.SetActive(!dialogueObj.activeSelf);
        textMeshProUGUI.text = $"{npcData.NpcName}: \n" + npcData.DialogueText;
    }

    public void Select()
    {
        //Do some visual changing
    }
}
