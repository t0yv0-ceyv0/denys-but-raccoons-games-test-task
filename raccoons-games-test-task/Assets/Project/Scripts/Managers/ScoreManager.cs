using UnityEngine;
using UnityEngine.Events;

namespace Project.Managers 
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Events")]
        public UnityEvent<int> OnScoreChanged;
        public UnityEvent<string> OnScoreChangedString;

        public int CurrentScore { get; private set; }

        #region Api

        public void AddScore(int points) 
        {
            CurrentScore += points;
            OnScoreChanged?.Invoke(CurrentScore);
            OnScoreChangedString?.Invoke(CurrentScore.ToString());
        }

        public void ResetScore()
        {
            CurrentScore = 0;
            OnScoreChanged?.Invoke(CurrentScore);
            OnScoreChangedString?.Invoke(CurrentScore.ToString());
        }

        #endregion

    }
}

