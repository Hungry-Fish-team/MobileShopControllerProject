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
                    LoadObjectToType(loadScript.typeOfItems.typesOfItem);
                    typeOfItemsInputField.interactable = false;
                    saveButton.interactable = false;
                }
            }
            else if (lastScene == "WORKPLACE")
            {
                if (isPrefabsLoaded == false)
                {
                    LoadObjectToType(gameManager.typesOfItem.typesOfItem);
                    lastTypesOfItems = typeOfItemsInputField.text;
                    typeOfItemsInputField.interactable = true;
                    saveButton.interactable = true;
                }

                CheckChanges();
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

        gameManager.typesOfItem.typesOfItem.Clear();
        for (int i = 0; i < newString.Length; i++)
        { 
            gameManager.typesOfItem.typesOfItem.Add(newString[i]);
        }

        SaveChanges();
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
