using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace JumpJam.Editor
{
    public class MeshSaveEditor : MonoBehaviour
    {
		[MenuItem("CONTEXT/MeshFilter/Save Mesh")]
		public static void SaveMeshInPlace(MenuCommand menuCommand)
		{
			MeshFilter mf = menuCommand.context as MeshFilter;
			Mesh m = mf.sharedMesh;
			SaveMesh(m, m.name, false, true);
		}

		[MenuItem("CONTEXT/MeshFilter/Save Mesh As New Instance")]
		public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
		{
			MeshFilter mf = menuCommand.context as MeshFilter;
			Mesh m = mf.sharedMesh;
			SaveMesh(m, m.name, true, true);
		}

		public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
		{
			string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
			if (string.IsNullOrEmpty(path)) return;

			path = FileUtil.GetProjectRelativePath(path);

			Mesh meshToSave = (makeNewInstance) ? Instantiate(mesh) : mesh;

			if (optimizeMesh)
			{
				MeshUtility.Optimize(meshToSave);
			}

			AssetDatabase.CreateAsset(meshToSave, path);
			AssetDatabase.SaveAssets();
		}
	}
}
