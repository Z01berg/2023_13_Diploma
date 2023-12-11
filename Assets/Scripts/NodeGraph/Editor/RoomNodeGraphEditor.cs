using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.MPE;

public class RoomNodeGraphEditor : EditorWindow
{
   private GUIStyle roomNodeStyle;
   private static RoomNodeGraphSO currentRoomNodeGraph;
   private RoomNodeSO currentRoomNode = null;
   private RoomNodeTypeListSO roomNodeTypeList;
   
   //Node layot
   private const float nodeWidth = 160f;
   private const float nodeHeight = 75f;
   private const int nodePadding = 25;
   private const int nodeBorder = 12;

   [MenuItem("Room Node Graph Editor", menuItem = "Window/2023_13_Diploma/Room Node Graph Editor")]
   
   private static void OpenWindow()
   {
      GetWindow<RoomNodeGraphEditor>("Room Node Graph Editor");
   }

   private void OnEnable()
   {
      //Define Node layot
      roomNodeStyle = new GUIStyle();
      roomNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
      roomNodeStyle.normal.textColor = Color.white;
      roomNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
      roomNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);
      
      //Load Room node types
      roomNodeTypeList = GameResources.Instance.roomNodeTypeList;
   }
   
   //Open the room node graph editor window if a room node graph scriptable object asset is double clicked in the inspector
   
   [OnOpenAsset(0)] //Need the namespace UnityEditor.Callback
   public static bool OnDoubleClickAsset(int instanceID, int line)
   {
      RoomNodeGraphSO roomNodeGraph = EditorUtility.InstanceIDToObject(instanceID) as RoomNodeGraphSO;

      if (roomNodeGraph != null)
      {
         OpenWindow();

         currentRoomNodeGraph = roomNodeGraph;

         return true;
      }

      return false;
   }

   //Draw Editor GUI
   private void OnGUI()
   {
      //if a scriptable object of type RoomNodeGraphSO has been selected then process
      if (currentRoomNodeGraph != null)
      {
         //Process Events
         ProcessEvents(Event.current);
         
         //Draw Room Nodes
         DrawRoomNodes();
      }

      if (GUI.changed)
      {
         Repaint();
      }
   }

   private void ProcessEvents(Event currentEvent)
   {
      //get room node that mouse is over if its null or not currently dragged
      if (currentRoomNode == null || currentRoomNode.isLeftClickDragging == false)
      {
         currentRoomNode = IsMouseOverRoomNode(currentEvent);
      }

      //if mouse isn't over a room node
      if (currentRoomNode == null)
      {
         ProcessRoomNodeGraphEvents(currentEvent);
      }
      else
      {
         //process room node events
         currentRoomNode.ProcessEvents(currentEvent);
      }
   }
   
   //check if mouse is over room node - if so then return the room node else return null
   private RoomNodeSO IsMouseOverRoomNode(Event currentEvent)
   {
      for (int i = currentRoomNodeGraph.roomNodeList.Count - 1; i >= 0; i--)
      {
         if (currentRoomNodeGraph.roomNodeList[i].rect.Contains(currentEvent.mousePosition))
         {
            return currentRoomNodeGraph.roomNodeList[i];
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
   }
   
   //Show the context menu
   private void ShowContextMenu(Vector2 mousePosition)
   {
      GenericMenu menu = new GenericMenu();
      
      menu.AddItem(new GUIContent("Create Room Node"), false, CreateRoomNode, mousePosition);
      
      menu.ShowAsContext();
   }
   
   //Create a room node at the mouse position
   private void CreateRoomNode(object mousePositionObject)
   {
      CreateRoomNode(mousePositionObject, roomNodeTypeList.list.Find(x => x.isNone));
   }
   
   //Create a room node at the mouse position BUT overload it for pass in RoomNodeType
   private void CreateRoomNode(object mousePositionObject, RoomNodeTypeSO roomNodeType)
   {
      Vector2 mousePosition = (Vector2)mousePositionObject;
      
      //create room node scriptable object asset
      RoomNodeSO roomNode = ScriptableObject.CreateInstance<RoomNodeSO>();
      
      //add room node to current room node graph room node list
      currentRoomNodeGraph.roomNodeList.Add(roomNode);
      
      //set room node values
      roomNode.Initialize(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentRoomNodeGraph,
         roomNodeType);
      
      //add room node to room node graph scriptable object asset database
      AssetDatabase.AddObjectToAsset(roomNode, currentRoomNodeGraph);
      
      AssetDatabase.SaveAssets();
   }
   
   //draw room node in the graph window
   private void DrawRoomNodes()
   {
      //loop through all room nodes and draw them
      foreach (RoomNodeSO roomNode in currentRoomNodeGraph.roomNodeList)
      {
         roomNode.Draw(roomNodeStyle);
      }

      GUI.changed = true;
   }
   
   
}
