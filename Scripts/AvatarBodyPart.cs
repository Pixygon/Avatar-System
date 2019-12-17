using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AvatarBodyPart : MonoBehaviour
{
    [SerializeField] private BodyPartType _Type = default;
    [SerializeField] private SkinnedMeshRenderer _renderer = default;
  
    public SkinnedMeshRenderer SkinnedMeshRenderer { get { return _renderer; } }

    public BodyPartType Type { get { return _Type; } }
    public enum BodyPartType
    {
        NOT_ASSIGNED,
        Arms, Hands, Legs, Feet,
        Head, Torso
    }
#if UNITY_EDITOR
    public void EditorInitialize(BodyPartType type)
    {
        _Type = type;
        _renderer = GetComponent<SkinnedMeshRenderer>();
    }
#endif
}
