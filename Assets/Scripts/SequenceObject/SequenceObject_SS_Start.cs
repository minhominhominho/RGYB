using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace RGYB
{
    public class SequenceObject_SS_Start : SequenceObject
    {
        [SerializeField] private const float rollGameScrollTime = 1f;
        [SerializeField] private Transform maskDestWorldPosition;

        public override IEnumerator SequenceJob()
        {
            // Roll Game Scroll
            Vector3 origPos = GameManager.Instance.GameBoardMask.transform.position;
            Vector3 destPos = maskDestWorldPosition.position;
            Debug.Log(destPos);
            while (GameManager.Instance.GameBoardMask.transform.position.x < destPos.x)
            {
                GameManager.Instance.GameBoardMask.transform.position = new Vector3(
                    GameManager.Instance.GameBoardMask.transform.position.x + (destPos.x - origPos.x) / 100, origPos.y, origPos.z
                    );
                yield return new WaitForSecondsRealtime(0.01f * rollGameScrollTime);
            }

            // FadeIn Cards
            List<SpriteRenderer> spr = new List<SpriteRenderer>();

            for (int i = 0; i < GameManager.Instance.FrontCards.Length; i++)
            {
                spr.Add(GameManager.Instance.FrontCards[i].GetComponent<SpriteRenderer>());
                spr.Add(GameManager.Instance.BackCards[i].GetComponent<SpriteRenderer>());
                spr[i].color = new Vector4(spr[i].color.r, spr[i].color.g, spr[i].color.b, 0);
                spr[i + 1].color = new Vector4(spr[i + 1].color.r, spr[i + 1].color.g, spr[i + 1].color.b, 0);
            }

            while (spr[0].color.a < 1)
            {
                foreach (SpriteRenderer s in spr)
                {
                    s.color = new Vector4(s.color.r, s.color.g, s.color.b, s.color.a + 0.01f);
                }
                yield return new WaitForSecondsRealtime(0.01f * FadingTime);
            }

            // Set interactable buttons
            GameManager.Instance.ButtonCanvas.interactable = true;
            GameManager.Instance.SetSelectButtonInteractable(false);

            // Call Next Sequence
            EndMySequence(new object[] { });
        }
    }
}