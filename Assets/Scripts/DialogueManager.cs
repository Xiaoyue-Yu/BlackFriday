
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI customerInitialText;

    [SerializeField] private Button option1Button;
    [SerializeField] private Button option2Button;
    [SerializeField] private Button option3Button;

    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;

    [Header("Ink")]
    [SerializeField] private TextAsset inkJSON;

    private Story currentStory;
    private bool dialoguePlaying = false;

    private void Start()
    {
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        CustomerGO.OnCustomerServeStart += OnEnterDialogue;
    }

    private void OnDisable()
    {
        CustomerGO.OnCustomerServeStart -= OnEnterDialogue;
    }

    private void OnEnterDialogue(string customerId)
    {
        dialoguePlaying = true;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }

        Debug.Log("Start dialogue for: " + customerId);

        if (inkJSON == null)
        {
            Debug.LogError("Ink JSON is not assigned");
            return;
        }

        currentStory = new Story(inkJSON.text);
        // currentStory.ChoosePathString(customerId);
        try
        {
            currentStory.ChoosePathString(customerId);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Ink path not found: " + customerId + "\n" + e.Message);
            return;
        }

        RefreshView();
    }

    private void RefreshView()
    {
        if (currentStory == null) return;

        if (currentStory.canContinue)
        {
            string line = currentStory.Continue().Trim();
            customerInitialText.text = line;
        }

        DisplayChoices();

        if (!currentStory.canContinue && currentStory.currentChoices.Count == 0)
        {
            EndDialogue();
        }
    }

    private void DisplayChoices()
    {
        option1Button.gameObject.SetActive(false);
        option2Button.gameObject.SetActive(false);
        option3Button.gameObject.SetActive(false);

        if (currentStory.currentChoices.Count > 0)
        {
            option1Button.gameObject.SetActive(true);
            option1Text.text = currentStory.currentChoices[0].text;
        }

        if (currentStory.currentChoices.Count > 1)
        {
            option2Button.gameObject.SetActive(true);
            option2Text.text = currentStory.currentChoices[1].text;
        }

        if (currentStory.currentChoices.Count > 2)
        {
            option3Button.gameObject.SetActive(true);
            option3Text.text = currentStory.currentChoices[2].text;
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (currentStory == null) return;
        if (choiceIndex >= currentStory.currentChoices.Count) return;

        currentStory.ChooseChoiceIndex(choiceIndex);
        RefreshView();
    }

    public void EndDialogue()
    {
        Debug.Log("Ending dialogue");
        dialoguePlaying = false;

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }

        currentStory = null;
    }
}
