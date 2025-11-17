using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Elevator : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float acceleration = 20;
    [SerializeField] private float maxSpeed = 10;

    // public
    
    public event Action<Floor> OnArriveAtFloor;
    public Floor CurrentFloor => _currentFloor;
    public Direction ElevatorDirection => _elevatorDirection;
    public List<Floor> Schedule => _schedule;

    // private
    private Rigidbody _rigidbody;
    private Floor _currentFloor = null;
    private Direction _elevatorDirection;

    private float _velocity;
    private float _timer = WAIT_TIME_AT_FLOOR;
    private List<Floor> _schedule = new List<Floor>();

    // const
    public const float WAIT_TIME_AT_FLOOR = 5f;

    // structs
    public enum Direction
    {
        Up,
        Down,
        None
    }

    // public functions

    /// <summary>
    /// Add a floor to the schedule
    /// </summary>
    /// <param name="floor"></param>
    /// <param name="atIndex"></param>
    public void AddToSchedule(Floor floor, int atIndex)
    {
        if(_schedule.Count == 0)
        {
            _timer = WAIT_TIME_AT_FLOOR; // wait before leaving
            _schedule.Add(floor);
        }
        else
        {
            _schedule.Insert(atIndex, floor);
        }
    }

    // unity functions 

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Debug.Assert(_rigidbody != null , $"{name} needs a rigidbody" );
    }

    private void OnEnable()
    {
        OnArriveAtFloor += HandleArriveAtFloor;
    }

    private void OnDisable()
    {
        OnArriveAtFloor -= HandleArriveAtFloor;
    }

    private void Update()
    {
        _timer -= Time.deltaTime; // count the timer
    }

    private void FixedUpdate()
    {
        if (_schedule.Count == 0)
        {
            _elevatorDirection = Direction.None;
            return;
        }
        if (_timer > 0f) return;


        Floor targetFloor = _schedule[0];

        Vector3 position = _rigidbody.position;
        float targetY = targetFloor.ElevatorStoppingHeight.position.y;

        float deltaY = targetY - position.y; // difference in height
           
        // prevent floating point mistakes
        const float positionEpsilon = 0.01f;

        if (Mathf.Abs(deltaY) < positionEpsilon)
        {
            position.y = targetY;
            _rigidbody.MovePosition(position);

            _currentFloor = targetFloor;
            _schedule.RemoveAt(0);
            _velocity = 0f;
            _elevatorDirection = Direction.None;
            _timer = WAIT_TIME_AT_FLOOR;

            OnArriveAtFloor?.Invoke(targetFloor);
            return;
        }

        // Determine direction
        float dir = Mathf.Sign(deltaY); // +1 up, -1 down
        _elevatorDirection = dir > 0 ? Direction.Up : Direction.Down;

        float absoluteVelocity = Mathf.Abs(_velocity);

        // GPT
        // How far we need to brake at current speed
        float brakingDistance = (acceleration > 0f)
            ? (absoluteVelocity * absoluteVelocity) / (2f * acceleration)
            : 0f;

        float distanceRemaining = Mathf.Abs(deltaY);

        
        if (distanceRemaining <= brakingDistance)
        {
            // Decelerate
            absoluteVelocity -= acceleration * Time.fixedDeltaTime;
            if (absoluteVelocity < 0f) absoluteVelocity = 0f;
        }
        else
        {
            // Accelerate
            absoluteVelocity += acceleration * Time.fixedDeltaTime;
            absoluteVelocity = Mathf.Min(absoluteVelocity, maxSpeed);
        }

        // Rebuild signed velocity
        _velocity = absoluteVelocity * dir;


        float newY = position.y + _velocity * Time.fixedDeltaTime;
        if ((dir > 0f && newY > targetY) || (dir < 0f && newY < targetY))
        {
            newY = targetY;
            _velocity = 0f;
        }

        Vector3 newPos = new Vector3(position.x, newY, position.z);
        _rigidbody.MovePosition(newPos);
    }

    // event handlers

    private void HandleArriveAtFloor(Floor floor)
    {
        _timer = WAIT_TIME_AT_FLOOR;
    }
}
