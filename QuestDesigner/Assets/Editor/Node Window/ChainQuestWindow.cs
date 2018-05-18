﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class ChainQuestWindow : EditorWindow
{
    private List<Node> allNodes;
    private GUIStyle windowStyle;
    private string currentName;
    private float toolbarHeight = 80;
    private Color color;

    private Node selectedNode;
    private Node startNode;

    //para el paneo
    private bool _panningScreen;
    private Vector2 graphPan;
    private Vector2 _originalMousePosition;
    private Vector2 prevPan;
    private Rect graphRect;

    public GUIStyle nodeTextFieldStyle;

    [MenuItem("QuestGenerator/ChainQuest")]
    public static void OpenWindow()
    {
        var myQuestWindow = GetWindow<ChainQuestWindow>();
        myQuestWindow.allNodes = new List<Node>();
        myQuestWindow.windowStyle = new GUIStyle();
        myQuestWindow.windowStyle.fontSize = 15;
        myQuestWindow.windowStyle.alignment = TextAnchor.MiddleCenter;
        myQuestWindow.windowStyle.fontStyle = FontStyle.Italic;
        myQuestWindow.windowStyle.normal.textColor = Color.white;
        myQuestWindow.color = GUI.backgroundColor;

        myQuestWindow.graphPan = new Vector2(0, myQuestWindow.toolbarHeight);
        myQuestWindow.graphRect = new Rect(0, myQuestWindow.toolbarHeight, 1000000, 1000000);

        myQuestWindow.nodeTextFieldStyle = new GUIStyle(EditorStyles.textField);
        myQuestWindow.nodeTextFieldStyle.wordWrap = true;
    }

    private void OnGUI()
    {
        CheckMouseInput(Event.current);

        EditorGUI.DrawRect(new Rect(0, 0, position.width, 40), Color.black);
        EditorGUILayout.LabelField("CHAIN QUEST EDITOR", windowStyle, GUILayout.Height(40));
        EditorGUILayout.Space();
        EditorGUILayout.BeginHorizontal();
        currentName = EditorGUILayout.TextField("Quest Name: ", currentName);
        EditorGUILayout.Space();
        if (Event.current.keyCode == KeyCode.Return && currentName != null)
        {
            AddNode();
        }
        if (GUILayout.Button("Create New Quest", GUILayout.Width(150), GUILayout.Height(15)) && currentName != null)
        {
            AddNode();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.DrawRect(new Rect(0, 77, position.width, 5), Color.black);
        EditorGUI.DrawRect(new Rect(0, 40, 3, 50), Color.black);
        EditorGUI.DrawRect(new Rect(position.width - 3, 40, 3, 50), Color.black);
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.gray);

        if (Event.current.keyCode == KeyCode.Delete && selectedNode != null && startNode == null)
        {
            Delete();
        }

        GUI.BeginGroup(graphRect);
        BeginWindows();

        for (int i = 0; i < allNodes.Count; i++)
        {
            foreach (var c in allNodes[i].connected)
                Handles.DrawLine(new Vector2(allNodes[i].myRect.position.x + allNodes[i].myRect.width / 2f, allNodes[i].myRect.position.y + allNodes[i].myRect.height / 2f), new Vector2(c.myRect.position.x + c.myRect.width / 2f, c.myRect.position.y + c.myRect.height / 2f));           
        }
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i] == selectedNode && startNode == null)
                GUI.backgroundColor = Color.yellow;
            else if (allNodes[i] == startNode)
                GUI.backgroundColor = Color.blue;

            allNodes[i].myRect = GUI.Window(i, allNodes[i].myRect, DrawNode, allNodes[i].nodeName);
            GUI.backgroundColor = color;
        }
        EndWindows();
        GUI.EndGroup();
    }

    private void AddNode()
    {
        allNodes.Add(new Node(0, 0, 200, 150, currentName));
        currentName = null;
        Repaint();
    }

    private void CheckMouseInput(Event currentE)
    {
        if (!graphRect.Contains(currentE.mousePosition) || !(focusedWindow == this || mouseOverWindow == this))
        {
            selectedNode = null;
            startNode = null;
            return;
        }
        /*//panning
        if (currentE.button == 2 && currentE.type == EventType.MouseDown)
        {
            _panningScreen = true;
            prevPan = new Vector2(graphPan.x, graphPan.y);
            _originalMousePosition = currentE.mousePosition;
        }
        else if (currentE.button == 2 && currentE.type == EventType.MouseUp)
            _panningScreen = false;

        if (_panningScreen)
        {
            var newX = prevPan.x + currentE.mousePosition.x - _originalMousePosition.x;
            graphPan.x = newX > 0 ? 0 : newX;

            var newY = prevPan.y + currentE.mousePosition.y - _originalMousePosition.y;
            graphPan.y = newY > toolbarHeight ? toolbarHeight : newY;

            Repaint();
        }*/


        //node selection
        Node overNode = null;
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].CheckMouse(Event.current, graphPan);
            if (allNodes[i].OverNode)
            {
                overNode = allNodes[i];
            }
        }
        var prevSel = selectedNode;
        if (currentE.button == 0 && currentE.type == EventType.MouseDown)
        {
            if (overNode != null)
            {
                selectedNode = overNode;
                if(startNode != null && selectedNode != startNode)
                {
                    if (!startNode.connected.Contains(selectedNode))
                    {
                        startNode.connected.Add(selectedNode);
                        selectedNode.connected.Add(startNode);
                    }
                }
                else
                {
                    startNode = null;
                }
            }
            else
            {
                selectedNode = null;
                startNode = null;
            }

            if (prevSel != selectedNode)
                Repaint();
        }
        if (currentE.button == 1 && currentE.type == EventType.MouseDown)
        {
            if (overNode != null)
            {
                startNode = null;
                selectedNode = overNode;
                Options();
            }
            else selectedNode = null;
        }
    }
    private void DrawNode(int id)
    {
        allNodes[id].ID = EditorGUILayout.FloatField("ID", allNodes[id].ID);
        if (!_panningScreen)
        {
            GUI.DragWindow();

            if (!allNodes[id].OverNode) return;

            if (allNodes[id].myRect.x < 0)
                allNodes[id].myRect.x = 0;

            if (allNodes[id].myRect.y < toolbarHeight - graphPan.y)
                allNodes[id].myRect.y = toolbarHeight - graphPan.y;
        }
    }
    private void Options()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Connect Node"), false, DrawLine);
        menu.AddItem(new GUIContent("Remove Conections"), false, RemoveLine);
        menu.AddItem(new GUIContent("Delete"), false, Delete);
        menu.ShowAsContext();
    }
    private void DrawLine()
    {
        startNode = selectedNode;
    }
    private void RemoveLine()
    {
        Debug.Log("Conection deleted: " + selectedNode.connected.Count);
        for (int i = 0; i < selectedNode.connected.Count; i = 0)
        {
            var select = selectedNode.connected[i];
            for (int j = 0; j < select.connected.Count; j++)
            {
                if (select.connected[j].Equals(selectedNode))
                {
                    select.connected.RemoveAt(j);
                }
            }
            selectedNode.connected.RemoveAt(i);
        }
    }
    private void Delete()
    {
        RemoveLine();
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].Equals(selectedNode))
            {
                selectedNode = null;
                allNodes.RemoveAt(i);
                return;
            }
        }
    }
}