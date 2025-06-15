using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "Scriptable Objects/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    public List<ItemData> ItemsDatas => itemDatas;
    
    [SerializeField] private List<ItemData> itemDatas = new List<ItemData>();
}
