using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace RGYB
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        string FILE_PATH_Data;
        string FILE_PATH_Shop;

        private Data savedData;
        private Shop[] savedShop;

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MainMenu")
            {
                ReadData();
                ReadShop();
                MenuManager.Instance.OpenMainMenu();
            }
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this.GetComponent<DataManager>();
                FILE_PATH_Data = Application.persistentDataPath + "/Data.json";
                FILE_PATH_Shop = Application.persistentDataPath + "/Shop.json";
            }
        }

        private void ReadData()
        {
            if (File.Exists(FILE_PATH_Data))
            {
                print("Read Data");
                FileStream filestream = new FileStream(FILE_PATH_Data, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(filestream, System.Text.Encoding.UTF8);
                var parsingText = sr.ReadToEnd();
                sr.Close();
                filestream.Close();
                savedData = JsonConvert.DeserializeObject<Data>(parsingText);
            }
            else
            {
                print("Create Data");
                TextAsset textAsset = Resources.Load<TextAsset>("Data");
                savedData = JsonUtility.FromJson<Data>(textAsset.ToString());
                SaveData();
            }
        }

        private void ReadShop()
        {
            if (File.Exists(FILE_PATH_Shop))
            {
                print("Read Shop");
                FileStream filestream = new FileStream(FILE_PATH_Shop, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(filestream, System.Text.Encoding.UTF8);
                var parsingText = sr.ReadToEnd();
                sr.Close();
                filestream.Close();
                parsingText = "{" + "\"Shops\":" + parsingText + "}";
                savedShop = JsonConvert.DeserializeObject<ShopReader>(parsingText).Shops;
            }
            else
            {
                print("Create Shop");
                TextAsset textAsset = Resources.Load<TextAsset>("Shop");
                savedShop = JsonUtility.FromJson<ShopReader>(textAsset.ToString()).Shops;
                SaveShop();
            }
        }

        public bool BuyItem(string name)
        {
            for (int i = 0; i < savedShop.Length; i++)
            {
                if (savedShop[i].Name == name)
                {
                    if (savedShop[i].IsBought == 1)
                    {
                        Debug.LogError("Something Wrong");
                        return false;
                    }

                    if (savedData.Credit >= savedShop[i].Price)
                    {
                        savedShop[i].IsBought = 1;
                        SaveShop();
                        return true;
                    }
                    else
                    {
                        Debug.Log("Not enough money");
                        return false;
                    }
                }
            }

            Debug.Log("No item named : " + name);

            return false;
        }

        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(savedData);
            File.WriteAllText(FILE_PATH_Data, json);
        }

        private void SaveShop()
        {
            string json = JsonConvert.SerializeObject(savedShop);
            File.WriteAllText(FILE_PATH_Shop, json);
        }

        #region Set & Get
        public void SetCardSkin(string cardSkin)
        {
            savedData.CardSkin = cardSkin;
            SaveData();
        }

        public Sprite GetCardSkin()
        {
            return Resources.Load<Sprite>($"CardSkin/{savedData.CardSkin}");
        }


        public void SetPortrait(string portrait)
        {
            savedData.Portrait = portrait;
            SaveData();
        }

        public Sprite GetPortrait()
        {
            return  Resources.Load<Sprite>($"Portrait/{savedData.Portrait}");
        }

        public void SetNickName(string nickName)
        {
            savedData.NickName = nickName;
            SaveData();
        }

        public string GetNickName()
        {
            return savedData.NickName;
        }

        public void SetTitle(string title)
        {
            savedData.Title = title;
            SaveData();
        }

        public string GetTitle()
        {
            return savedData.Title;
        }

        public void SetCredit(int credit)
        {
            savedData.Credit = credit;
            SaveData();
        }

        public int GetCredit()
        {
            return savedData.Credit;
        }

        public void SetScore(int score)
        {
            savedData.Score = score;
            SaveData();
        }

        public int GetScore()
        {
            return savedData.Score;
        }

        public void SetGender(string gender)
        {
            savedData.Gender = gender;
            SaveData();
        }

        public string GetGender()
        {
            return savedData.Gender;
        }

        public void SetMBTI(string mbti)
        {
            savedData.MBTI = mbti;
            SaveData();
        }

        public string GetMBTI()
        {
            return savedData.MBTI;
        }

        public void SetTendency(string tendency)
        {
            savedData.Tendency = tendency;
            SaveData();
        }

        public string GetTendency()
        {
            return savedData.Tendency;
        }

        #endregion
    }

    [System.Serializable]
    public struct Data
    {
        public string CardSkin;
        public string Portrait;
        public string NickName;
        public string Title;
        public int Credit;
        public int Score;
        public string Gender;
        public string MBTI;
        public string Tendency;

    }

    [System.Serializable]
    public struct Shop
    {
        public string Type; // CardSkin, Portrait, Title
        public string Name;
        public int Price;
        public int IsBought;
    }

    class ShopReader
    {
        public Shop[] Shops;
    }
}
