using UnityEngine;

[CreateAssetMenu(fileName = "NpcData", menuName = "Scriptable Objects/NpcData")]
public class NpcData : ScriptableObject
{
    public string NpcName => npcName;
    public string DialogueText => dialogueText;

    [SerializeField] private string npcName;
    [SerializeField][TextArea] private string dialogueText; //TODO: Maybe build a node based dialoue system
}
