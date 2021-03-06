﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using Object = UnityEngine.Object;

public class ViewAllQuests : EditorWindow
{
    public GUIStyle myStyle;
    public GUIStyle style;
    private string _SearchName;
    public List<Object> assetList;
    private string newFolderName;
    private bool constantSearch = false;
    private bool _showFoldout;
    private string constantSearchText;
    private Vector2 sp = Vector2.zero;
    private CreateSingleQuest wEditSingle;
    private CreateChainQuest wEditChain;
    public bool addQuest = false;
    public CreateChainQuest createChainQuest;
    public static int sa;
    public static QuestRequirement.requirementsType op;

    private List<int> allQuestId = new List<int>();


    [MenuItem("Quest Designer/ViewAllQuests")]
    public static void OpenWindow()
    {
        var w = GetWindow<ViewAllQuests>();
        w.minSize = new Vector2(600,300);
        w.StartFunctions(w);

    }
    public void StartFunctions(ViewAllQuests w)
    {
        w.style = new GUIStyle();
        w.style.fontStyle = FontStyle.Italic;
        w.style.alignment = TextAnchor.MiddleCenter;
        w.style.wordWrap = true;


        w.myStyle = new GUIStyle();
        w.myStyle.fontStyle = FontStyle.BoldAndItalic;
        w.myStyle.fontSize = 15;
        w.myStyle.alignment = TextAnchor.MiddleCenter;
        w.myStyle.wordWrap = true;
        w.constantSearchText = "Constant Search";

        w.assetList = new List<Object>();

        if (createChainQuest!= null)
        {
            for (int i = 0; i < createChainQuest.allNodes.Count; i++)
            {
                allQuestId.Add(createChainQuest.allNodes[i].questID);
            }
        }

        w.Show();

    }

    private void OnGUI()
    {
        if (!addQuest)
        {
            QuestFinder();

        }
        else
        {
            AddQuestFinder();
        }
        if (createChainQuest != null)
        {
            for (int i = 0; i < createChainQuest.allNodes.Count; i++)
            {
                allQuestId.Add(createChainQuest.allNodes[i].questID);
            }
        }
        Repaint();
    }

    private void QuestFinder()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        if (GUILayout.Button("New Single Quest"))
        {
            wEditSingle = (CreateSingleQuest)ScriptableObject.CreateInstance(typeof(CreateSingleQuest));
            wEditSingle.wantsMouseMove = true;
            wEditSingle.Show();
        }
        GUILayout.EndVertical();
        GUILayout.BeginVertical();
        if (GUILayout.Button("New Chain Quest"))
        {
            wEditChain = (CreateChainQuest)ScriptableObject.CreateInstance(typeof(CreateChainQuest));
            wEditChain.wantsMouseMove = true;
            wEditChain.chain = null;
            wEditChain.StartFunctions(wEditChain);
                
        }
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Busqueda de Quest:", myStyle);
        var aux = _SearchName;
        _SearchName = EditorGUILayout.TextField(aux);
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        if (constantSearch)
        {
            GUI.backgroundColor = Color.green;

        }
        if (GUILayout.Button(constantSearchText))
        {
            if (constantSearch)
            {
                constantSearch = false;
                constantSearchText = "Constant Search";
            }
            else
            {
                constantSearch = true;
                constantSearchText = "On Constant Search";

            }

        }
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Clear"))
        {
            assetList.Clear();
        }

        if (GUILayout.Button("Search"))
        {
            assetList.Clear();
            string[] paths = AssetDatabase.FindAssets(_SearchName, new string[] { "Assets/Quests" });


            for (int i = 0; i < paths.Length; i++)
            {

                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
                AssetDatabase.GetMainAssetTypeAtPath(paths[i]);
                if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SingleQuest) || AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(ChainQuest))
                {
                    var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));
                    assetList.Add(loaded);
                }

            }


        }
        GUILayout.EndHorizontal();


        if (aux != _SearchName && constantSearch)
        {
            assetList.Clear();
            string[] paths = AssetDatabase.FindAssets(_SearchName, new string[] { "Assets/Quests" });



            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);

                if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SingleQuest) || AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(ChainQuest))
                {
                    var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));
                    assetList.Add(loaded);
                }
            }
        }
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        WriteSearch(assetList);



    }
    private void AddQuestFinder()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("Busqueda de Quest:", myStyle);
        var aux = _SearchName;
        _SearchName = EditorGUILayout.TextField(aux);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (constantSearch)
        {
            GUI.backgroundColor = Color.green;

        }
        if (GUILayout.Button(constantSearchText))
        {
            if (constantSearch)
            {
                constantSearch = false;
                constantSearchText = "Constant Search";
            }
            else
            {
                constantSearch = true;
                constantSearchText = "On Constant Search";

            }

        }
        GUI.backgroundColor = Color.white;

        if (GUILayout.Button("Clear"))
        {
            assetList.Clear();
        }

        if (GUILayout.Button("Search"))
        {
            assetList.Clear();
            string[] paths = AssetDatabase.FindAssets(_SearchName, new string[] { "Assets/Quests" });



            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);
                AssetDatabase.GetMainAssetTypeAtPath(paths[i]);
                if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SingleQuest) || AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(ChainQuest))
                {
                    var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));
                    assetList.Add(loaded);
                }

            }


        }
        GUILayout.EndHorizontal();


        if (aux != _SearchName && constantSearch)
        {
            assetList.Clear();
            string[] paths = AssetDatabase.FindAssets(_SearchName, new string[] { "Assets/Quests" });



            for (int i = 0; i < paths.Length; i++)
            {
                paths[i] = AssetDatabase.GUIDToAssetPath(paths[i]);

                if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SingleQuest) || AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(ChainQuest))
                {
                    var loaded = AssetDatabase.LoadAssetAtPath(paths[i], typeof(Object));
                    assetList.Add(loaded);
                }
            }
        }
        WriteSearch(assetList);



    }
    public void WriteSearch(List<Object> assetList)
    {

        sp = EditorGUILayout.BeginScrollView(sp);

        for (int i = 0; i < assetList.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(assetList[i].ToString());
            if (GUILayout.Button("Delete"))
            {
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(assetList[i]));
                assetList.RemoveAt(i);
                WriteSearch(assetList);
                if(createChainQuest == wEditChain)
                {
                    createChainQuest.Close();
                }
                if(wEditSingle != null)
                {
                    wEditSingle.Close();
                }
                return;
            }
            if(createChainQuest == null)
            {
                if (GUILayout.Button("Edit"))
                {
                    if (AssetDatabase.GetMainAssetTypeAtPath(AssetDatabase.GetAssetPath(assetList[i])) == typeof(SingleQuest))
                    {
                        wEditSingle = (CreateSingleQuest)EditorWindow.GetWindow(typeof(CreateSingleQuest));
                        wEditSingle.wantsMouseMove = true;
                        wEditSingle.singleQuest = (SingleQuest)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(assetList[i]), typeof(SingleQuest));
                        wEditSingle.Open();
                    }
                    else if (AssetDatabase.GetMainAssetTypeAtPath(AssetDatabase.GetAssetPath(assetList[i])) == typeof(ChainQuest))
                    {
                        wEditChain = (CreateChainQuest)ScriptableObject.CreateInstance(typeof(CreateChainQuest));
                        wEditChain.wantsMouseMove = true;
                        wEditChain.chain = (ChainQuest)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(assetList[i]), typeof(ChainQuest));
                        wEditChain.StartFunctions(wEditChain);
                        Close();
                    }
                }
            }
            else
            {

                if (AssetDatabase.GetMainAssetTypeAtPath(AssetDatabase.GetAssetPath(assetList[i])) == typeof(ChainQuest) && createChainQuest.chain != (ChainQuest)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(assetList[i]), typeof(ChainQuest)))
                {
                    if (GUILayout.Button("Edit"))
                    {
                        wEditChain = (CreateChainQuest)ScriptableObject.CreateInstance(typeof(CreateChainQuest));
                        wEditChain.wantsMouseMove = true;
                        wEditChain.chain = (ChainQuest)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(assetList[i]), typeof(ChainQuest));
                        wEditChain.StartFunctions(wEditChain);
                        Close();
                    }
                }
            }
            if (addQuest && AssetDatabase.GetMainAssetTypeAtPath(AssetDatabase.GetAssetPath(assetList[i])) == typeof(SingleQuest))
            {
                var singleQuest = (SingleQuest)AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath(assetList[i]), typeof(SingleQuest));
                if (!allQuestId.Contains(singleQuest.questID))

                {
                    if (GUILayout.Button("Add"))
                    {
                        allQuestId = createChainQuest.AddFoundNode(singleQuest);
                        Repaint();
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

    }

    public void RemoveQuestId(int id)
    {
        allQuestId.Remove(id);
        Repaint();
    }
}
