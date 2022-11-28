using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace RGYB
{
    public class MainMenuManager : MonoBehaviour
    {
        public static MainMenuManager Instance = null;

        [SerializeField] private GameObject[] MainMenuPanels;
        [SerializeField] private GameObject[] CustomMatchPanel;
        [SerializeField] private TMP_InputField CreateCustomRoomName;
        [SerializeField] private TMP_InputField CreateCustomRoomPassword;
        [SerializeField] private TMP_InputField JoinCustomRoomName;
        [SerializeField] private TMP_InputField JoinCustomRoomPassword;
        [SerializeField] private Toggle IsRandomRole;
        [SerializeField] private Toggle IsFirstSelect;

        public void test()
        {
            Debug.Log(PhotonManager.Instance.GetRoomName());
        }

        private void Awake()
        {
            Instance = this.GetComponent<MainMenuManager>();
            PhotonManager.Instance.ConnectPhotonServer();
        }

        public void ShowMainMenuPanel(int index)
        {
            Debug.Log("ShowMainMenuPanel (" + index + ")");

            foreach (GameObject g in MainMenuPanels)
            {
                g.SetActive(false);
            }

            MainMenuPanels[index].SetActive(true);
        }

        public void RandomMatch()
        {
            Debug.Log("RandomMatch()");
            PhotonManager.Instance.JoinRandomRoom();
        }

        public void StopRandomMatch()
        {
            Debug.Log("StopRandomMatch()");
            PhotonManager.Instance.CheckAndLeaveRoom();
        }

        public void ShowCustomMatchPanel(int index)
        {
            Debug.Log("ShowCustomMatchPanel (" + index + ")");

            CreateCustomRoomName.text = "";
            CreateCustomRoomPassword.text = "";
            JoinCustomRoomName.text = "";
            JoinCustomRoomPassword.text = "";

            foreach (GameObject g in CustomMatchPanel)
            {
                g.SetActive(false);
            }

            CustomMatchPanel[index].SetActive(true);
        }

        public void ToggleRandom()
        {
            IsFirstSelect.gameObject.SetActive(!IsRandomRole.isOn);
        }

        public void CreateCustomRoom()
        {
            Debug.Log("CreateCustomRoom()");

            if (CreateCustomRoomName.text == "")
            {
                Debug.LogWarning("Enter the room name");
                // TODO : Fail UI
            }
            else
            {
                CustomMatchPanel[1].SetActive(false);
                CustomMatchPanel[3].SetActive(true);
                PhotonManager.Instance.CreateRoom(true, CreateCustomRoomName.text, CreateCustomRoomPassword.text, IsRandomRole.isOn, IsFirstSelect.isOn);
            }
        }

        public void BackToCreateCustomRoom()
        {
            Debug.Log("BackToCreateCustomRoom()");

            CustomMatchPanel[1].SetActive(true);
            CustomMatchPanel[3].SetActive(false);

            PhotonManager.Instance.CheckAndLeaveRoom();
        }

        public void JoinCustomRoom()
        {
            Debug.Log("JoinCustomRoom()");

            if (JoinCustomRoomName.text == "")
            {
                Debug.LogWarning("Enter the room name");
                FailToJoinCustomRoom();
            }
            else
            {
                PhotonManager.Instance.JoinCustomRoom(JoinCustomRoomName.text, JoinCustomRoomPassword.text);
            }
        }

        public void FailToJoinCustomRoom()
        {
            Debug.Log("FailToJoinCustomRoom()");
            // TODO : Fail UI
        }

        public void ExitGame()
        {
            Debug.Log("ExitGame()");
            Application.Quit();
        }

        #region OptionPanel
        // TODO : Sound, Resoltion + a
        #endregion


        #region CreditPanel
        // TODO : Show credit
        #endregion
    }
}