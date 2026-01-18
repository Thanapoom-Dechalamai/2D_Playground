using UnityEngine;
using UnityEngine.InputSystem;
using Game.Ability;
using Game.Character;

namespace Game.Input
{
    [RequireComponent(typeof(AbilityUser))]
    public class AbilityInputReader : MonoBehaviour
    {
        private GameInput _input;
        private AbilityUser _abilityUser;
        private IMoveIntent _moveIntent;

        private void Awake()
        {
            _abilityUser = GetComponent<AbilityUser>();
            _moveIntent = GetComponent<IMoveIntent>();

            _input = new GameInput();

            var player = _input.Player;
            
            player.Dash.performed += ctx => OnDash();
            
            player.Slot1.performed += ctx => OnSlotPressed(0);
            player.Slot2.performed += ctx => OnSlotPressed(1);
            player.Slot3.performed += ctx => OnSlotPressed(2);
        }

        private void OnEnable() => _input.Player.Enable();
        private void OnDisable() => _input.Player.Disable();

        private void OnDestroy()
        {
            var player = _input.Player;
            
            player.Dash.performed -= ctx => OnDash();
            
            player.Slot1.performed -= ctx => OnSlotPressed(0);
            player.Slot2.performed -= ctx => OnSlotPressed(1);
            player.Slot3.performed -= ctx => OnSlotPressed(2);
        }

        private void OnDash()
        {
            Vector2 dir = _moveIntent != null ? _moveIntent.GetMoveIntent() : Vector2.zero;
            var ctx = new ActivationContext { Direction = dir };

            _abilityUser.TryActivateById("dash", ctx);
        }

        private void OnSlotPressed(int slotIndex)
        {
            Vector2 dir = _moveIntent != null ? _moveIntent.GetMoveIntent() : Vector2.zero;
            var ctx = new ActivationContext { Direction = dir };
            _abilityUser.TryActivateSlot(slotIndex, ctx);
        }
    }
}
