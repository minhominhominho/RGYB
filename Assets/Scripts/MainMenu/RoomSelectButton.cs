using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

namespace RGYB
{
    public class RoomSelectButton : MonoBehaviour
    {
        private TextMeshProUGUI RoomName;

        private void OnEnable()
        {
            RoomName = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        }

        public void CustomSelectRoom()
        {
            MenuManager.Instance.CustomSelectRoom(RoomName.text);
        }
    }
}