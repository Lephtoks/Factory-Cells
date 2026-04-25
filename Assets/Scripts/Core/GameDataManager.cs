using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-1000)]
    public class GameDataManager : Singleton<GameDataManager>
    {
        public GameData Data = new();
    }
}