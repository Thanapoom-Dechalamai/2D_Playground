using Game.Character;
using Game.Character.States;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour, ICharacterContext
    {
        [SerializeField] private float moveSpeed = 5f;
        public float MoveSpeed => moveSpeed;
        public IMoveIntent MoveIntent { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public CharacterStateMachine StateMachine { get; private set; }

        private Vector2 _movementVelocity;


        private void Awake()
        {
            MoveIntent = GetComponent<IMoveIntent>();
            Rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            StateMachine = new CharacterStateMachine();
            
            StateMachine.RegisterState(new IdleState(this, StateMachine));
            
            StateMachine.RegisterState(new MoveState(this, StateMachine));
            
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

        public void ApplyMovement(Vector2 velocity)
        {
            _movementVelocity = velocity;
        }

        private void CaculateVelocity()
        {
            Vector2 newPos = Rigidbody.position + _movementVelocity * Time.fixedDeltaTime;

            Rigidbody.MovePosition(newPos);
        }
    }
}