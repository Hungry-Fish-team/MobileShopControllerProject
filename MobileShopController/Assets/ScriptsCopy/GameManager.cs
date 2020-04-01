using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;

public class GameManager : MonoBehaviour
{
    LoadScript loadScript;

    public List<ItemScript> searchItems;

    public string[] allFiles;
    public List<ItemScript> items;
    public TypeOfItemScript typesOfItem;

    public string scene = "SHOP";

    public GameObject prefabOfItem;
    public GameObject searchInputField;
    
    public bool isPrefabsLoaded = false;

    private void InitializationAllObjects()
    {
        loadScript = GameObject.Find("GameManager").GetComponent<LoadScript>();
        searchInputField = GameObject.Find("SearchInputField");

        GameObject changeSceneButton = GameObject.Find("ChangeSceneButton");
        changeSceneButton.transform.GetChild(0).GetComponent<Text>().text = scene;
    }

    private void Start()
    {
        InitializationAllObjects();

        loadScript.LoadFilesFromServer();
        LoadFromItemsFromPath();
    }

    private void LoadFromItemsFromPath()
    {
        allFiles = Directory.GetFiles("Assets/Resources/AllItems");
        for(int i = 0; i < allFiles.Length; i++)
        {
            allFiles[i] = Path.GetFileNameWithoutExtension(allFiles[i]);
        }

        for(int i = 0; i < (allFiles.Length / 2) - 1; i++)
        {
            items.Add(Resources.Load("allItems/" + allFiles[i * 2]) as ItemScript);
        }

        typesOfItem = Resources.Load("allItems/" + allFiles[allFiles.Length - 2]) as TypeOfItemScript;
    }

    private void Update()
    {
        if (searchInputField.GetComponent<InputField>().text == "")
        {
            if (scene == "WORKPLACE")
            {
                if (loadScript.isFileLoaded == true)
                {
                    LoadObjectToItems(items);
                }
            }

            if (scene == "SHOP")
            {
                if (loadScript.isFileLoaded == true)
                {
                    LoadObjectToItems(loadScript.items);
                }
            }
        }

        SearchFunc();
    }

    private void LoadObjectToItems(List<ItemScript> items)
    {
        if (isPrefabsLoaded == false)
        {
            DestroyAllLoadedObjectToItems();

            GameObject prefabItemsScrollView = GameObject.Find("PrefabItemsScrollView");

            if (items != null) {
                for (int i = 0; i < items.Count; i++)
                {
                    GameObject newPrefabOfItem = Instantiate(prefabOfItem, prefabItemsScrollView.transform.GetChild(0).GetChild(0));
                    newPrefabOfItem.GetComponent<PrefabOfItemScript>().item = items[i];
                }
            }
            isPrefabsLoaded = true;
        }
    }

    private void DestroyAllLoadedObjectToItems()
    {
        GameObject prefabItemsScrollView = GameObject.Find("PrefabItemsScrollView");

        for (int i = 0; i < prefabItemsScrollView.transform.GetChild(0).GetChild(0).childCount; i++)
        {
            Destroy(prefabItemsScrollView.transform.GetChild(0).GetChild(0).GetChild(i).gameObject);
        }
        isPrefabsLoaded = false;
    }

    public void CreateNewItem()
    {
        Transform createNewItemButtonInputField = GameObject.Find("CreateNewItemButton").transform;

        if (scene == "WORKPLACE")
        {
            if (createNewItemButtonInputField.GetChild(1).GetComponent<InputField>().text != "")
            {
                if(FindNewNameOfItem(createNewItemButtonInputField.GetChild(1).GetComponent<InputField>().text) != true){
                    ItemScript newItem = ScriptableObject.CreateInstance<ItemScript>();

                    newItem.name = createNewItemButtonInputField.GetChild(1).GetComponent<InputField>().text;
                    newItem.vendorCode = createNewItemButtonInputField.GetChild(1).GetComponent<InputField>().text;

#if UNITY_EDITOR
                    AssetDatabase.CreateAsset(newItem, "Assets/Resources/AllItems/" + newItem.name + ".asset");
#endif

                    Debug.Log("CreateNewItem");

                    items.Add(newItem);

                    isPrefabsLoaded = false;
                    LoadObjectToItems(items);
                }
                else
                Debug.Log("Такой код товара уже есть");
            }
            else
            Debug.Log("Пустой код товара");
        }
        else
        Debug.Log("Смени режим просмотра (SHOP -> WORKPLACE)");
    }

    private bool FindNewNameOfItem(string newName)
    {
        bool isThisItemCreated = false;

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].vendorCode == newName)
            {
                isThisItemCreated = true;
                break;
            }
        }
        if(isThisItemCreated == false)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ChangeScene()
    {
        GameObject changeSceneButton = GameObject.Find("ChangeSceneButton");

        if(changeSceneButton.transform.GetChild(0).GetComponent<Text>().text == "SHOP")
        {
            changeSceneButton.transform.GetChild(0).GetComponent<Text>().text = "WORKPLACE";
            scene = "WORKPLACE";
        }
        else
        {
            changeSceneButton.transform.GetChild(0).GetComponent<Text>().text = "SHOP";
            scene = "SHOP";
        }

        isPrefabsLoaded = false;
    }

    string lastSearchString = "";

    private void SearchFunc()
    {
        if (searchInputField.GetComponent<InputField>().text != "")
        {
            if (lastSearchString != searchInputField.GetComponent<InputField>().text)
            {
                if (scene == "SHOP") {
                    FindItemFromFile(searchInputField.GetComponent<InputField>().text);
                    isPrefabsLoaded = false;
                    LoadObjectToItems(searchItems);
                }
                else if (scene == "WORKPLACE")
                {
                    FindItemFromDisk(searchInputField.GetComponent<InputField>().text);
                    isPrefabsLoaded = false;
                    LoadObjectToItems(searchItems);
                }

                lastSearchString = searchInputField.GetComponent<InputField>().text;
            }
            isPrefabsLoaded = false;
        }
    }

    private void FindItemFromFile(string nameOfItem)
    {
        ClearSearchItems();
        
        foreach (ItemScript item in loadScript.items)
        {
            if (item.name == nameOfItem)
            {
                searchItems.Add(item);
            }
        }
    }

    private void FindItemFromDisk(string nameOfItem)
    {
        ClearSearchItems();

        foreach (ItemScript item in items)
        {
            if (item.name == nameOfItem)
            {
                searchItems.Add(item);
            }
        }
    }

    private void ClearSearchItems()
    {
        if (searchItems != null) {
            searchItems.Clear();
        }
    }
}
