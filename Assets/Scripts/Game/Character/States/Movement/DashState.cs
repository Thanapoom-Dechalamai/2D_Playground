using Game.Character.States.Movement;
using Game.Player;
using UnityEngine;

namespace Game.Character.States
{
    public class DashState : CharacterState
    {
        private Vector2 _direction;
        private float _speed;
        private float _duration;
        private float _timer;
        private bool _lockMovement;

        public DashState(ICharacterContext character, CharacterStateMachine stateMachine) : base(character, stateMachine) { }

        public void Configure(Vector2 direction, float speed, float duration, bool lockMovement)
        {
            _direction = direction;
            _speed = speed;
            _duration = duration;
            _lockMovement = lockMovement;
        }

        public override void Enter()
        {
            _timer = _duration;

            if (_lockMovement && Character is PlayerController player)
            {
                player.SetMovementLocked(true);
            }
        }

        public override void Update()
        {
            _timer -= UnityEngine.Time.deltaTime;

            if (Character is PlayerController player)
            {
                player.ForceApplyMovement(_direction * _speed);
            }
            else
            {
                Character.ApplyMovement(_direction * _speed);
            }

            if (_timer <= 0f)
            {
                StateMachine.ChangeState<IdleState>();
            }
        }

        public override void Exit()
        {
            if (_lockMovement && Character is PlayerController player)
            {
                player.SetMovementLocked(false);
            }

            Character.ApplyMovement(Vector2.zero);
        }
    }
}
