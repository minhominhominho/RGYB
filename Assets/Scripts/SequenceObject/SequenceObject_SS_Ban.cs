using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_Ban : SequenceObject
    {
        //public override IEnumerator SequenceJob()
        //{
        //    while (GameManager.Instance.BannedCard == -1)
        //    {
        //        PassedTime += 0.001f;
        //        yield return new WaitForSecondsRealtime(0.001f);
        //    }

        //    if (PassedTime + ExtraTimeForReciever > GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
        //        Debug.LogError("TimeOut : Something bad happens");

        //    // Lock
        //    GameManager.Instance.SetAllBackCardsState(CardState.NotBeChanged);

        //    // TODO : Effect
        //    GameManager.Instance.BackCards[GameManager.Instance.BannedCard].GetComponent<SpriteRenderer>().sprite =
        //        GameManager.Instance.FrontCards[GameManager.Instance.BannedCard].GetComponent<SpriteRenderer>().sprite;
        //    int tempIdx = 0;
        //    if (GameManager.Instance.OpenedCard == GameManager.Instance.BannedCard) tempIdx = 1;
        //    GameManager.Instance.BackCards[GameManager.Instance.BannedCard].transform
        //        .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Banned";

        //    EndMySequence(new object[] { });
        //}
    }
}