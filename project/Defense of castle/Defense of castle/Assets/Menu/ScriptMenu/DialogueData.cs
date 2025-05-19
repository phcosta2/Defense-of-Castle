// DialogueData.cs
// Purpose: Defines the data structures for dialogue.
// Create this as a new C# script in your project (e.g., in your Scripts/Dialogue folder).

using UnityEngine;
using System.Collections.Generic; // Required for List

// This class represents a single line of dialogue with a speaker.
[System.Serializable] // Allows this class to be seen and edited in the Unity Inspector
public class DialogueLine
{
    public string speakerName;      // The name of the character speaking this line.
    [TextArea(3, 10)]               // Makes the 'sentence' field a larger text box in the Inspector.
    public string sentence;         // The actual text content of the dialogue line.
}

// This class represents a complete dialogue, which is a collection of DialogueLines.
[System.Serializable] // Allows this class to be seen and edited in the Unity Inspector
public class Dialogue
{
    public string dialogueName;     // An optional name to identify this dialogue (e.g., "Intro Speech").
    public List<DialogueLine> lines; // A list to hold all the individual lines of this dialogue.
}