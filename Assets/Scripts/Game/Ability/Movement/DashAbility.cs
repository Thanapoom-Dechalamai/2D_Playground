using Game.Character;
using ScriptableObjects.Ability;
using UnityEngine;

namespace Game.Ability
{
    [DisallowMultipleComponent]
    public class DashAbility : AbilityBase
    {
        protected override void OnActivateInternal(ActivationContext ctx)
        {
            var ownerTransform = Owner?.OwnerTransform;
            if (ownerTransform == null)
            {
                Debug.LogWarning("DashAbility: OwnerTransform null.");
                Finish();
                return;
            }

            var dashable = Owner.GetService<IDashable>();
            if (dashable == null)
            {
                Debug.LogWarning($"DashAbility: Owner {ownerTransform.name} does not provide IDashable.");
                Finish();
                return;
            }

            Vector2 dir = ctx.Direction;
            if (dir.sqrMagnitude <= 0.0001f)
            {
                var charCtx = Owner.GetService<ICharacterContext>();
                
                if (charCtx != null && charCtx.MoveIntent != null)
                    dir = charCtx.MoveIntent.GetMoveIntent();
                
                if (dir.sqrMagnitude <= 0.0001f)
                    dir = Vector2.right;
            }

            float duration = 0.25f;
            float speed = 12f;

            if (data is DashAbilityData dashData)
            {
                duration = Mathf.Max(0.001f, dashData.dashDuration);
                if (dashData.dashDistance > 0f)
                {
                    speed = dashData.dashDistance / duration;
                }
            }

            bool started = dashable.StartDash(dir.normalized, speed, duration, lockMovement: true);
            if (!started)
            {
                Debug.LogError($"DashAbility: StartDash rejected by {ownerTransform.name}.");
            }

            Finish();
        }
    }
}
