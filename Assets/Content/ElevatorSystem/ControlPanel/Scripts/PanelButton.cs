using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class PanelButton : MonoBehaviour
{
    // public
    public Button Button => _button;

    // private

    private Button _button;
    private Floor _owningFloor; // floor this panel is on
    private Floor _requestedFloor;
    private ElevatorDispatcher _dispatcher;     // dispatcher to send request to

    // public functions

    /// <summary>
    /// Initialize this button with everything it needs.
    /// </summary>
    public void Init(Floor owningFloor, Floor requestedFloor, ElevatorDispatcher dispatcher)
    {
        _owningFloor = owningFloor;
        _requestedFloor = requestedFloor;
        _dispatcher = dispatcher;

        // Clear old listeners if any, then hook up new one
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClicked);
    }

    // unity functions

    private void Awake()
    {
        _button = GetComponent<Button>();
        Debug.Assert (_button != null, $"{name} needs a button component");
    }

    private void OnClicked()
    {
        if (_dispatcher == null || _owningFloor == null || _requestedFloor == null)
        {
            Debug.LogError($"{name} is not properly initialized");
            return;
        }

        _dispatcher.RequestFloor(_requestedFloor, _owningFloor);
    }
}
