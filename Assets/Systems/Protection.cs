using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Protection : ISystem
{

    public void UpdateSystem()
    {
        var world = World.Instance;
        foreach (var entityID in world.entities)
        {
            if (!world.ShouldEntityBeUpdated(entityID))
            {
                continue;
            }

            var collisionCount = world.collisionCountTab[entityID];
            var state = world.stateTab[entityID];
            var protectionTime = world.protectionTimeTab[entityID];
            var cooldownTime = world.cooldownTimeTab[entityID];
            float protectionCollisionCount = ECSController.Instance.Config.protectionCollisionCount;
            float protectionDuration = ECSController.Instance.Config.protectionDuration;
            float protectionCooldown = ECSController.Instance.Config.protectionCooldown;


            // Change state to protected if condition met
            if (state.state == State.CircleState.Dynamic && collisionCount.collisionCount >= protectionCollisionCount && protectionTime.protectionTime <= 0)
            {
                state.state = State.CircleState.Protected;
                protectionTime.protectionTime = 0;
                collisionCount.collisionCount = 0;
            }

            // Protection and cooldown timers
            else if (state.state == State.CircleState.Protected)
            {
                protectionTime.protectionTime += Time.deltaTime;

                // End of protection
                if (protectionTime.protectionTime >= protectionDuration)
                {
                    state.state = State.CircleState.Cooldown;
                    protectionTime.protectionTime = 0;
                }
            }
            else if (state.state == State.CircleState.Cooldown)
            {
                cooldownTime.cooldownTime += Time.deltaTime;

                // End of cooldown
                if (cooldownTime.cooldownTime >= protectionCooldown)
                {
                    state.state = State.CircleState.Dynamic;
                    cooldownTime.cooldownTime = 0;
                }
            }

        }
    }

    public string Name { get; } = "Protection";
}