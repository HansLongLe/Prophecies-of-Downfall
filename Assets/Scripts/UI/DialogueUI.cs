using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{

    [SerializeField] private GameObject dialogue;
    [SerializeField] private string[] texts;

    public delegate void DialogueUIWithoutArgs();

    public static event DialogueUIWithoutArgs Interact;

    private GameObject dialogueText;
    private Text currentDialogueText;
    private GameObject buttonText;
    private GameObject dialogueWindow;
    private GameObject button;

    private int textIndex = 0;
    // Start is called before the first frame update
    private void Start()
    {
        dialogueText = dialogue.transform.Find("dialogueText").GameObject();
        buttonText = dialogue.transform.Find("buttonText").GameObject();
        dialogueWindow = dialogue.transform.Find("dialogueWindow").GameObject();
        button = dialogue.transform.Find("button").GameObject();
        currentDialogueText = dialogueText.GetComponent<Text>();
        dialogueText.SetActive(false);
        buttonText.SetActive(false);
        dialogueWindow.SetActive(false);
        button.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent<HeroKnight>(out var _)) return;
        buttonText.SetActive(true);
        button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<HeroKnight>(out var _)) return;
        buttonText.SetActive(false);
        button.SetActive(false);
        dialogueText.SetActive(false);
        dialogueWindow.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext value)
    {
        if (!buttonText.activeSelf || !button.activeSelf || PausedMenu.menuOpened) return;
        if (!value.performed) return;
        dialogueText.SetActive(true);
        dialogueWindow.SetActive(true);
        Interact?.Invoke();
        if (textIndex >= texts.Length) return; 
        currentDialogueText.text = texts[textIndex++];

    }
}
