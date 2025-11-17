using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ControlPanel controlPanel;
    [SerializeField] private Transform elevatorStoppingHeight;

    // public

    public Transform ElevatorStoppingHeight => elevatorStoppingHeight;
    public ControlPanel ControlPanel => controlPanel;
}
