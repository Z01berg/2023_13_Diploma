using System;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class CoverPosition : MonoBehaviour
    {

        public void UpdatePosition(Transform trans)
        {
            transform.position = trans.position;
        }
    }
}