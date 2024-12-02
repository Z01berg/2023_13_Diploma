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
        public float drawingCardsSpeed = 500f;
        public float hoveringCardsSpeed = 500f;
        public float zoomSpeed = 0.3f;
    }
}