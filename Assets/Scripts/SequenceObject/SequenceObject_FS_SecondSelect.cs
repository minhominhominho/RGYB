using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_SecondSelect : SequenceObject
    {
        public override IEnumerator SequenceJob()
        {
            GameManager.Instance.TimerImage.fillAmount = 0;

            // Set card state
            GameManager.Instance.SetAllFrontCardsState(CardState.None);

            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup();

            while (!GameManager.Instance.CheckPanelClosed())
            {
                yield return NormalWait;
            }

            // Turn off turn sign
            GameObject tSignMask = GameManager.Instance.TurnSignMask[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
            GameObject tSignMaskDest = GameManager.Instance.TurnSignMaskDestPosition[GameManager.Instance.IsFirstSelectPlayer ? 0 : 1];
            Vector3 startPos = tSignMask.transform.localPosition;
            Vector3 destPos = tSignMaskDest.transform.localPosition;
            while (tSignMask.transform.localPosition.y > tSignMaskDest.transform.localPosition.y + 0.01f || tSignMask.transform.localPosition.y < tSignMaskDest.transform.localPosition.y - 0.01f)
            {
                tSignMask.transform.localPosition = new Vector3(
                     tSignMask.transform.localPosition.x,
                      tSignMask.transform.localPosition.y + (destPos.y - startPos.y) / 20,
                       tSignMask.transform.localPosition.z
                    );
                yield return FadeWait;
            }
            tSignMask.transform.localPosition = destPos;
            tSignMaskDest.transform.localPosition = startPos;

            // Turn on turn sign
            SoundManager.Instance.PlaySFX(SFXType.Sequece_TurnSign);
            tSignMask = GameManager.Instance.TurnSignMask[GameManager.Instance.IsFirstSelectPlayer ? 1 : 0];
            tSignMaskDest = GameManager.Instance.TurnSignMaskDestPosition[GameManager.Instance.IsFirstSelectPlayer ? 1 : 0];
            startPos = tSignMask.transform.localPosition;
            destPos = tSignMaskDest.transform.localPosition;
            while (tSignMask.transform.localPosition.y > tSignMaskDest.transform.localPosition.y + 0.01f || tSignMask.transform.localPosition.y < tSignMaskDest.transform.localPosition.y - 0.01f)
            {
                tSignMask.transform.localPosition = new Vector3(
                     tSignMask.transform.localPosition.x,
                      tSignMask.transform.localPosition.y + (destPos.y - startPos.y) / 20,
                       tSignMask.transform.localPosition.z
                    );
                yield return FadeWait;
            }
            tSignMask.transform.localPosition = destPos;
            tSignMaskDest.transform.localPosition = startPos;

            // Wait first select player
            while (GameManager.Instance.SecondSelectedCard == -1)
            {
                PassedTime += 0.001f;
                yield return ServerWait;
            }

            if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
                Debug.LogError("TimeOut : Something bad happens");

            GameManager.Instance.SetSubmit(0, 4);
            yield return new WaitForSeconds(0.5f);

            EndMySequence(new object[] { });
        }
    }
}