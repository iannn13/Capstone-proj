using UnityEngine;
using UnityEngine.UI;

public class PanelView2 : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button unviewbutton;  // Button to hide the panel
    [Header("Button")]
    [SerializeField] private Button Moneyinteract;  // Button to show the panel
    [Header("Panel")]
    [SerializeField] private GameObject Panel2;  // The panel to show/hide

    void Start()
    {
        // Ensure the phonenuminteract button has a Button component
        if (Moneyinteract != null)
        {
            Moneyinteract.onClick.AddListener(OnShowPanelButtonClicked);
        }
        else
        {
            Debug.LogError("phonenuminteract does not have a Button component!");
        }

        // Ensure the unviewbutton hides the panel when clicked
        if (unviewbutton != null)
        {
            unviewbutton.onClick.AddListener(OnHidePanelButtonClicked);
        }
        else
        {
            Debug.LogError("unviewbutton does not have a Button component!");
        }

        // Hide the unview button and panel initiall
        // Add debug information
        Debug.Log("Panel initially hidden, waiting for interaction.");
    }

    // This method will be called when phonenuminteract button is clicked
    private void OnShowPanelButtonClicked()
    {
        Debug.Log("Show button clicked!");

        // Ensure the panel is deactivated before showing it
        if (!Panel2.activeSelf)
        {
            Panel2.SetActive(true);  // Show the panel
            unviewbutton.gameObject.SetActive(true);  // Show the button to hide the panel
            Debug.Log("Panel shown.");
        }
        else
        {
            Debug.LogWarning("Panel is already active.");
        }
    }

    // This method will be called when unviewbutton is clicked to hide the panel
    private void OnHidePanelButtonClicked()
    {
        Debug.Log("Hide button clicked!");
        if (Panel2.activeSelf)
        {
            Panel2.SetActive(false);  // Hide the panel
            unviewbutton.gameObject.SetActive(false);  // Hide the unview button
            Debug.Log("Panel hidden.");
            Moneyinteract.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("Panel is already hidden.");
        }
    }
}
