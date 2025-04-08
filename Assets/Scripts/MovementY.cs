using UnityEngine;

public class MovementY : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minY = -3.4f;
    [SerializeField] private float maxY = 1.5f;
    
    private int direction = 1;
    private Transform objectTransform;

    private void Awake()
    {
        objectTransform = transform;
    }

    private void Update()
    {
        MoveObject();
        CheckBoundaries();
    }

    private void MoveObject()
    {
        Vector2 newPosition = objectTransform.position;
        newPosition.y += speed * direction * Time.deltaTime;
        objectTransform.position = newPosition;
    }

    private void CheckBoundaries()
    {
        if (objectTransform.position.y > maxY || objectTransform.position.y < minY)
        {
            direction *= -1;
        }
    }
}
