using ExitGames.Client.Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RGYB
{
    public class SequenceObject : MonoBehaviour
    {
        [HideInInspector] public Coroutine SequenceJobCoroutine;
        [HideInInspector] public Coroutine CheckOpponentSequenceCoroutine;
        [HideInInspector] public byte MyOrder;
        [HideInInspector] public float PassedTime;
        [HideInInspector] public const float ExtraTimeForReciever = 3.0f;
        [HideInInspector] public const float FadingTime = 0.5f;


        public virtual void CallMySequence(int order)
        {
            MyOrder = (byte)order;
            PassedTime = 0;
            this.gameObject.SetActive(true);
            SequenceJobCoroutine = StartCoroutine(SequenceJob());
        }

        // Overall flow of the sequence including effects
        public virtual IEnumerator SequenceJob()
        {
            Debug.Log("SequenceJob()");
            yield return new WaitForSecondsRealtime(3);
            EndMySequence(new object[] { });
        }

        public virtual void EndMySequence(object[] eventContent)
        {
            Debug.Log("EndMySequence()");
            PhotonManager.Instance.Synchronization(MyOrder, eventContent);
            CheckOpponentSequenceCoroutine = StartCoroutine(CheckOpponentSequence());
        }

        public virtual IEnumerator CheckOpponentSequence()
        {
            Debug.Log("CheckOpponentSequence()");
            while (MyOrder != GameManager.Instance.OpponentOrder)
            {
                yield return new WaitForSecondsRealtime(0.01f);
            }
            EndSequence();
        }

        // 각각의 SequenceObject가 각자의 일 끝내고 알아서 호출해야됨
        public virtual void EndSequence()
        {
            Debug.Log("EndSequence()");
            GameManager.Instance.CallNextSequence();
            this.gameObject.SetActive(false);
        }

        public IEnumerator Timer(float time)
        {
            // TODO : 빨간줄 타는 효과 구현
            yield return new WaitForSecondsRealtime(0);
        }
    }
}