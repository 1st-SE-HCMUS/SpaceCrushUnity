using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Item_Panel_Script : MonoBehaviour
{
    public Image ItemImage;
    public Text Price;
    public Text Description;
    public Button equipButton;
    public Button buyButton;
    //public int itemIndex; //Index of item in "List<ShopItems> shopItem"
    ShopController_Script.ShopItems localItem;

  

    void Start()
    {
        buyButton.onClick.AddListener(buyItem);
        equipButton.onClick.AddListener(equipItem);
    }
    public int updateData(ShopController_Script.ShopItems item)
    {
        localItem = item;
        equipButton.gameObject.SetActive(true);

        //Debug.Log("Local item name: " + localItem.programmingName);
        ItemImage.sprite = Resources.Load<Sprite>(item.imagePath);
        Price.text = item.price.ToString();
        Description.text = item.description;
        if(item.type == 1)//one time use (auto equip?)
        {
            equipButton.gameObject.SetActive(false);
        }
        if(item.type > 1)//equipment
        {
            MainController_Script.UserData.Inventory userInventory = MainController_Script.userData.inventory;
            //If user owned item (in user inventory) hide buy button
            if (   userInventory.ship.Exists(i => i == item.programmingName)
                || userInventory.item.Exists(i => i == item.programmingName)
                || userInventory.bullet.Exists(i => i.name == item.programmingName))
            {
                buyButton.gameObject.SetActive(false);
                equipButton.interactable = true; //owned => can be equiped
            }
            else//Buy button is active => must buy before equip
            {
                equipButton.gameObject.SetActive(false);
            }


            MainController_Script.UserData.Equiped userEquiped = MainController_Script.userData.equiped;
            //print("Equiped bullet: " + userEquiped.bullet.name);


            //if item is equiped (in user "equiped"),  set "Interactable" of equipButton to false
            if (   userEquiped.ship == item.programmingName
                || userEquiped.item.Exists(i => i == item.programmingName)
                || userEquiped.bullet.name == item.programmingName)
            {
                equipButton.interactable = false;
            }
        }
        return 0;
    }


    void buyItem()
    {
        Debug.Log("Buyed");
        //Debug.Log("Local item name: " + localItem.programmingName);

        MainController_Script.UserData userData = MainController_Script.userData;
        if(userData.inventory.gold >= localItem.price)//enough money
        {
            userData.inventory.gold -= localItem.price;

            //bring to user inventory
            if(localItem.type == 1)//one use item
            {
                userData.inventory.item.Add(localItem.programmingName);
                userData.equiped.item.Add(localItem.programmingName);
            }
            if (localItem.type == 2)//bullet
            {
                //Debug.Log("Buyed a bullet");
                MainController_Script.UserData.Bullet bullet = new MainController_Script.UserData.Bullet();
                bullet.name = localItem.programmingName;
                //Debug.Log("Bullet name: " + localItem.programmingName);

                bullet.sound = localItem.sound;
                userData.inventory.bullet.Add(bullet);
                //Debug.Log(userData.inventory.bullet[2].name);
            }
            if(localItem.type == 3)//ship
            {
                userData.inventory.ship.Add(localItem.programmingName);
            }

        }

        //save data
        MainController_Script.saveData();       
        
    }

    void equipItem()
    {
        MainController_Script.UserData userData = MainController_Script.userData;
        //bring to user "equiped"
        if (localItem.type == 1)//one use item
        {
            userData.equiped.item.Add(localItem.programmingName);
        }
        if (localItem.type == 2)//bullet
        {
            //Debug.Log("Buyed a bullet");
            MainController_Script.UserData.Bullet bullet = new MainController_Script.UserData.Bullet();
            bullet.name = localItem.programmingName;
            //Debug.Log("Bullet name: " + localItem.programmingName);
            bullet.sound = localItem.sound;
            userData.equiped.bullet = bullet;
            //Debug.Log(userData.inventory.bullet[2].name);
        }
        if (localItem.type == 3)//ship
        {
            userData.equiped.ship = localItem.programmingName;
        }

        //save data
        MainController_Script.saveData();
    }
}