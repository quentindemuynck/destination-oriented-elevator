using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private GridLayoutGroup buttonGrid;
    [SerializeField] private TextMeshPro interactionText;
    [SerializeField] private Canvas UICanvas;

    // public

    public GridLayoutGroup ButtonGrid => buttonGrid;  

    // private
    private PanelButton button;
    private Floor _owningFloor; // the floor this controlPanel is on
    private ElevatorDispatcher _elevatorDispatcher; // the elevator dispatcher this belongs to

    private bool _isPlayerInRange = false;
    private bool _isMenuOpened = false;


    // unity functions

    private void Awake()
    {
        Debug.Assert(buttonGrid != null);
        Debug.Assert(interactionText != null);
        Debug.Assert(UICanvas != null);

        UICanvas.gameObject.SetActive(_isMenuOpened);
    }
    // public functions

    /// <summary>
    /// Initializes the Panel
    /// </summary>
    public void Init(Floor owningFloor, ElevatorDispatcher dispatcher)
    {
        _elevatorDispatcher = dispatcher;
        _owningFloor = owningFloor;
    }
    
    /// <summary>
    /// Toggles the control panel ui menu
    /// </summary>
    public void ToggleMenuOpened()
    {
        OpenMenu(!_isMenuOpened);
    }

    // private

    private void OpenMenu(bool open)
    {
        _isMenuOpened = open;
        UICanvas.gameObject?.SetActive(open);
    }

    // event handlers

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger entered");
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player ==  null ) return;

        interactionText.gameObject.SetActive(true);
        player.ControlPanelInRange = this;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        var player = other.gameObject.GetComponent<PlayerController>();
        if (player == null) return;

        player.ControlPanelInRange = null;
        interactionText.gameObject.SetActive(false);

        // Safety
        OpenMenu(false);
    }
}
