using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PrefabOfItemScript : MonoBehaviour
{
    GameManager gameManager;

    public ItemScript item;

    public ScrollView imagesScrollView;
    public InputField nameItemInputField;
    public InputField venderCodeInputField;
    public InputField firstTypeItemInputField;
    public InputField firstCostItemInputField;
    public InputField secondCostItemInputField;
    public InputField sizeItemInputField;
    public InputField briefInfoOfItemInputField;
    public InputField compositionOfItemInputField;
    public InputField manufacturingFirmInputField;
    public InputField descriptionOfItemInputField;
    public UnityEngine.UI.Toggle isThereItemOnShopToggle;

    string lastNameItem = "";
    string lastVenderCode = "";
    string lastFirstTypeItem = "";
    string lastFirstCostItem = "";
    string lastSecondCostItem = "";
    string lastSizeOfItem = "";
    string lastBriefInfoOfItem = "";
    string lastCompositionOfItem = "";
    string lastManufacturingFirm = "";
    string lastDescriptionOfItem = "";
    [SerializeField]
    bool lastIsThereItemOnShopToggle = false;

    private void InitializationAllItems()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //imagesScrollView = transform.GetChild(1).GetComponent<ScrollView>();
        nameItemInputField = transform.GetChild(2).GetComponent<InputField>();
        venderCodeInputField = transform.GetChild(3).GetComponent<InputField>();
        firstTypeItemInputField = transform.GetChild(4).GetComponent<InputField>();
        firstCostItemInputField = transform.GetChild(5).GetComponent<InputField>();
        secondCostItemInputField = transform.GetChild(6).GetComponent<InputField>();
        sizeItemInputField = transform.GetChild(7).GetComponent<InputField>();
        briefInfoOfItemInputField = transform.GetChild(8).GetComponent<InputField>();
        compositionOfItemInputField = transform.GetChild(9).GetComponent<InputField>();
        manufacturingFirmInputField = transform.GetChild(10).GetComponent<InputField>();
        descriptionOfItemInputField = transform.GetChild(11).GetComponent<InputField>();
        isThereItemOnShopToggle = transform.GetChild(12).GetComponent<UnityEngine.UI.Toggle>();
    }

    private void Start()
    {
        InitializationAllItems();

        LoadInfoFromItemObject();
    }

    private void CheckChanges()
    {
        if (lastNameItem != nameItemInputField.text)
        {
            lastNameItem = nameItemInputField.text;
            item.nameItem = nameItemInputField.text;

            SaveChanges();
        }
        if (lastVenderCode != venderCodeInputField.text)
        {
            lastVenderCode = venderCodeInputField.text;
            item.vendorCode = venderCodeInputField.text;

            SaveChanges();
        }
        if (lastFirstTypeItem != firstTypeItemInputField.text)
        {
            lastFirstTypeItem = firstTypeItemInputField.text;
            item.firstTypeItem = firstTypeItemInputField.text;

            SaveChanges();
        }
        if (lastFirstCostItem != firstTypeItemInputField.text)
        {
            lastFirstCostItem = firstTypeItemInputField.text;
            float.TryParse(firstCostItemInputField.text, out item.firstCostItem);

            SaveChanges();
        }
        if (lastSecondCostItem != secondCostItemInputField.text)
        {
            lastSecondCostItem = secondCostItemInputField.text;
            float.TryParse(secondCostItemInputField.text, out item.secondCostItem);

            SaveChanges();
        }
        if (lastSecondCostItem != secondCostItemInputField.text)
        {
            lastSizeOfItem = sizeItemInputField.text;
            item.sizeOfItem = ConvertStringToSizeMass();

            SaveChanges();
        }
        if (lastBriefInfoOfItem != briefInfoOfItemInputField.text)
        {
            lastBriefInfoOfItem = briefInfoOfItemInputField.text;
            item.briefInfoOfItem = briefInfoOfItemInputField.text;

            SaveChanges();
        }
        if (lastCompositionOfItem != compositionOfItemInputField.text)
        {
            lastCompositionOfItem = compositionOfItemInputField.text;
            item.compositionOfItem = compositionOfItemInputField.text;

            SaveChanges();
        }
        if (lastManufacturingFirm != manufacturingFirmInputField.text)
        {
            lastManufacturingFirm = manufacturingFirmInputField.text;
            item.manufacturingFirm = manufacturingFirmInputField.text;

            SaveChanges();
        }
        if (lastDescriptionOfItem != descriptionOfItemInputField.text)
        {
            lastDescriptionOfItem = descriptionOfItemInputField.text;
            item.descriptionOfItem = descriptionOfItemInputField.text;

            SaveChanges();
        }
        if (lastIsThereItemOnShopToggle != isThereItemOnShopToggle.isOn)
        {
            lastIsThereItemOnShopToggle = isThereItemOnShopToggle.isOn;
            item.isThereItemOnStore = lastIsThereItemOnShopToggle;

            SaveChanges();
        }
    }

    private void SaveChanges()
    {
#if UNITY_EDITOR
        // записываем изменения над item в Undo
        Undo.RecordObject(item, "Test Scriptable Editor Modify");
        // помечаем тот самый item как "грязный" и сохраняем.
        EditorUtility.SetDirty(item);
#endif
    }

    public void Update()
    {
        CheckChanges();
    }

    private void LoadInfoFromItemObject()
    {
        nameItemInputField.text = item.nameItem;
        venderCodeInputField.text = item.vendorCode;
        firstTypeItemInputField.text = item.firstTypeItem;
        firstCostItemInputField.text = item.firstCostItem.ToString();
        secondCostItemInputField.text = item.secondCostItem.ToString();
        sizeItemInputField.text = ConvertSizeMassToString();
        briefInfoOfItemInputField.text = item.briefInfoOfItem;
        compositionOfItemInputField.text = item.compositionOfItem;
        manufacturingFirmInputField.text = item.manufacturingFirm;
        descriptionOfItemInputField.text = item.descriptionOfItem;
        if(item.isThereItemOnStore == true)
        {
            isThereItemOnShopToggle.isOn = true;
        }
        else
        {
            isThereItemOnShopToggle.isOn = false;
        }

        if (gameManager.scene == "SHOP")
        {
            nameItemInputField.interactable = false;
            venderCodeInputField.interactable = false;
            firstTypeItemInputField.interactable = false;
            firstCostItemInputField.interactable = false;
            secondCostItemInputField.interactable = false;
            sizeItemInputField.interactable = false;
            briefInfoOfItemInputField.interactable = false;
            compositionOfItemInputField.interactable = false;
            manufacturingFirmInputField.interactable = false;
            descriptionOfItemInputField.interactable = false;
            isThereItemOnShopToggle.interactable = false;
        }
    }

    private string ConvertSizeMassToString()
    {
        if (item.sizeOfItem != null)
        {
            string buffString = "";
            for (int i = 0; i < item.sizeOfItem.Length; i++)
            {
                buffString += item.sizeOfItem[i] + " ";
            }
            return buffString;
        }
        return "";
    }

    private int[] ConvertStringToSizeMass()
    {
        string buffString = sizeItemInputField.text;
        string[] buffList;
        int[] buffMass;

        buffList = buffString.Split(new char[] { ' ' });

        buffMass = new int[buffList.Length - 1];

        for (int i = 0; i < buffList.Length - 1; i++)
        {
            int.TryParse(buffList[i], out buffMass[i]);
        }

        return buffMass;
    }

    private void SaveAllChanges()
    {
        item.nameItem = nameItemInputField.text;
        item.vendorCode = venderCodeInputField.text;
        item.firstTypeItem = firstTypeItemInputField.text;

        float.TryParse(firstCostItemInputField.text, out item.firstCostItem);
        float.TryParse(secondCostItemInputField.text, out item.secondCostItem);

        item.sizeOfItem = ConvertStringToSizeMass();

        item.briefInfoOfItem = briefInfoOfItemInputField.text;
        item.compositionOfItem = compositionOfItemInputField.text;
        item.manufacturingFirm = manufacturingFirmInputField.text;
        item.descriptionOfItem = descriptionOfItemInputField.text;

        item.isThereItemOnStore = isThereItemOnShopToggle.isOn;
    }
}
