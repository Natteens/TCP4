using ComponentUtils.ComponentUtils.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tcp4.Assets.Resources.Scripts.Managers
{
    public class GameAssets : Singleton<GameAssets>
    {
        public List<Sprite> clientSprites;
        public List<string> clientNames;
    }
}