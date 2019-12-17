using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections;
using System.Linq;

namespace Holopipe.Avatar
{
    public class AvatarBody : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _renderer = default;
        [SerializeField] private AvatarBodyPartSet _bpSet = default;
        private Animator _anim;
        private SkinnedMeshRenderer[] _bodyParts;
        private bool[] _isABPOccluded = new bool[6];
        private int[] _abpCollectionIndecies;
        

        public int[] BodyPartIndecies { get { return _abpCollectionIndecies; } }
        public SkinnedMeshRenderer SkinRenderer { get { return _renderer; } }
        public AvatarBodyPartSet ABPSet { get { return _bpSet; } }


        //TODO: Should account for occluded bodypats. The bodypart should still be assigned to bodyparts list, but not instantiated and batched
        /// <summary>
        /// Apply a new set of bodyparts to avatar. Function should validate and fix any issues with given definition
        /// </summary>
        /// <param name="abpCollection"></param>
        /// TODO: Make sure the function efficiently validates the given definition.
        public void SetDefinition(ABPCollection abpCollection, AvatarBodyPartMask bpMask ,int collectionIndex)
        {
            //Validates the definiton. Return false if any part is misslabeled
            //if (!_definition.ValidateDefinition())
            {
                //Debug.LogError("Definition not valid!");
                //return;
            }

            //Sets up local index array thats points to which collection is used per slot

#if UNITY_EDITOR
            _abpCollectionIndecies = new int[6];
            _bodyParts = new SkinnedMeshRenderer[6];
#elif !UNITY_EDITOR
            if (_abpCollectionIndecies == null || _bodyParts == null)
            {
                _abpCollectionIndecies = new int[6];
                _bodyParts = new SkinnedMeshRenderer[6];
            }
#endif




            for (int i = 0; i < _abpCollectionIndecies.Length; i++)
            {
                _abpCollectionIndecies[i] = collectionIndex;
            }

            _bodyParts[0] = abpCollection.Definition.Head.SkinnedMeshRenderer;
            _bodyParts[1] = abpCollection.Definition.Torso.SkinnedMeshRenderer;

            _bodyParts[2] = abpCollection.Definition.Arms.SkinnedMeshRenderer;
            _bodyParts[3] = abpCollection.Definition.Hands.SkinnedMeshRenderer;
            _bodyParts[4] = abpCollection.Definition.Legs.SkinnedMeshRenderer;
            _bodyParts[5] = abpCollection.Definition.Feet.SkinnedMeshRenderer;

            CombineMeshes(_bodyParts, _renderer , bpMask);

        }

        /// <summary>
        /// FUx tgus later bby
        /// </summary>
        /// <param name="ABP"></param>
        /// <param name="ABPindex"></param>
        /// <param name="collectionIndex"></param>
        public void AddABP(AvatarBodyPart ABP,int ABPindex ,int collectionIndex)
        {

        }

        /// <summary>
        /// Fix This
        /// </summary>
        /// <param name="ABPindex"></param>
        public void RemoveABP(int ABPindex)
        {
        }
        /// <summary>
        /// Fix this
        /// </summary>
        private void ClearABPs()
        {

        }

        /// <summary>
        /// FIx this function
        /// </summary>
        /// <param name="clothingPieces"></param>
        public void UpdateABPClothingOcclution(AvatarClothingPiece[] clothingPieces)
        {
            //Gets the bodyMasks of all clothing pieces
            for (int i = 0; i < clothingPieces.Length; i++)
            {
                if (clothingPieces[i] == null) continue;

                foreach (int index in EnumFlagsAttribute.ReturnSelectedElements(clothingPieces[i].BodyMask))
                {
                    _isABPOccluded[index] = true;
                }
            }

            ClearABPs();
            List<GameObject> nonOccludedABPs = new List<GameObject>();

            for (int i = 0; i < _isABPOccluded.Length; i++)
            {
                if (_isABPOccluded[i])
                {
                   
                }
            }

            

        }

        public void CombineMeshes(SkinnedMeshRenderer[] sourceRenderers, SkinnedMeshRenderer targetRenderer, AvatarBodyPartMask bpMask)
        {
            Mesh combinedMesh = new Mesh();
            combinedMesh.name = "New mesh";
            Mesh[] meshes = new Mesh[sourceRenderers.Length];
            for (int i = 0; i < sourceRenderers.Length; i++)
            {
                //If hts masked away, we dont add the mesh to the list. This way, we could still keep a reference to it and bake it in later, if we wanted to.
                if (!bpMask.HasFlag((AvatarBodyPartMask)Mathf.Pow(2,i)))
                {
                    meshes[i] = sourceRenderers[i].sharedMesh;
                }               
            }

            //Transformation data for the bind pose. Supposed to help orient the mesh
            Matrix4x4 transformMatrix = targetRenderer.transform.worldToLocalMatrix;
            Quaternion rotation = Quaternion.LookRotation(transformMatrix.GetColumn(2), transformMatrix.GetColumn(1));

            //Creates a custom list of bones. Exported meshes often times reffers to their bones differently. This fixes that.
            //The targetrenderer need to have its root bone assigned.
            List<Transform> bones = targetRenderer.rootBone.GetComponentsInChildren<Transform>().ToList();
            Dictionary<string, int> boneDict = new Dictionary<string, int>();
            for (int i = 0; i < bones.Count; i++)
            {
                boneDict.Add(bones[i].name, i);
            }

            //Lists for data of the combined mesh.
            List<Vector3> vertecies = new List<Vector3>();
            List<Vector3> normals = new List<Vector3>();
            List<Vector2> uvs = new List<Vector2>();
            List<int> triangles = new List<int>();
            List<BoneWeight1> boneWeights = new NativeArray<BoneWeight1>().ToList();
            List<byte> bonesPerVertex = new NativeArray<byte>().ToList();

            int vertexOffset = 0;

            //Loop through all meshes, and add their data to the lists.
            for (int i = 0; i < meshes.Length; i++)
            {
                if(meshes[i] == null)
                {
                    continue;
                }

                Vector3[] vs = meshes[i].vertices;
                Vector3[] ns = meshes[i].normals;
                int[] ts = meshes[i].triangles;
                List<BoneWeight1> bws = new List<BoneWeight1>();
                NativeArray<BoneWeight1> refBws = meshes[i].GetAllBoneWeights();
                NativeArray<byte> bpv = meshes[i].GetBonesPerVertex();

                for (int o = 0; o < vs.Length; o++)
                    vs[o] = transformMatrix.MultiplyPoint3x4(vs[o]);

                for (int o = 0; o < ns.Length; o++)
                    ns[o] = rotation * ns[o];

                for (int o = 0; o < ts.Length; o++)
                    ts[o] += vertexOffset;

                //bwOffset = boneDict[sourceRenderers[i].rootBone.name];

                for (int o = 0; o < refBws.Length; o++)
                {
                    BoneWeight1 newBw = new BoneWeight1
                    {
                        boneIndex = boneDict[sourceRenderers[i].bones[refBws[o].boneIndex].name],
                        weight = refBws[o].weight,
                    };
                    bws.Add(newBw);
                }

                vertecies.AddRange(vs);
                normals.AddRange(ns);
                uvs.AddRange(meshes[i].uv);
                triangles.AddRange(ts);
                boneWeights.AddRange(bws);
                bonesPerVertex.AddRange(bpv.ToList());
                vertexOffset += meshes[i].vertexCount;
            }

            combinedMesh.vertices = vertecies.ToArray();
            combinedMesh.normals = normals.ToArray();
            combinedMesh.uv = uvs.ToArray();
            combinedMesh.triangles = triangles.ToArray();
            //combinedMesh.boneWeights = boneWeights.ToArray();

            if (boneWeights == null || boneWeights.Count == 0)
                Debug.Log("Emptyu");


            NativeArray<BoneWeight1> bwNArray = new NativeArray<BoneWeight1>(boneWeights.ToArray(), Allocator.Temp);
            NativeArray<Byte> bpvNArray = new NativeArray<byte>(bonesPerVertex.ToArray(), Allocator.Temp);

            combinedMesh.SetBoneWeights(bpvNArray, bwNArray);
            Matrix4x4[] bindPoses = new Matrix4x4[bones.Count];

            for (int i = 0; i < bindPoses.Length; i++)
            {
                bindPoses[i] = bones[i].worldToLocalMatrix * transform.localToWorldMatrix;
            }
            targetRenderer.bones = bones.ToArray();
            combinedMesh.bindposes = bindPoses;
            combinedMesh.RecalculateBounds();

            targetRenderer.sharedMesh = combinedMesh;
        }

#if UNITY_EDITOR

        public void EditorInitialize(Transform rootBone)
        {
            if(_renderer == null)
            {
                _renderer = GetComponentInChildren<SkinnedMeshRenderer>();

                if(_renderer == null)
                {
                    GameObject skinObject =new GameObject("Skin");
                    skinObject.transform.parent = transform;
                    _renderer = skinObject.AddComponent<SkinnedMeshRenderer>();
                    _renderer.rootBone = rootBone;
                }
            }

            if(_anim == null)
            {
                _anim = GetComponent<Animator>();

                if(_anim == null)
                {
                    _anim = gameObject.AddComponent<Animator>();
                }
            }
        }

#endif

        /// <summary>
        /// Contains all body segments
        /// </summary>
        [Serializable]
        public class AvatarBodyPartSet
        {
            [SerializeField] public AvatarBodyPart Head = default;
            [SerializeField] public AvatarBodyPart Torso = default;
            [Space]
            [SerializeField] public AvatarBodyPart Arms = default;
            [SerializeField] public AvatarBodyPart Hands = default;
            [SerializeField] public AvatarBodyPart Legs = default;
            [SerializeField] public AvatarBodyPart Feet = default;

        }

    }
}


