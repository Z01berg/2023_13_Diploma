using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnButton : MonoBehaviour
    {
        private bool _isPlayerTurn = true;
        public Button button;

        public void ChangeStyle()
        {
            if (_isPlayerTurn)
            {
                _isPlayerTurn = !_isPlayerTurn;
                ColorBlock cb = button.colors;
                cb.normalColor = new Color(111, 18, 39);
                cb.highlightedColor = new Color(224, 66, 72);
                button.GetComponentInChildren<Text>().text = "Enemy's Turn";
            }
            else
            {
                _isPlayerTurn = !_isPlayerTurn;
                ColorBlock cb = button.colors;
                cb.normalColor = new Color(64, 99, 255);
                cb.highlightedColor = new Color(66, 154, 224);
                button.GetComponentInChildren<Text>().text = "Your Turn";
            }
        }
    }
}