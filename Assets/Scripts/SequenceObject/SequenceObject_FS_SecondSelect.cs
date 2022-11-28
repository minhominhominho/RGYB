using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_SecondSelect : SequenceObject
    {
        //private const float fadingTime = 0.5f;

        //public override IEnumerator SequenceJob()
        //{
        //    // Set card state
        //    GameManager.Instance.SetAllFrontCardsState(CardState.Receiving);
        //    GameManager.Instance.ResetFrontCards();

        //    // Wait
        //    while (GameManager.Instance.SecondSelectedCard == -1)
        //    {
        //        PassedTime += 0.001f;
        //        yield return new WaitForSecondsRealtime(0.001f);
        //    }

        //    if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
        //        Debug.LogError("TimeOut : Something bad happens");

        //    // Lock
        //    GameManager.Instance.SetAllBackCardsState(CardState.NotBeChanged);

        //    // TODO : Effect
        //    int tempIdx = 0;
        //    if (GameManager.Instance.SecondSelectedCard == GameManager.Instance.OpenedCard) tempIdx = 1;
        //    else if (GameManager.Instance.SecondSelectedCard == GameManager.Instance.FirstSelctedCard) tempIdx = 1;
        //    GameManager.Instance.FrontCards[GameManager.Instance.SecondSelectedCard].transform
        //        .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Second Selected";

        //    EndMySequence(new object[] { });
        //}
    }
}