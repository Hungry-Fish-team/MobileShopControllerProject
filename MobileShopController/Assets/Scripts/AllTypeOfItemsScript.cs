using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AllTypeOfItemsScript : MonoBehaviour
{
    GameManager gameManager;
    LoadScript loadScript;

    Button saveButton;

    public InputField typeOfItemsInputField;

    string lastScene = "";

    public bool isPrefabsLoaded;

    string lastTypesOfItems = "";

    public int functionNumber;

    private void InitializationAllObjects()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        loadScript = GameObject.Find("GameManager").GetComponent<LoadScript>();

        typeOfItemsInputField = transform.gameObject.GetComponent<InputField>();

        saveButton = GameObject.Find("CreateNewTypeButton").GetComponent<Button>();
    }

    private void Start()
    {
        InitializationAllObjects();
    }

    private void Update()
    {
        if (loadScript.isFileLoaded == true)
        {
            if (lastScene != gameManager.scene)
            {
                ClearAllTypesFromList();
                lastScene = gameManager.scene;
            }

            if (lastScene == "SHOP")
            {
                if (isPrefabsLoaded == false)
                {
                    LoadObjectToTypeWithOwnInfo();
                    typeOfItemsInputField.interactable = false;
                    saveButton.interactable = false;
                }
            }
            else if (lastScene == "WORKPLACE")
            {
                if (isPrefabsLoaded == false)
                {
                    LoadObjectToTypeWithOwnInfo();
                    lastTypesOfItems = typeOfItemsInputField.text;
                    typeOfItemsInputField.interactable = true;
                    saveButton.interactable = true;
                }

                CheckChanges();
            }
        }
    }

    private void LoadObjectToTypeWithOwnInfo()
    {
        if (lastScene == "SHOP")
        {
            if (functionNumber == 0)
            {
                LoadObjectToType(loadScript.typeOfItems.typesOfItem);
            }
            else if (functionNumber == 1)
            {
                LoadObjectToType(loadScript.typeOfItems.nameOfCategoryForMainWindow);
            }
            else if (functionNumber == 2)
            {
                LoadObjectToType(loadScript.typeOfItems.secondNameOfCategoryForMainWindow);
            }
        }
        else if (lastScene == "WORKPLACE")
        {
            if (functionNumber == 0)
            {
                LoadObjectToType(gameManager.typesOfItem.typesOfItem);
            }
            else if (functionNumber == 1)
            {
                LoadObjectToType(gameManager.typesOfItem.nameOfCategoryForMainWindow);
            }
            else if (functionNumber == 2)
            {
                LoadObjectToType(gameManager.typesOfItem.secondNameOfCategoryForMainWindow);
            }
        }
    }

    private void CheckChanges()
    {
        if (lastTypesOfItems != typeOfItemsInputField.text)
        {
            lastTypesOfItems = typeOfItemsInputField.text;

            LoadTypeToObject();
        }
    }

    private void SaveChanges()
    {
#if UNITY_EDITOR
        // записываем изменения над loadScript в Undo
        Undo.RecordObject(gameManager.typesOfItem, "Test Scriptable Editor Modify");
        // помечаем тот самый loadScript как "грязный" и сохраняем.
        EditorUtility.SetDirty(gameManager.typesOfItem);
#endif
    }

    private void LoadObjectToType(List<String> typesOfItems)
    {
        ClearAllTypesFromList();

        for (int i = 0; i < typesOfItems.Count - 1; i++)
        {
            typeOfItemsInputField.text += typesOfItems[i] + " ";
        }
        typeOfItemsInputField.text += typesOfItems[typesOfItems.Count - 1];

        lastTypesOfItems = typeOfItemsInputField.text;

        isPrefabsLoaded = true;
    }

    private void LoadTypeToObject()
    {
        string[] newString;

        newString = typeOfItemsInputField.text.Split(new char[] { ' ' });

        LoadTypeToObjectWithOwnFunc(newString);

        SaveChanges();
    }

    private void LoadTypeToObjectWithOwnFunc(string[] newString)
    {
        if(functionNumber == 0)
        { 
            gameManager.typesOfItem.typesOfItem.Clear();
            for (int i = 0; i < newString.Length; i++)
            {
                gameManager.typesOfItem.typesOfItem.Add(newString[i]);
            }
        }
        else if (functionNumber == 1)
        {
            gameManager.typesOfItem.nameOfCategoryForMainWindow.Clear();
            for (int i = 0; i < newString.Length; i++)
            {
                gameManager.typesOfItem.nameOfCategoryForMainWindow.Add(newString[i]);
            }
        }
        else if (functionNumber == 2)
        {
            gameManager.typesOfItem.secondNameOfCategoryForMainWindow.Clear();
            for (int i = 0; i < newString.Length; i++)
            {
                gameManager.typesOfItem.secondNameOfCategoryForMainWindow.Add(newString[i]);
            }
        }
    }

    private void ClearAllTypesFromList()
    {
        if (typeOfItemsInputField.text != "")
        {
            Debug.Log(typeOfItemsInputField.text);
            typeOfItemsInputField.text = "";
        }
        isPrefabsLoaded = false;
    }

    public void SaveAllTypes()
    {
        LoadTypeToObject();
    }
}
