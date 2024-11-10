using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class Typewriter : MonoBehaviour
{
    [System.Serializable]
    public struct Dialogue
    {
        public string speakerText;
        public string dialogueText;
        public Color color;
    }

    [Header("Attributes")]
    public List<Dialogue> dialogue;
    int activeDialogue = 0;
    public bool IsPlaying { get; private set; } = false;
    [SerializeField] bool automaticTransitions = true;
    [SerializeField]
    [Range(0.01f, 0.05f)]
    float typingInterval = 0.05f;

    [Header("UI Components")]
    [SerializeField]
    TMP_Text speakerText;
    [SerializeField]
    TMP_Text dialogueText;
    [SerializeField]
    Button nextButton;
    [SerializeField]
    Button previousButton;

    private void Start()
    {
        nextButton.onClick.AddListener(() =>
        {
            SetNextDialogue();
        }); 
        
        previousButton.onClick.AddListener(() =>
        {
            SetPreviousDialogue();
        });

        StartDialogue();
    }

    // Typewriter effect
    Coroutine typewriterCoroutine;
    IEnumerator TypewriterCoroutine()
    {
        IsPlaying = true;
        int characterIndex = 0;
        string activeDialogueString = dialogue[activeDialogue].dialogueText;
        int targetLength = activeDialogueString.Length;
        speakerText.text = dialogue[activeDialogue].speakerText;
        speakerText.color = dialogue[activeDialogue].color;
        while (dialogueText.text.Length != targetLength)
        {
            // Line break in advance to avoid breaks while word is being typed
            /*
            var textInfo = Text.textInfo;
            int lastLineIndex = textInfo.lineCount - 1;

            if (lastLineIndex >= 0)
            {
                int firstCharacterIndex = textInfo.lineInfo[lastLineIndex].firstCharacterIndex;
                int lastCharacterIndex = textInfo.lineInfo[lastLineIndex].lastCharacterIndex;
                int currentLineLength = lastCharacterIndex - firstCharacterIndex + 1;

                // Debug.Log("Current line length: " + currentLineLength);

                RectTransform textContainer = Text.transform.parent.GetComponent<RectTransform>();
                float lineBreakThreshold = (textContainer.rect.width * 0.9f) / 10;

                if (currentLineLength > lineBreakThreshold && activeDialogueString[characterIndex] == ' ')
                {
                    string lineBreakString = "<br>";
                    Text.text += lineBreakString;
                    targetLength += lineBreakString.Length - 1;
                }
            }
            */

            dialogueText.text += activeDialogueString[characterIndex];
            characterIndex++;
            yield return new WaitForSeconds(typingInterval);
        }

        if (automaticTransitions)
        {
            yield return new WaitForSeconds(2.5f);
            typewriterCoroutine = null;
            IsPlaying = false;
            SetNextDialogue();
        }
        else
        {
            typewriterCoroutine = null;
            IsPlaying = false;
        }
    }

    // Start the dialogue
    void StartDialogue()
    {
        if (typewriterCoroutine != null || dialogue.Count == 0) return;
        EndDialogue();
        typewriterCoroutine = StartCoroutine(TypewriterCoroutine());
    }

    // End the dialogue
    void EndDialogue(bool clearText = true)
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }

        if (clearText)
        {
            speakerText.text = "";
            dialogueText.text = "";
        }
    }

    // This may seem redundant but it is explicit for readability
    void SetNextDialogue()
    {
        if(dialogue.Count == 0) return; // No dialogue to navigate...

        int nextIndex = activeDialogue + 1;
        if (nextIndex < dialogue.Count)
        {
            EndDialogue();
            activeDialogue = nextIndex;
            StartDialogue();

            previousButton.interactable = true;
        }
        
        if(nextIndex >= dialogue.Count - 1)
        {
            nextButton.interactable = false;
        }
    }
    void SetPreviousDialogue()
    {
        if (dialogue.Count == 0) return; // No dialogue to navigate...

        int previousIndex = activeDialogue - 1;
        if (previousIndex >= 0)
        {
            EndDialogue();
            activeDialogue = previousIndex;
            StartDialogue();

            nextButton.interactable = true;
        }

        if(previousIndex <= 0)
        {
            previousButton.interactable = false;
        }
    }
}
