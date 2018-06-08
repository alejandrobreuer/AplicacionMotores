using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class CreateChainQuest : EditorWindow
{

    //Variable para que me pacen el chainquest
    public ChainQuest chain;


    private List<NodeQuest> allNodes;
    private GUIStyle windowStyle;
    private string chainname;
    private string currentName;
    private float toolbarHeight = 115;
    private Color color;
    private ViewAllQuests questFinder;
    private CreateSingleQuest singleQuestEditor;

    private NodeQuest selectedNode;
    private NodeQuest startNode;

    //para el paneo
    private bool _panningScreen;
    private Vector2 graphPan;
    private Vector2 _originalMousePosition;
    private Vector2 prevPan;
    private Rect graphRect;

    public GUIStyle nodeTextFieldStyle;

    [MenuItem("Quest Designer/Chain Quests/Create Chain Quest")]
    public static void OpenWindow()
    {
        var myQuestWindow = GetWindow<CreateChainQuest>();
        myQuestWindow.StartFunctions(myQuestWindow);
    }
    public void StartFunctions(CreateChainQuest myQuestWindow)
    {
        if (myQuestWindow.chain != null)
        {
            myQuestWindow.name = myQuestWindow.chain.myname;
            myQuestWindow.allNodes = new List<NodeQuest>();
            for (int i = 0; i < myQuestWindow.chain.quests.Count; i++)
            {
                myQuestWindow.allNodes.Add(new NodeQuest(myQuestWindow.chain.mySingleQuestRects[i].x, myQuestWindow.chain.mySingleQuestRects[i].y, myQuestWindow.chain.mySingleQuestRects[i].width, myQuestWindow.chain.mySingleQuestRects[i].height, myQuestWindow.chain.name));
                var a = myQuestWindow.allNodes[i];
                a.questID = myQuestWindow.chain.quests[i].questID;
                a.name = myQuestWindow.chain.quests[i].name;
                a.description = myQuestWindow.chain.quests[i].description;
                myQuestWindow.allNodes[i] = a;
            }
            for (int i = 0; i < myQuestWindow.allNodes.Count; i++)
            {
                var a = myQuestWindow.chain.quests[i].conectedQuests;
                for (int j = 0; j < a.Count; j++)
                {
                    var b = a[j];
                    for (int y = 0; y < myQuestWindow.allNodes.Count; y++)
                    {
                        if (b == myQuestWindow.allNodes[y].questID)
                        {
                            allNodes[i].connected.Add(allNodes[y]);
                        }
                    }
                }
            }
            Repaint();
        }
        else
        {
            myQuestWindow.name = "Chain Quest Editor";
            myQuestWindow.allNodes = new List<NodeQuest>();
        }
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
        myQuestWindow.Show();
    }

    private void OnGUI()
    {
        CheckMouseInput(Event.current);

        EditorGUI.DrawRect(new Rect(0, 0, position.width, 40), Color.black);
        EditorGUILayout.LabelField(name, windowStyle, GUILayout.Height(40));
        EditorGUILayout.Space();
        currentName = EditorGUILayout.TextField("Quest Name: ", currentName);
        EditorGUILayout.BeginHorizontal();
        if (Event.current.keyCode == KeyCode.Return && currentName != null)
        {
            AddNode();
        }
        if (GUILayout.Button("Create New Quest", GUILayout.Width(140), GUILayout.Height(15)) && currentName != null)
        {
            AddNode();
        }
        if (GUILayout.Button("Find Quest", GUILayout.Width(100), GUILayout.Height(15)))
        {
            if (questFinder == null)
            {
                questFinder = (ViewAllQuests)ScriptableObject.CreateInstance(typeof(ViewAllQuests));
                //questFinder = (ViewAllQuests)EditorWindow.GetWindow(typeof(ViewAllQuests));
                questFinder.addQuest = true;
                questFinder.wantsMouseMove = true;
                questFinder.createChainQuest = this;
                questFinder.StartFunctions(questFinder);
                //ViewAllQuests.OpenWindow();
            }
            else questFinder.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.DrawRect(new Rect(0, 112, position.width, 5), Color.black);
        EditorGUI.DrawRect(new Rect(0, 40, 3, 80), Color.black);
        EditorGUI.DrawRect(new Rect(position.width - 3, 40, 3, 80), Color.black);
        EditorGUI.DrawRect(new Rect(0, toolbarHeight, position.width, position.height - toolbarHeight), Color.gray);

        if (Event.current.keyCode == KeyCode.Delete && selectedNode != null && startNode == null)
        {
            Delete();
        }
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Clean", GUILayout.Width(60), GUILayout.Height(15)))
        {
            for (int i = 0; i < allNodes.Count; i = 0)
            {
                selectedNode = allNodes[i];
                RemoveLine();
                allNodes.RemoveAt(i);
            }
        }
        if (GUILayout.Button("Save", GUILayout.Width(40), GUILayout.Height(15)))
        {
            save();
        }
        EditorGUILayout.EndHorizontal();
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

            allNodes[i].myRect = GUI.Window(i, allNodes[i].myRect, DrawNode, allNodes[i].name);
            GUI.backgroundColor = color;
        }
        EndWindows();
        GUI.EndGroup();
    }
    public void AddFoundNode(SingleQuest s)
    {
        allNodes.Add(new NodeQuest(0, 0, 200, 80, s.name));
        var a = allNodes[allNodes.Count - 1];
        a.questID = s.questID;
        a.name = s.name;
        a.description = s.description;
        allNodes[allNodes.Count - 1] = a;
        Repaint();
        Debug.Log("ada");
    }
    private void AddNode()
    {
        allNodes.Add(new NodeQuest(0, 0, 200, 80, currentName));
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
        NodeQuest overNode = null;
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
                if (startNode != null && selectedNode != startNode)
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
        allNodes[id].questID = EditorGUILayout.IntField("ID", allNodes[id].questID);
        allNodes[id].name = EditorGUILayout.TextField("Name", allNodes[id].name);
        allNodes[id].description = EditorGUILayout.TextField("Quest Name: ", allNodes[id].description);
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
    private void save()
    {
        if (chain == null)
        {
            ScriptableObjectUtility.CreateAsset<ChainQuest>("Chain_1");
            chain = (ChainQuest)AssetDatabase.LoadAssetAtPath("Assets/" + "Chain_1" + ".asset", typeof(object));
        }
        chain.quests = new List<SingleQuest>();
        chain.mySingleQuestRects = new List<Rect>();
        for (int i = 0; i < allNodes.Count; i++)
        {
            chain.mySingleQuestRects.Add(allNodes[i].myRect);
            ScriptableObjectUtility.CreateAsset<SingleQuest>(allNodes[i].questID.ToString());
            var a = (SingleQuest)AssetDatabase.LoadAssetAtPath("Assets/" + allNodes[i].questID.ToString() + ".asset", typeof(object));
            a.description = allNodes[i].description;
            a.questID = allNodes[i].questID;
            a.name = allNodes[i].name;
            chain.quests.Add(a);
            chain.quests[i].conectedQuests = new List<int>();
            for (int j = 0; j < allNodes[i].connected.Count; j++)
            {
                chain.quests[i].conectedQuests.Add(allNodes[i].connected[j].questID);
            }
        }
    }
    private void Options()
    {
        GenericMenu menu = new GenericMenu();
        menu.AddItem(new GUIContent("Connect Node"), false, DrawLine);
        menu.AddItem(new GUIContent("Remove Conections"), false, RemoveLine);
        menu.AddItem(new GUIContent("Edit"), false, OpenSingleQuestEditor);
        menu.AddItem(new GUIContent("Delete"), false, Delete);
        menu.AddItem(new GUIContent("Destroy"), false, DestroyFromUnity);
        menu.ShowAsContext();
    }

    private void DestroyFromUnity()
    {
        RemoveLine();
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].Equals(selectedNode))
            {
                allNodes.RemoveAt(i);
                continue;
            }
        }
        AssetDatabase.DeleteAsset("Assets/" + selectedNode.questID.ToString() + ".asset");
        selectedNode = null;
        save();
    }

    private void DrawLine()
    {
        startNode = selectedNode;
    }
    private void RemoveLine()
    {
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
    private void OpenSingleQuestEditor()
    {
        if (singleQuestEditor == null)
        {
            singleQuestEditor = (CreateSingleQuest)ScriptableObject.CreateInstance(typeof(CreateSingleQuest));
            singleQuestEditor.wantsMouseMove = true;
            singleQuestEditor.singleQuest = (SingleQuest)AssetDatabase.LoadAssetAtPath("Assets/" + selectedNode.questID + ".asset", typeof(object));
            singleQuestEditor.node = selectedNode;
            singleQuestEditor.chain = this;
            singleQuestEditor.Show();
            selectedNode = null;
        }
    }
}
