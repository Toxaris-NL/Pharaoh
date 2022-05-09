using System.Collections.Generic;

using Microsoft.Xna.Framework;

namespace Pharaoh
{
    /// <summary>
    /// A class that can represent a game object with several animated sprites
    /// </summary>
    public class AnimatedGameObject : SpriteGameObject
    {
        private Dictionary<string, Animation> _animations;

        public AnimatedGameObject(float depth) : base(null, depth)
        {
            _animations = new Dictionary<string, Animation>();
        }

        public void LoadAnimation(string assetName, string id, bool looping, float frameTime)
        {
            Animation animation = new Animation(assetName, _depth, looping, frameTime);
            _animations[id] = animation;
        }

        public void PlayAnimation(string id, bool forceRestart = false, int startSheetIndex = 0)
        {
            // if the animation is already playing, do nothing
            if (!forceRestart && _sprite == _animations[id])
            {
                return;
            }

            _animations[id].Play(startSheetIndex);
            _sprite = _animations[id];
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_sprite != null)
            {
                ((Animation)_sprite).Update(gameTime);
            }
        }
    }
}