using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace JumpJam
{
    public class RocksPlaceTool : EditorWindow
    {
        [SerializeField] private Transform _rocksRoot = null;
        [SerializeField] private List<Transform> _rockPrefabs = new List<Transform>();
        private List<Transform> _rocks = new List<Transform>(128);
        private Vector2Int _fieldSize = Vector2Int.one;
        private Vector3 _offset = Vector3.zero;

        [MenuItem("Window/Jump Jam/Rocks Place")]
        private static void Init()
        {
            // Get existing open window or if none, make a new one:
            RocksPlaceTool window = GetWindow<RocksPlaceTool>();
            window.Show();
        }

        private void OnGUI()
        {
            _rocksRoot = EditorGUILayout.ObjectField("Rocks Root", _rocksRoot, typeof(Transform), true) as Transform;

            SerializedObject serialObj = new SerializedObject(this);
            SerializedProperty serialProp = serialObj.FindProperty("_rockPrefabs");
            EditorGUILayout.PropertyField(serialProp, true);

            _fieldSize = EditorGUILayout.Vector2IntField("Field Size", _fieldSize);
            _offset = EditorGUILayout.Vector3Field("Offset", _offset);

            if (_rockPrefabs.Count < 1)
            {
                return;
            }

            var rockPrefab = _rockPrefabs[Random.Range(0, _rockPrefabs.Count)];
            if (GUILayout.Button("Spawn"))
            {
                foreach (var rock in _rocks)
                {
                    DestroyImmediate(rock.gameObject);
                }

                _rocks.Clear();

                for (int i = 0; i < _fieldSize.x; i++)
                {
                    for (int j = 0; j < _fieldSize.y; j++)
                    {
                        if (i != 0 && i != _fieldSize.x - 1 && j != 0 && j != _fieldSize.y - 1)
                        {
                            continue;
                        }

                        var rock = PrefabUtility.InstantiatePrefab(rockPrefab, SceneManager.GetActiveScene()) as Transform;
                        rock.parent = _rocksRoot;
                        rock.localPosition = new Vector3(i, 0, j) + _offset;

                        _rocks.Add(rock);
                    }
                }
            }
        }
    }
}
