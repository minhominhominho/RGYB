using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using ExitGames.Client.Photon;
using System;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Reflection;
using UnityEngine.UI;

namespace RGYB
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        private WaitForSeconds normalWait = new WaitForSeconds(0.01f);
        private WaitForSeconds fadingWait = new WaitForSeconds(0.05f * CanvasGroupFadingTime);

        [Header("UI")]
        [SerializeField] private const float CanvasGroupFadingTime = 0.5f;
        [SerializeField] private float CanvasGroupShownTime = 5f;
        [HideInInspector] public CanvasGroup CurrentCanvasGroup = null;
        private bool isClosingCurrentCanvasGroup = false;

        public CanvasGroup SequenceCanvas;
        public CanvasGroup ButtonUI;
        public CanvasGroup EmotionSelectUI;
        public CanvasGroup SelectPopUpFromButtonUI;
        public CanvasGroup OptionPopUpFromButtonUI;
        public CanvasGroup ExitPopUpFromButtonUI;

        public GameObject SelectButton;
        public Image EmotionButton;
        public Image MyEmotion;
        public Image OpponentEmotion;
        [SerializeField] private Sprite[] Emotions;

        [Header("Game")]
        public GameObject GameBoardMask;
        public GameObject[] TurnSignMask;
        public GameObject[] TurnSignMaskDestPosition;

        public GameObject[] FrontCards;
        public GameObject[] BackCards;
        private List<CardEffect> frontCardEffects = new List<CardEffect>();

        // 0 : Opponent, 1 : Ban, 2 : Open, 3 : My
        public SpriteRenderer[] Submits;
        public Sprite[] CardSprites;
        public GameObject CannotBeSelectedEffectObject;
        public GameObject BanEffectObject;
        public GameObject OpenEffectObject;

        public Image TimerImage;

        [Header("GameSequences")]
        [HideInInspector] public bool IsFirstSelectPlayer;
        public GameSequence[] GameSequences;
        [HideInInspector] public int SequenceIndex = -1;

        [HideInInspector] public int OpponentOrder = -1;
        [HideInInspector] public int FirstSelctedCard = -1;
        [HideInInspector] public int OpenedCard = -1;
        [HideInInspector] public int BannedCard = -1;
        [HideInInspector] public int SecondSelectedCard = -1;
        [HideInInspector] public int CannotBeBannedCard = -1;

        private bool isRunExit = false;


#if UNITY_EDITOR
        public void LoadSequence()
        {
            Debug.Log("LoadSequence()");

            // Here, GameSequences has several "GameSequence" with 
            // stringGameSequenceType, MyGameSequenceType, FullSequenceSeconds
            GameSequences = JsonReader.ReadSequence();

            // Active all "SequenceObject" object in the scene to be found
            GameObject parent = GameObject.Find("SequenceObject");
            for (int i = 0; i < parent.transform.childCount; i++) parent.transform.GetChild(i).gameObject.SetActive(true);

            // Get all "SequenceObject" object in the scene
            UnityEngine.Object[] SequenceObjectsInScene = FindObjectsOfType(typeof(SequenceObject));
            int[] visited = new int[SequenceObjectsInScene.Length];

            // Match "SequenceObject" in the scene to the "GameSequence" in the GameSequences
            for (int i = 0; i < GameSequences.Length; i++)
            {
                for (int j = 0; j < SequenceObjectsInScene.Length; j++)
                {
                    if (visited[j] == 0 && SequenceObjectsInScene[j].name.Contains(GameSequences[i].stringGameSequenceType))
                    {
                        visited[j] = 1;
                        GameSequences[i].MySequenceObject = SequenceObjectsInScene[j].GetComponent<SequenceObject>();
                        break;
                    }
                }
            }

            for (int i = 0; i < GameSequences.Length; i++)
            {
                if (SequenceCanvas.transform.Find(GameSequences[i].stringGameSequenceType) == null) continue;
                GameSequences[i].CavasGroupObject =
                  SequenceCanvas.transform.Find(GameSequences[i].stringGameSequenceType).GetComponent<CanvasGroup>();
            }

            // Deactive all "SequenceObject" object in the scene to hide
            for (int i = 0; i < parent.transform.childCount; i++) parent.transform.GetChild(i).gameObject.SetActive(false);
        }
#endif

        private void Awake()
        {
            Instance = this.GetComponent<GameManager>();

            // TO be on!!!!!
            IsFirstSelectPlayer = PhotonManager.Instance.IsFirstSelectPlayer();

            SetGameEnvironment();
        }

        public void SetGameEnvironment()
        {
            // Set GameBoardMask active
            GameBoardMask.SetActive(true);

            // Initialize info variables
            SequenceIndex = OpponentOrder = FirstSelctedCard = OpenedCard = BannedCard = SecondSelectedCard = -1;

            // Set Cards alpha zero & Set CardEffects
            for (int i = 0; i < FrontCards.Length; i++)
            {
                frontCardEffects.Add(FrontCards[i].GetComponent<CardEffect>());
                FrontCards[i].GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 0);
                BackCards[i].GetComponent<SpriteRenderer>().sprite = DataManager.Instance.GetCardSkin();
                BackCards[i].GetComponent<SpriteRenderer>().color = new Vector4(255, 255, 255, 0);
            }

            // Set card skin
            CardSprites[4] = DataManager.Instance.GetCardSkin();
        }

        private void Start()
        {
            if (GameSequences.Length == 0)
            {
                Debug.Log("No Sequence exist");
                return;
            }

            CallNextSequence();
        }

        public void CallNextSequence()
        {
            if (SequenceIndex >= GameSequences.Length - 1)
            {
                Debug.Log("End of the game");
                return;
            }

            SequenceIndex++;

            Debug.LogFormat("SequenceIndex : {0}, SequenceType : {1}", SequenceIndex, GameSequences[SequenceIndex].MyGameSequenceType);
            GameSequences[SequenceIndex].MySequenceObject.CallMySequence(SequenceIndex);
        }


        #region Button & UI Methods

        public void SetActiveFakeSelectButton(bool isAcitve)
        {
            SelectButton.SetActive(isAcitve);
        }

        public bool CheckPanelClosed()
        {
            return CurrentCanvasGroup == null;
        }

        public void OpenSequenceCanvasGroup(bool isTimer = true)
        {
            OpenCanvasGroup(GameSequences[SequenceIndex].CavasGroupObject, true, true, isTimer);
        }

        public void OpenWrongSelectCanvasGroup()
        {
            SoundManager.Instance.PlaySFX(SFXType.Sequece_CannotSelect);
            OpenCanvasGroup(SelectPopUpFromButtonUI, true, true);
        }

        public void OpenCanvasGroup(CanvasGroup cg, bool isSequenceInfo = false, bool isFade = false, bool isTimer = true)
        {
            if (CurrentCanvasGroup != null)
            {
                if (isSequenceInfo) CloseCurrentCanvasGroup(false);
                else return;
            }

            ButtonUI.interactable = false;
            if (isFade)
            {
                CurrentCanvasGroup = cg;
                if (isSequenceInfo && isTimer) StartCoroutine(sequenceCanvasTimer());
                StartCoroutine(fadeInCurrentCanvasGroup());
            }
            else
            {
                CurrentCanvasGroup = cg;
                CurrentCanvasGroup.gameObject.SetActive(true);
                CurrentCanvasGroup.alpha = 1;
                CurrentCanvasGroup.interactable = true;
            }
        }

        public void CloseCurrentCanvasGroup(bool isFade = false)
        {
            if (CurrentCanvasGroup == null || isClosingCurrentCanvasGroup) return;
            isRunExit = false;

            isClosingCurrentCanvasGroup = true;
            if (isFade)
            {
                StartCoroutine(fadeOutCurrentCanvasGroup());
            }
            else
            {
                CurrentCanvasGroup.interactable = false;
                CurrentCanvasGroup.alpha = 0;
                CurrentCanvasGroup.gameObject.SetActive(false);
                CurrentCanvasGroup = null;
                isClosingCurrentCanvasGroup = false;
                ButtonUI.interactable = true;
            }
        }

        private IEnumerator fadeInCurrentCanvasGroup()
        {
            if ((GameSequenceType)SequenceIndex != GameSequenceType.Result && CurrentCanvasGroup != SelectPopUpFromButtonUI)
                SoundManager.Instance.PlaySFX(SFXType.Sequece_AlertPopUpOpen);
            CurrentCanvasGroup.gameObject.SetActive(true);
            CurrentCanvasGroup.alpha = 0;
            while (CurrentCanvasGroup.alpha < 1)
            {
                CurrentCanvasGroup.alpha += 0.05f;
                yield return fadingWait;
            }
            CurrentCanvasGroup.interactable = true;
        }

        private IEnumerator fadeOutCurrentCanvasGroup()
        {
            SoundManager.Instance.PlaySFX(SFXType.Sequece_AlertPopUpClose);
            while (CurrentCanvasGroup.alpha > 0)
            {
                CurrentCanvasGroup.alpha -= 0.05f;
                yield return fadingWait;
            }
            CurrentCanvasGroup.interactable = false;
            CurrentCanvasGroup.alpha = 0;
            CurrentCanvasGroup.gameObject.SetActive(false);
            CurrentCanvasGroup = null;
            isClosingCurrentCanvasGroup = false;
            ButtonUI.interactable = true;
        }

        // Sequence Canvas will be closed automatically if it is not closed n seconds after opened.
        private IEnumerator sequenceCanvasTimer()
        {
            float passedTime = 0;

            while (CanvasGroupShownTime > passedTime && CurrentCanvasGroup != null)
            {
                passedTime += 0.01f;
                yield return normalWait;
            }

            if (CanvasGroupShownTime <= passedTime) CloseCurrentCanvasGroup(true);
            else yield return null;
        }

        public void ClickButtonUI(string buttonName)
        {
            if (CurrentCanvasGroup == null)
            {
                if (buttonName == "Option")
                {
                    OpenCanvasGroup(OptionPopUpFromButtonUI, false, true);
                }
                else if (buttonName == "Exit")
                {
                    isRunExit = true;
                    OpenCanvasGroup(ExitPopUpFromButtonUI, false, true);
                }
            }
        }

        public void SendEmotion(int num)
        {
            Debug.Log("Num : " + num);
            PhotonManager.Instance.SendEmotion(num);
            StartCoroutine(ShowEmotion(num, true));
        }

        public void GetEmotion(int num)
        {
            StartCoroutine(ShowEmotion(num, false));
        }

        private IEnumerator ShowEmotion(int num, bool isMine) // true -> mine
        {
            SoundManager.Instance.PlaySFX(SFXType.Emotion);
            Image whoseEmotion = isMine ? MyEmotion : OpponentEmotion;

            if (isMine)
            {
                EmotionButton.gameObject.GetComponent<CircleCollider2D>().enabled = false;
                EmotionButton.color = new Vector4(255, 255, 255, 0);
            }

            whoseEmotion.sprite = Emotions[num];

            whoseEmotion.color = new Vector4(255, 255, 255, 0);
            while (whoseEmotion.color.a < 1)
            {
                whoseEmotion.color = new Vector4(255, 255, 255, whoseEmotion.color.a + 0.05f);
                yield return fadingWait;
            }
            yield return new WaitForSeconds(2f);
            while (whoseEmotion.color.a > 0)
            {
                whoseEmotion.color = new Vector4(255, 255, 255, whoseEmotion.color.a - 0.05f);
                yield return fadingWait;
            }
            whoseEmotion.color = new Vector4(255, 255, 255, 0);

            if (isMine)
            {
                while (EmotionButton.color.a < 1)
                {
                    EmotionButton.color = new Vector4(255, 255, 255, EmotionButton.color.a + 0.05f);
                    yield return fadingWait;
                }
                EmotionButton.color = new Vector4(255, 255, 255, 255);
                EmotionButton.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            }
        }

        public void ExitButton()
        {
            if (isRunExit) DataManager.Instance.SetScore(DataManager.Instance.GetScore() - 100);
            PhotonManager.Instance.ExitGame();
        }
        #endregion


        #region Card Methods

        // 0 : Opponent, 1 : Ban, 2 : Open, 3 : My
        public void SetSubmit(int submit, int cardColor)
        {
            Submits[submit].sprite = CardSprites[cardColor];
            StartCoroutine(fadeSubmitCard(submit));
        }

        private IEnumerator fadeSubmitCard(int submit)
        {
            while (Submits[submit].color.a < 1)
            {
                Submits[submit].color = new Vector4(255, 255, 255, Submits[submit].color.a + 0.05f);
                yield return fadingWait;
            }
        }

        public void CannotBeSelectedEffect(bool isActive)
        {
            if (SequenceIndex == (int)GameSequenceType.Ban)
            {
                if (!isActive) Instantiate(CannotBeSelectedEffectObject, FrontCards[CannotBeBannedCard].gameObject.transform);
                else FrontCards[CannotBeBannedCard].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                FrontCards[CannotBeBannedCard].GetComponent<BoxCollider2D>().enabled = isActive ? true : false;
            }
            else if (SequenceIndex == (int)GameSequenceType.OpenNonSelected)
            {
                if (!isActive) Instantiate(CannotBeSelectedEffectObject, FrontCards[FirstSelctedCard].gameObject.transform);
                else FrontCards[FirstSelctedCard].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                FrontCards[FirstSelctedCard].GetComponent<BoxCollider2D>().enabled = isActive ? true : false;
            }
            else if (SequenceIndex == (int)GameSequenceType.SecondSelect)
            {
                if (!isActive) Instantiate(CannotBeSelectedEffectObject, FrontCards[BannedCard].gameObject.transform);
                else FrontCards[BannedCard].gameObject.transform.GetChild(0).gameObject.SetActive(false);
                FrontCards[BannedCard].GetComponent<BoxCollider2D>().enabled = isActive ? true : false;
            }
        }

        public void BanSignEffect()
        {
            BanEffectObject.SetActive(true);
        }

        public void OpenSignEffect()
        {
            OpenEffectObject.SetActive(true);
        }

        public void SetFrontCardState(int index, CardState cardState)
        {
            frontCardEffects[index].SetCardState(cardState);
        }

        public void SetAllFrontCardsState(CardState cardState)
        {
            for (int i = 0; i < frontCardEffects.Count; i++)
            {
                SetFrontCardState(i, cardState);
            }
        }

        public void CardSelected(int cardNum)
        {
            if (SequenceIndex == (int)GameSequenceType.FirstSelect) FirstSelctedCard = cardNum;
            else if (SequenceIndex == (int)GameSequenceType.Ban) BannedCard = cardNum;
            else if (SequenceIndex == (int)GameSequenceType.OpenNonSelected) OpenedCard = cardNum;
            else if (SequenceIndex == (int)GameSequenceType.SecondSelect) SecondSelectedCard = cardNum;

            for (int i = 0; i < frontCardEffects.Count; i++)
            {
                if (SequenceIndex == (int)GameSequenceType.FirstSelect && i == FirstSelctedCard) continue;
                else if (SequenceIndex == (int)GameSequenceType.Ban && i == BannedCard) continue;
                else if (SequenceIndex == (int)GameSequenceType.OpenNonSelected && i == OpenedCard) continue;
                else if (SequenceIndex == (int)GameSequenceType.SecondSelect && i == SecondSelectedCard) continue;

                frontCardEffects[i].SetCardState(CardState.Selective);
                frontCardEffects[i].NoneEffect();
            }
        }

        public void ResetFrontCards()
        {
            for (int i = 0; i < frontCardEffects.Count; i++)
            {
                frontCardEffects[i].NoneEffect();
            }
        }

        public int PickRandomCard()
        {
            List<int> randomPool = new List<int>();

            // Called : FirstSelect, OpenNonSelected, Ban
            if (IsFirstSelectPlayer)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (SequenceIndex == (int)GameSequenceType.Ban && i == CannotBeBannedCard) continue;
                    if (SequenceIndex == (int)GameSequenceType.OpenNonSelected && i == FirstSelctedCard) continue;
                    randomPool.Add(i);
                }
            }
            // Called : SecondSelect
            else
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i == BannedCard) continue;
                    randomPool.Add(i);
                }
            }

            return randomPool[UnityEngine.Random.Range(0, randomPool.Count)];
        }

        #endregion


        public GameResult GetResult()
        {
            if (FirstSelctedCard == SecondSelectedCard) return GameResult.WinTogether;
            if (Math.Abs(FirstSelctedCard - SecondSelectedCard) == 2) return GameResult.LoseTogether;

            if (FirstSelctedCard - SecondSelectedCard == -1 || FirstSelctedCard - SecondSelectedCard == 3)
            {
                if (IsFirstSelectPlayer) return GameResult.WinAlone;
                else return GameResult.LoseAlone;
            }
            else if (SecondSelectedCard - FirstSelctedCard == -1 || SecondSelectedCard - FirstSelctedCard == 3)
            {
                if (IsFirstSelectPlayer) return GameResult.LoseAlone;
                else return GameResult.WinAlone;
            }
            else
            {
                Debug.LogError("It can't be occur!");
                return GameResult.WinTogether;
            }
        }
    }

    [System.Serializable]
    public enum GameResult
    {
        WinAlone, WinTogether, LoseAlone, LoseTogether
    }

    [System.Serializable]
    public enum GameSequenceType
    {
        Start, FirstSelect, Ban, OpenNonSelected, SecondSelect, Result
    }

    [System.Serializable]
    public class GameSequence
    {
        [HideInInspector] public string stringGameSequenceType;
        public GameSequenceType MyGameSequenceType;
        public CanvasGroup CavasGroupObject;
        public SequenceObject MySequenceObject;
        public float FullSequenceSeconds;

        public void Mapping()
        {
            MyGameSequenceType = (GameSequenceType)Enum.Parse(typeof(GameSequenceType), stringGameSequenceType);
        }
    }
}
