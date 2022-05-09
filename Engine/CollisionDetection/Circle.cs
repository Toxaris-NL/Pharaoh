using Microsoft.Xna.Framework;

namespace Pharaoh
{
    public struct Circle
    {
        public float Radius { get; private set; }
        public Vector2 Center { get; private set; }

        private Circle(float radius, Vector2 center)
        {
            Radius = radius;
            Center = center;
        }
    }
}