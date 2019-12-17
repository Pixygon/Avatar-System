using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Holopipe.Avatar
{
    [ExecuteAlways]
    public class AvatarEntity : MonoBehaviour
    {
        [SerializeField] private AvatarBody _avatarBody = default;
        [SerializeField] private ClothingHolder _clothingHolder = default;
        [SerializeField] private AvatarHairHolder _hairHolder = default;
        [SerializeField] private AvatarDefinition _definition = default;
        [SerializeField] private Animator _anim = default;

        [SerializeField][HideInInspector]private GameObject _clothingHolderGO;


        public AvatarBody AvatarBody { get { return _avatarBody; } }
        public ClothingHolder ClothingHolder { get { return _clothingHolder; } }
        public AvatarHairHolder HairHolder { get { return _hairHolder; } }
        public AvatarDefinition AvatarDefinition { get { return _definition; } }
        public Animator Animator { get { return _anim; } }

#if UNITY_EDITOR

        [SerializeField] [HideInInspector] private bool _initialized;

        /// <summary>
        /// Sets up the Avatar prefabs, so i dont have to do so much meaningless work on reimport of models
        /// </summary>
        private void Reset()
        {
            if (_initialized)
                return;

            if (EditorApplication.isPlaying)
                return;
            if(_clothingHolderGO == null)
            {
                _clothingHolderGO = new GameObject("Clothing / Accessories");
                _clothingHolderGO.transform.parent = transform;
            }

            if (_definition == null)
            {
                _definition = GetComponent<AvatarDefinition>();

                if(_definition == null)
                {
                    _definition = gameObject.AddComponent<AvatarDefinition>();
                }

                _definition.EditorInitialize();

            }
            _definition.Initialize();

            if (_avatarBody == null)
            {
                _avatarBody = GetComponent<AvatarBody>();

                if (_avatarBody == null)
                {
                    _avatarBody = gameObject.AddComponent<AvatarBody>();
                }
            }
            _avatarBody.EditorInitialize(_definition.RootBone);

            if (_clothingHolder == null)
            {
                _clothingHolder = GetComponentInChildren<ClothingHolder>();

                if(_clothingHolder == null)
                {
                    _clothingHolder = _clothingHolderGO.AddComponent<ClothingHolder>();
                }
            }
            _clothingHolder.Initialize(this);
            
            if(_hairHolder == null)
            {
                _hairHolder = GetComponentInChildren<AvatarHairHolder>();

                if(_hairHolder == null)
                {
                    _hairHolder = _clothingHolderGO.AddComponent<AvatarHairHolder>();
                }
            }
            _hairHolder.Initialize(this);

            if(_anim == null)
            {
                _anim = GetComponent<Animator>();
            }

            _initialized = true;
            print("Setup AvatarEntity");
        }
        
#endif
    }
}

