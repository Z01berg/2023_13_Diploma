using System;

namespace UI
{
    /**
     * Klasa ta przechowuje informacje na temat szybkości animacji kart.
     * Większość zmian dokonuje się w inspektorze
    */
    [Serializable]
    public class AnimationConfig
    {
        public float positionChangeSpeed = 500f;
        public float zoomSpeed = 0.3f;
    }
}