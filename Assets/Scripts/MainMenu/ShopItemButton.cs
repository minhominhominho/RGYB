using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms.Impl;
using System.Reflection;

namespace RGYB
{
    public class ShopItemButton : MonoBehaviour
    {
        public string Type;
        private string itemName;


        public void BuyItem()
        {
            if (Type == "Title")
            {
                itemName = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
            }
            else
            {
                itemName = GetComponent<Image>().sprite.name;
            }
            MenuManager.Instance.ShopCheckBuyItem(Type, itemName);
        }
    }
}