using System;
using System.Collections.Generic;
using UnityEngine;

namespace Holopipe.Avatar
{
    public class AvatarClothingPiece : MonoBehaviour
    {
        [Tooltip("Select the slot the clothing peice occupies")]
        [SerializeField] private ClothingPieceType _clothingPieceType = default;

        [Tooltip("Select All BodyParts that are fully occluded by this clothing peice")]
        [EnumFlagsAttribute]
        [SerializeField] private AvatarBodyPartMask _ABPMask = default;

        [SerializeField] private SkinnedMeshRenderer _renderer = default;

        public GameObject Armature;

        public ClothingPieceType ClothingPieceType { get { return _clothingPieceType; } }
        public AvatarBodyPartMask BodyMask { get { return _ABPMask; } }
        public SkinnedMeshRenderer Renderer { get{ return _renderer; } }
        
    }

    public enum ClothingPieceType
    {
        Head,
        Torso,
        Hands,
        Legs,
        Feet
    }

}


