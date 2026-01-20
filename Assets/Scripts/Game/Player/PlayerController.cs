using Game.Character;
using Game.Character.States;
using Game.Character.States.Movement;
using UnityEngine;

namespace Game.Player
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(StatusComponent))]
    public class PlayerController : MonoBehaviour, ICharacterContext, IDashable
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5f;
        public float MoveSpeed => moveSpeed;
        public IMoveIntent MoveIntent { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public CharacterStateMachine StateMachine { get; private set; }
        public StatusComponent CurrentStatus => _playerStatus;

        private bool _isMovementLocked;
        private Vector2 _movementVelocity;

        private IdleState _idleState;
        private MoveState _moveState;
        private DashState _dashState;

        private StatusComponent _playerStatus;

        private void Awake()
        {
            MoveIntent = GetComponent<IMoveIntent>();
            Rigidbody = GetComponent<Rigidbody2D>();
            _playerStatus = GetComponent<StatusComponent>();
        }

        private void Start()
        {
            StateMachine = new CharacterStateMachine();

            _idleState = new IdleState(this, StateMachine);
            _moveState = new MoveState(this, StateMachine);
            _dashState = new DashState(this, StateMachine);

            StateMachine.RegisterState(_idleState);
            StateMachine.RegisterState(_moveState);
            StateMachine.RegisterState(_dashState);

            StateMachine.Initialize<IdleState>();
        }

        private void Update()
        {
            StateMachine?.Update();
        }

        private void FixedUpdate()
        {
            CaculateVelocity();
        }

        private void CaculateVelocity()
        {
            Vector2 newPos = Rigidbody.position + _movementVelocity * UnityEngine.Time.fixedDeltaTime;
            Rigidbody.MovePosition(newPos);
        }

        public void ApplyMovement(Vector2 velocity)
        {
            if (_isMovementLocked) return;
            _movementVelocity = velocity;
        }

        public void ForceApplyMovement(Vector2 velocity)
        {
            _movementVelocity = velocity;
        }

        public void SetMovementLocked(bool locked)
        {
            _isMovementLocked = locked;
        }

        public bool StartDash(Vector2 direction, float speed, float duration, bool lockMovement = true)
        {
            if (direction.sqrMagnitude <= 0.0001f) return false;

            _dashState.Configure(direction.normalized, speed, duration, lockMovement);
            StateMachine.ChangeState<DashState>();
            return true;
        }

        public void CancelDash()
        {
            if (StateMachine.CurrentState is DashState)
            {
                StateMachine.ChangeState<IdleState>();
            }
        }
    }
}
