using System;
using UnityEngine;

public class MoveToPositionAI : MovementAi
{
    [SerializeField]
    private float minX;
    [SerializeField]
    private float maxX;
    [SerializeField]
    private float minY;
    [SerializeField]
    private float maxY;
    private Vector3 moveLocation;

    public override void initialize(Transform playerTransform, Transform selfTransform)
    {
        base.initialize(playerTransform, selfTransform);
        if (minX >= maxX || minY >= maxY)
        {
            throw new Exception("Invalid min/max values for MoveToPositionAI. Ensure that minX < maxX and minY < maxY.");
        }
        moveLocation = new Vector3(UnityEngine.Random.Range(minX, maxX), UnityEngine.Random.Range(minY, maxY));
        Debug.Log("MoveToPositionAI initialized with moveLocation: " + moveLocation);
    }
    public override Vector3 GetMoveDirection()
    {
        // Move towards the player
        if ((moveLocation - self.position).magnitude < 0.1f)
        {
            return Vector3.zero;
        }
        else
        {
            return (moveLocation - self.position).normalized;
        }
    }
    
}
