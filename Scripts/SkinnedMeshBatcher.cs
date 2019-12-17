using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Holopipe.Avatar
{
    public class SkinnedMeshBatcher
    {

        public void CombineShinnedMeshes(SkinnedMeshRenderer[] sourceRenderers, SkinnedMeshRenderer targetRenderer)
        {
            Mesh combinedMesh = new Mesh();
            combinedMesh.name = "New mesh";

            Mesh[] meshes = new Mesh[sourceRenderers.Length];
            for (int i = 0; i < sourceRenderers.Length; i++)
            {
                meshes[i] = sourceRenderers[i].sharedMesh;
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
            List<BoneWeight> boneWeights = new List<BoneWeight>();
            List<byte> bonesPerVertex = new List<byte>();
            int vertexOffset = 0;

            //Loop through all meshes, and add their data to the lists.
            for (int i = 0; i < meshes.Length; i++)
            {
                Vector3[] vs = meshes[i].vertices;
                Vector3[] ns = meshes[i].normals;
                int[] ts = meshes[i].triangles;
                BoneWeight[] bws = new BoneWeight[meshes[i].boneWeights.Length];

                for (int o = 0; o < vs.Length; o++)
                    vs[o] = transformMatrix.MultiplyPoint3x4(vs[o]);

                for (int o = 0; o < ns.Length; o++)
                    ns[o] = rotation * ns[o];

                for (int o = 0; o < ts.Length; o++)
                    ts[o] += vertexOffset;

                for (int o = 0; o < bws.Length; o++)
                {
                    bws[o] = new BoneWeight()
                    {
                        //This finds the index of the meshes refferences bones in the new bone list.
                        boneIndex0 = boneDict[sourceRenderers[i].bones[meshes[i].boneWeights[o].boneIndex0].name],
                        boneIndex1 = boneDict[sourceRenderers[i].bones[meshes[i].boneWeights[o].boneIndex1].name],
                        boneIndex2 = boneDict[sourceRenderers[i].bones[meshes[i].boneWeights[o].boneIndex2].name],
                        boneIndex3 = boneDict[sourceRenderers[i].bones[meshes[i].boneWeights[o].boneIndex3].name],

                        weight0 = meshes[i].boneWeights[o].weight0,
                        weight1 = meshes[i].boneWeights[o].weight1,
                        weight2 = meshes[i].boneWeights[o].weight2,
                        weight3 = meshes[i].boneWeights[o].weight3
                    };
                }

                vertecies.AddRange(vs);
                normals.AddRange(ns);
                uvs.AddRange(meshes[i].uv);
                triangles.AddRange(ts);
                boneWeights.AddRange(bws);
                vertexOffset += meshes[i].vertexCount;
            }

            combinedMesh.vertices = vertecies.ToArray();
            combinedMesh.normals = normals.ToArray();
            combinedMesh.uv = uvs.ToArray();
            combinedMesh.triangles = triangles.ToArray();
            combinedMesh.boneWeights = boneWeights.ToArray();

            Matrix4x4[] bindPoses = new Matrix4x4[bones.Count];

            for (int i = 0; i < bindPoses.Length; i++)
            {
                bindPoses[i] = bones[i].worldToLocalMatrix * targetRenderer.transform.localToWorldMatrix;
            }
            targetRenderer.bones = bones.ToArray();
            combinedMesh.bindposes = bindPoses;
            combinedMesh.RecalculateBounds();

            targetRenderer.sharedMesh = combinedMesh;
        }
    }
}


