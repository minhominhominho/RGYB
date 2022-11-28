using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_FS_Ban : SequenceObject
    {
        //private Coroutine timer;
        //private bool isSent = false;

        //public override IEnumerator SequenceJob()
        //{
        //    GameManager.Instance.CannotBeBannedCard = GameManager.Instance.FirstSelctedCard - 1;
        //    if (GameManager.Instance.CannotBeBannedCard == -1) GameManager.Instance.CannotBeBannedCard = 3;

        //    timer = StartCoroutine(Timer());

        //    yield return null;
        //}

        //private IEnumerator Timer()
        //{
        //    while (PassedTime < GameManager.Instance.GameSequences[(int)MyOrder].FullSequenceSeconds)
        //    {
        //        PassedTime += 0.001f;
        //        yield return new WaitForSecondsRealtime(0.001f);
        //    }

        //    if (!isSent)
        //    {
        //        isSent = true;
        //        // Not select until timeout
        //        if (GameManager.Instance.BannedCard == -1)
        //        {
        //            GameManager.Instance.BannedCard = GameManager.Instance.PickRandomCard();
        //        }

        //        // Lock
        //        GameManager.Instance.SetFrontCardState(GameManager.Instance.BannedCard, CardState.NotBeChanged);

        //        // TODO : Effect
        //        int tempIdx = 0;
        //        if (GameManager.Instance.OpenedCard == GameManager.Instance.BannedCard) tempIdx = 1;
        //        GameManager.Instance.FrontCards[GameManager.Instance.BannedCard].transform
        //              .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Banned";

        //        // Anyway just send selected card (by random or recent selected)
        //        EndMySequence(new object[] { GameManager.Instance.BannedCard });
        //    }
        //}

        //// Attached to "End turn Button" or Called when sequence timeout occured
        //public void EndSelection()
        //{
        //    if (!isSent)
        //    {
        //        isSent = true;
        //        Debug.Log("EndSelection()");

        //        if (GameManager.Instance.BannedCard == -1)
        //        {
        //            Debug.Log("Card not selected");
        //            isSent = false;
        //            // TODO : Effect or Popup
        //            return;
        //        }

        //        if (GameManager.Instance.BannedCard == GameManager.Instance.CannotBeBannedCard)
        //        {
        //            Debug.Log("Cannot ban this card");
        //            isSent = false;
        //            // TODO : Effect or Popup
        //            return;
        //        }

        //        // Lock
        //        GameManager.Instance.SetFrontCardState(GameManager.Instance.BannedCard, CardState.NotBeChanged);

        //        // TODO : Effect
        //        int tempIdx = 0;
        //        if (GameManager.Instance.OpenedCard == GameManager.Instance.BannedCard) tempIdx = 1;
        //        GameManager.Instance.FrontCards[GameManager.Instance.BannedCard].transform
        //              .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Banned";

        //        StopCoroutine(Timer());

        //        EndMySequence(new object[] { GameManager.Instance.BannedCard });
        //    }
        //}
    }
}