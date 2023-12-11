using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    //Don't Open this YOU WILL DIE (literally ^_^)
    #region Editor Code

#if UNITY_EDITOR
    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;
    
    //initialise node
    public void Initialize(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.roomNodeGraph = nodeGraph;
        this.roomNodeType = roomNodeType;
        
        //load room node type list
        roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
    }
    
    //draw node with the nodestyle
    public void Draw(GUIStyle nodeStyle)
    {
        //draw node box using begin area
        GUILayout.BeginArea(rect, nodeStyle);
        
        //start region to detect popup selection changes
        EditorGUI.BeginChangeCheck();
        
        //display a popup using the RoomNodeType name values that canbe selected from (default to the currently set roomNodeType)
        int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
        int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

        roomNodeType = roomNodeTypeList.list[selection];

        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(this);
        }
        
        GUILayout.EndArea();
    }
    
    //populate a string array with the room node types to display that can be selected
    public string[] GetRoomNodeTypesToDisplay()
    {
        string[] roomArray = new string[roomNodeTypeList.list.Count];

        for (int i = 0; i < roomNodeTypeList.list.Count; i++)
        {
            if (roomNodeTypeList.list[i].displayInNodeGraphEditor)
            {
                roomArray[i] = roomNodeTypeList.list[i].roomNodeTypeName;
            }
        }

        return roomArray;
    }
    
    //process events for the node
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            //process mouse down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;
            
            //process mouse up Events
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;
            
            //process mouse drag Events
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;
            
            default:
                break;
        }
    }
    
    //process mouse down events
    private void ProcessMouseDownEvent(Event currentEvent)
    {
        //left click down
        if (currentEvent.button == 0)
        {
            ProcessLeftClickDownEvent();
        }
    }
    
    //process left click down event
    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;
        
        //toggle node selection
        if (isSelected == true)
        {
            isSelected = false;
        }
        else
        {
            isSelected = true;
        }
    }
    
    //process mouse up event
    public void ProcessMouseUpEvent(Event currentEvent)
    {
        //if left click up
        if (currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }
    
    //process left click up event
    private void ProcessLeftClickUpEvent()
    {
        if (isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }
    
    //procces mouse drag event
    private void ProcessMouseDragEvent(Event currentEvent)
    {
        //process left click drag event
        if (currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }
    
    //process left mouse drag event
    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;

        DragNode(currentEvent.delta);
        GUI.changed = true;
    }
    
    //drag node
    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }
    
#endif

    #endregion Editor Code
    
}
