using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Protection : ISystem
{

    public void UpdateSystem()
    {
        foreach (var entityID in World.Instance.entities)
        {
            var collisionCount = World.Instance.collisionCountTab[entityID];
            var state = World.Instance.stateTab[entityID];
            var protectionTime = World.Instance.protectionTimeTab[entityID];
            var cooldownTime = World.Instance.cooldownTimeTab[entityID];
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