using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holopipe.Avatar
{
    public static class AvatarUtilities
    {
        public static int BpToIndex(AvatarBodyPart bp)
        {

            switch (bp.Type)
            {
                case AvatarBodyPart.BodyPartType.Head:
                    return 0;
                case AvatarBodyPart.BodyPartType.Torso:
                    return 1;
                case AvatarBodyPart.BodyPartType.Arms:
                    return 2;
                case AvatarBodyPart.BodyPartType.Hands:
                    return 3;
                case AvatarBodyPart.BodyPartType.Legs:
                    return 4;
                case AvatarBodyPart.BodyPartType.Feet:
                    return 5;
                case AvatarBodyPart.BodyPartType.NOT_ASSIGNED:
                    Debug.LogError("BP Not assigned");
                    return -1;
            }
            return -1;
        }

        public static AvatarBodyPart IndexToBp(int index, ABPCollection collection)
        {
            switch (index)
            {
                case 0:
                    return collection.Definition.Head;
                case 1:
                    return collection.Definition.Torso;
                case 2:
                    return collection.Definition.Arms;
                case 3:
                    return collection.Definition.Hands;
                case 4:
                    return collection.Definition.Legs;
                case 5:
                    return collection.Definition.Feet;
                default:
                    return null;



            }
        }
    }
}

