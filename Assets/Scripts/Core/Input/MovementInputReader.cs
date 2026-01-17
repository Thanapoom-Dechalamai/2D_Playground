using Game.Character;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    public class MovementInputReader : MonoBehaviour, IMoveIntent
    {
        private GameInput _input;
        private Vector2 _moveIntent;
        
        private void Awake()
        {
            _input = new GameInput();

            _input.Player.Move.performed += OnMove;
            _input.Player.Move.canceled += OnMove;
        }

        private void OnEnable()
        {
            _input.Player.Enable();
        }

        private void OnDisable()
        {
            _input.Player.Disable();
        }

        private void OnDestroy()
        {
            _input.Player.Move.performed -= OnMove;
            _input.Player.Move.canceled -= OnMove;
        }
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            _moveIntent = value.sqrMagnitude > 1f ? value.normalized : value;
        }

        public Vector2 GetMoveIntent() => _moveIntent;
    }
}
