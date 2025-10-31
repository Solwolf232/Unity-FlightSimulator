using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class UiManagerScript : MonoBehaviour
{

    [Header("Pc Canvases")]
    [SerializeField] GameObject LevelDiffCanvas;
    [SerializeField] GameObject LobbyCanvas;
    [SerializeField] GameObject PhasesCanvas;

    [Header("Vr Canvases")]
    [SerializeField] GameObject Lobby_Vr_Canvas;
    [SerializeField] GameObject Level_Diff_Vr_Canvas;

    [Header("Data")]
    [SerializeField] LevelDiffData LevelDiffcultyData;
    [SerializeField] PlayerDataScript Player_Data;

    [Header("Input Fielnd")]
    [SerializeField] TMP_InputField nameInputField;
    [SerializeField] TMP_InputField AgeInputField;




    [Header("Private Vals")]
    private bool is_on_Vr;

    private void Start()
    {
        if(FindObjectOfType<PlayerMovement_Vr>() != null)
        {
            is_on_Vr = true;

            LobbyCanvas.SetActive(false);
            Lobby_Vr_Canvas.SetActive(true);
        }
        else
        {
            is_on_Vr = false;

            LobbyCanvas.SetActive(true) ;
            Lobby_Vr_Canvas.SetActive(false);
        }
    }

    #region Lobby Buttons



    public void OnStartButton()
    {
        if (!is_on_Vr)
        {
            LevelDiffCanvas.SetActive(true);
            LobbyCanvas.SetActive(false);
        }
        else
        {
            Level_Diff_Vr_Canvas.SetActive(true);
            Lobby_Vr_Canvas.SetActive(false) ;
        }

    }


    public void OnQuitButton()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }

    #endregion

    #region Level Diffuclty Buttons

    public void OnEasyButton()
    {
        LevelDiffcultyData.currentLevelDiff = LevelDiff.Easy;
        LevelDiffCanvas.SetActive(false);
        SavePlayerData();


        LoadScene("FlightScene");


        


    }

    public void OnMediumButton()
    {
        LevelDiffcultyData.currentLevelDiff = LevelDiff.Medium;
        LevelDiffCanvas.SetActive(false);
        SavePlayerData();


        LoadScene("FlightScene");



    }

    public void OnHardButton()
    {
        LevelDiffcultyData.currentLevelDiff = LevelDiff.Hard;
        LevelDiffCanvas.SetActive(false);


        SavePlayerData();

        LoadScene("FlightScene");



    }
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    } 



    #endregion

    #region Data
   
    private void SavePlayerData()
    {
        string name = nameInputField.text;
        int age;

        bool isAgeValid = int.TryParse(AgeInputField.text, out age) && CheckAge(age);
        bool isNameValid = CheckName(name);

        if (isAgeValid && isNameValid)
        {
            Player_Data.Player_Name = name;
            Player_Data.Player_Age = age;
            Debug.Log("Player data saved!");
        }
        else
        {
            Debug.LogWarning("Invalid input: " +
                             (isNameValid ? "" : "Name ") +
                             (!isAgeValid ? "Age" : ""));
        }
    }

    private bool CheckAge(int age)
    {
        if(age < 0)
            return false;
        return true;
    }

    private bool CheckName(string name)
    {
        return !string.IsNullOrWhiteSpace(name);

    }


    #endregion
}
