using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holopipe.Avatar
{
    public class AvatarHairHolder : MonoBehaviour
    {
        [SerializeField] private AvatarDefinition _definition;

        private AvatarHairPeice connectedHair;

        private MeshRenderer connectedHairRenderer;


        public void Initialize(AvatarEntity avatarEnitiy)
        {
            _definition = avatarEnitiy.AvatarDefinition;
        }

        public void SetNewHair(AvatarHairPeice newHair)
        {
            if(connectedHair != null)
            {
                Destroy(connectedHair);
            }

            connectedHair = Instantiate(newHair.gameObject).GetComponent<AvatarHairPeice>();
            connectedHair.transform.parent = _definition.Head;
            connectedHair.transform.localPosition = Vector3.zero;
            connectedHair.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }

        public void SetNewHairSimple(MeshRenderer newHair, Material hairMat)
        {
            if (connectedHair != null)
            {
                Destroy(connectedHair);
            }

            connectedHairRenderer = Instantiate(newHair);
            connectedHairRenderer.material = hairMat;
            connectedHairRenderer.transform.parent = _definition.Head;
            connectedHairRenderer.transform.localPosition = Vector3.zero;
            connectedHairRenderer.transform.localEulerAngles = new Vector3(-90, 0, 0);
        }
    }
}

