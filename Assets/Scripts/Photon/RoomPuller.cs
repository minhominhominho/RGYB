using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System;


namespace RGYB
{
    public class RoomPuller : MonoBehaviourPunCallbacks
    {
        Action<List<RoomInfo>> callback = null;
        LoadBalancingClient client = null;

        private void Update()
        {
            if (client != null)
            {
                // �ݺ������� service �Լ��� ȣ������� ����Ŭ���̾�Ʈ�� ������ ������
                client.Service();
            }
        }

        public void OnGetRoomsInfo(Action<List<RoomInfo>> callback)
        {
            // ����Ŭ���̾�Ʈ�� ������ ������ ���ӽ�Ų��.
            this.callback = callback;
            client = new LoadBalancingClient();
            client.AddCallbackTarget(this);
            client.StateChanged += OnStateChanged;
            client.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            client.AppVersion = PhotonNetwork.NetworkingClient.AppVersion;
            client.EnableLobbyStatistics = true;

            // ���� ���ÿ��� ���� ������ �������־�� �Ѵ�. (FixedRegion �����ϱ�)
            client.ConnectToRegionMaster(PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion);
        }

        void OnStateChanged(ClientState previousState, ClientState state)
        {
            Debug.Log("���� Ŭ���̾�Ʈ ���� : " + state);

            // ����Ŭ���̾�Ʈ�� ������ ������ �����ϸ� �κ�� ���� ��Ų��.
            if (state == ClientState.ConnectedToMasterServer)
            {
                client.OpJoinLobby(null);
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            Debug.Log("���� Ŭ���̾�Ʈ �븮��Ʈ ������Ʈ");

            if (callback != null)
            {
                callback(roomList);
            }
            // ��� �۾��� ������ ���� Ŭ���̾�Ʈ ���� ����
            client.Disconnect();
        }
    }
}