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


    public List<NodeQuest> allNodes;
    private GUIStyle windowStyle;
    private string chainname;
    private string currentName;
    private float toolbarHeight = 125;
    private Color color;
    private ViewAllQuests questFinder;
    private CreateSingleQuest singleQuestEditor;
    private string prevName;
    private List<string> prevNameQuests;

    private NodeQuest selectedNode;
    private NodeQuest startNode;

    //para el paneo
    private Vector2 graphPan;
    private Vector2 _originalMousePosition;
    private Vector2 prevPan;
    private Rect graphRect;
    private QuestSaveManager saveManger;
    private bool needsave;

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
            myQuestWindow.prevName = chain.myname;
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
                int indx = 0;
                for (int c = 0; c < myQuestWindow.chain.connectedQuestsId.Count; c++)
                {
                    if (myQuestWindow.chain.connectedQuestsId[c] == myQuestWindow.allNodes[i].questID)
                    {
                        indx = c;
                    }
                }
                List<int> aux = new List<int>();
                string rest = myQuestWindow.chain.serialicedConectedQuest[indx];
                for (int j = 0; j < rest.Length; j = 0)
                {
                    var str = rest.Remove(myQuestWindow.chain.serialicedConectedQuest[indx].IndexOf("_"));
                    aux.Add(System.Convert.ToInt32(str));
                    rest = rest.Substring(rest.IndexOf("_") + 1);
                }
                List<int> a = aux;
                for (int j = 0; j < a.Count; j++)
                {
                    for (int y = 0; y < myQuestWindow.allNodes.Count; y++)
                    {
                        if (myQuestWindow.allNodes[y].questID == a[j])
                        {
                            myQuestWindow.allNodes[i].connected.Add(myQuestWindow.allNodes[y]);
                        }
                    }
                }
                //
            }
            Repaint();
        }
        else
        {
            myQuestWindow.name = "Chain Quest Editor";
            myQuestWindow.prevName = myQuestWindow.name;
            myQuestWindow.allNodes = new List<NodeQuest>();
        }
        myQuestWindow.needsave = false;
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
        if ((QuestSaveManager)AssetDatabase.LoadAssetAtPath("Assets/Quests/Save/" + "SaveManager" + ".asset", typeof(object)) == null)
        {
            ScriptableObjectUtility.CreateAsset<QuestSaveManager>("Save", "SaveManager");
        }
        saveManger = (QuestSaveManager)AssetDatabase.LoadAssetAtPath("Assets/Quests/Save/" + "SaveManager" + ".asset", typeof(object));
    }

    private void OnGUI()
    {
        if(singleQuestEditor != null)
        {
            selectedNode = null;
        }
        CheckMouseInput(Event.current);

        EditorGUI.DrawRect(new Rect(0, 0, position.width, 45), Color.black);
        EditorGUILayout.LabelField(name, windowStyle, GUILayout.Height(40));
        EditorGUILayout.Space();
        name = EditorGUILayout.TextField("Chain Name: ", name);
        currentName = EditorGUILayout.TextField("Single Quest Name: ", currentName);
        EditorGUILayout.BeginHorizontal();
        if (Event.current.keyCode == KeyCode.Return && currentName != "")
        {
            AddNode();
        }
        if (GUILayout.Button("Create New Quest", GUILayout.Width(140), GUILayout.Height(15)) && currentName != "")
        {
            AddNode();
        }
        if (GUILayout.Button("Find Quest", GUILayout.Width(100), GUILayout.Height(15)))
        {
            if (questFinder == null)
            {
                questFinder = (ViewAllQuests)ScriptableObject.CreateInstance(typeof(ViewAllQuests));
                questFinder.addQuest = true;
                questFinder.wantsMouseMove = true;
                questFinder.createChainQuest = this;
                questFinder.StartFunctions(questFinder);
            }
            else questFinder.Close();
        }
        EditorGUILayout.EndHorizontal();
        EditorGUI.DrawRect(new Rect(0, 122, position.width, 5), Color.black);
        EditorGUI.DrawRect(new Rect(0, 45, 3, 80), Color.black);
        EditorGUI.DrawRect(new Rect(position.width - 3, 45, 3, 80), Color.black);
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
                selectedNode = allNodes[0];
                Delete();
            }
            return;
        }
        if(chain == null || needsave)
        {
            GUI.backgroundColor = Color.red;
            Repaint();
        }
        else if ((ChainQuest)AssetDatabase.LoadAssetAtPath("Assets/Quests/Chain/" + name + "_" + chain.chainQuestID + ".asset", typeof(object)) == null || needsave)
        {
            GUI.backgroundColor = Color.red;
            Repaint();
        }
        if (GUILayout.Button("Save", GUILayout.Width(40), GUILayout.Height(15)))
        {
            save();
            needsave = false;
            Repaint();
        }
        GUI.backgroundColor = color;
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
            if (AssetDatabase.LoadAssetAtPath("Assets/Quests/Single/" + allNodes[i].name + "_" + allNodes[i].questID.ToString() + ".asset", typeof(object)) == null)
            {
                if(startNode == null)
                {
                    GUI.backgroundColor = Color.red;
                }
                needsave = true;
            }
            allNodes[i].myRect = GUI.Window(i, allNodes[i].myRect, DrawNode, allNodes[i].name);
            GUI.backgroundColor = color;
        }
        EndWindows();
        GUI.EndGroup();
        Repaint();
    }
    public List<int> AddFoundNode(SingleQuest s)
    {
        allNodes.Add(new NodeQuest(0, 0, 150, 40, s.name));
        var a = allNodes[allNodes.Count - 1];
        a.name = s.name;
        a.description = s.description;
        a.questID = s.questID;
        allNodes[allNodes.Count - 1] = a;
        var b = new List<int>();
        for (int i = 0; i < allNodes.Count; i++)
        {
            b.Add(allNodes[i].questID);
        }
        Repaint();
        return b;
    }
    private void AddNode()
    {
        allNodes.Add(new NodeQuest(0, 0, 150, 40, currentName));
        allNodes[allNodes.Count - 1].questID = saveManger.SINGLEID;
        saveManger.SINGLEID++;
        currentName = "";
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
                        needsave = true;
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
        GUILayout.BeginHorizontal();
        GUI.backgroundColor = Color.green;
        if(startNode == null && singleQuestEditor == null)
        {
            if (GUILayout.Button("Edit", GUILayout.Width(45), GUILayout.Height(15)))
            {
                OpenSingleQuestEditor();
                Repaint();
            }
            if (GUILayout.Button("Connect", GUILayout.Width(60), GUILayout.Height(15)))
            {
                DrawLine();
            }
        }
        GUILayout.EndHorizontal();
        GUI.DragWindow();

        if (!allNodes[id].OverNode) return;

        if (allNodes[id].myRect.x < 0)
            allNodes[id].myRect.x = 0;

        if (allNodes[id].myRect.y < toolbarHeight - graphPan.y)
            allNodes[id].myRect.y = toolbarHeight - graphPan.y;
    }
    public void save()
    {
        if (chain == null)
        {
            var a = saveManger.CHAINID;
            ScriptableObjectUtility.CreateAsset<ChainQuest>("Chain", name + "_" + a);
            chain = (ChainQuest)AssetDatabase.LoadAssetAtPath("Assets/Quests/Chain/" + name + "_" + a + ".asset", typeof(object));
            chain.chainQuestID = a;
            chain.myname = name;
            prevName = name;
            saveManger.CHAINID++;
        }
        else if(name != prevName)
        {
            var a = chain.chainQuestID;
            AssetDatabase.RenameAsset("Assets/Quests/Chain/" + prevName + "_" + a + ".asset", name + "_" + a);
            chain.myname = name;
            prevName = name;
        }
        chain.quests = new List<SingleQuest>();
        chain.mySingleQuestRects = new List<Rect>();
        for (int i = 0; i < allNodes.Count; i++)
        {
            chain.mySingleQuestRects.Add(allNodes[i].myRect);
            if(AssetDatabase.LoadAssetAtPath("Assets/Quests/Single/" + allNodes[i].name + "_" + allNodes[i].questID.ToString() + ".asset", typeof(object)) == null)
            {
                ScriptableObjectUtility.CreateAsset<SingleQuest>("Single", allNodes[i].name + "_" + allNodes[i].questID.ToString());
            }
            var a = (SingleQuest)AssetDatabase.LoadAssetAtPath("Assets/Quests/Single/" + allNodes[i].name + "_" + allNodes[i].questID.ToString() + ".asset", typeof(object));
            a.description = allNodes[i].description;
            a.questID = allNodes[i].questID;
            a.name = allNodes[i].name;
            chain.quests.Add(a);
            chain.connectedQuestsId = new List<int>();
            chain.serialicedConectedQuest = new List<string>();
            for (int j = 0; j < allNodes.Count; j++)
            {
                string c = "";
                for (int y = 0; y < allNodes[j].connected.Count; y++)
                {
                    c += allNodes[j].connected[y].questID + "_";
                }
                chain.serialicedConectedQuest.Add(c);
                chain.connectedQuestsId.Add(allNodes[j].questID);
            }
        }
    }
    private void Options()
    {
        if(singleQuestEditor == null)
        {
            GenericMenu menu = new GenericMenu();
            menu.AddItem(new GUIContent("Connect Node"), false, DrawLine);
            menu.AddItem(new GUIContent("Remove Conections"), false, RemoveLine);
            menu.AddItem(new GUIContent("Edit"), false, OpenSingleQuestEditor);
            menu.AddItem(new GUIContent("Delete"), false, Delete);
            menu.AddItem(new GUIContent("Destroy"), false, DestroyFromUnity);
            menu.ShowAsContext();
        }
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
        if ((SingleQuest)AssetDatabase.LoadAssetAtPath("Assets/Quests/Single/" + selectedNode.name + "_" + selectedNode.questID.ToString() + ".asset", typeof(object)) != null)
        {
            AssetDatabase.DeleteAsset("Assets/Quests/Single/" + selectedNode.name + "_" + selectedNode.questID.ToString() + ".asset");
        }
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
                    needsave = true;
                }
            }
            selectedNode.connected.RemoveAt(i);
            //
        }
    }
    private void Delete()
    {
        RemoveLine();
        for (int i = 0; i < allNodes.Count; i++)
        {
            if (allNodes[i].Equals(selectedNode))
            {
                if(questFinder != null)
                {
                    questFinder.RemoveQuestId(selectedNode.questID);
                }
                selectedNode = null;
                allNodes.RemoveAt(i);
                Repaint();
                return;
            }
        }
        Repaint();
    }
    private void OpenSingleQuestEditor()
    {
        if (singleQuestEditor == null)
        {
            singleQuestEditor = (CreateSingleQuest)ScriptableObject.CreateInstance(typeof(CreateSingleQuest));
            singleQuestEditor.wantsMouseMove = true;
            singleQuestEditor.singleQuest = (SingleQuest)AssetDatabase.LoadAssetAtPath("Assets/Quests/Single/" + selectedNode.name + "_" + selectedNode.questID + ".asset", typeof(object));
            singleQuestEditor.node = selectedNode;
            singleQuestEditor.chain = this;
            singleQuestEditor.Open();
            selectedNode = null;
        }
    }
}
