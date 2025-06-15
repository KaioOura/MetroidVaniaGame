using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/AttackDataScriptableObject", order = 1)]
public class AttackData : ActionData
{
    [Header("Attack Data")]
    [SerializeField] private int _colliderIndex;
    [SerializeField] protected FrameActionHitBox[] _frameActionHitboxes;
    [SerializeField] protected float _damage;
    public FrameActionHitBox[] FrameActionHitboxes => _frameActionHitboxes;


    [ContextMenu("Copy Collider Values")]
    public void CopyColliderValues()
    {
        _frameActionHitboxes[_colliderIndex].ColliderInfos.CopySelectedTransformPosition();
    }

}

[Serializable]
public class FrameActionHitBox : FrameAction
{
    public ColliderInfos ColliderInfos;
}


// public class ArrayElementSelector : MonoBehaviour
// {
//     public AttackData attackData;
// }
//
// [CustomEditor(typeof(ArrayElementSelector))]
// public class ArrayElementSelectorEditor : Editor
// {
//     private int selectedIndex = -1;
//
//     public override void OnInspectorGUI()
//     {
//         ArrayElementSelector script = (ArrayElementSelector)target;
//
//         for (int i = 0; i < script.attackData.FrameActionHitboxes.Length; i++)
//         {
//             EditorGUILayout.BeginHorizontal();
//
//             if (GUILayout.Button($"Element {i}", GUILayout.Width(100)))
//             {
//                 selectedIndex = i;
//                 Debug.Log($"Selected element index: {i}, value: {script.attackData.FrameActionHitboxes[i]}");
//             }
//
//             //script.attackData.FrameActionHitboxes[i] = EditorGUILayout.IntField(script.attackData.FrameActionHitboxes[i]);
//
//             EditorGUILayout.EndHorizontal();
//         }
//
//         if (selectedIndex != -1)
//         {
//             EditorGUILayout.HelpBox($"Selected element index: {selectedIndex}, value: {script.attackData.FrameActionHitboxes[selectedIndex]}", MessageType.Info);
//         }
//
//         if (GUI.changed)
//         {
//             EditorUtility.SetDirty(script);
//         }
//      }
// }
