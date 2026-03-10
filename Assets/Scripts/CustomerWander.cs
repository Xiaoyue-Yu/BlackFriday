using UnityEngine;

public class CustomerWander : MonoBehaviour
{
    public enum CustomerState
    {
        Wandering,          
        GoingToCounter,     
        WaitingAtCounter,   
        Leaving            
    }

    [Header("State")]
    public CustomerState currentState = CustomerState.Wandering;

    private Vector2 counterPosition = new Vector2(0f, 2f);
    private Vector2 exitPosition = new Vector2(0f, -6f);

    [Header("Wondering")]
    public float moveSpeed = 1.5f;
    public float wanderRadius = 4.0f;
    public float waitTime = 2.0f;

    private Vector2 startPosition;
    private Vector2 targetPosition;
    private float waitTimer;
    private Animator animator;

    private void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        PickNewDestination();
    }

    public void GoToCheckout()
    {
        if (currentState != CustomerState.Wandering) return;

        currentState = CustomerState.GoingToCounter;
        targetPosition = counterPosition;
    }

    private void Update()
    {
        switch (currentState)
        {
            case CustomerState.Wandering:
                UpdateWandering();
                break;

            case CustomerState.GoingToCounter:
                MoveTowardsPoint(counterPosition);

                if (Vector2.Distance(transform.position, counterPosition) < 0.1f)
                {
                    currentState = CustomerState.WaitingAtCounter;
                    waitTimer = 3.0f; 
                    if (animator != null) animator.SetBool("isMoving", false); 
                }
                break;

            case CustomerState.WaitingAtCounter:
                waitTimer -= Time.deltaTime;

                if (waitTimer <= 0)
                {
                    currentState = CustomerState.Leaving;
                    targetPosition = exitPosition; 
                }
                break;

            case CustomerState.Leaving:
                MoveTowardsPoint(exitPosition);

                if (Vector2.Distance(transform.position, exitPosition) < 0.1f)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }


    private void UpdateWandering()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            if (animator != null) animator.SetBool("isMoving", false);
            return;
        }

        MoveTowardsPoint(targetPosition);

        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            waitTimer = waitTime;
            PickNewDestination();
        }
    }

    private void MoveTowardsPoint(Vector2 target)
    {
        Vector2 currentPos = transform.position;
        Vector2 direction = target - currentPos;

        transform.position = Vector2.MoveTowards(currentPos, target, moveSpeed * Time.deltaTime);

        if (direction.sqrMagnitude > 0.01f)
        {
            transform.up = direction.normalized;
            if (animator != null) animator.SetBool("isMoving", true);
        }
    }

    private void PickNewDestination()
    {
        Vector2 randomOffset = Random.insideUnitCircle * wanderRadius;
        targetPosition = startPosition + randomOffset;
    }
}