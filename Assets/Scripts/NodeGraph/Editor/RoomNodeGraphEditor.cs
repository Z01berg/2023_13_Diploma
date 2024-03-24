using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.MPE;

/**
 * Public class RoomNodeGraphEditor jest edytorem grafu węzłów pokoi, który umożliwia tworzenie i zarządzanie grafem pokoi.
 *
 * Zawiera funkcje do:
 * - rysowania interfejsu graficznego edytora
 * - obsługi zdarzeń myszy
 * - dodawania, usuwania i łączenia węzłów pokoi
 * - rysowania połączeń między węzłami
 *
 * Możliwe akcje:
 * - utworzenie węzła pokoju w wybranej pozycji na grafie
 * - usuwanie zaznaczonych połączeń między węzłami
 * - usuwanie zaznaczonych węzłów pokoju
 * - zaznaczenie wszystkich węzłów pokoju
 *
 * EditorWindow jest otwierany przez menu Room Node Graph Editor
 * Po otwarciu okna, można przeciągać węzły pokoju, dodawać nowe węzły klikając prawym przyciskiem myszy, łączyć węzły i wiele innych.
 */

public class RoomNodeGraphEditor : EditorWindow
{
   private GUIStyle _roomNodeStyle;
   private GUIStyle _roomNodeSelectedStyle;
   private static RoomNodeGraphSO _currentRoomNodeGraph;

   private Vector2 _graphOffset;
   private Vector2 _graphDrag;
   
   private RoomNodeSO _currentRoomNode = null;
   private RoomNodeTypeListSO _roomNodeTypeList;
   
   //Node layot
   private const float _NODE_WIDTH = 160f;
   private const float _NODE_HEIGHT = 75f;
   private const int _NODE_PADDING = 25;
   private const int _NODE_BORDER = 12;
   
   //Connecting line values
   private const float _CONNECTING_LINE_WIDTH = 3f;
   private const float _CONNECTING_LINE_ARROW_SIZE = 6f;
   
   //grid Spacing
   private const float _GRID_LARGE = 100f;
   private const float _GRID_SMALL = 25f;

   [MenuItem("Room Node Graph Editor", menuItem = "Window/2023_13_Diploma/Room Node Graph Editor")]
   
   private static void OpenWindow()
   {
      GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
   }

   private void OnEnable()
   {
      //Selection change event (on inspector)
      Selection.selectionChanged += InspectorSelectionChanged;
      
      //Define Node layot
      _roomNodeStyle = new GUIStyle();
      _roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
      _roomNodeStyle.normal.textColor = Color.white;
      _roomNodeStyle.padding = new RectOffset(_NODE_PADDING, _NODE_PADDING, _NODE_PADDING, _NODE_PADDING);
      _roomNodeStyle.border = new RectOffset(_NODE_BORDER, _NODE_BORDER, _NODE_BORDER, _NODE_BORDER);
      
      //define selected node style
      _roomNodeSelectedStyle = new GUIStyle();
      _roomNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
      _roomNodeSelectedStyle.normal.textColor = Color.white;
      _roomNodeSelectedStyle.padding = new RectOffset(_NODE_PADDING, _NODE_PADDING, _NODE_PADDING, _NODE_PADDING);
      _roomNodeSelectedStyle.border = new RectOffset(_NODE_BORDER, _NODE_BORDER, _NODE_BORDER, _NODE_BORDER);
      
      //Load Room node types
      _roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
   }

   private void OnDisable()
   {
      //Unsubscribe Selection change event (on inspector)
      Selection.selectionChanged -= InspectorSelectionChanged;
   }

   //Open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector
   
   [OnOpenAsset(0)] //Need the namespace UnityEditor.Callback
   public static bool OnDoubleClickAsset(int instanceID, int line)
   {
      RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

      if (roomNodeGraph != null)
      {
         OpenWindow();

         _currentRoomNodeGraph = roomNodeGraph;

         return true;
      }

      return false;
   }

   //Draw Editor GUI
   private void OnGUI()
   {
      //if a scriptable object of type RoomNodeGraphSO has been selected then process
      if (_currentRoomNodeGraph != null)
      {
         //Draw Grid
         DrawBackgroundGrid(_GRID_SMALL, 0.2f, Color.gray);
         DrawBackgroundGrid(_GRID_LARGE, 0.3f, Color.gray);
         
         //Draw line if being dragged
         DrawDraggedLine();
         
         //Process Events
         ProcessEvents(Event.current);
         
         //draw connections between room node
         DrawRoomConnections();
         
         //Draw Room Nodes
         DrawRoomNodes();
      }

      if (GUI.changed)
      {
         Repaint();
      }
   }
   
   //draw a background grid for room node editor
   private void DrawBackgroundGrid(float gridSize, float gridOpacity, Color gridColor)
   {
      int verticalLineCount = Mathf.CeilToInt((position.width + gridSize) / gridSize);
      int horizontalLineCount = Mathf.CeilToInt((position.height + gridSize) / gridSize);

      Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);
      
      _graphOffset += _graphDrag * 0.5f;

      Vector3 gridOffset = new Vector3(_graphOffset.x % gridSize, _graphOffset.y % gridSize, 0);

      for (int i = 0; i < verticalLineCount; i++)
      {
         Handles.DrawLine(new Vector3(gridSize * i, -gridSize, 0) + gridOffset, new Vector3(gridSize * i, position.height + gridSize, 0f) + gridOffset);
      }
      
      for (int i = 0; i < horizontalLineCount; i++)
      {
         Handles.DrawLine(new Vector3(-gridSize, gridSize * i, 0) + gridOffset, new Vector3(position.width + gridSize, gridSize * i, 0f) + gridOffset);
      }

      Handles.color = Color.white;
   }

   private void DrawDraggedLine()
   {
      if (_currentRoomNodeGraph.linePosition != Vector2.zero)
      {
         //draw line from node to line position
         Handles.DrawBezier(_currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, _currentRoomNodeGraph.linePosition,
            _currentRoomNodeGraph.roomNodeToDrawLineFrom.rect.center, _currentRoomNodeGraph.linePosition, Color.white, null, _CONNECTING_LINE_WIDTH);
      }
   }

   private void ProcessEvents(Event currentEvent)
   {
      //reste graph drag
      _graphDrag = Vector2.zero;
      
      //get room node that mouse is over if its null or not currently dragged
      if (_currentRoomNode == null || _currentRoomNode.isLeftClickDragging == false)
      {
         _currentRoomNode = IsMouseOverRoomNode(currentEvent);
      }

      //if mouse isn't over a room node OR we are curently dragging a line from node and process graph
      if (_currentRoomNode == null || _currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
      {
         ProcessRoomNodeGraphEvents(currentEvent);
      }
      else
      {
         //process room node events
         _currentRoomNode.ProcessEvents(currentEvent);
      }
   }
   
   //check if mouse is over room node - if so then return the room node else return null
   private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
   {
      for (int i = _currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)
      {
         if (_currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
         {
            return _currentRoomNodeGraph.roomNodeList[i];
         }
      }

      return null;
   }

   //Process Room Node Graph Events
   private void ProcessRoomNodeGraphEvents(Event currentEvent)
   {
      switch (currentEvent.type)
      {
         //Process Mouse Down Events
         case EventType.MouseDown:
            ProcessMouseDownEvent(currentEvent);
            break;
         
         //process mouse up event
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
   
   //Process mouse down events on the room node graph (not over a node)
   private void ProcessMouseDownEvent(Event currentEvent)
   {
      //Process right click mouse down on graph event (show context menu)
      if (currentEvent.button == 1)
      {
         ShowContextMenu(currentEvent.mousePosition);
      }
      //process left mouse down on graph event
      else if (currentEvent.button == 0)
      {
         ClearLineDrag();
         ClearAllSelectedRoomNodes();
      }
   }
   
   //Show the context menu
   private void ShowContextMenu(Vector2 mousePosition)
   {
      GenericMenu menu = new GenericMenu();
      
      menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
      menu.AddSeparator("");
      menu.AddItem(new GUIContent("Select All Room Nodes"), false, SelectAllRoomNode);
      menu.AddSeparator("");
      menu.AddItem(new GUIContent("Delete Selected Links"), false, DeleteSelectedRoomNodeLinks);
      menu.AddItem(new GUIContent("Delete Selected Nodes"), false, DeleteSelectedRoomNodes);
      
      menu.ShowAsContext();
   }
   
   //Create a room node at the mouse position
   private void CreateRoomNode(object mousePositionObject)
   {
      //if current node graph empty then add entrance room node first
      if (_currentRoomNodeGraph.roomNodeList.Count == 0)
      {
         CreateRoomNode(new Vector2(200f, 200f), _roomNodeTypeList.list.Find(x => x.isEntrance));
      }
      
      CreateRoomNode(mousePositionObject, _roomNodeTypeList.list.Find(x => x.isNone));
   }
   
   //Create a room node at the mouse position BUT overload it for pass in RoomNodeType
   private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
   {
      Vector2 mousePosition = (Vector2)mousePositionObject;
      
      //create room node scriptable object asset
      RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();
      
      //add room node to current room node graph room node list
      _currentRoomNodeGraph.roomNodeList.Add(roomNode);
      
      //set room node values
      roomNode.Initialize(new Rect(mousePosition, new Vector2(_NODE_WIDTH, _NODE_HEIGHT)), _currentRoomNodeGraph,
         roomNodeType);
      
      //add room node to room node graph scriptable object asset database
      AssetDatabase.AddObjectToAsset(roomNode, _currentRoomNodeGraph);
      
      AssetDatabase.SaveAssets();
      
      //refresh graph node dictionary
      _currentRoomNodeGraph.OnValidate();
   }
   
   //delete the links between the selected Room Nodes
   private void DeleteSelectedRoomNodeLinks()
   {
      //iterate through all rom nodes
      foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.roomNodeList)
      {
         if (roomNode.isSelected && roomNode.childRoomNodeIDList.Count > 0)
         {
            for (int i = roomNode.childRoomNodeIDList.Count -1; i >= 0; i--)
            {
               //get child room node
               RoomNodeSO childRoomNode = _currentRoomNodeGraph.GetRoomNode(roomNode.childRoomNodeIDList[i]);
               
               //if child node is selected
               if (childRoomNode != null && childRoomNode.isSelected)
               {
                  //remove childID from parent
                  roomNode.RemoveChildRoomNodeIDFromRoomNode(childRoomNode.ID);
                  
                  //remove parentID from child
                  childRoomNode.RemoveParentRoomNodeIDFromRoomNode(roomNode.ID);
               }
            }
         }
      }
      //clear all selected nodes
      ClearAllSelectedRoomNodes();
   }
   
   //delete the nodes which are selected
   private void DeleteSelectedRoomNodes()
   {
      Queue<RoomNodeSO> roomNodeDeletionQueue = new Queue<RoomNodeSO>();
      
      // loop through
      foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.roomNodeList)
      {
         if (roomNode.isSelected && !roomNode.roomNodeType.isEntrance)
         {
            roomNodeDeletionQueue.Enqueue(roomNode);
            
            //iterate through child room node ids
            foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
            {
               //retrive child room node
               RoomNodeSO childRoomNode = _currentRoomNodeGraph.GetRoomNode(childRoomNodeID);

               if (childRoomNode != null)
               {
                  //remove parentID from child room node
                  childRoomNode.RemoveParentRoomNodeIDFromRoomNode(roomNode.ID);
               }
            }
            
            //iterate through parent room node ids
            foreach (string parentRoomNodeID in roomNode.parentRoomNodeIDList)
            {
               //retrive parent node
               RoomNodeSO parentRoomNode = _currentRoomNodeGraph.GetRoomNode(parentRoomNodeID);

               if (parentRoomNode != null)
               {
                  //remove childID from parent node
                  parentRoomNode.RemoveChildRoomNodeIDFromRoomNode(roomNode.ID);
               }
            }
            
         }
      }
      
      //delete queued room nodes
      while (roomNodeDeletionQueue.Count > 0)
      {
         //get room node from queue
         RoomNodeSO roomNodeToDelete = roomNodeDeletionQueue.Dequeue();
         
         //remove node from dictionary
         _currentRoomNodeGraph.roomNodeDictionary.Remove(roomNodeToDelete.ID);
         
         //remove node from list
         _currentRoomNodeGraph.roomNodeList.Remove(roomNodeToDelete);
         
         //remove node from asset database
         DestroyImmediate(roomNodeToDelete, true);
         
         //save asset database
         AssetDatabase.SaveAssets();
      }
   }
   
   //clear selection from all room nodes
   private void  ClearAllSelectedRoomNodes()
   {
      foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.roomNodeList)
      {
         if (roomNode.isSelected)
         {
            roomNode.isSelected = false;

            GUI.changed = true;
         }
      }
   }
   
   //Select all room nodes
   private void SelectAllRoomNode()
   {
      foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.roomNodeList)
      {
         roomNode.isSelected = true;
      }
      GUI.changed = true;
   }
   
   //process mouse up event
   private void ProcessMouseUpEvent(Event currentEvent)
   {
      //if releasing the right mouse button and dragging a line
      if (currentEvent.button == 1 && _currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
      {
         //check if over a room node
         RoomNodeSO roomNode = IsMouseOverRoomNode(currentEvent);
         
         if (roomNode != null)
         {
            //if so set it as a child of the parent room node if it can be added
            if (_currentRoomNodeGraph.roomNodeToDrawLineFrom.AddChildRoomNodeIDToRoomNode(roomNode.ID))
            {
               //set parent ID in child room node
               roomNode.AddParentRoomNodeIDRoomNode(_currentRoomNodeGraph.roomNodeToDrawLineFrom.ID);
            }
         }
         
         ClearLineDrag();
      }
   }
   
   //process mouse drag event
   private void ProcessMouseDragEvent(Event currentEvent)
   {
      //process right click drag event - draw line
      if (currentEvent.button == 1)
      {
         ProcessRightMouseDragEvent(currentEvent);
      }
      //process left mouse drag event - drag node graph
      else if (currentEvent.button == 0)
      {
         ProcessLeftMouseDragEvent(currentEvent.delta);
      }
   }
   
   //process right mouse drag event - draw line
   private void ProcessRightMouseDragEvent(Event currentEvent)
   {
      if (_currentRoomNodeGraph.roomNodeToDrawLineFrom != null)
      {
         DragConnectingLine(currentEvent.delta); 
         GUI.changed = true;
      }
   }
   
   //process left mouse drag event - drag node graph
   private void ProcessLeftMouseDragEvent(Vector2 dragDelta)
   {
      _graphDrag = dragDelta;

      for (int i = 0; i < _currentRoomNodeGraph.roomNodeList.Count; i++)
      {
         _currentRoomNodeGraph.roomNodeList[i].DragNode(dragDelta);
      }

      GUI.changed = true;
   }
   
   //drag connecting line from room to node
   public void DragConnectingLine(Vector2 delta)
   {
      _currentRoomNodeGraph.linePosition += delta;
   }
   
   //Clear line drag from a room node
   private void ClearLineDrag()
   {
      _currentRoomNodeGraph.roomNodeToDrawLineFrom = null;
      _currentRoomNodeGraph.linePosition = Vector2.zero;
      GUI.changed = true;
   }
   
   //draw connections in the graph window between room nodes
   private void DrawRoomConnections()
   {
      //loop through all room nodes
      foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.roomNodeList)
      {
         if (roomNode.childRoomNodeIDList.Count > 0)
         {
            //loop through child room nodes
            foreach (string childRoomNodeID in roomNode.childRoomNodeIDList)
            {
               //get child room node from dictionary
               if (_currentRoomNodeGraph.roomNodeDictionary.ContainsKey(childRoomNodeID))
               {
                  DrawConnectionLine(roomNode, _currentRoomNodeGraph.roomNodeDictionary[childRoomNodeID]);

                  GUI.changed = true;
               }
            }
         }
      }
   }
   
   //draw connection line between the parent room node and child room node
   private void DrawConnectionLine(RoomNodeSO parentRoomNode, RoomNodeSO childRoomNode)
   {
      //get line start and end position
      Vector2 startPosition = parentRoomNode.rect.center;
      Vector2 endPosition = childRoomNode.rect.center;
      
      // calculate midway point
      Vector2 midPosition = (endPosition + startPosition) / 2f;
      
      //vector from start to end position of line
      Vector2 direction = endPosition - startPosition;
      
      //calculate normalized perpedencular positions from the mid point
      Vector2 arrowTailPoint1 =
         midPosition - new Vector2(-direction.y, direction.x).normalized * _CONNECTING_LINE_ARROW_SIZE;
      Vector2 arrowTailPoint2 =
         midPosition + new Vector2(-direction.y, direction.x).normalized * _CONNECTING_LINE_ARROW_SIZE;
      
      //calculate mid point offset position for arrow head
      Vector2 arrowHeadPoint = midPosition + direction.normalized * _CONNECTING_LINE_ARROW_SIZE;
      
      //draw arrow
      Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, _CONNECTING_LINE_WIDTH);
      Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, _CONNECTING_LINE_WIDTH);
      
      //draw line
      Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, _CONNECTING_LINE_WIDTH);
      
      GUI.changed = true; 
   }
   
   //draw room node in the graph window
   private void DrawRoomNodes()
   {
      //loop through all room nodes and draw them
      foreach (RoomNodeSO roomNode in _currentRoomNodeGraph.roomNodeList)
      {
         if (roomNode.isSelected)
         {
            roomNode.Draw(_roomNodeSelectedStyle);
         }
         else
         {
            roomNode.Draw(_roomNodeStyle);
         }
      }

      GUI.changed = true;
   }
   
   //Selection changed in the inspector
   private void InspectorSelectionChanged()
   {
      RoomNodeGraphSO roomNodeGraph = Selection.activeObject as RoomNodeGraphSO;

      if (roomNodeGraph != null)
      {
         _currentRoomNodeGraph = roomNodeGraph;
         GUI.changed = true;
      }
   }
}
