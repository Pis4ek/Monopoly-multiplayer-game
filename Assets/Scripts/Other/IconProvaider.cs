using System.Collections.Generic;
using UnityEngine.U2D;
using UnityEngine;

namespace Assets.Scripts.Other
{
    public class IconProvaider
    {
        private Dictionary<string, Sprite> _icons = new();

        public IconProvaider(SpriteAtlas _cellAtlas)
        {
            var sprites = new Sprite[_cellAtlas.spriteCount];
            _cellAtlas.GetSprites(sprites);

            foreach (var sprite in sprites)
            {
                if (sprite != null)
                {
                    var name = sprite.name.Remove(sprite.name.Length - 7);
                    _icons.Add(name, sprite);
                    //UnityEngine.Debug.Log(name);
                }
            }
        }

        public Sprite this[string key] => GetIcon(key);

        public Sprite GetIcon(string key)
        {
            if (_icons.ContainsKey(key))
            {
                return _icons[key];
            }
            else
            {
                return _icons["TST1"];
            }
        }
    }
}
