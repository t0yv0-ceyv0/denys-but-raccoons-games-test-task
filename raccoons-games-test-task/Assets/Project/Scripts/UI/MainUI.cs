using Project.Boosters;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI 
{
    public class MainUI : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _autoMergeButton;
        [SerializeField] private AutoMergeBooster _autoMergeBooster;


        #region Unity Lifecycle
        private void Awake()
        {
            _autoMergeButton.onClick.AddListener(async () => await OnAutoMergeButtonClick());
        }
        #endregion

        #region Internals
        private async Task OnAutoMergeButtonClick() 
        {
            _autoMergeButton.interactable = false;

            await _autoMergeBooster.ExecuteAutoMerge();

            _autoMergeButton.interactable = true;
        }
        #endregion
    }
}

