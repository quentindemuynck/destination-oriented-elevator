using System.Collections.Generic;
using UnityEngine;
using System;

public class Elevator : MonoBehaviour
{
    // public
    public List<Floor> Schedule { get; } = new List<Floor>();
    public event Action<Floor> OnArriveAtFloor;
    public Floor CurrentFloor => _currentFloor;
    public Direction ElevatorDirection => _elevatorDirection;

    // private
    private Floor _currentFloor = null;
    private Direction _elevatorDirection;

    // structs
    public enum Direction
    {
        Up,
        Down,
        None
    }
}
