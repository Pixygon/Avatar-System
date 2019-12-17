using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holopipe.Avatar
{
    public class ClothingHolder : MonoBehaviour
    {

        [Tooltip("Clothing peices order: Head, Torso, Hands, Legs, Feet")]
        [SerializeField] private AvatarClothingPiece[] _clothingPeices = default;
        [SerializeField] private SkinnedMeshRenderer _bodyRend = default;

        public AvatarClothingPiece[] ClothingPieces { get { return _clothingPeices; } }

        public void Initialize(AvatarEntity entity)
        {
            _bodyRend = entity.AvatarBody.SkinRenderer;
        }
        public void AttatchClothingPeice(AvatarClothingPiece newClothingPeice)
        {
            if(_clothingPeices == null || _clothingPeices.Length < 5)
            {
                _clothingPeices = new AvatarClothingPiece[5];
            }

            AvatarClothingPiece newCP = Instantiate(newClothingPeice, transform);

            newCP.transform.position = Vector3.zero;

            Debug.Log(newCP.Renderer.bones.Length + " " + _bodyRend.bones.Length);

            newCP.Renderer.rootBone = _bodyRend.rootBone;
            newCP.Renderer.bones = _bodyRend.bones;
            //Destroy(newCP.Armature);
            //DestroyImmediate(newCP.Armature);
            //Destroy(newClothingPeice.sol)

            int cpIndex = (int)newClothingPeice.ClothingPieceType;
            //Destroy the allready attatched clothing peice
            if(_clothingPeices[cpIndex] != null)
            {
                Destroy(_clothingPeices[cpIndex].gameObject);
            }

            _clothingPeices[cpIndex] = newCP;

        }


        public void SetClothingSimple(SkinnedMeshRenderer[] clothings, Material[] clothingMats)
        {
            Dictionary<string, Transform> boneDict = new Dictionary<string, Transform>();
            for (int i = 0; i < _bodyRend.bones.Length; i++)
                boneDict.Add(_bodyRend.bones[i].name, _bodyRend.bones[i]);

            Transform[] newBoneList = new Transform[0];

            for (int i = 0; i < clothings.Length; i++)
            {
                if (clothings[i] == null)
                    Debug.Log("Clothing at index " + i + " is null");


                SkinnedMeshRenderer newRend = Instantiate(clothings[i], transform);
                newBoneList = new Transform[newRend.bones.Length];
                for (int o = 0; o < newRend.bones.Length; o++)
                {
                    if (boneDict.ContainsKey(newRend.bones[o].name))
                    {
                        newBoneList[o] = boneDict[newRend.bones[o].name];
                    }
                }
                newRend.bones = newBoneList;
                newRend.rootBone = _bodyRend.rootBone;
                newRend.material = clothingMats[i];
            }
        }
    

#if UNITY_EDITOR
        //Validate all attatched peices if set in editor

        void OnValidate()
        {
            if (_clothingPeices == null)
                return;

            if (_clothingPeices.Length > 5)
            {
                AvatarClothingPiece[] newDef = new AvatarClothingPiece[5];

                for (int i = 0; i < newDef.Length; i++)
                {
                    newDef[i] = _clothingPeices[i];
                }
                _clothingPeices = newDef;
            }
        }
#endif
    }
}

