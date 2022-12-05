using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_OpenNonSelected : SequenceObject
    {
        public override IEnumerator SequenceJob()
        {
            // Open Canvas Group
            GameManager.Instance.OpenSequenceCanvasGroup();

            while (!GameManager.Instance.CheckPanelClosed())
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }

            while (GameManager.Instance.OpenedCard == -1)
            {
                PassedTime += 0.001f;
                yield return new WaitForSecondsRealtime(0.001f);
            }

            if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
                Debug.LogError("TimeOut : Something bad happens");

            GameManager.Instance.SetSubmit(2, GameManager.Instance.OpenedCard);
            yield return new WaitForSecondsRealtime(0.5f);
            GameManager.Instance.OpenSignEffect();
            yield return new WaitForSecondsRealtime(0.5f);

            EndMySequence(new object[] { });
        }
    }
}