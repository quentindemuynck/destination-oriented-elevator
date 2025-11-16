using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class PanelButton : MonoBehaviour
{
    // public
    public Floor RequestedFloor {  get; set; }
    public Button Button => _button;

    // private

    private Button _button;

    // unity functions

    private void Awake()
    {
        _button = GetComponent<Button>();
        Debug.Assert (_button != null, $"{name} needs a button component");
    }
}
