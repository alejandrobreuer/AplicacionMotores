using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Rect myRect;
    public string nodeName;
    public float ID;
    private bool _overNode;
    public List<Node> connected;

    public Node(float x, float y, float width, float height, string name)
    {
        myRect = new Rect(x, y, width, height);
        connected = new List<Node>();
        nodeName = name;
    }

    public void CheckMouse(Event cE, Vector2 pan)
    {
        if (myRect.Contains(cE.mousePosition - pan))
            _overNode = true;
        else
            _overNode = false;
    }

    public bool OverNode
    { get { return _overNode; } }
}
