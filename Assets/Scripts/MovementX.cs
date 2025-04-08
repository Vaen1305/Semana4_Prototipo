using UnityEngine;

public class MovementX : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float minX = -6f;
    [SerializeField] private float maxX = 6f;
    
    private int direction = 1;
    private Transform objectTransform;

    private void Awake()
    {
        objectTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        MoveObject();
        CheckBoundaries();
    }

    private void MoveObject()
    {
        Vector2 newPosition = objectTransform.position;
        newPosition.x += speed * direction * Time.deltaTime;
        objectTransform.position = newPosition;
    }

    private void CheckBoundaries()
    {
        if (objectTransform.position.x > maxX || objectTransform.position.x < minX)
        {
            direction *= -1;
        }
    }
}