using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ControlPanel controlPanel;
    [SerializeField] private Transform elevatorStoppingHeight;

    // public

    public ControlPanel ControlPanel => controlPanel;
}
