using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RGYB
{
    public class EmotionChecker : MonoBehaviour
    {
        // TODO : 아직 구현 못함
        public int EmotionNum;
        private bool isMouseOn = false;
        [SerializeField] private Sprite hoverImage;
        private Sprite normalImage;

        private void Awake()
        {
            normalImage = GetComponent<Image>().sprite;
        }

        private void OnMouseEnter()
        {
            isMouseOn = true;
            GetComponent<Image>().sprite = hoverImage;
        }

        private void OnMouseExit()
        {
            isMouseOn = false;
            GetComponent<Image>().sprite = normalImage;
        }

        private void OnDisable()
        {
            if (isMouseOn) GameManager.Instance.SendEmotion(EmotionNum);
            isMouseOn = false;
            GetComponent<Image>().sprite = normalImage;
        }
    }
}