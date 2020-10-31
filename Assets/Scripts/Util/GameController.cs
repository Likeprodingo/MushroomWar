using UnityEngine;

namespace Util
{
    public class GameController : MonoBehaviour
    {
        public static bool IsActive = true;

        public void StopGame()
        {
            IsActive = !IsActive;
        }
    }
}