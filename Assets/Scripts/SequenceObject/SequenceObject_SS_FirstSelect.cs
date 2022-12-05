using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_FirstSelect : SequenceObject
    {
        public override IEnumerator SequenceJob()
        {
            // Set card state
            GameManager.Instance.SetAllFrontCardsState(CardState.None);

            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup();

            while (!GameManager.Instance.CheckPanelClosed())
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }

            // Turn on turn sign
            GameObject tSign = GameManager.Instance.TurnSign[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
            tSign.SetActive(true);
            SpriteRenderer spriteRenderer = tSign.GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0);
            while (spriteRenderer.color.a < 1)
            {
                spriteRenderer.color = new Vector4(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, spriteRenderer.color.a + 0.01f);
                yield return new WaitForSecondsRealtime(0.01f * FadingTime);
            }

            // Wait first select player
            while (GameManager.Instance.FirstSelctedCard == -1)
            {
                PassedTime += 0.001f;
                yield return new WaitForSecondsRealtime(0.001f);
            }

            if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
                Debug.LogError("TimeOut : Something bad happens");

            GameManager.Instance.SetSubmit(0, 4);
            yield return new WaitForSecondsRealtime(0.5f);

            EndMySequence(new object[] { });
        }
    }
}