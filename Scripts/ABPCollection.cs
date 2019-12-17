using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holopipe.Avatar
{
    [ExecuteAlways]
    public class ABPCollection : MonoBehaviour
    {
        [SerializeField] private AvatarBody.AvatarBodyPartSet _collectionDefinition = default;
        [SerializeField] private GameObject _armature = default;

        [SerializeField] private int _lodLevel = 0;
        public AvatarDefinition.Gender _gender = default;
        public AvatarBody.AvatarBodyPartSet Definition { get { return _collectionDefinition; } }
        public GameObject Armature { get { return _armature; } }

        public int LodLevel { get { return _lodLevel; } }
        public AvatarDefinition.Gender Gender { get { return _gender; } }


#if UNITY_EDITOR

        public void Reset()
        {
            SkinnedMeshRenderer[] allBps = GetComponentsInChildren<SkinnedMeshRenderer>();
            Debug.Log("Initialize ABP Collection");


            #region Set up body parts

            if (_collectionDefinition.Head == null)
            {
                for (int o = 0; o < allBps.Length; o++)
                {
                    if (allBps[o].name.ToLower().Contains("head"))
                    {
                        _collectionDefinition.Head = allBps[o].gameObject.AddComponent<AvatarBodyPart>();
                        _collectionDefinition.Head.EditorInitialize(AvatarBodyPart.BodyPartType.Head);
                    }
                }
            }

            if (_collectionDefinition.Torso == null)
            {
                for (int o = 0; o < allBps.Length; o++)
                {
                    if (allBps[o].name.ToLower().Contains("torso") || allBps[o].name.ToLower().Contains("chest"))
                    {
                        _collectionDefinition.Torso = allBps[o].gameObject.AddComponent<AvatarBodyPart>();
                        _collectionDefinition.Torso.EditorInitialize(AvatarBodyPart.BodyPartType.Torso);
                    }
                }
            }

            if (_collectionDefinition.Arms == null)
            {
                for (int o = 0; o < allBps.Length; o++)
                {
                    if (allBps[o].name.ToLower().Contains("arm"))
                    {
                        _collectionDefinition.Arms = allBps[o].gameObject.AddComponent<AvatarBodyPart>();
                        _collectionDefinition.Arms.EditorInitialize(AvatarBodyPart.BodyPartType.Arms);
                    }
                }
            }

            if (_collectionDefinition.Hands == null)
            {
                for (int o = 0; o < allBps.Length; o++)
                {
                    if (allBps[o].name.ToLower().Contains("hand"))
                    {
                        _collectionDefinition.Hands = allBps[o].gameObject.AddComponent<AvatarBodyPart>();
                        _collectionDefinition.Hands.EditorInitialize(AvatarBodyPart.BodyPartType.Hands);
                    }
                }
            }

            if (_collectionDefinition.Legs == null)
            {
                for (int o = 0; o < allBps.Length; o++)
                {
                    if (allBps[o].name.ToLower().Contains("leg"))
                    {
                        _collectionDefinition.Legs = allBps[o].gameObject.AddComponent<AvatarBodyPart>();
                        _collectionDefinition.Legs.EditorInitialize(AvatarBodyPart.BodyPartType.Legs);
                    }
                }
            }

            if (_collectionDefinition.Feet == null)
            {
                for (int o = 0; o < allBps.Length; o++)
                {
                    if (allBps[o].name.ToLower().Contains("feet") || allBps[o].name.ToLower().Contains("foot") || allBps[o].name.ToLower().Contains("fot"))
                    {
                        _collectionDefinition.Feet = allBps[o].gameObject.AddComponent<AvatarBodyPart>();
                        _collectionDefinition.Feet.EditorInitialize(AvatarBodyPart.BodyPartType.Feet);
                    }
                }
            }
            #endregion

            foreach (Transform t in transform.GetComponentsInChildren<Transform>())
            {
                if(t.childCount > 1)
                {
                    _armature = t.gameObject;
                    return;
                }
            }

        }

#endif

    }
}

