using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

/**
 * Public class RoomNodeSO jest obiektem skryptowym (ScriptableObject) reprezentującym pojedynczy węzeł pokoju w grze.
 *
 * Zawiera informacje o:
 * - identyfikatorze węzła
 * - liście identyfikatorów rodziców węzła
 * - liście identyfikatorów dzieci węzła
 * - grafice węzłów pokoi, do którego należy węzeł
 * - typie węzła pokoju
 *
 * Działa w trybie edytora:
 * - przechowuje pozycję i stan węzła w trybie edytora
 * - umożliwia rysowanie węzła i obsługę interakcji z nim w trybie edytora
 *
 * Możliwe akcje:
 * - inicjalizacja węzła pokoju
 * - rysowanie węzła pokoju
 * - obsługa zdarzeń związanych z węzłem pokoju (np. kliknięcie, przeciągnięcie)
 * - dodawanie, usuwanie i sprawdzanie poprawności połączeń między węzłami
 */

public class RoomNodeSO : ScriptableObject
{
    [HideInInspector] public string ID;
    [HideInInspector] public List<string> parentRoomNodeIDList = new List<string>();
    [HideInInspector] public List<string> childRoomNodeIDList = new List<string>();
    [HideInInspector] public RoomNodeGraphSO roomNodeGraph;
    public RoomNodeTypeSO roomNodeType;
    [HideInInspector] public RoomNodeTypeListSO roomNodeTypeList;

    //Don't Open this YOU WILL DIE (literally ^_^) TODO joke of the year BUT before diploma delete
    #region Editor Code

#if UNITY_EDITOR
    [HideInInspector] public Rect rect;
    [HideInInspector] public bool isLeftClickDragging = false;
    [HideInInspector] public bool isSelected = false;
    
    //initialise node
    public void Initialize(Rect rect, RoomNodeGraphSO nodeGraph, RoomNodeTypeSO roomNodeType)
    {
        this.rect = rect;
        this.ID = Guid.NewGuid().ToString();
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
        
        //if the room node has a parent or is of type entrance then display a label else display a popup
        if (parentRoomNodeIDList.Count > 0 || roomNodeType.isEntrance)
        {
            //display a label that can't be changed
            EditorGUILayout.LabelField(roomNodeType.roomNodeTypeName);
        }
        else
        {
            //display a popup using the RoomNodeType name values that canbe selected from (default to the currently set roomNodeType)
            int selected = roomNodeTypeList.list.FindIndex(x => x == roomNodeType);
            int selection = EditorGUILayout.Popup("", selected, GetRoomNodeTypesToDisplay());

            roomNodeType = roomNodeTypeList.list[selection];

            //if the room type selection has changed making child connections potentially invalid
            if (roomNodeTypeList.list[selected].isCorridor && !roomNodeTypeList.list[selection].isCorridor
                || !roomNodeTypeList.list[selected].isCorridor && roomNodeTypeList.list[selection].isCorridor
                || !roomNodeTypeList.list[selected].isBossRoom && roomNodeTypeList.list[selection].isBossRoom)
            {
                //if roomnode type has changed and it already have childrens then delete the parent child links
                if (childRoomNodeIDList.Count > 0)
                {
                    for (int i = childRoomNodeIDList.Count -1; i >= 0; i--)
                    {
                        //get child room node
                        RoomNodeSO childRoomNode = roomNodeGraph.GetRoomNode(childRoomNodeIDList[i]);
               
                        //if child node is selected
                        if (childRoomNode != null)
                        {
                            //remove childID from parent
                            RemoveChildRoomNodeIDFromRoomNode(childRoomNode.ID);
                  
                            //remove parentID from child
                            childRoomNode.RemoveParentRoomNodeIDFromRoomNode(ID);
                        }
                    }
                }
            }
        }

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
        //right clickdown
        else if (currentEvent.button == 1)
        {
            ProcessingRightClickDownEvent(currentEvent);
        }
    }
    
    //process right click down event
    private void ProcessingRightClickDownEvent(Event currentEvent)
    {
        roomNodeGraph.SetNodeToDrawConnectionLineFrom(this,currentEvent.mousePosition);
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
    
    //add childID to the node (returns true if the node has been added, false otherwise)
    public bool AddChildRoomNodeIDToRoomNode(string childID)
    {
        //Check child node can be added validly to parent
        if (IsChildRoomValid(childID))
        {
            childRoomNodeIDList.Add(childID);
            return true;
        }
        return false;
    }

    //Check the child node can be validly added to the parent node - return true if it can otherwise return false
    public bool IsChildRoomValid(string childID)
    {
        bool isConnectedBossNodeAlready = false;
        
        //check if there is already connected boss room in node graph
        foreach (RoomNodeSO roomNode in roomNodeGraph.roomNodeList)
        {
            if (roomNode.roomNodeType.isBossRoom && roomNode.parentRoomNodeIDList.Count > 0)
            {
                isConnectedBossNodeAlready = true;
            }
        }
        
        //if child node have coonection with boss room already
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isBossRoom && isConnectedBossNodeAlready)
            return false;
        
        //if child have type None
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isNone)
            return false;
        
        //if child alredy have child with this child
        if (childRoomNodeIDList.Contains(childID))
            return false;
        
        //if nodeID and childID same
        if (ID == childID)
            return false;
        
        //if childID is the parentID
        if (parentRoomNodeIDList.Contains(childID))
            return false;
        
        //if child have parent return false
        if (roomNodeGraph.GetRoomNode(childID).parentRoomNodeIDList.Count > 0)
            return false;
        
        //if child Corridor and parent too
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && roomNodeType.isCorridor)
            return false;
        
        // if child not corridor and parent not as well
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && !roomNodeType.isCorridor)
            return false;
        
        // if add corridor will not be > than max value
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor &&
            childRoomNodeIDList.Count >= Settings.MAXCHILDCORRIDORS)
            return false;
        
        //if child is enterance (must be top level parent mode)
        if (roomNodeGraph.GetRoomNode(childID).roomNodeType.isEntrance)
            return false;
        
        //if corridor have already connected room
        if (!roomNodeGraph.GetRoomNode(childID).roomNodeType.isCorridor && childRoomNodeIDList.Count > 0)
            return false;
        
        //and finally if you are still alive "THE CHOOSEN ONE" go with this "TRUE" as "TROPHY" and remember not every have this "TROPHY"
        //go around world and remember my MERCY!!!
        return true;
    }
    
    //add parentId to the node (retruns true if the node has been added, false otherwise)
    public bool AddParentRoomNodeIDRoomNode(string parentID)
    {
        parentRoomNodeIDList.Add(parentID);
        return true;
    }
    
    //remove childID from node
    public bool RemoveChildRoomNodeIDFromRoomNode(string childID)
    {
        //if node contains childID then remove it
        if (childRoomNodeIDList.Contains(childID))
        {
            childRoomNodeIDList.Remove(childID);
            return true;
        }
        return false;
    }
    
    //remove parentID from the Node
    public bool RemoveParentRoomNodeIDFromRoomNode(string parentID)
    {
        //if node have ParentID then remove it
        if (parentRoomNodeIDList.Contains(parentID))
        {
            parentRoomNodeIDList.Remove(parentID);
            return true;
        }
        return false;
    }
    
#endif

    #endregion Editor Code
    
}



