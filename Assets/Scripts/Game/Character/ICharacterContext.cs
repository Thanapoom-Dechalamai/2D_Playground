using UnityEngine;

namespace Game.Character
{
    public interface ICharacterContext
    {
        float MoveSpeed { get; }
        IMoveIntent MoveIntent { get; }

        void ApplyMovement(Vector2 velocity);
    }

    public interface IMoveIntent
    {
        Vector2 GetMoveIntent();
    }

    public interface IDashable
    {
        bool StartDash(Vector2 direction, float speed, float duration, bool lockMovement = true);

        void CancelDash();
    }

}
