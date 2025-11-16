using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private ControlPanel controlPanel;

    // public

    public ControlPanel ControlPanel => controlPanel;
}
