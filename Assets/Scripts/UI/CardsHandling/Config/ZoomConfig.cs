using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Config
{
    /**
     * Klasa ta przechowuje ustawienia, które wpływają na ustawienia przybliżenia/powiększenia karty
     * Większość zmian dokonuje się w inspektorze
    */
    [Serializable]
    public class ZoomConfig
    {
        public bool zoomOnHover;

        [Range(1f, 5f)] public float multiplier = 1;

        public float overrideYPositionOfChosenCard = -1; // -1 oznacza że nic się nie zmienia, określa o ile idzie w góre po hoverze lub wyborze
        public float overrideYPositionOfUnUsedCard = -1; // -1 oznacza że nic się nie zmienia, określa o ile idzie w dół gdy inna karta jest wybrana

        [Header("UI Layer")] public int defaultSortOrder;

        public bool bringToFrontOnHover;

        public int zoomedSortOrder;
    }
}