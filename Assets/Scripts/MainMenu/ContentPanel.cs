using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class ContentPanel : MonoBehaviour
    {
        [SerializeField] private Menu menu;
        [SerializeField] private GameObject menuText;
        [SerializeField] private GameObject blockingImage;
        private RectTransform rect;
        private BoxCollider2D boxCollider2D;
        private const float pressedYPosAdding = 0.5f;

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
            boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void OnMouseDown()
        {
            rect.position = new Vector3(rect.position.x, rect.position.y + pressedYPosAdding, rect.position.z);
            menuText.SetActive(true);
        }

        private void OnMouseUp()
        {
            rect.position = new Vector3(rect.position.x, rect.position.y - pressedYPosAdding, rect.position.z);
            menuText.SetActive(false);
        }

        private void OnMouseUpAsButton()
        {
            MenuManager.Instance.SetFront(menu);
        }

        public void RevealBlockingImage(bool reveal)
        {
            blockingImage.SetActive(reveal);
            boxCollider2D.enabled= reveal;
        }

        public Menu GetMenu()
        {
            return menu;
        }
    }
}