using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class ElevatorDispatcher : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private List<Elevator> elevators = new List<Elevator>();
    [SerializeField] private List<Floor> floors = new List<Floor>();

    [Header("Data")]
    [SerializeField] private GameObject floorButtonPrefab;

    // unity functions

    private void Awake()
    {
        Debug.Assert(floorButtonPrefab != null, $"{name} needs a floorButtonPrefab");

        InitializeFloors();
        InitializeElevators();
    }

    // public functions

    /// <summary>
    /// Request a certain floor to the elevator dispatcher.
    /// This will then calculate which elevator is the most optimal
    /// and will add that floor to the schedule of that elevator
    /// </summary>
    /// <param name="floor"> The destination floor that the requester intends to travel to.</param>
    /// <returns> The index of the floor</returns>
    public int RequestFloor(Floor requestedFloor, Floor currentFloor)
    {
        ElevatorChoice elevatorChoice = GetOptimalElevator(requestedFloor, currentFloor);
        UpdateElevatorSchedule(requestedFloor, elevatorChoice);

        return elevatorChoice.ElevatorIndex;
    }

    // private functions

    private ElevatorChoice GetOptimalElevator(Floor requestedFloor, Floor currentFloor)
    {
        ElevatorChoice elevatorChoice = new ElevatorChoice();
        elevatorChoice.Elevator = elevators.First();
        elevatorChoice.InsertIndex = elevatorChoice.Elevator.Schedule.Count;
        elevatorChoice.ElevatorIndex = 0;

        return elevatorChoice;
    }

    private void UpdateElevatorSchedule(Floor floor, ElevatorChoice elevatorChoice)
    {
        elevatorChoice.Elevator.AddToSchedule(floor, elevatorChoice.InsertIndex);
    }

    private void InitializeFloors()
    {
        foreach (var floor in floors)
        {
            floor.ControlPanel.Init(floor, this);

            for (int i = 0; i < floors.Count; i++)
            {
                // initialize button
                GameObject button = Instantiate(floorButtonPrefab);
                button.transform.SetParent(floor.ControlPanel.ButtonGrid.transform, false);

                // set text
                var text = button.GetComponentInChildren<TMP_Text>(true);
                if (text != null)
                {
                    text.text = i.ToString();
                }
                else
                {
                    Debug.LogError($"The button prefab: {floorButtonPrefab.name} needs a text component");
                }

                // tell the button which floor it should go to
                var panelButton = button.GetComponent<PanelButton>();
                if (button != null)
                {
                    panelButton.Init(floor, floors[i], this);  
                }
                else
                {
                    Debug.LogError($"The button prefab: {floorButtonPrefab.name} needs a button component");
                }

            }
        }
    }

    private void InitializeElevators()
    {
        if ( floors.Count == 0 ) return; 
        foreach (var elevator in elevators)
        {
            Vector3 pos = elevator.transform.position;
            pos.y = floors.First().ElevatorStoppingHeight.position.y;
            elevator.transform.position = pos;
        }
    }

    // structs
    private struct ElevatorChoice
    {
        public Elevator Elevator;
        public int InsertIndex;
        public int ElevatorIndex;
    }
}
