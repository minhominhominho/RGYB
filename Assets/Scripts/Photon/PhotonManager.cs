using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using ExitGames.Client.Photon;
using System.Diagnostics.Tracing;

namespace RGYB
{
    public class PhotonManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static PhotonManager instance = null;
        private static GameObject goInstance = null;
        public static PhotonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    goInstance = new GameObject();
                    DontDestroyOnLoad(goInstance);

                    instance = goInstance.AddComponent<PhotonManager>();
                }
                return instance;
            }
        }
        [SerializeField] private bool isConnecting;
        [SerializeField] private const byte maxPlayersPerRoom = 2;
        [SerializeField] private const string gameVersion = "1";
        [SerializeField] private const float timeOut = 10;
        private bool isExitCalled = false;


        #region Public Methods
        public void ConnectPhotonServer()
        {
            // keep track of the will to join a room, because when we come back from the game we will get a callback that we are connected, so we need to know what to do then
            isConnecting = true;

            if (PhotonNetwork.NetworkClientState != ClientState.ConnectingToMasterServer)
            {
                // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
                if (PhotonNetwork.IsConnected)
                {
                    // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
                    PhotonNetwork.JoinLobby();
                }
                else
                {
                    // #Critical, we must first and foremost connect to Photon Online Server.
                    PhotonNetwork.GameVersion = gameVersion;
                    PhotonNetwork.ConnectUsingSettings();
                }
            }
        }

        public void CheckAndLeaveRoom()
        {
            Debug.Log("CheckAndLeaveRoom()");

            if (PhotonNetwork.CurrentRoom != null)
                PhotonNetwork.LeaveRoom();
        }

        public void CreateRoom(bool isCustom, string customRoomName = null, string customPassword = null, bool isRandomRole = true, bool masterIsFirstSelect = true)
        {
            CheckAndLeaveRoom();

            string roomName = "";
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.IsOpen = true;
            roomOptions.MaxPlayers = maxPlayersPerRoom;
            int role = isRandomRole ? UnityEngine.Random.Range(0, 2) : (masterIsFirstSelect ? 1 : 0);
            roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "Role", role } };

            if (isCustom)
            {
                roomName += customRoomName + "_" + customPassword;
                roomOptions.IsVisible = false;
            }
            else
            {
                roomName += "RoomName_" + UnityEngine.Random.Range(0, 5000).ToString();
                roomOptions.IsVisible = true;
            }

            PhotonNetwork.CreateRoom(roomName, roomOptions, null);
            Debug.LogFormat("Create{0}Room() : {1}, {2}", (isCustom ? "Custom" : "Random"), roomName, role == 1 ? "Master First Select" : "Master Second Select");
        }

        public void JoinRandomRoom()
        {
            CheckAndLeaveRoom();

            PhotonNetwork.JoinRandomRoom();
        }

        public void JoinCustomRoom(string customRoomName, string customPassword)
        {
            CheckAndLeaveRoom();

            string roomName = customRoomName + "_" + customPassword;
            PhotonNetwork.JoinRoom(roomName);
        }

        public string GetRoomName()
        {
            if (PhotonNetwork.CurrentRoom != null)
                return PhotonNetwork.CurrentRoom.Name;
            else
                return null;
        }

        public void ExitGame()
        {
            if (!isExitCalled)
            {
                isExitCalled = true;
                CheckAndLeaveRoom();
                StartCoroutine(WaitUntilLeaving());
            }
        }

        IEnumerator WaitUntilLeaving()
        {
            Debug.LogFormat("Loading MainMenu Scene");
            float timeOutCount = 0;

            while (timeOutCount <= timeOut && PhotonNetwork.NetworkClientState == ClientState.Leaving)
            {
                timeOutCount += 0.1f;
                yield return new WaitForSecondsRealtime(0.1f);
            }

            if (timeOutCount >= timeOut)
            {
                Debug.LogError("TimeOut!");
            }
            else
            {
                PhotonNetwork.LoadLevel("MainMenu");
            }

            isExitCalled = false;
        }

        #endregion


        #region MonoBehaviourPunCallbacks Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster()");
            // PhotonNetwork.AutomaticallySyncScene = true;


            // we don't want to do anything if we are not attempting to join a room.
            // this case where isConnecting is false is typically when you lost or quit the game, when this level is loaded, OnConnectedToMaster will be called, in that case
            // we don't want to do anything.
            if (isConnecting)
            {
                // #Critical: The first we try to do is to join a potential existing room. If there is, good, else, we'll be called back with OnJoinRandomFailed()
                PhotonNetwork.JoinLobby();
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("OnDisconnected() : {0}", cause);
            // TODO : Reconnect
        }

        public override void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby()");
            //PhotonNetwork.JoinRandomOrCreateRoom();
        }

        // public override void OnRoomListUpdate(List<RoomInfo> roomList)
        // {
        //     // 룸 리스트 콜백은 로비에 접속했을때 자동으로 호출된다.
        //     // 로비에서만 호출 가능
        //     Debug.LogFormat($"OnRoomListUpdate() : RoomCount {0}", roomList.Count);
        // }

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom()");
            if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom && !PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                Debug.LogFormat("Loading Game Scene");
                int role = (int)PhotonNetwork.CurrentRoom.CustomProperties["Role"];
                PhotonNetwork.LoadLevel("Game_" + ((1 - role) == 1 ? "FirstSelect" : "SecondSelect"));
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed()");
            CreateRoom(false);
            Debug.Log("Waiting for Opponent");
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRoomFailed()");
            MainMenuManager.Instance.FailToJoinCustomRoom();
        }

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting

            if (PhotonNetwork.CurrentRoom.PlayerCount == maxPlayersPerRoom && PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
                Debug.LogFormat("Loading Game Scene");
                int role = (int)PhotonNetwork.CurrentRoom.CustomProperties["Role"];
                PhotonNetwork.LoadLevel("Game_" + (role == 1 ? "FirstSelect" : "SecondSelect"));
            }
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
            ExitGame();
        }

        public bool IsFirstSelectPlayer()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Role"] == 1)
                    return true;
                else return false;
            }
            else
            {
                if ((int)PhotonNetwork.CurrentRoom.CustomProperties["Role"] == 1)
                    return false;
                else return true;
            }
        }
        #endregion


        #region IOnEventCallback Methods
        // Called in SequenceObject to syncronize sequence order
        public void Synchronization(byte eventCode, object[] eventContent)
        {
            Debug.LogFormat("Send Synchronization " + eventCode);
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            PhotonNetwork.RaiseEvent(eventCode, eventContent, raiseEventOptions, SendOptions.SendReliable);
        }

        public void SendEmotion(int num)
        {
            Debug.LogFormat("Send Emotion " + num);
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
            object[] eventContent = new object[] { num };
            PhotonNetwork.RaiseEvent(100, eventContent, raiseEventOptions, SendOptions.SendReliable);
        }

        public void OnEvent(EventData photonEvent)
        {
            // SequenceOrder 0~100
            if (photonEvent.Code >= 0 && photonEvent.Code < 100)
            {
                Debug.LogFormat("Receive Synchronization " + photonEvent.Code);
                GameManager.Instance.OpponentOrder = photonEvent.Code;

                if (photonEvent.Code != GameManager.Instance.SequenceIndex)
                {
                    Debug.LogError("Something's wrong");
                    return;
                }

                if (photonEvent.Code == (int)GameSequenceType.FirstSelect && GameManager.Instance.FirstSelctedCard == -1)
                {
                    object[] data = (object[])photonEvent.CustomData;
                    GameManager.Instance.FirstSelctedCard = (int)data[0];
                    Debug.LogFormat("Opponent First Selection : {0}", GameManager.Instance.FirstSelctedCard);
                }
                else if (photonEvent.Code == (int)GameSequenceType.Ban && GameManager.Instance.BannedCard == -1)
                {
                    object[] data = (object[])photonEvent.CustomData;
                    GameManager.Instance.BannedCard = (int)data[0];
                    Debug.LogFormat("Opponent Non Selected Card : {0}", GameManager.Instance.BannedCard);
                }
                else if (photonEvent.Code == (int)GameSequenceType.OpenNonSelected && GameManager.Instance.OpenedCard == -1)
                {
                    object[] data = (object[])photonEvent.CustomData;
                    GameManager.Instance.OpenedCard = (int)data[0];
                    Debug.LogFormat("Opponent Non Selected Card : {0}", GameManager.Instance.OpenedCard);
                }
                else if (photonEvent.Code == (int)GameSequenceType.SecondSelect && GameManager.Instance.SecondSelectedCard == -1)
                {
                    object[] data = (object[])photonEvent.CustomData;
                    GameManager.Instance.SecondSelectedCard = (int)data[0];
                    Debug.LogFormat("Opponent Non Selected Card : {0}", GameManager.Instance.SecondSelectedCard);
                }
            }
            // Emotion 100
            else if (photonEvent.Code == 100)
            {
                Debug.Log("Get Emotion");
                object[] data = (object[])photonEvent.CustomData;
                GameManager.Instance.GetEmotion((int)data[0]);
            }
            // Not used
            //// CardEffect 101
            //else if (photonEvent.Code == 101)
            //{
            //    object[] data = (object[])photonEvent.CustomData;
            //    // Debug.LogFormat("Received Card_{0}, Effect_{1}", (int)data[0], (int)data[1]);
            //    GameManager.Instance.SyncronizeCardEffect((int)data[0], (int)data[1]);
            //}
        }

        #endregion
    }
}