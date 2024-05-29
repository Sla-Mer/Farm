using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject newGameMenu;
    public InputFieldValidator inputFieldValidator;

    private void Start()
    {
        inputFieldValidator = GetComponent<InputFieldValidator>();
       
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        mainMenu.SetActive(true);
        newGameMenu.SetActive(false);
    }

    public void ShowNewGameMenu()
    {
        mainMenu.SetActive(false);
        newGameMenu.SetActive(true);
    }

    public void StartNewGame()
    {
        string playerName = inputFieldValidator.GetValidatedPlayerName();
        int worldSeed = inputFieldValidator.GetValidatedSeed();

        // Сохранение данных игрока и сида
        GameData gameData = new GameData(playerName, worldSeed.ToString(), true);
        SaveSystem.SaveGame(gameData);

        // Запуск сцены игры
        SceneManager.LoadScene("SampleScene");
    }

    public void ContinueGame()
    {
        GameData gameData = new GameData(false);
        SaveSystem.SaveGame(gameData);

        SceneManager.LoadScene("SampleScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
