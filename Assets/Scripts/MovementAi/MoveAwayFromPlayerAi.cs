using System;
using UnityEngine;

public class MoveAwayFromPlayerAi : MovementAi
{
    [SerializeField]
    private float stopDistance;
    public override Vector3 GetMoveDirection()
    {
        // // Move towards the player
        if ((self.position - player.position).magnitude > stopDistance)
        {
            return Vector3.zero;
        }
        else
        {
            return (self.position - player.position).normalized;
        }
    }
}
