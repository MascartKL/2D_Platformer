
using System.Collections.Generic;


using UnityEngine;
using UnityEngine.UI;
using System.IO;
//using Newtonsoft.Json;


using System.Runtime.Serialization.Formatters.Binary;

public class test : MonoBehaviour
{
   // public ItemList itemList;
    public Item item;
   // public GameObject obj;

    public ItemConstructor itCons;

    public Sprite emptySprite;
    //[SerializeField] private List<GameObject> slots = new List<GameObject>();
    [SerializeField] private Sprite[] imageType;



    [SerializeField] private GameObject mainStat;

    [SerializeField] private GameObject slotHelmet;
    [SerializeField] private GameObject slotChest;
    [SerializeField] private GameObject slotSword;

    [SerializeField] private List<GameObject> slotPlayer = new List<GameObject>();
    [SerializeField] private List<bool> isBusySlotPlayer = new List<bool>();
    [SerializeField] private List<Item> playersItem = new List<Item>();

    [SerializeField] private List<GameObject> slotReward = new List<GameObject>();

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

    [SerializeField] private Item itemSave;
    [SerializeField] private int[] slotsJson;

   // [SerializeField] private List<string> stringJson = new List<string>();

   // private int listMax = 0;

    private int selectedItemIndex;
    private int selectedItemType;

   // public int butIndex ;

    //  public Image img;

    public GameObject itemListObj;

    public GameObject itemListImg;

    private Item tmpItem;
    private Color tmpColor;
    private Sprite tmpSprite;

   // public GameObject testText;

    //private int testJson;

  // [SerializeField] private string stringJson;

    [SerializeField] private Item[] itemArr;
   [SerializeField] private string[] data;
   // [SerializeField] private int datatest;


   // [SerializeField] private string saveFilename;

    private string filepath;

    private void Awake()
    {
        //filepath = Path.Combine(Application.dataPath, this.saveFilename);
    }
    // Start is called before the first frame update
    void Start()
    {
        LevelController.LevelEnd += LevelReward;

        for (int i = 0; i < 4; i++)
        {

            slotsSwordBusy[i] = false;
            slotsChestBusy[i] = false;
            slotsHelmetBusy[i] = false;
        }

        
        for(int i=0; i < 3;i++)
        {
            isBusySlotPlayer[i] = false;           
        }
    }

    void LevelReward(int level)
    {
        item.Constructor(1, 1, "1", 1);
        Debug.Log(level);
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filepath, FileMode.Create);

        // Item itemSave = new Item();

        itemSave = itemsSword[0];

        bf.Serialize(fs, itemSave);

        fs.Close();
    }

    public void Load()
    {
        if (!File.Exists(filepath))
            return;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(filepath, FileMode.Open);

        //Item itemSave = (Item)bf.Deserialize(fs);
        itemSave = (Item)bf.Deserialize(fs);
        fs.Close();
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

    }

    public void TestButton(GameObject go)
    {
        itemListImg.SetActive(true);


        switch(go.transform.parent.name)
        {
            case "Swords":
                {
                    selectedItemIndex = slotsSword.IndexOf(go);
                    selectedItemType = 1;
                    break;
                }
            case "Chests":
                {
                    selectedItemIndex = slotsChest.IndexOf(go);
                    selectedItemType = 2;
                    break;
                }
            case "Helmets":
                {
                    selectedItemIndex = slotsHelmet.IndexOf(go);
                    selectedItemType = 3;
                    break;
                }

            default:
                {
                    Debug.Log(go.transform.parent.name);
                    break;
                }

        }
        Debug.Log(selectedItemIndex);

        itemListImg.transform.Find("Icon").gameObject.GetComponent<Image>().sprite = go.transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
        itemListImg.transform.Find("Icon_back").gameObject.GetComponent<Image>().color = go.GetComponent<Image>().color;
        mainStat.GetComponent<Text>().text = item.mainStatType.ToString() +"       " + item.mainStatValue.ToString();


    }

    public void AddButton()
    {

        item.Constructor(Random.Range(1, 4), Random.Range(1,4), "Attack", Random.Range(0, 1000));
       // listMax += 1;
        AddVisual(item);
    }

    public void UseButton()
    {
        Debug.Log(selectedItemIndex);
        itemListImg.SetActive(false);
        switch (selectedItemType)
        {
            case 1:
                {
                    Debug.Log(Player.baseDamage + "до");
                    tmpSprite = slotPlayer[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    tmpColor = slotPlayer[0].GetComponent<Image>().color;
                    tmpItem = playersItem[0];
                    slotPlayer[0].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsSword[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    slotPlayer[0].GetComponent<Image>().color = slotsSword[selectedItemIndex].GetComponent<Image>().color;

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
                    tmpSprite = slotPlayer[1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    tmpColor = slotPlayer[1].GetComponent<Image>().color;

                    slotPlayer[1].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsChest[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    slotPlayer[1].GetComponent<Image>().color = slotsChest[selectedItemIndex].GetComponent<Image>().color;
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
                    tmpSprite = slotPlayer[2].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    tmpItem = playersItem[2];
 
                    slotPlayer[2].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = slotsHelmet[selectedItemIndex].transform.Find("Icon").gameObject.GetComponent<Image>().sprite;
                    slotPlayer[2].GetComponent<Image>().color = slotsHelmet[selectedItemIndex].GetComponent<Image>().color;
  
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

    public string SaveJson(Item it)
    {
        return JsonUtility.ToJson(itemsSword);
    }
    public void LoadJson(string json)
    {
        itemsSword = JsonUtility.FromJson<List<Item>>(json);
    }
    public string Save(Item data)
    {
        return JsonUtility.ToJson(itemsSword);
    }
    public void Load(string json)
    {
        itemsSword = JsonUtility.FromJson<List<Item>>(json);
    }
    private void AddVisual(Item it)
    {
        Debug.Log(itemsSword);
        switch (it.itemType)
        {
            case 1:
                {
                   
                    itemsSword.Add(it);
                    slotsSword[itemsSword.IndexOf(it)].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[0];
                    slotsSwordBusy[itemsSword.IndexOf(it)] = true;
                    Debug.Log(itemsSword.IndexOf(it));
                     SetColorRarity(it, slotsSword, itemsSword);
                   // string[] data = new string[4];
                  // data[itemsSword.IndexOf(it)] = JsonUtility.ToJson(itemsSword[itemsSword.IndexOf(it)], true);
                    if(itemsSword.IndexOf(it) == 4)
                    {
                        File.WriteAllLines(Application.streamingAssetsPath + "/Save.json", data);
                    }
                    break;
                }
            case 2:
                {
                    itemsChest.Add(it);
                    slotsChest[itemsChest.IndexOf(it)].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[1];
                    slotsChestBusy[itemsChest.IndexOf(it)] = true;
                    Debug.Log(itemsChest.IndexOf(it));
                     SetColorRarity(it, slotsChest, itemsChest);
                    break;
                }
            case 3:
                {
                    itemsHelmet.Add(it);
                    slotsHelmet[itemsHelmet.IndexOf(it)].transform.Find("Icon").gameObject.GetComponent<Image>().sprite = imageType[2];
                    slotsHelmetBusy[itemsHelmet.IndexOf(it)] = true;
                    Debug.Log(itemsHelmet.IndexOf(it));
                     SetColorRarity(it, slotsHelmet, itemsHelmet);
                    break;
                }
        }
    }

    private void Refresh()
    {
        for (int i = 0; i < 3; i++)
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

/*[System.Serializable]
public struct ItemList
{
    public int itemRarity;
    public int itemColor;
    public int itemIcon;
    public int mainStat;
    private GameObject go;
}*/