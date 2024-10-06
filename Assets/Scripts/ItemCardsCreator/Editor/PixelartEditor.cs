using CardActions;
using Codice.Client.BaseCommands.Merge.Xml;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;
using Image = UnityEngine.UIElements.Image;
using Toggle = UnityEngine.UIElements.Toggle;

public class PixelartEditor : EditorWindow
{
    private Image _image;
    public static void ShowWindow(Image image)
    {
        PixelartEditor wnd = GetWindow<PixelartEditor>();
        wnd.titleContent = new GUIContent("PixelartEditor");

        wnd.minSize = new Vector2(450, 200);
        wnd.maxSize = new Vector2(1920, 720);
        
        wnd._image = image;
        wnd._image.scaleMode = ScaleMode.ScaleToFit;
        wnd._image.style.width = wnd.rootVisualElement.style.width;
        wnd._image.style.height = wnd.rootVisualElement.style.height;

        wnd.CreateGUI();
    }

    public void CreateGUI()
    {
        Repaint();
        VisualElement ve = new VisualElement();
        rootVisualElement.Add(ve);

        ve.Add(_image);
    }
}
