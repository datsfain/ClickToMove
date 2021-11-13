using UnityEngine;


public class MoveCommand
{
    private Transform targetTransform;
    private Vector2 destination;
    private Vector2 direction;
    private float remainingDistace;
    public bool isCompleted { get; private set; }

    public void Begin()
    {
        Vector3 movement = destination - (Vector2)targetTransform.position;
        direction = movement.normalized;
        remainingDistace = movement.magnitude;
    }

    public MoveCommand(Transform targetTransform, Vector2 destination)
    {
        this.targetTransform = targetTransform;
        this.destination = destination;
    }

    public void Tick(float deltaTime, float speed)
    {
        float deltaDistance = speed * deltaTime;
        Vector3 deltaMovement = direction * deltaDistance;

        remainingDistace -= deltaDistance;
        targetTransform.position += deltaMovement;

        if (remainingDistace <= 0)
        {
            isCompleted = true;
            targetTransform.position = destination;
        }
    }
}
