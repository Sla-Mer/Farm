using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class InputFieldValidator : MonoBehaviour
{
    public TMP_InputField seedInputField;
    public TMP_InputField playerNameInputField;
    public int minValue = 1;
    public int maxValue = 5000;
    public string defaultPlayerName = "farmer";

    private void Start()
    {
        if (seedInputField != null)
        {
            seedInputField.onEndEdit.AddListener(ValidateSeedInput);
        }

        if (playerNameInputField != null)
        {
            playerNameInputField.onEndEdit.AddListener(ValidatePlayerNameInput);
        }
    }

    private void ValidateSeedInput(string input)
    {
        if (int.TryParse(input, out int value))
        {
            if (value < minValue)
            {
                value = minValue;
            }
            else if (value > maxValue)
            {
                value = maxValue;
            }

            seedInputField.text = value.ToString();
        }
        else
        {
            seedInputField.text = minValue.ToString(); 
        }
    }

    private void ValidatePlayerNameInput(string input)
    {
        if (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, "^[a-zA-Z]+$"))
        {
            playerNameInputField.text = defaultPlayerName;
        }
    }

    public string GetValidatedPlayerName()
    {
        string playerName = playerNameInputField.text;
        if (string.IsNullOrEmpty(playerName) || !Regex.IsMatch(playerName, "^[a-zA-Z]+$"))
        {
            return defaultPlayerName;
        }
        return playerName;
    }

    public int GetValidatedSeed()
    {
        if (int.TryParse(seedInputField.text, out int value))
        {
            if (value < minValue)
            {
                return minValue;
            }
            else if (value > maxValue)
            {
                return maxValue;
            }
            return value;
        }
        return minValue;  
    }
}
