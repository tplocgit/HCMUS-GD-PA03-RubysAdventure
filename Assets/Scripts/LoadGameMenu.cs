using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class LoadGameMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdownTypes;
    [SerializeField] private TMP_Dropdown dropdownSaves;
    [SerializeField] private GameObject loadingAlert;
    [SerializeField] public static Dictionary<int, string> indexToExt = new Dictionary<int, string>()
    {
        { 0, "hd" },
        { 1, "json"},
    };
    private int prevSelected = 0;

    // Start is called before the first frame update
    void Start()
    {
        InitDropdownSave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitDropdownSave()
    {
        string[] saves = ReadAvailableSaveNames(Application.persistentDataPath, indexToExt[dropdownTypes.value]);
        this.prevSelected = dropdownTypes.value;
        dropdownSaves.ClearOptions();
        SetOptionForDropdown(dropdownSaves, saves);
    }

    private string[] ReadAvailableSaveNames(string path, string ext)
    {
        return Directory.GetFiles(path, $"*.{ext}");
    }

    private void SetOptionForDropdown(TMP_Dropdown dropdown, string[] options)
    {
        List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
        foreach(var option in options)
        {
            optionDataList.Add(new TMP_Dropdown.OptionData() { text = option.Remove(0, Application.persistentDataPath.Length + 1) });
        }

        dropdown.AddOptions(optionDataList);
    }

    public void OnDropDownTypesChange(int typeIndex)
    {
        if(prevSelected == typeIndex) return;
        prevSelected = typeIndex;
        loadingAlert.SetActive(true);
        string[] saves = ReadAvailableSaveNames(Application.persistentDataPath, indexToExt[typeIndex]);
        dropdownSaves.ClearOptions();
        SetOptionForDropdown(dropdownSaves, saves);
        loadingAlert.SetActive(false);
    }

    public void SetLoadData()
    {
        MyGameManager.Instance.savePath = Path.Combine(Application.persistentDataPath, dropdownSaves.captionText.text);
        MyGameManager.Instance.saveType = dropdownSaves.value;
        MyGameManager.Instance.isNewGame = false;
    }
}
