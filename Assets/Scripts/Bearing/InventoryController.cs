using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using UnityEngine.SceneManagement;

public class InventoryController : MonoBehaviour
{
    public Item item;
    public Sprite emptySprite;
    [SerializeField] private GameObject mainStat;


    [SerializeField] private Sprite[] imageType;

    [SerializeField] private List<GameObject> slotsPlayer = new List<GameObject>();
    [SerializeField] private List<bool> isBusySlotPlayer = new List<bool>();
    [SerializeField] private List<Item> playersItem = new List<Item>();

    [SerializeField] private List<GameObject> slotsReward = new List<GameObject>();
    [SerializeField] private List<Item> itemsReward = new List<Item>();

    /// ////////////////////////////////////////////////////////////////////////////////////////
    [SerializeField] private List<Item> itemsSword = new List<Item>();
    [SerializeField] private List<Item> itemsChest = new List<Item>();
    [SerializeField] private List<Item> itemsHelmet = new List<Item>();

    [SerializeField] private List<GameObject> slotsSword = new List<GameObject>();
    [SerializeField] private List<GameObject> slotsChest = new List<GameObject>();
    [SerializeField] private List<GameObject> slotsHelmet = new List<GameObject>();

    [SerializeField] private List<bool> slotsSwordBusy = new List<bool>();
    [SerializeField] private List<bool> slotsChestBusy = new List<bool>();
    [SerializeField] private List<bool> slotsHelmetBusy = new List<bool>();

    private int selectedItemIndex;
    private int selectedItemType;
    private Item selectedItem; 

    private Item tmpItem;
    private Color tmpColor;
    private Sprite tmpSprite;

    public GameObject inventory;

    public Sprite takenItem;

    [Header("For buttons")]
    public GameObject itemListImg;
    public GameObject buttonsForGame;
    public GameObject buttonsForMenu;
    public GameObject buttonTake;
    public GameObject levelComplImg;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {

            slotsSwordBusy[i] = false;
            slotsChestBusy[i] = false;
            slotsHelmetBusy[i] = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                isBusySlotPlayer[i] = false;
            }
        }
        Load();

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            buttonsForMenu.SetActive(true);
            buttonsForGame.SetActive(false);
        }
        else
        {
            buttonsForMenu.SetActive(false);
            buttonsForGame.SetActive(true);


            inventory.SetActive(false); // приходится скрывать в начале инвентарь чтобы работала делегата 
        }
        
        

        LevelController.LevelEnd += LevelEnd;

        
    }

    void OnApplicationQuit()
    {
        Save();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Save");
            Save();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Load");
            Load();
        }
        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Debug.Log("Level Complete");
                levelComplImg.SetActive(true);
            }
        }

    }

   

    void LevelEnd(int level)
    {
        item.Constructor(Random.Range(1, 4), Random.Range(1, 4), "Attack", Random.Range(0, 1000));          // делегата
        selectedItem = item;
        AddDataReward(item);       
    }

    void ButtonReward()
    {
        AddData(selectedItem, true);
    }

    public void Save()
    {
        string pathSword = Application.streamingAssetsPath + "/SwordStashInfo.json";//"b:/Repo_g/2dplatform/Assets/SaveFiles/SwordStashInfo.json";
        string pathChest = Application.streamingAssetsPath + "/ChestStashInfo.json";//"b:/Repo_g/2dplatform/Assets/SaveFiles/ChestStashInfo.json";
        string pathHelmet = Application.streamingAssetsPath + "/HelmetStashInfo.json";//"b:/Repo_g/2dplatform/Assets/SaveFiles/HelmetStashInfo.json";
        string pathPlayer = Application.streamingAssetsPath + "/PlayerStashInfo.json";//"b:/Repo_g/2dplatform/Assets/SaveFiles/PlayerStashInfo.json";

        using (StreamWriter swSword = new StreamWriter(pathSword, false, Encoding.Default))
        {
            var noteSword = JsonConvert.SerializeObject(itemsSword);
            swSword.WriteLine(noteSword);
        }

        using (StreamWriter swChest = new StreamWriter(pathChest, false, Encoding.Default))
        {
            var noteChest = JsonConvert.SerializeObject(itemsChest);
            swChest.WriteLine(noteChest);
        }

        using (StreamWriter swHelmet = new StreamWriter(pathHelmet, false, Encoding.Default))
        {
            var noteHelmet = JsonConvert.SerializeObject(itemsHelmet);
            swHelmet.WriteLine(noteHelmet);
        }

        using (StreamWriter swPlayer = new StreamWriter(pathPlayer, false, Encoding.Default))
        {
            var notePlayer = JsonConvert.SerializeObject(playersItem);
            swPlayer.WriteLine(notePlayer);
        }
    }

    public void Load()
    {
        string  pathSword = Application.streamingAssetsPath + "/SwordStashInfo.json";//"b:/Repo_g/2dplatform/Assets/SaveFiles/SwordStashInfo.json";
        string pathChest = Application.streamingAssetsPath + "/ChestStashInfo.json"; //"b:/Repo_g/2dplatform/Assets/SaveFiles/ChestStashInfo.json";
        string pathHelmet = Application.streamingAssetsPath + "/HelmetStashInfo.json"; //"b:/Repo_g/2dplatform/Assets/SaveFiles/HelmetStashInfo.json";
        string pathPlayer = Application.streamingAssetsPath + "/PlayerStashInfo.json"; //"b:/Repo_g/2dplatform/Assets/SaveFiles/PlayerStashInfo.json";

        StreamReader srSword = new StreamReader(pathSword);
        StreamReader srChest = new StreamReader(pathChest);
        StreamReader srHelmet = new StreamReader(pathHelmet);
        StreamReader srPlayer = new StreamReader(pathPlayer);

        var noteSword = srSword.ReadToEnd();
        var noteChest = srChest.ReadToEnd();
        var noteHelmet = srHelmet.ReadToEnd();
        var notePlayer = srPlayer.ReadToEnd();

        itemsSword = JsonConvert.DeserializeObject<List<Item>>(noteSword);
        itemsChest = JsonConvert.DeserializeObject<List<Item>>(noteChest);
        itemsHelmet = JsonConvert.DeserializeObject<List<Item>>(noteHelmet);
        playersItem = JsonConvert.DeserializeObject<List<Item>>(notePlayer);

        for (int i = 0; i < itemsSword.Count; i++)
        {
            AddData(itemsSword[i], false);                              
        }
        for (int i = 0; i < itemsChest.Count; i++)
        {
            AddData(itemsChest[i], false);
        }
        for (int i = 0; i < itemsHelmet.Count; i++)
        {
            AddData(itemsHelmet[i], false);
        }

        for (int i = 0; i < 3; i++)
        {
            AddDataPlayer(playersItem[i]);
        }

    }

    

    public void LeaveButton()
    {
        itemListImg.SetActive(false);
    }

    public void TakeButton()
    {
        Debug.Log(selectedItem.itemType);
        
        switch(selectedItem.itemType)
        {
            case 1:
                {
                    if (itemsSword.Count < 10)
                    {
                        slotsReward[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = takenItem;
                        AddData(selectedItem, true);
                    }
                    else
                    {
                        inventory.SetActive(true);
                        Debug.Log("too many items, delete 1");
                    }
                    break;
                }
            case 2:
                {
                    if (itemsChest.Count < 10)
                    {
                        AddData(selectedItem, true);
                        slotsReward[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = takenItem;
                    }
                    else
                    {
                        inventory.SetActive(true);
                        Debug.Log("too many items, delete 1");
                    }
                    break;
                }
            case 3:
                {
                    if (itemsHelmet.Count < 10)
                    {
                        AddData(selectedItem, true);
                        slotsReward[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = takenItem;
                    }
                    else
                    {
                        inventory.SetActive(true);
                        Debug.Log("too many items, delete 1");
                    }
                    break;
                }
        }

        itemListImg.SetActive(false);
        
    }

    public void TestButton(GameObject go)
    {
        itemListImg.SetActive(true);

        switch(go.transform.parent.name)
        {
            case "Swords":
                {
                    buttonsForMenu.SetActive(true);
                    buttonsForGame.SetActive(false);
                    selectedItemIndex = slotsSword.IndexOf(go);
                    selectedItemType = 1;
                    break;
                }
            case "Chests":
                {
                    buttonsForMenu.SetActive(true);
                    buttonsForGame.SetActive(false);
                    selectedItemIndex = slotsChest.IndexOf(go);
                    selectedItemType = 2;
                    break;
                }
            case "Helmets":
                {
                    buttonsForMenu.SetActive(true);
                    buttonsForGame.SetActive(false);
                    selectedItemIndex = slotsHelmet.IndexOf(go);
                    selectedItemType = 3;
                    break;
                }
            case "LevelComplete":
                {
                    buttonsForMenu.SetActive(false);
                    buttonsForGame.SetActive(true);
                    selectedItemIndex = 0;
                    selectedItemType = selectedItem.itemType;
                    Debug.Log("level compl");
                    break;
                }
            default:
                {
                    Debug.Log(go.transform.parent.name);
                    break;
                }

        }
        Debug.Log("selectedItemIndex   " + selectedItemIndex);

        itemListImg.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = go.transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
        itemListImg.transform.Find("Icon_back").gameObject.GetComponent<Image>().color = go.GetComponent<Image>().color;
        mainStat.GetComponent<Text>().text = item.mainStatType.ToString() +"       " + item.mainStatValue.ToString();


    }

    public void AddButton()
    {

        item.Constructor(Random.Range(1, 4), Random.Range(1,4), "Attack", Random.Range(0, 1000));
        AddData(item, true);
    }

    public void UseButton()
    {
        Debug.Log(selectedItemIndex);
        itemListImg.SetActive(false);
        switch (selectedItemType)
        {
            case 1:
                {
                    
                    tmpSprite = slotsPlayer[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    tmpColor = slotsPlayer[0].GetComponent<Image>().color;
                    tmpItem = playersItem[0];
                    slotsPlayer[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsSword[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    slotsPlayer[0].GetComponent<Image>().color = slotsSword[selectedItemIndex].GetComponent<Image>().color;

                    playersItem[0] = itemsSword[selectedItemIndex];
                    
                    if (isBusySlotPlayer[0] == false)
                    {

                        Player.baseDamage += playersItem[0].mainStatValue;
                        itemsSword.RemoveAt(selectedItemIndex);
                        slotsSwordBusy[selectedItemIndex] = false;
                        slotsSword[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                        slotsSword[selectedItemIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    }
                    else
                    {
                        Player.baseDamage = Player.baseDamage - tmpItem.mainStatValue + itemsSword[selectedItemIndex].mainStatValue;
                        itemsSword[selectedItemIndex] = tmpItem;
                        slotsSword[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = tmpSprite;
                        slotsSword[selectedItemIndex].GetComponent<Image>().color = tmpColor;
                    }
                   
                    isBusySlotPlayer[0] = true;
                    
                    Debug.Log(Player.baseDamage + "после");
                    break;
                }
            case 2:
                {
                    tmpItem = playersItem[1];
                    tmpSprite = slotsPlayer[1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    tmpColor = slotsPlayer[1].GetComponent<Image>().color;

                    slotsPlayer[1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsChest[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    slotsPlayer[1].GetComponent<Image>().color = slotsChest[selectedItemIndex].GetComponent<Image>().color;
                    playersItem[1] = itemsChest[selectedItemIndex];


                    if (isBusySlotPlayer[1] == false)
                    {
                        itemsChest.RemoveAt(selectedItemIndex);
                        slotsChestBusy[selectedItemIndex] = false;
                        slotsChest[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                        slotsChest[selectedItemIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    }
                    else
                    {
                        itemsChest[selectedItemIndex] = tmpItem;
                        slotsChest[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = tmpSprite;
                        slotsChest[selectedItemIndex].GetComponent<Image>().color = tmpColor;
                    }
                    isBusySlotPlayer[1] = true;
                    break;

                }
            case 3:
                {
                    tmpItem = playersItem[2];
                    tmpSprite = slotsPlayer[2].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    tmpItem = playersItem[2];
 
                    slotsPlayer[2].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsHelmet[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    slotsPlayer[2].GetComponent<Image>().color = slotsHelmet[selectedItemIndex].GetComponent<Image>().color;
  
                    playersItem[2] = itemsHelmet[selectedItemIndex];


                    if (isBusySlotPlayer[2] == false)
                    {
                        itemsHelmet.RemoveAt(selectedItemIndex);
                        slotsHelmetBusy[selectedItemIndex] = false;
                        slotsHelmet[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                        slotsHelmet[selectedItemIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    }
                    else
                    {
                        itemsHelmet[selectedItemIndex] = tmpItem;
                        slotsHelmet[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = tmpSprite;
                        slotsHelmet[selectedItemIndex].GetComponent<Image>().color = tmpColor;
                    }
                    isBusySlotPlayer[2] = true;
                    break;
                }
            default:
                {
                    Debug.Log("Switch error");
                    break;
                }

        }

        Refresh();
    }
    public void DeleteButton()
    {
        itemListImg.SetActive(false);
        Debug.Log(selectedItemIndex);

        switch(selectedItemType)
        {
            case 1:
                {
                    itemsSword.RemoveAt(selectedItemIndex);
                    slotsSword[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                    slotsSword[selectedItemIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    slotsSwordBusy[selectedItemIndex] = false;
                    break;
                }
             case 2:
                {
                    itemsChest.RemoveAt(selectedItemIndex);
                    slotsChest[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                    slotsChest[selectedItemIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    slotsChestBusy[selectedItemIndex] = false;
                    break;
                }
            case 3:
                {
                    itemsHelmet.RemoveAt(selectedItemIndex);
                    slotsHelmet[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                    slotsHelmet[selectedItemIndex].GetComponent<Image>().color = new Color(0, 0, 0, 1f);
                    slotsHelmetBusy[selectedItemIndex] = false;
                    break;
                }
        }

        Refresh();

    }

    private void AddData(Item it, bool addInList)
    {
        Debug.Log(itemsSword.Count);    // если не вызвать количество элементов, он будет выкидывать Null reference на  if (itemsSword.Count < 10)
        switch (it.itemType)
        {
            case 1:
                {
                    if (itemsSword.Count < 10)
                    {
                        if (addInList == true)
                        {
                            itemsSword.Add(it);
                        }
                        slotsSword[itemsSword.IndexOf(it)].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[0];
                        slotsSwordBusy[itemsSword.IndexOf(it)] = true;
                        Debug.Log(itemsSword.IndexOf(it));
                        SetColorRarity(it, slotsSword, itemsSword);
                    }
                    else
                    {
                        Debug.Log("too many items");
                    }
                    break;
                }
            case 2:
                {
                    if (itemsChest.Count < 10)
                    {
                        if (addInList == true)
                        {
                            itemsChest.Add(it);
                        }
                        slotsChest[itemsChest.IndexOf(it)].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[1];
                        slotsChestBusy[itemsChest.IndexOf(it)] = true;
                        Debug.Log(itemsChest.IndexOf(it));
                        SetColorRarity(it, slotsChest, itemsChest);
                    }
                    else
                    {
                        Debug.Log("too many items");
                    }
                    break;
                }
            case 3:
                {
                    if (itemsHelmet.Count < 10)
                    {
                        
                        if (addInList == true)
                        {
                            itemsHelmet.Add(it);
                        }
                        slotsHelmet[itemsHelmet.IndexOf(it)].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[2];
                        slotsHelmetBusy[itemsHelmet.IndexOf(it)] = true;
                        Debug.Log(itemsHelmet.IndexOf(it));
                        Debug.Log(slotsHelmetBusy[itemsHelmet.IndexOf(it)]);
                        SetColorRarity(it, slotsHelmet, itemsHelmet);
                    }
                    else
                    {
                        Debug.Log("too many items");
                    }
                    break;
                }
        }
    }

    private void AddDataPlayer(Item it)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            switch (it.itemType)
            {
                case 1:
                    {
                        playersItem[0] = it;
                        slotsPlayer[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[0];
                        SetColorRarity(it, slotsPlayer, playersItem);
                        break;
                    }
                case 2:
                    {
                        playersItem[1] = it;
                        slotsPlayer[1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[1];
                        SetColorRarity(it, slotsPlayer, playersItem);
                        break;
                    }
                case 3:
                    {
                        playersItem[2] = it;
                        slotsPlayer[2].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[2];
                        SetColorRarity(it, slotsPlayer, playersItem);
                        break;
                    }
            }
        }
    }

    private void AddDataReward(Item it)
    {
        itemsReward[0] = it;
        SetColorRarity(it, slotsReward, itemsReward);
        switch (it.itemType)
        {
            case 1:
                {
                    slotsReward[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[0];
                    break;
                }
            case 2:
                {
                    slotsReward[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[1];
                    break;
                }
            case 3:
                {
                    slotsReward[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[2];
                    break;
                }
        }
    }

    private void Refresh()
    {
        for (int i = 0; i < 9; i++)
        {
            switch (selectedItemType)
            {
                case 1:
                    {
                        if (slotsSwordBusy[i] == false && slotsSwordBusy[i + 1] == true)
                        {
                            slotsSword[i].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsSword[i + 1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                            slotsSword[i].GetComponent<Image>().color = slotsSword[i + 1].GetComponent<Image>().color;

                            slotsSword[i + 1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                            slotsSword[i + 1].GetComponent<Image>().color = new Color(1, 1, 1, 1f);

                            slotsSwordBusy[i] = true;
                            slotsSwordBusy[i + 1] = false;

                            Debug.Log("swap");
                            
                        }
                        break;
                    }
                case 2:
                    {
                        if (slotsChestBusy[i] == false && slotsChestBusy[i + 1] == true)
                        {
                            slotsChest[i].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsChest[i + 1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                            slotsChest[i].GetComponent<Image>().color = slotsChest[i + 1].GetComponent<Image>().color;

                            slotsChest[i + 1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                            slotsChest[i + 1].GetComponent<Image>().color = new Color(1, 1, 1, 1f);

                            slotsChestBusy[i] = true;
                            slotsChestBusy[i + 1] = false;

                            Debug.Log("swap");
                        }
                        break;
                    }
                case 3:
                    {
                        if (slotsHelmetBusy[i] == false && slotsHelmetBusy[i + 1] == true)
                        {
                            slotsHelmet[i].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsHelmet[i + 1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                            slotsHelmet[i].GetComponent<Image>().color = slotsHelmet[i + 1].GetComponent<Image>().color;

                            slotsHelmet[i + 1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = emptySprite;
                            slotsHelmet[i + 1].GetComponent<Image>().color = new Color(1, 1, 1, 1f);

                            slotsHelmetBusy[i] = true;
                            slotsHelmetBusy[i + 1] = false;

                            Debug.Log("swap");
                        }
                        break;
                    }
            }
        }
    }

    private void SetColorRarity(Item it, List<GameObject> slot, List<Item> item)
    {
        switch (it.itemRarity)
        {
            case 1:
                {
                    slot[item.IndexOf(it)].GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
                    
                    break;
                }
            case 2:
                {
                    slot[item.IndexOf(it)].GetComponent<Image>().color = new Color(0, 1, 0, 0.5f);
                    break;
                }
            case 3:
                {
                    slot[item.IndexOf(it)].GetComponent<Image>().color = new Color(0, 0, 1, 0.5f);
                    break;
                }
        }
    }
}


[System.Serializable]
public struct Item
{
    public int itemRarity;
    public int itemType;
    public string mainStatType;
    public int mainStatValue;
    private GameObject go;

    public void Constructor(int rarity, int type, string mainS,int mainSValue)
    {
        itemRarity = rarity;
        itemType = type;
        mainStatType = mainS;
        mainStatValue = mainSValue;       
    }

}
