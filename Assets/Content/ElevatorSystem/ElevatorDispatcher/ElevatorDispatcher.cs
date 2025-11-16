using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

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
    }

    // public functions

    /// <summary>
    /// Request a certain floor to the elevator dispatcher.
    /// This will then calculate which elevator is the most optimal
    /// and will add that floor to the schedule of that elevator
    /// </summary>
    /// <param name="floor"> The destination floor that the requester intends to travel to.</param>
    /// <returns> The index of the floor</returns>
    int RequestFloor(Floor requestedFloor, Floor currentFloor)
    {
        ElevatorChoice elevatorChoice = GetOptimalElevator(requestedFloor, currentFloor);
        UpdateElevatorSchedule(requestedFloor, elevatorChoice);

        return elevatorChoice.ElevatorIndex;
    }

    // private functions

    private ElevatorChoice GetOptimalElevator(Floor requestedFloor, Floor currentFloor)
    {
        ElevatorChoice elevator = new ElevatorChoice();

        return elevator;
    }

    private void UpdateElevatorSchedule(Floor floor, ElevatorChoice elevatorChoice)
    {
        elevatorChoice.Elevator.Schedule.Insert(elevatorChoice.InsertIndex, floor);
    }

    private void InitializeFloors()
    {
        foreach (var floor in floors)
        {
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

                // subscribe to button event
                var panelButton = button.GetComponent<PanelButton>();
                if (button != null)
                {
                    panelButton.RequestedFloor = floors[i];    
                }
                else
                {
                    Debug.LogError($"The button prefab: {floorButtonPrefab.name} needs a button component");
                }

            }
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
