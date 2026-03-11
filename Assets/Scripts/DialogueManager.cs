using System.Text;
using UnityEngine;
using Ink.Runtime;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class CustomerPortraitEntry
{
    public string customerId;
    public Sprite portrait;
}

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI customerInitialText;
    [SerializeField] private Image portraitImage;

    [SerializeField] private Button option1Button;
    [SerializeField] private Button option2Button;
    [SerializeField] private Button option3Button;

    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;
    [SerializeField] private TextMeshProUGUI option3Text;

    [Header("Portraits")]
    [SerializeField] private CustomerPortraitEntry[] customerPortraits;

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

        Debug.Log("OnEnterDialogue fired with: " + customerId);

        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
            Debug.Log("Dialogue panel opened");
        }
        else
        {
            Debug.LogError("dialoguePanel is null");
            return;
        }

        UpdatePortrait(customerId);

        if (inkJSON == null)
        {
            Debug.LogError("Ink JSON is not assigned");
            return;
        }

        currentStory = new Story(inkJSON.text);

        try
        {
            currentStory.ChoosePathString(customerId);
            Debug.Log("Successfully chose Ink path: " + customerId);
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

        StringBuilder visibleLines = new StringBuilder();

        while (currentStory.canContinue)
        {
            string line = currentStory.Continue().Trim();
            if (string.IsNullOrEmpty(line))
            {
                continue;
            }

            if (visibleLines.Length > 0)
            {
                visibleLines.AppendLine();
            }

            visibleLines.Append(line);

            if (currentStory.currentChoices.Count > 0)
            {
                break;
            }
        }

        if (customerInitialText != null)
        {
            customerInitialText.text = visibleLines.ToString();
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

    private void UpdatePortrait(string customerId)
    {
        if (portraitImage == null) return;

        foreach (CustomerPortraitEntry entry in customerPortraits)
        {
            if (entry != null && entry.customerId == customerId)
            {
                portraitImage.sprite = entry.portrait;
                portraitImage.enabled = entry.portrait != null;
                return;
            }
        }

        portraitImage.sprite = null;
        portraitImage.enabled = false;
    }
}
