using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RGYB
{
    public class Mouse : MonoBehaviour
    {
        private GameObject MousePointer;
        private RectTransform MousePointerRect;
        private Vector2 mousePosition;
        private Vector3 Offset = new Vector3(25f, -20f, 0);

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            MousePointer = transform.GetChild(0).gameObject;
            MousePointerRect = MousePointer.GetComponent<RectTransform>();
            Cursor.visible = false;
        }

        void Update()
        {
            mousePosition = Input.mousePosition + Offset;
            MousePointerRect.position = mousePosition;
        }
    }
}