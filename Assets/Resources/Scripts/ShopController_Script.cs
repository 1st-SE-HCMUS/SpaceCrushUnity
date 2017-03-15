using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


public class ShopController_Script : MonoBehaviour {

    public Text gold;
    public GameObject scrollPanel;
    public GameObject viewport;
    public GameObject freshItemPanel;
    public float distanceBetweenPanels = 300;
    public static bool viewportDragging = false;
    string dataPath;
    public class ShopItems
    {   
        public int id;
        public string name;
        public string description;
        public int price;
        public string imagePath;
        public int type;//1 = One Time Use :: 2 = Equipment
        public string programmingName;
        public string sound;

    }
    List<GameObject> itemPanelList = new List<GameObject>();
    public static List<ShopItems> shopItems;
 


    //private string dataString;
    //private JsonData itemsData;   
   

	// Use this for initialization
	void Start ()
    {
        dataPath = Application.dataPath + "/Resources/data/ItemList.json";

        if (!loadShopData(ref shopItems, dataPath))
        {
            Debug.LogError("Shop data not found!");//popup
        }
        //Debug.Log(shopItems[0].imagePath);
        initShopData(ref itemPanelList);
	}

    void Update()
    {
        //Debug.Log((scrollPanel.GetComponent<RectTransform>().rect.width - freshItemPanel.GetComponent<RectTransform>().rect.width) / 2.0f);
        int MinPanelIndex = MinDistPanelIndex(itemPanelList, shopItems.Count);//Get index of panel has minimum distance from center of viewport
        float XPos = Mathf.Lerp(viewport.GetComponent<RectTransform>().anchoredPosition.x, MinPanelIndex * -(freshItemPanel.GetComponent<RectTransform>().rect.width + distanceBetweenPanels), 30 * Time.deltaTime);
        //float MinDistance = Mathf.Min(ItemPanelDistances);
        //Debug.Log(XPos);
        //Debug.Log(scrollPanel.GetComponent<RectTransform>().rect.width);
        if (!viewportDragging)
        {
            viewport.GetComponent<RectTransform>().anchoredPosition = new Vector3(XPos, viewport.GetComponent<RectTransform>().anchoredPosition.y, viewport.GetComponent<RectTransform>().localPosition.z);

        }
        //print(MinPanelIndex + " " + itemPanelList[1].GetComponent<RectTransform>().transform.position.x + " " + itemPanelList[2].GetComponent<RectTransform>().transform.position.x);

        //Set gold value through user data
        updateGold();
        updateItemPanels();
    }


    int MinDistPanelIndex(List<GameObject> itemPanelList, int N)//Get index of panel has minimum distance from center of viewport
    {
        float[] ItemPanelDistances = new float[shopItems.Count];
        int MinPanelIndex = 0;
        float MinDistance = float.MaxValue;//compare to center of viewport

        for (int i = 0; i < N; i++)
        {
            ItemPanelDistances[i] = Mathf.Abs(this.transform.position.x - itemPanelList[i].GetComponent<RectTransform>().position.x);
            if (ItemPanelDistances[i] < MinDistance)
            {
                MinPanelIndex = i;
                MinDistance = ItemPanelDistances[i];
            }
        }
        return MinPanelIndex;
    }

    public void BeginDrag()
    {
        viewportDragging = true;
    }
    public void EndDrag()
    {
        viewportDragging = false;
    }

    bool loadShopData(ref List<ShopItems> shopItems, string dataPath)
    {
        if (!File.Exists(dataPath))
        {
            //Debug.Log("Data missing!");
            return false;
        }

        string dataString = File.ReadAllText(dataPath);
        
        JObject allItemData = JObject.Parse(dataString);
        JArray shopDataJArray = (JArray)allItemData["shopItem"];


        shopItems = shopDataJArray.ToObject<List<ShopItems>>();
        //Debug.Log("Shop Item: " + shopItems[0].description);
        return true;
    }

    int initShopData(ref List<GameObject> itemPanelList)
    {
        //Instantiate Item Panels
        viewport.GetComponent<RectTransform>().sizeDelta = new Vector2((scrollPanel.GetComponent<RectTransform>().rect.width - freshItemPanel.GetComponent<RectTransform>().rect.width)/2.0f+ freshItemPanel.GetComponent<RectTransform>().sizeDelta.x * shopItems.Count + (shopItems.Count - 1) * distanceBetweenPanels, freshItemPanel.GetComponent<RectTransform>().sizeDelta.y);
        for (int i = 0; i < shopItems.Count; i++)
        {
            itemPanelList.Add((GameObject)Instantiate(freshItemPanel));
            itemPanelList[i].transform.SetParent(viewport.transform, false);
            //set position
            Vector2 NewPanelPosition = new Vector2((scrollPanel.GetComponent<RectTransform>().rect.width ) / 2.0f + itemPanelList[i].GetComponent<RectTransform>().rect.width * i + i * distanceBetweenPanels, itemPanelList[i].GetComponent<RectTransform>().localPosition.y);// "-" cos panels are under their anchor
            //Debug.Log(scrollPanel.GetComponent<RectTransform>().rect.width / 2.0f + " " + (scrollPanel.GetComponent<RectTransform>().rect.width) / 2.0f + itemPanelList[0].GetComponent<RectTransform>().rect.width * i + i * distanceBetweenPanels);
            itemPanelList[i].GetComponent<RectTransform>().anchoredPosition = NewPanelPosition;
            //Debug.Log(itemPanelList[i].GetComponent<RectTransform>().anchoredPosition.x);

            //Set index to palnel
            //itemPanelList[i].GetComponent<Item_Panel_Script>().itemIndex = i;

            
            
        }

        
        return 0;
    }

    void updateGold()
    {
        gold.text = MainController_Script.userData.inventory.gold.ToString();
    }

    void updateItemPanels()
    {
        //call load function to load informations itseft
        for (int i = 0; i < shopItems.Count; i++)
        {
            itemPanelList[i].GetComponent<Item_Panel_Script>().updateData(shopItems[i]);
        }
    }
}
