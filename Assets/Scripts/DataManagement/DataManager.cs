using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace RGYB
{
    public class DataManager : MonoBehaviour
    {
        public static DataManager Instance;
        string FILE_PATH;

        private Data savedData;


        private void Awake()
        {
            if (Instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                Instance = this.GetComponent<DataManager>();
                FILE_PATH = Application.persistentDataPath + "/Data.json";
                MainMenuManager.Instance.StartGame();
                ReadData();
            }
        }

        private void ReadData()
        {
            if (File.Exists(FILE_PATH))
            {
                FileStream filestream = new FileStream(FILE_PATH, FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(filestream, System.Text.Encoding.UTF8);
                var parsingText = sr.ReadToEnd();
                sr.Close();
                filestream.Close();
                savedData = JsonConvert.DeserializeObject<Data>(parsingText);
                savedData.LastLoginDate = Convert.ToDateTime(savedData.LastLoginString);
            }
            else
            {
                TextAsset textAsset = Resources.Load<TextAsset>("Data");
                savedData = JsonUtility.FromJson<Data>(textAsset.ToString());
                savedData.LastLoginDate = Convert.ToDateTime(savedData.LastLoginString);
            }
        }

        public int GetScore()
        {
            return savedData.Score;
        }

        public int GetCredit()
        {
            return savedData.Credit;
        }

        public DateTime GetLastLoginDate()
        {
            return savedData.LastLoginDate;
        }

        public void SetScore(int score)
        {
            savedData.Score = score;
            SaveData();
        }

        public void SetCredit(int credit)
        {
            savedData.Credit = credit;
            SaveData();
        }

        public void SetDate(DateTime date)
        {
            savedData.LastLoginDate = date;
            savedData.LastLoginString = date.ToString("yyyy/MM/dd");
            SaveData();
        }

        private void SaveData()
        {
            string json = JsonConvert.SerializeObject(savedData);
            File.WriteAllText(FILE_PATH, json);
        }
    }

    [System.Serializable]
    struct Data
    {
        public int Score;
        public int Credit;
        public string LastLoginString;
        public DateTime LastLoginDate;
    }
}
