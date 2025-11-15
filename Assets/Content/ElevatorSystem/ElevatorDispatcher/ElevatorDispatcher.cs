using UnityEngine;
using System.Collections.Generic;

public class ElevatorDispatcher : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private List<Elevator> elevators = new List<Elevator>();
    [SerializeField] private List<Floor> floors = new List<Floor>();

    //private

    // public functions

    /// <summary>
    /// Request a certain floor to the elevator dispatcher.
    /// This will then calculate which elevator is the most optimal
    /// and will add that floor to the schedule of that elevator
    /// </summary>
    /// <param name="floor"> The destination floor that the requester intends to travel to.</param>
    /// <returns> The index of the floor</returns>
    int RequestFloor(Floor floor)
    {
        ElevatorChoice elevatorChoice = GetOptimalElevator(floor);
        UpdateElevatorSchedule(floor, elevatorChoice);

        return elevatorChoice.ElevatorIndex;
    }

    // private functions

    private ElevatorChoice GetOptimalElevator(Floor floor)
    {
        ElevatorChoice elevator = new ElevatorChoice();

        return elevator;
    }

    private void UpdateElevatorSchedule(Floor floor, ElevatorChoice elevatorChoice)
    {
        elevatorChoice.Elevator.Schedule.Insert(elevatorChoice.InsertIndex, floor);
    }

    // structs
    private struct ElevatorChoice
    {
        public Elevator Elevator;
        public int InsertIndex;
        public int ElevatorIndex;
    }
}
