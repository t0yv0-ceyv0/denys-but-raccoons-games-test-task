using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Configs 
{
    [CreateAssetMenu(fileName = "PlayerInputsConfig", menuName = "Game/Configs/PlayerInputsConfig")]
    public class PlayerInputsConfig : ScriptableObject
    {
        public InputActionReference dragAction;
        public InputActionReference positionAction;
    }
}
