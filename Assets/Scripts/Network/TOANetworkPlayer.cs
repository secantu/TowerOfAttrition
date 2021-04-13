using System.Collections;
using System.Collections.Generic;
using Mirror;
using TMPro;
using UnityEngine;

public class TOANetworkPlayer : NetworkBehaviour
{
    [SerializeField] [Tooltip("Displays the players displayName")]
    private TMP_Text displayNameText = null;

    [SerializeField] [Tooltip("Mesh to change colors of")]
    private Renderer displayColorRenderer = null;

    [SerializeField] [Tooltip("How short the players name can be")]
    private int minimumWordLength = 2;

    [SerializeField] [Tooltip("How long the players name can be")]
    private int maxWordLength = 15;

    [SerializeField] [Tooltip("Are white spaces allowed in players name")]
    private bool whiteSpacesAllowedInName = false;

    [SerializeField] [Tooltip("Are numbers allowed in players name")]
    private bool numbersAllowedInName = false;

    [SerializeField] [Tooltip("List of Special Characters not allowed in players name")]
    private List<string> badSpecialChars;

    [SerializeField] [Tooltip("List of characters that are numbers")]
    private List<string> numberChars;

    [SerializeField] [Tooltip("File that has all the black listed words in it")]
    private TextAsset blackListTextFile;

    //stores the blackListTextFile information into a single string that the developer can parse through
    private string fileString;

    [SerializeField] [Tooltip("Words that are not allowed to be used")]
    private List<string> blackListedWords;


    [SyncVar(hook = nameof(HandleDisplayNameUpdate))]
    [SerializeField] [Tooltip("Player Name displayed to all players")]
    private string displayName = "Missing Name";

    [SyncVar(hook = nameof(HandleDisplayColorUpdate))]
    [SerializeField] [Tooltip("Player color displayed to all players")]
    private Color displayColor = Color.black;

    private void Start()
    {
        //read from Black List text file and make a list to compare to
        fileString = blackListTextFile.text;
        blackListedWords = new List<string>();
        blackListedWords.AddRange(fileString.Split("\n"[0]));
    }

    #region Server

    [Server]
    public void SetDisplayName(string newDisplayName)
    {
        displayName = newDisplayName;
    }

    [Server]
    public void SetDisplayColor(Color newDisplayColor)
    {
        displayColor = newDisplayColor;
    }

    [Command]
    private void CmdSetDisplayName(string newDisplayName)
    {
        //check if newDisplayName length is the proper length. return if it is not
        if (newDisplayName.Length < minimumWordLength || newDisplayName.Length > maxWordLength) return;

        //loop through black listed words and compare to newDisplayName. Return if it is found
        foreach(string word in blackListedWords)
        {
            if (word == newDisplayName) return;
        }

        //if white spaces are not allowed and newDisplayName contains a white space, return
        if(!whiteSpacesAllowedInName)
        {
            if (newDisplayName.Contains(" ")) return;
        }

        //if numbers are not allowed in the display name and newDisplayName contains a number, return
        if (!numbersAllowedInName)
        {
            foreach (string number in numberChars)
            {
                if (newDisplayName.Contains(number)) return;
            }
        }

        //if newDisplayName contains a special character not allowed, return
        foreach(string badSpecialChar in badSpecialChars)
        {
            if(newDisplayName.Contains(badSpecialChar)) return;
        }

        //if all name validation has passed, set it
        RpcLogNewName(newDisplayName);
        SetDisplayName(newDisplayName);
    }

    #endregion

    #region Client
    private void HandleDisplayNameUpdate(string oldName, string newName)
    {
        displayNameText.text = newName;
    }

    private void HandleDisplayColorUpdate(Color oldColor, Color newColor)
    {
        displayColorRenderer.material.SetColor("_Color", newColor);
    }

    [ContextMenu("Set Player Name")]
    private void SetPlayerName()
    {
        CmdSetDisplayName("new-guy");
    }

    [ClientRpc]
    private void RpcLogNewName(string newDisplayName)
    {
        Debug.Log(newDisplayName);
    }

    #endregion
}
