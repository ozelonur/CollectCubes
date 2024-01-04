#region Header

// Developed by Onur ÖZEL

#endregion

#if UNITY_EDITOR

using System.Collections.Generic;
using System.IO;
using _GAME_.Scripts.Bears;
using _GAME_.Scripts.Bears.Cube;
using _GAME_.Scripts.Models;
using _GAME_.Scripts.Utils;
using _ORANGEBEAR_.Scripts.Bears;
using _ORANGEBEAR_.Scripts.Enums;
using _ORANGEBEAR_.Scripts.ScriptableObjects;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace _ORANGEBEAR_.Scripts.Editor
{
    public class LevelCreator : EditorWindow
    {
        #region Fields

        private GameObject _level;
        private Level _levelData;

        private DirectoryInfo _directoryInfo;

        private int _count;

        private SerializedObject _serializedObject;
        private SerializedProperty _serializedProperty;

        #endregion

        #region Serialized Fields

        [SerializeField] private CustomLevelData[] customLevelDatas;

        #endregion

        [MenuItem("Orange Bear/Create Level")]
        public static void OpenLevelCreatorWindow()
        {
            GetWindow<LevelCreator>("LevelCreator");
        }

        private void OnEnable()
        {
            _serializedObject = new SerializedObject(this);
            _serializedProperty = _serializedObject.FindProperty("customLevelDatas");
        }

        private void OnGUI()
        {
            _serializedObject.Update();

            GUILayout.Label("Level Prefab", EditorStyles.boldLabel);
            _level = (GameObject)EditorGUILayout.ObjectField("Level Prefab", _level, typeof(GameObject), false);
            GUILayout.Label("Level Data", EditorStyles.boldLabel);
            _levelData = (Level)EditorGUILayout.ObjectField("Level Data", _levelData, typeof(Level), false);
            GUILayout.Label("Custom Level Data", EditorStyles.boldLabel);

            EditorGUILayout.PropertyField(_serializedProperty, true);
            _serializedObject.ApplyModifiedProperties();

            if (GUILayout.Button("Create Level"))
            {
                int index = 0;
                for (int i = 0; i < customLevelDatas.Length; i++)
                {
                    CreateLevel(index);
                    index++;
                }
            }

            if (GUILayout.Button("Clear All Player Prefs"))
            {
                PlayerPrefs.DeleteAll();
            }
        }

        #region Private Methods

        private void CreateLevel(int index)
        {
            DirectoryInfo info = new DirectoryInfo("Assets/[GAME]/Levels");
            var fileInfo = info.GetFiles();

            _count = fileInfo.Length;

            if (_count == 1)
            {
                _count = 1;
            }

            else
            {
                _count = ((_count - 1) / 4) + 1;
            }


            GameObject levelReference = (GameObject)PrefabUtility.InstantiatePrefab(_level);

            GameLevelBear gameLevelBear = (GameLevelBear)levelReference.GetComponent<LevelBear>();
            gameLevelBear.levelType = customLevelDatas[index].LevelType;

            Transform parent = levelReference.transform.Find("CubesParent");

            if (customLevelDatas[index].LevelType == LevelType.Time)
            {
                CooldownTimer timer = levelReference.AddComponent<CooldownTimer>();

                CubePositionsController cubePositionsController =
                    levelReference.AddComponent<CubePositionsController>();
                
                cubePositionsController.parent = parent;
                timer.cooldownTime = customLevelDatas[index].LevelTime;
                CreateCubesAndCalculateOtherShapesPositions(cubePositionsController, parent, index);
            }

            else
            {
                CreateCubes(parent, customLevelDatas[index].LevelReference, customLevelDatas[index].Scale);
            }


            GameObject pVariant =
                PrefabUtility.SaveAsPrefabAsset(levelReference, $"Assets/[GAME]/Levels/_Level {_count}.prefab");


            DestroyImmediate(levelReference);


            string dataPath = AssetDatabase.GetAssetPath(_levelData.GetInstanceID());

            AssetDatabase.CopyAsset(dataPath, $"Assets/[GAME]/Levels/Level {_count}.asset");

            Level asset = AssetDatabase.LoadAssetAtPath<Level>($"Assets/[GAME]/Levels/Level {_count}.asset");
            asset.LevelPrefab = pVariant;

            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
        }

        private void CreateCubes(Transform parent, Texture2D texture2D, float scaleValue)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/[GAME]/Prefabs/Cube.prefab");
            Texture2D texture = texture2D;

            SpawnController spawnController = parent.root.GetComponent<SpawnController>();

            int width = texture.width;
            int height = texture.height;

            float scale = scaleValue;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Color color = texture.GetPixel(x, y);

                    if (!(color.a > 0)) continue;

                    Vector3 cubePosition = new Vector3
                    {
                        x = (x - width / 2) * scale,
                        y = scale / 2f,
                        z = (y - height / 2) * scale
                    };

                    GameObject cube =
                        (GameObject)PrefabUtility.InstantiatePrefab(prefab);

                    cube.transform.SetParent(parent);

                    CubeController cubeController = cube.GetComponent<CubeController>();

                    CubeData cubeData = new CubeData
                    {
                        cube = cubeController,
                        color = color
                    };

                    spawnController.cubes.Add(cubeData);

                    cube.transform.position = cubePosition;
                    cube.transform.localScale = Vector3.one * scale;
                }
            }
        }

        private void CreateCubesAndCalculateOtherShapesPositions(CubePositionsController cubePositionsController,
            Transform parent, int index)
        {
            TextureAndScaleData[] textureAndScaleDatas = customLevelDatas[index].TextureAndScaleDatas;

            CreateCubes(parent, customLevelDatas[index].TextureAndScaleDatas[0].texture,
                customLevelDatas[index].TextureAndScaleDatas[0].scale);

            List<List<CalculatedLevelData>> levelDatas = new List<List<CalculatedLevelData>>();

            for (int i = 1; i < textureAndScaleDatas.Length; i++)
            {
                Texture2D texture = textureAndScaleDatas[i].texture;

                List<CalculatedLevelData> calculatedLevelDatas = new List<CalculatedLevelData>();

                int width = texture.width;
                int height = texture.height;

                float scale = textureAndScaleDatas[i].scale;

                for (int x = 0; x < width; x++)
                {
                    for (int y = 0; y < height; y++)
                    {
                        Color color = texture.GetPixel(x, y);

                        if ((color.a > 0))
                        {
                            Vector3 cubePosition = new Vector3
                            {
                                x = (x - width / 2) * scale,
                                y = scale / 2f,
                                z = (y - height / 2) * scale
                            };

                            CalculatedLevelData calculatedLevelData = new CalculatedLevelData
                            {
                                position = cubePosition,
                                scale = scale,
                                color = color
                            };

                            calculatedLevelDatas.Add(calculatedLevelData);
                        }
                    }
                }

                CalculatedLevelDataListStorer calculatedLevelDataListStorer = new CalculatedLevelDataListStorer
                {
                    calculatedPositions = calculatedLevelDatas
                };

                levelDatas.Add(calculatedLevelDatas);
                cubePositionsController.calculatedLevelDatas.Add(calculatedLevelDataListStorer);
            }

            // cubePositionsController.calculatedLevelDatas = levelDatas;
        }

        #endregion
    }
}


#endif