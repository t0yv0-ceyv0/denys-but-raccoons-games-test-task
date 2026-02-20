using Project.Cubes;
using UnityEngine;

namespace Project.Managers
{
    public class GameFieldManager : MonoBehaviour
    {
        [Header("Components")]
        [field: SerializeField]
        public Transform CubesContainer { get; private set; }

        #region Api

        public void AddChild(CubeBase cube) 
        {
            cube.Transform.SetParent(CubesContainer);
        }

        public void ClearGameField() 
        {
            foreach (Transform child in CubesContainer)
            {
                Destroy(child.gameObject);
            }
        }
        #endregion
    }
}
