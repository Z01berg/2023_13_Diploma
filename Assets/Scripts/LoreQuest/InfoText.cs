using System;
using UnityEngine;

namespace LoreQuest
{
    public class InfoText : MonoBehaviour
    {
        [SerializeField] private GameObject _popUpText;
        [SerializeField] private ShowCaseEvent _showCaseEvent;
        
        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                _popUpText.SetActive(true);
                _showCaseEvent._inRange = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collider)
        {
            if (collider.CompareTag("Player"))
            {
                _popUpText.SetActive(false);
                _showCaseEvent._inRange = true;
            }
        }
        
    }
}