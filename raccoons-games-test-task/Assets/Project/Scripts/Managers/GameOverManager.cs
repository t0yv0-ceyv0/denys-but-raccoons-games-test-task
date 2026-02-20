using Project.Cubes;
using UnityEngine;
using UnityEngine.Events;

namespace Project.Managers 
{
    [RequireComponent(typeof(BoxCollider))]
    public class GameOverManager : MonoBehaviour
    {

        public UnityEvent OnGameOver;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<CubeBase>(out CubeBase cube)) 
            {
                if (cube.CurrentState == CubeState.OnBoard) 
                {
                    OnGameOver?.Invoke();
                }
            }
        }

    }
}

