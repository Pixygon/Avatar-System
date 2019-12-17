using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Holopipe.Avatar
{
    [CustomEditor(typeof(AvatarBody))]
    public class AvatarBodyEditor : Editor
    {


        private bool _isEdit;

        ABPCollection _abpCollection;
        Material _skinMat;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AvatarBody body = target as AvatarBody;


            if (!_isEdit)
            {
                if (GUILayout.Button("Edit Avatar"))
                {
                    _isEdit = true;
                }
            }
            else
            {
                GUILayout.Space(12);
                GUILayout.Label("Fill out fields to edit avatar");

                _abpCollection = EditorGUILayout.ObjectField(_abpCollection, typeof(ABPCollection), true) as ABPCollection;
                _skinMat = EditorGUILayout.ObjectField(_skinMat, typeof(Material), true) as Material;

                if (GUILayout.Button("Apply"))
                {

                    body.SetDefinition(_abpCollection, new AvatarBodyPartMask(), 0);
                    body.SkinRenderer.material = _skinMat;
                    _isEdit = false;
                }

                if (GUILayout.Button("Back"))
                {
                    _isEdit = false;
                }


            }


        }
    }
}

