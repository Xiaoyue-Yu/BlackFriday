using Ink.Runtime;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink JSON file")]
    public TextAsset inkJSON;

    private Story story;

    void Start()
    {
        Debug.Log("QuestDialogueScene started");

        // Load the Ink story
        story = new Story(inkJSON.text);

        // Start with a specific customer (you can randomize later)
        story.ChoosePathString("customer1");

        Debug.Log("Starting dialogue...\n");

        ContinueStory();
    }

    void ContinueStory()
    {
        while (story.canContinue)
        {
            string line = story.Continue();
            Debug.Log("Customer says: " + line);
        }

        PrintChoices();
        ReadVariables();
    }

    void PrintChoices()
    {
        if (story.currentChoices.Count > 0)
        {
            Debug.Log("Player Choices:");

            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Debug.Log(i + ": " + story.currentChoices[i].text);
            }
        }
    }

    void ReadVariables()
    {
        if (story.variablesState.GlobalVariableExistsWithName("trust"))
        {
            int trust = (int)story.variablesState["trust"];
            Debug.Log("Trust score: " + trust);
        }

        if (story.variablesState.GlobalVariableExistsWithName("outfitHint"))
        {
            string hint = (string)story.variablesState["outfitHint"];
            Debug.Log("Outfit hint: " + hint);
        }
    }
}