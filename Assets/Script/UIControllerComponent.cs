using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class UIControllerComponent : MonoBehaviour
{
    public GameControllerComponent gameController;

    public GameObject MainMenuUI;

    public GameObject SettingsMenuUI;

    public Slider XSizeSlider;

    public Slider YSizeSlider;

    public Slider ColorSlider;


    // Start is called before the first frame update
    void Start()
    {
        XSizeSlider.minValue = gameController.minGameSize.x;
        XSizeSlider.maxValue = gameController.maxGameSize.x;

        YSizeSlider.minValue = gameController.minGameSize.y;
        YSizeSlider.maxValue = gameController.maxGameSize.y;

        ColorSlider.minValue = gameController.minimalColorCount;
        ColorSlider.maxValue = ColorCollection.Instance.colors.Length;

        DisableAllWindowUI();
        ShowMainMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartRandomGameButtonPressed()
    {
        DisableAllWindowUI();
        gameController.GenerateRandomGame();
    }

    public void StartGameWithSettingsButtonPressed()
    {
        DisableAllWindowUI();

        Vector2Int size = new Vector2Int((int)XSizeSlider.value, (int)YSizeSlider.value);

        gameController.GenerateGame(size, (int)ColorSlider.value);
    }

    void DisableAllWindowUI()
    {
        MainMenuUI.SetActive(false);
        SettingsMenuUI.SetActive(false);
    }

    public void ShowMainMenu()
    {
        DisableAllWindowUI();
        MainMenuUI.SetActive(true);
    }

    public void ShowSettingMenu()
    {
        DisableAllWindowUI();
        SettingsMenuUI.SetActive(true);
    }



}
