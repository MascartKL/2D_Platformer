using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemConstructor : MonoBehaviour
{
    public int itemRarity;
    // 1 -cammon 
    // 2 - uncommon
    // 3 - rare
    // 4 - epic
    // 5 - legengary
    public float chanceRarity;

    public int itemType;
    // 1 - sword
    // 2 - body armour
    // 3 - helmet
    public float chanceType;

    public int mainStat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Constructor(int levelNum)
    {
        chanceRarity = Random.Range(0f, 101f);
        if(levelNum < 3)
        {
            if(95f > chanceRarity)
            {
                itemRarity = 1;
            }
            else
            {
                itemRarity = 2;
            }
        }

        chanceType = Random.Range(0f, 100f);
        if(chanceType <= 33f)
        {
            itemType = 1;
        }else if(chanceType >= 66f)
        {
            itemType = 3;
        }else
        {
            itemType = 2;
        }



    }
}
