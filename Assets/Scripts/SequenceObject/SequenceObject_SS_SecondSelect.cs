using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace RGYB
{
    public class SequenceObject_SS_SecondSelect : SequenceObject
    {
        //private const float fadingTime = 0.5f;
        //private Coroutine timer;
        //private bool isSent = false;

        //public override IEnumerator SequenceJob()
        //{
        //    // FadeOut Back of Cards
        //    List<SpriteRenderer> spr = new List<SpriteRenderer>();
        //    for (int i = 0; i < GameManager.Instance.BackCards.Length; i++)
        //    {
        //        spr.Add(GameManager.Instance.BackCards[i].GetComponent<SpriteRenderer>());
        //    }

        //    while (spr[0].color.a >= 0)
        //    {
        //        foreach (SpriteRenderer s in spr)
        //        {
        //            s.color = new Vector4(s.color.r, s.color.g, s.color.b, s.color.a - 0.01f);
        //        }
        //        yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    }

        //    foreach (GameObject g in GameManager.Instance.BackCards)
        //    {
        //        g.SetActive(false);
        //    }

        //    // FadeIn Front Cards
        //    spr.Clear();
        //    for (int i = 0; i < GameManager.Instance.FrontCards.Length; i++)
        //    {
        //        spr.Add(GameManager.Instance.FrontCards[i].GetComponent<SpriteRenderer>());
        //        spr[i].color = new Vector4(spr[i].color.r, spr[i].color.g, spr[i].color.b, 0);
        //        GameManager.Instance.FrontCards[i].SetActive(true);
        //    }

        //    // TODO : Effect
        //    GameManager.Instance.FrontCards[GameManager.Instance.OpenedCard].transform
        //        .GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Opened";
        //    int tempIdx = 0;
        //    if (GameManager.Instance.OpenedCard == GameManager.Instance.BannedCard) tempIdx = 1;
        //    GameManager.Instance.FrontCards[GameManager.Instance.BannedCard].transform
        //        .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Banned";

        //    while (spr[0].color.a < 1)
        //    {
        //        foreach (SpriteRenderer s in spr)
        //        {
        //            s.color = new Vector4(s.color.r, s.color.g, s.color.b, s.color.a + 0.01f);
        //        }
        //        yield return new WaitForSecondsRealtime(0.01f * fadingTime);
        //    }

        //    // Set card state
        //    GameManager.Instance.SetAllFrontCardsState(CardState.Selective);
        //    GameManager.Instance.SetFrontCardState(GameManager.Instance.BannedCard, CardState.NotBeChanged);

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
        //        if (GameManager.Instance.SecondSelectedCard == -1)
        //        {
        //            GameManager.Instance.SecondSelectedCard = GameManager.Instance.PickRandomCard();
        //        }

        //        // Lock
        //        GameManager.Instance.SetAllFrontCardsState(CardState.NotBeChanged);

        //        // TODO : Effect
        //        GameManager.Instance.FrontCards[GameManager.Instance.FirstSelctedCard].transform
        //            .GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "First Selected";
        //        int tempIdx = 0;
        //        if (GameManager.Instance.SecondSelectedCard == GameManager.Instance.OpenedCard) tempIdx = 1;
        //        else if (GameManager.Instance.SecondSelectedCard == GameManager.Instance.FirstSelctedCard) tempIdx = 1;
        //        GameManager.Instance.FrontCards[GameManager.Instance.SecondSelectedCard].transform
        //            .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Second Selected";
                
        //        // Anyway just send selected card (by random or recent selected)
        //        EndMySequence(new object[] { GameManager.Instance.SecondSelectedCard });
        //    }
        //}

        //// Attached to "End turn Button" or Called when sequence timeout occured
        //public void EndSelection()
        //{
        //    if (!isSent)
        //    {
        //        isSent = true;
        //        Debug.Log("EndSelection()");

        //        if (GameManager.Instance.SecondSelectedCard == -1)
        //        {
        //            Debug.Log("Card not selected");
        //            isSent = false;
        //            // TODO : Effect or Popup
        //            return;
        //        }

        //        // Lock
        //        GameManager.Instance.SetAllFrontCardsState(CardState.NotBeChanged);

        //        // TODO : Effect
        //        GameManager.Instance.FrontCards[GameManager.Instance.FirstSelctedCard].transform
        //            .GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = "First Selected";
        //        int tempIdx = 0;
        //        if (GameManager.Instance.SecondSelectedCard == GameManager.Instance.OpenedCard) tempIdx = 1;
        //        else if (GameManager.Instance.SecondSelectedCard == GameManager.Instance.FirstSelctedCard) tempIdx = 1;
        //        GameManager.Instance.FrontCards[GameManager.Instance.SecondSelectedCard].transform
        //            .GetChild(0).GetChild(tempIdx).GetComponent<TextMeshProUGUI>().text = "Second Selected";

        //        StopCoroutine(Timer());

        //        EndMySequence(new object[] { GameManager.Instance.SecondSelectedCard });
        //    }
        //}
    }
}