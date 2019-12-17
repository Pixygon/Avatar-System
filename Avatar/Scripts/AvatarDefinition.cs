using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holopipe.Avatar
{
    public class AvatarDefinition : MonoBehaviour
    {
        [Header("Attatch appropriate joints here for lookup")]
        [SerializeField] private Transform _head = default;
        [SerializeField] private Transform _leftHand = default;
        [SerializeField] private Transform _rightHand = default;
        [Space]
        [SerializeField] private Transform _rootBone = default;

        [SerializeField] private Gender _characterGender = default;

        public Transform Head { get { return _head; } }
        public Transform LeftHand { get { return _leftHand; } }
        public Transform RightHand { get { return _rightHand; } }
        public Transform RootBone { get { return _rootBone; } }
        public Gender CharacterGender { get { return _characterGender; } }

        public void Initialize()
        {
            //_head = transform.Find("head");
            //_leftHand = transform.Find("hand_L");
            //_rightHand = transform.Find("hand_R");
        }
        public enum Gender
        {
            Neutral,
            male,
            female
        }


#if UNITY_EDITOR

        public void EditorInitialize()
        {
            Transform[] allChildren = GetComponentsInChildren<Transform>();

            for (int i = 0; i < allChildren.Length; i++)
            {
                if (allChildren[i].name.ToLower().Contains("head"))
                {
                    _head = allChildren[i];
                }
                else if (allChildren[i].name.ToLower().Contains("hand"))
                {
                    if (allChildren[i].name.Contains("L"))
                    {
                        _leftHand = allChildren[i];
                    }
                    else if (allChildren[i].name.Contains("R"))
                    {
                        _rightHand = allChildren[i];
                    }
                }
                else if (allChildren[i].name.ToLower().Contains("root"))
                {
                    _rootBone = allChildren[i];
                }
            }

        }

#endif

    }
}

