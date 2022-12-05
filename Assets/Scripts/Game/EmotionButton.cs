using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

namespace RGYB
{
    public class EmotionButton : MonoBehaviour
    {
        private void OnMouseDown()
        {
            GameManager.Instance.OpenCanvasGroup(GameManager.Instance.EmotionSelectUI, false, false);
        }

        private void OnMouseUp()
        {
            GameManager.Instance.CloseCurrentCanvasGroup(false);
        }
    }
}