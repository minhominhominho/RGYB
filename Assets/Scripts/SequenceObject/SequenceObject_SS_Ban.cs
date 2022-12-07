using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_Ban : SequenceObject
    {
        public override IEnumerator SequenceJob()
        {
            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup();

            while (!GameManager.Instance.CheckPanelClosed())
            {
                yield return NormalWait;
            }

            while (GameManager.Instance.BannedCard == -1)
            {
                PassedTime += 0.001f;
                yield return ServerWait;
            }

            if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
                Debug.LogError("TimeOut : Something bad happens");

            GameManager.Instance.SetSubmit(1, GameManager.Instance.BannedCard);
            yield return new WaitForSeconds(0.5f);
            GameManager.Instance.BanSignEffect();
            yield return new WaitForSeconds(0.5f);

            EndMySequence(new object[] { });
        }
    }
}