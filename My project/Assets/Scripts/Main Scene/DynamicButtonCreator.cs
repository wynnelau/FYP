using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class DynamicButtonCreator : MonoBehaviour
{
    public GameObject buttonPrefab; // Assign your button prefab in the inspector
    public Transform buttonParent; // Assign the parent transform for the buttons in the inspector

    private List<GameObject> createdButtons = new List<GameObject>();

    // This method creates a dynamic button with the given text and assigns a callback function to it.
    public void CreateButton(string buttonText)
    {
        GameObject newButton = Instantiate(buttonPrefab, buttonParent);
        Button buttonComponent = newButton.GetComponent<Button>();

        // Set the button's text
        Text buttonTextComponent = newButton.GetComponentInChildren<Text>();
        if (buttonTextComponent != null)
        {
            buttonTextComponent.text = buttonText;
        }

        // Add a click event handler to the button
        buttonComponent.onClick.AddListener(() =>
        {
            // Handle button click here
            Debug.Log("Button Clicked: " + buttonText);
        });

        createdButtons.Add(newButton);
    }

    public void DeleteAllButtons()
    {
        foreach (GameObject button in createdButtons)
        {
            // Destroy each button GameObject
            Destroy(button);
        }

        // Clear the list of created buttons
        createdButtons.Clear();
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
