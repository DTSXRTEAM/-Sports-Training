using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class TMPro_DropdownValidator : MonoBehaviour
{
    public List<TMP_Dropdown> dropdowns; // List of all TMP_Dropdown components to be validated
    public Button proceedButton; // Drag and drop the Proceed button here in the Inspector

    void Start()
    {
        // Ensure the list is not null
        if (dropdowns == null)
        {
            dropdowns = new List<TMP_Dropdown>();
        }

        // If no dropdowns were assigned, try to find them in children
        if (dropdowns.Count == 0)
        {
            dropdowns.AddRange(GetComponentsInChildren<TMP_Dropdown>());
        }

        // If proceedButton was not assigned, try to find it in children
        if (proceedButton == null)
        {
            proceedButton = GetComponentInChildren<Button>();
        }

        // Add listener to each dropdown
        foreach (var dropdown in dropdowns)
        {
            dropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(); });
        }

        // Initially disable the proceed button
        proceedButton.interactable = false;
    }

    void DropdownValueChanged()
    {
        bool allValid = true;

        // Check if all dropdowns have a selected value other than the default
        foreach (var dropdown in dropdowns)
        {
            if (dropdown.value == 0) // Adjust this condition based on TMPro dropdown specifics
            {
                allValid = false;
                break;
            }
        }

        // Enable or disable the proceed button based on the validation
        proceedButton.interactable = allValid;
    }
}
