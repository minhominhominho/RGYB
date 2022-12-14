using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using UnityEngine.Analytics;
using UnityEngine.SocialPlatforms.Impl;
using System.Reflection;
using Unity.VisualScripting.Antlr3.Runtime;
using Photon.Realtime;

namespace RGYB
{
    [Serializable]
    public enum Menu { Home, Matching, Custom, Stat, Profile, Shop, Option, Exit }

    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance = null;
        private WaitForSeconds fadeWait = new WaitForSeconds(0.025f);
        private WaitForSeconds wait = new WaitForSeconds(0.001f);
        [HideInInspector] public bool IsResetPhase = false;

        [Header("Main")]
        [SerializeField] private CanvasGroup Logo;
        [SerializeField] private GameObject[] CanvasObject;
        [SerializeField] private GameObject[] ContentObject;
        private ContentPanel[] ContentPanels;
        [SerializeField] private CanvasGroup FadeCanvasGroup;
        private Menu prevMenu;

        [Header("Home")]
        [SerializeField] private Image Home_PortraitImage;
        [SerializeField] private TextMeshProUGUI Home_NickNameText;
        [SerializeField] private TextMeshProUGUI Home_TitleText;
        [SerializeField] private TextMeshProUGUI Home_CreditText;
        [SerializeField] private GameObject[] Home_Quests;
        [SerializeField] private Image Home_AdImage;
        [SerializeField] private TextMeshProUGUI Home_AdCreditText;

        [Header("Matching")]
        [SerializeField] private TextMeshProUGUI Matching_ScoreText;
        [SerializeField] private RectTransform Matching_MatchingButtonRect;
        [SerializeField] private Toggle Matching_ClassicToggle;
        [SerializeField] private Toggle Matching_HardcoreToggle;
        private CanvasGroup Matching_Matching_ClassicToggleCanvasGroup;
        private CanvasGroup Matching_HardcoreToggleCanvasGroup;
        private Image Matching_MatchButtonImage;
        private Sprite Matching_NormalButtonSprite;
        [SerializeField] private Sprite Matching_MatchingButtonSprite;
        private bool Matching_isSearching;
        private bool Matching_isLocked;
        private bool Matching_isClassic = true;

        [Header("Custom")]
        [SerializeField] private CanvasGroup Custom_DecoUI;
        [SerializeField] private CanvasGroup Custom_CreateGameUI;
        [SerializeField] private CanvasGroup Custom_SearchGameUI;

        [SerializeField] private GameObject Custom_CreateGamePanelObject;
        [SerializeField] private GameObject Custom_CreateGameIconObject;
        [SerializeField] private GameObject Custom_SearchGamePanelObject;
        [SerializeField] private GameObject Custom_SearchGameIconObject;

        [SerializeField] private GameObject Custom_PanelTargetObject;
        [SerializeField] private GameObject Custom_IconTargetObject;
        [SerializeField] private GameObject Custom_PanelDestObject;
        [SerializeField] private GameObject Custom_IconDestObject;

        [SerializeField] private Toggle Custom_SetClosedToggle;
        [SerializeField] private TMP_InputField Custom_CreateCustomRoomName;
        [SerializeField] private TMP_InputField Custom_CreateCustomRoomPassword;
        [SerializeField] private Toggle Custom_CreateClassicToggle;
        [SerializeField] private Toggle Custom_CreateHardcoreToggle;
        private bool Custom_CreateIsClassic = true;
        [SerializeField] private Toggle Custom_CreateIsFirstToggle;
        [SerializeField] private Toggle Custom_CreateIsSecondToggle;
        [SerializeField] private Toggle Custom_CreateIsRandomToggle;
        private int Custom_CreateRole = 2;

        [SerializeField] private CanvasGroup Custom_CreateGameCanvasGroup;
        [SerializeField] private CanvasGroup Custom_EnterRoomNameAlretPanel;

        [SerializeField] private Button Custom_CustomGameStartButton;
        [SerializeField] private GameObject Custom_CustomLoading;

        [SerializeField] private Sprite Custom_Sprite;
        [SerializeField] private CanvasGroup Custom_WaitRoomCanvasGroup;
        [SerializeField] private TextMeshProUGUI Custom_RoomNameText;
        [SerializeField] private TextMeshProUGUI Custom_Player1RoleText;
        [SerializeField] private TextMeshProUGUI Custom_Player2RoleText;
        [SerializeField] private TextMeshProUGUI Custom_Player1NicknameText;
        [SerializeField] private TextMeshProUGUI Custom_Player2NicknameText;
        [SerializeField] private TextMeshProUGUI Custom_GameModeText;
        [SerializeField] private TextMeshProUGUI Custom_RoleModeText;

        [SerializeField] private CanvasGroup Custom_SearchGameCanvasGroup;

        [SerializeField] private CanvasGroup Custom_DirectJoinCanvasGroup;
        [SerializeField] private CanvasGroup Custom_EnterRoomNameAlretPanel2;
        [SerializeField] private TextMeshProUGUI Custom_EnterRoomNameAlretPanel2Text;
        [SerializeField] private TMP_InputField JoinCustomRoomName;
        [SerializeField] private TMP_InputField JoinCustomRoomPassword;

        [SerializeField] private CanvasGroup Custom_OpponentExitAlretPanel;

        [SerializeField] private GameObject Custom_RoomPrefab;
        [SerializeField] private GameObject Custom_RoomListPanel;

        private bool Custom_IsUpdatingRoomList = false;
        private string Custom_SelectedRoomName;
        private int Custom_SelectedGameMode;
        private int Custom_SelectedRRandomRole;

        [Header("Stat")]

        [Header("Profile")]
        [SerializeField] private CanvasGroup[] Profile_OutsideCanvasGroup;
        [SerializeField] private CanvasGroup[] Profile_InsideCanvasGroup;

        [SerializeField] private Image Profile_PortraitImage;
        [SerializeField] private Image[] Profile_PortraitSelectImage;

        [SerializeField] private TextMeshProUGUI Profile_NickNameText;
        [SerializeField] private TMP_InputField Profile_NickNameInputField;

        [SerializeField] private TextMeshProUGUI Profile_ShownTitleText;
        [SerializeField] private TextMeshProUGUI[] Profile_TitleText;

        [SerializeField] private TextMeshProUGUI Profile_ScoreText;
        [SerializeField] private TextMeshProUGUI Profile_CreditText;

        [SerializeField] private Image Profile_CardSkinImage;
        [SerializeField] private Image[] Profile_CardSkinSelectImage;


        [Header("Shop")]
        [SerializeField] private TextMeshProUGUI Shop_CreditText;
        [SerializeField] private GameObject Shop_CardSkinsFrame;
        [SerializeField] private GameObject Shop_PortraitsFrame;
        [SerializeField] private GameObject Shop_PortraitsPrefab;
        [SerializeField] private GameObject Shop_TitlesFrame;
        [SerializeField] private GameObject Shop_TitlesPrefab;

        [SerializeField] private CanvasGroup Shop_CheckPurchaseCanvasGroup;
        [SerializeField] private CanvasGroup Shop_NotEnoughMoneyCanvasGroup;
        private string shop_tempType;
        private string shop_tempName;


        //[Header("Option")]
        [Header("Exit")]
        [SerializeField] private GameObject Exit_CheckPanel;


        public Menu GetPrevMenu()
        {
            return prevMenu;
        }

        private void Awake()
        {
            Instance = this.GetComponent<MenuManager>();
            PhotonManager.Instance.ConnectPhotonServer();

            ContentPanels = new ContentPanel[ContentObject.Length];
            for (int i = 0; i < ContentObject.Length; i++)
            {
                ContentPanels[i] = ContentObject[i].GetComponent<ContentPanel>();
            }
        }

        public void OpenMainMenu()
        {
            StartCoroutine(WaitAndSetData());
        }

        private IEnumerator WaitAndSetData()
        {
            SoundManager.Instance.PlayMenu(MenuType.Logo);
            Logo.alpha = 1;
            while (Logo.alpha > 0)
            {
                Logo.alpha -= 0.05f;
                yield return fadeWait;
            }
            Logo.alpha = 0;
            Logo.gameObject.SetActive(false);

            SetData();
        }

        public void SetData()
        {
            IsResetPhase = true;
            Debug.Log("SetData");
            // Data : Portrait, NickName, Title, Credit, Score, Gender, MBTI, Tendency
            // Shop : Card, Portrait, Title

            // Home
            if (DataManager.Instance.GetPortrait() == null) Debug.Log("NULL");
            Home_PortraitImage.sprite = DataManager.Instance.GetPortrait();
            Home_NickNameText.text = DataManager.Instance.GetNickName();
            Home_TitleText.text = DataManager.Instance.GetTitle();
            Home_CreditText.text = DataManager.Instance.GetCredit().ToString();
            // TODO : [] Home_Quests;
            // TODO : Home_AdImage;
            // TODO : Home_AdCreditText;

            // Matching
            Matching_ScoreText.text = "점수 : " + DataManager.Instance.GetScore().ToString();

            // Custom
            Custom_CustomGameStartButton.interactable = false;

            // Profile
            ProfileSetData();

            // Shop
            ShopUpdate();
            IsResetPhase = false;
        }

        public void SetFront(Menu menu)
        {
            SoundManager.Instance.PlayMenu(MenuType.MenuSelect);
            IsResetPhase = true;
            #region Initialize Hone Panel
            if (menu == Menu.Home)
                SetData();
            #endregion

            #region Initialize Matching Panel
            if (prevMenu == Menu.Matching)
            {
                if (Matching_isSearching) RandomMatchButton();
                if (!Matching_isClassic) RandomMathcModeToggle();
            }
            #endregion

            #region Initialize Custom Panel
            if (prevMenu == Menu.Custom)
            {
                Custom_CustomLoading.SetActive(true);
                Custom_CustomGameStartButton.interactable = false;
                Custom_OpponentExitAlretPanel.gameObject.SetActive(false);
                Custom_Player2NicknameText.text = null;

                Custom_CreateGameUI.interactable = true;
                Custom_SearchGameUI.interactable = true;
                Custom_SearchGameUI.alpha = 1;
                Custom_CreateGameUI.alpha = 1;
                Custom_DecoUI.alpha = 1;

                Image panelTargetImage = Custom_PanelTargetObject.GetComponent<Image>();
                Image iconTargetImage = Custom_IconTargetObject.GetComponent<Image>();
                var tempColor = panelTargetImage.color;
                tempColor.a = 0f;
                panelTargetImage.color = tempColor;
                iconTargetImage.color = tempColor;

                Custom_SetClosedToggle.SetIsOnWithoutNotify(false);
                Custom_CreateCustomRoomPassword.gameObject.GetComponent<CanvasGroup>().interactable = false;

                if (!Custom_CreateIsClassic) CustomMathcModeToggle();
                if (Custom_CreateRole != 2) CustomSetCreateRoleToggle(2);

                Custom_CreateGameCanvasGroup.gameObject.SetActive(false);
                Custom_CreateGameCanvasGroup.interactable = false;
                Custom_CreateGameCanvasGroup.alpha = 0;

                Custom_SearchGameCanvasGroup.gameObject.SetActive(false);
                Custom_SearchGameCanvasGroup.interactable = false;
                Custom_SearchGameCanvasGroup.alpha = 0;

                Custom_CreateCustomRoomName.text = "";
                Custom_CreateCustomRoomPassword.text = "";

                Custom_EnterRoomNameAlretPanel.gameObject.SetActive(false);

                Custom_WaitRoomCanvasGroup.gameObject.SetActive(false);
                Custom_WaitRoomCanvasGroup.interactable = false;
                Custom_WaitRoomCanvasGroup.alpha = 0;


                Custom_DirectJoinCanvasGroup.gameObject.SetActive(false);
                Custom_DirectJoinCanvasGroup.interactable = false;
                Custom_DirectJoinCanvasGroup.alpha = 0;

                Custom_EnterRoomNameAlretPanel2.gameObject.SetActive(false);
                Custom_EnterRoomNameAlretPanel2.interactable = false;
                Custom_EnterRoomNameAlretPanel2.alpha = 0;
                Custom_EnterRoomNameAlretPanel2Text.text = null;
                JoinCustomRoomName.text = null;
                JoinCustomRoomPassword.text = null;

                PhotonManager.Instance.CheckAndLeaveRoom();
            }
            #endregion

            #region Initialize Profile Panel
            if (menu == Menu.Profile)
            {
                ProfileSetData();
            }
            if (prevMenu == Menu.Profile)
            {
                ProfileResetCanvasGroup();
            }

            #endregion

            #region Initialize Shop Panel
            ShopClosePanel();
            #endregion

            #region Initialize Exit Panel
            ExitButton(false);
            #endregion

            // Move Cards (roll)
            for (int i = 0; i < CanvasObject.Length; i++)
            {
                if (i == 0)
                {
                    ContentPanels[((int)menu + i) % ContentObject.Length].RevealBlockingImage(false);
                }
                else
                {
                    ContentPanels[((int)menu + i) % ContentObject.Length].RevealBlockingImage(true);
                }
                ContentObject[((int)menu + i) % ContentObject.Length].transform.SetParent(CanvasObject[i].transform, false);
                ContentObject[((int)menu + i) % ContentObject.Length].GetComponent<RectTransform>().localPosition = Vector3.zero;
            }
            prevMenu = menu;
            IsResetPhase = false;
        }

        public void FadeInBlack()
        {
            FadeCanvasGroup.gameObject.SetActive(true);
            FadeCanvasGroup.alpha = 0;
            StartCoroutine(fadeInBlack());
        }

        private IEnumerator fadeInBlack()
        {
            while (FadeCanvasGroup.alpha < 1)
            {
                FadeCanvasGroup.alpha += 0.05f;
                yield return fadeWait;
            }
            FadeCanvasGroup.alpha = 1;
        }

        #region Matching
        public void RandomMatchButton()
        {
            if (Matching_MatchButtonImage == null) Matching_MatchButtonImage = Matching_MatchingButtonRect.gameObject.GetComponent<Image>();
            if (Matching_NormalButtonSprite == null) Matching_NormalButtonSprite = Matching_MatchButtonImage.sprite;

            if (Matching_isSearching)
            {
                SoundManager.Instance.PlayMenu(MenuType.MatchStart);
                Matching_isSearching = false;
                Matching_isLocked = true;
                StartCoroutine(freeIsSearching());
                StopCoroutine(spinningButton());
                Debug.Log("StopRandomMatch");
                Matching_MatchButtonImage.sprite = Matching_NormalButtonSprite;
                Matching_MatchingButtonRect.rotation = Quaternion.Euler(Vector3.zero);
                PhotonManager.Instance.CheckAndLeaveRoom();
            }
            else if (!Matching_isSearching && !Matching_isLocked)
            {
                SoundManager.Instance.PlayMenu(MenuType.MatchStart);
                SetInteractableToggles(false);
                Matching_isSearching = true;
                Debug.Log("StartRandomMatch");
                Matching_MatchButtonImage.sprite = Matching_MatchingButtonSprite;
                StartCoroutine(spinningButton());
                PhotonManager.Instance.JoinRandomRoom();
            }
        }

        private IEnumerator spinningButton()
        {
            Vector3 rot = Vector3.zero;
            while (Matching_isSearching)
            {
                rot = new Vector3(0, 0, rot.z + 360 / 20);
                Matching_MatchingButtonRect.rotation = Quaternion.Euler(rot);
                yield return fadeWait;
            }
        }

        private IEnumerator freeIsSearching()
        {
            Matching_MatchButtonImage.color = Color.gray;
            yield return new WaitForSeconds(1f);
            Matching_isLocked = false;
            Matching_MatchButtonImage.color = Color.white;
            SetInteractableToggles(true);
        }

        private void SetInteractableToggles(bool active)
        {
            Matching_ClassicToggle.interactable = active;
            Matching_HardcoreToggle.interactable = active;

            if (Matching_Matching_ClassicToggleCanvasGroup == null)
                Matching_Matching_ClassicToggleCanvasGroup = Matching_ClassicToggle.gameObject.GetComponent<CanvasGroup>();
            if (Matching_HardcoreToggleCanvasGroup == null)
                Matching_HardcoreToggleCanvasGroup = Matching_HardcoreToggle.gameObject.GetComponent<CanvasGroup>();

            if (active)
            {
                Matching_Matching_ClassicToggleCanvasGroup.alpha = 1;
                Matching_HardcoreToggleCanvasGroup.alpha = 1;
            }
            else
            {
                Matching_Matching_ClassicToggleCanvasGroup.alpha = 0.6f;
                Matching_HardcoreToggleCanvasGroup.alpha = 0.6f;
            }
        }

        public void RandomMathcModeToggle()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectToggle);
            if (Matching_isClassic)
            {
                Matching_ClassicToggle.isOn = false;
                Matching_HardcoreToggle.isOn = true;
                Matching_isClassic = false;
            }
            else
            {
                Matching_ClassicToggle.isOn = true;
                Matching_HardcoreToggle.isOn = false;
                Matching_isClassic = true;
            }
        }

        #endregion

        #region Custom

        public void CustomGamePanelButton(bool isCreate)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            if (!isCreate) CustomGetRoomList();
            StartCoroutine(customGamePanelButton(isCreate));
        }

        private IEnumerator customGamePanelButton(bool isCreate)
        {
            Custom_CreateGameUI.interactable = false;
            Custom_SearchGameUI.interactable = false;
            CanvasGroup another = isCreate ? Custom_SearchGameUI : Custom_CreateGameUI;
            CanvasGroup selected = isCreate ? Custom_CreateGameUI : Custom_SearchGameUI;

            Image panelTargetImage = Custom_PanelTargetObject.GetComponent<Image>();
            Image iconTargetImage = Custom_IconTargetObject.GetComponent<Image>();
            var tempColor = panelTargetImage.color;
            tempColor.a = 1f;
            panelTargetImage.color = tempColor;
            iconTargetImage.color = tempColor;

            Vector3 panelStartPos = (isCreate ? Custom_CreateGamePanelObject : Custom_SearchGamePanelObject).transform.position;
            Vector3 panelDestPos = Custom_PanelDestObject.transform.position;
            Vector3 panelStartScale = Custom_CreateGamePanelObject.transform.localScale;
            Vector3 panelDestScale = Custom_PanelDestObject.transform.localScale;
            Vector3 iconStartPos = (isCreate ? Custom_CreateGameIconObject : Custom_SearchGameIconObject).transform.position;
            Vector3 iconDestPos = Custom_IconDestObject.transform.position;

            Custom_PanelTargetObject.transform.position = panelStartPos;
            Custom_PanelTargetObject.transform.localScale = panelStartScale;
            Custom_IconTargetObject.transform.position = iconStartPos;
            iconTargetImage.sprite = (isCreate ? Custom_CreateGameIconObject : Custom_SearchGameIconObject).GetComponent<Image>().sprite;

            // 1. Fade Out none selected & DecoUIM
            selected.alpha = 0;
            while (another.alpha > 0)
            {
                another.alpha -= 0.05f;
                Custom_DecoUI.alpha -= 0.05f;
                yield return fadeWait;
            }
            another.alpha = 0;

            // 2. Move & Extend target panel & icon
            while (Custom_IconTargetObject.transform.position.y < iconDestPos.y)
            {
                Custom_PanelTargetObject.transform.position = new Vector3(
                    Custom_PanelTargetObject.transform.position.x + (panelDestPos.x - panelStartPos.x) / 20,
                    Custom_PanelTargetObject.transform.position.y + (panelDestPos.y - panelStartPos.y) / 20,
                    Custom_PanelTargetObject.transform.position.z + (panelDestPos.x - panelStartPos.z) / 20
                    );
                Custom_PanelTargetObject.transform.localScale = new Vector3(
                    Custom_PanelTargetObject.transform.localScale.x + (panelDestScale.x - panelStartScale.x) / 20,
                    Custom_PanelTargetObject.transform.localScale.y + (panelDestScale.y - panelStartScale.y) / 20,
                    Custom_PanelTargetObject.transform.localScale.z + (panelDestScale.x - panelStartScale.z) / 20
                    );
                Custom_IconTargetObject.transform.position = new Vector3(
                    Custom_IconTargetObject.transform.position.x + (iconDestPos.x - iconStartPos.x) / 20,
                    Custom_IconTargetObject.transform.position.y + (iconDestPos.y - iconStartPos.y) / 20,
                    Custom_IconTargetObject.transform.position.z + (iconDestPos.x - iconStartPos.z) / 20
                    );
                yield return fadeWait;
            }
            Custom_PanelTargetObject.transform.position = panelDestPos;
            Custom_PanelTargetObject.transform.localScale = panelDestScale;
            Custom_IconTargetObject.transform.position = iconDestPos;

            // 3. FadeIn CanvasGroup
            CanvasGroup cgcg = isCreate ? Custom_CreateGameCanvasGroup : Custom_SearchGameCanvasGroup;
            cgcg.gameObject.SetActive(true);
            cgcg.interactable = false;
            cgcg.alpha = 0;
            while (cgcg.alpha < 1)
            {
                cgcg.alpha += 0.05f;
                yield return fadeWait;
            }
            cgcg.alpha = 1;
            cgcg.interactable = true;
        }

        public void CustomSetClosedToggle()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectToggle);
            if (Custom_SetClosedToggle.isOn == false)
            {
                Custom_CreateCustomRoomPassword.gameObject.GetComponent<CanvasGroup>().interactable = false;
            }
            else
            {
                Custom_CreateCustomRoomPassword.gameObject.GetComponent<CanvasGroup>().interactable = true;
            }
        }

        public void CustomMathcModeToggle()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectToggle);
            if (Custom_CreateIsClassic)
            {
                Custom_CreateClassicToggle.isOn = false;
                Custom_CreateHardcoreToggle.isOn = true;
                Custom_CreateIsClassic = false;
            }
            else
            {
                Custom_CreateClassicToggle.isOn = true;
                Custom_CreateHardcoreToggle.isOn = false;
                Custom_CreateIsClassic = true;
            }
        }

        public void CustomSetCreateRoleToggle(int val)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectToggle);
            if (Custom_CreateRole == val)
            {
                Custom_CreateRole = 2;
            }
            else
            {
                Custom_CreateRole = val;
            }

            Custom_CreateIsFirstToggle.SetIsOnWithoutNotify(Custom_CreateRole == 0 ? true : false);
            Custom_CreateIsSecondToggle.SetIsOnWithoutNotify(Custom_CreateRole == 1 ? true : false);
            Custom_CreateIsRandomToggle.SetIsOnWithoutNotify(Custom_CreateRole == 2 ? true : false);
        }

        public void CustomCreateRoomButton()
        {
            Debug.Log("CustomCreateRoomButton()");

            if (Custom_CreateCustomRoomName.text == "")
            {
                Debug.LogWarning("Enter the room name");
                CustomEnterRoomNameAlretPanel(true);
            }
            else
            {
                SoundManager.Instance.PlayMenu(MenuType.EnterWaitRoom);
                PhotonManager.Instance.CreateRoom(
                    Custom_SetClosedToggle.isOn,
                    Custom_CreateCustomRoomName.text,
                    Custom_CreateCustomRoomPassword.text,
                    Custom_CreateRole == 2 ? true : false,
                    Custom_CreateRole == 0 ? true : false,
                    Custom_CreateIsClassic ? 1 : 0
                    );
                StartCoroutine(customCreaterEnterWaitRoom());
            }
        }

        private IEnumerator customCreaterEnterWaitRoom()
        {
            // Map Data
            Custom_RoomNameText.text = Custom_CreateCustomRoomName.text;
            if (Custom_CreateRole == 2)
            {
                Custom_Player1RoleText.text = "랜덤 역할";
                Custom_Player2RoleText.text = "랜덤 역할";
                Custom_RoleModeText.text = "랜덤 역할";
            }
            else if (Custom_CreateRole == 0)
            {
                Custom_Player1RoleText.text = "출제자";
                Custom_Player2RoleText.text = "응시자";
                Custom_RoleModeText.text = "지정 역할";
            }
            else if (Custom_CreateRole == 1)
            {
                Custom_Player1RoleText.text = "응시자";
                Custom_Player2RoleText.text = "출제자";
                Custom_RoleModeText.text = "지정 역할";
            }
            Custom_Player1NicknameText.text = null;
            Custom_Player1NicknameText.text = DataManager.Instance.GetNickName();
            Custom_GameModeText.text = Custom_CreateIsClassic ? "클래식" : "하드코어";

            // Hide & Fade
            Custom_IconTargetObject.GetComponent<Image>().sprite = Custom_Sprite;
            Custom_CreateGameCanvasGroup.interactable = false;
            Custom_CreateGameCanvasGroup.alpha = 0;
            Custom_CreateGameCanvasGroup.gameObject.SetActive(false);

            Custom_WaitRoomCanvasGroup.gameObject.SetActive(true);
            Custom_WaitRoomCanvasGroup.alpha = 0;
            Custom_WaitRoomCanvasGroup.interactable = false;

            while (Custom_WaitRoomCanvasGroup.alpha < 1)
            {
                Custom_WaitRoomCanvasGroup.alpha += 0.05f;
                yield return fadeWait;
            }
            Custom_WaitRoomCanvasGroup.alpha = 1;
            Custom_WaitRoomCanvasGroup.interactable = true;
        }

        public void CustomEnterRoomNameAlretPanel(bool active)
        {
            SoundManager.Instance.PlayMenu(MenuType.MatchUnable);
            StartCoroutine(customFadePanel(Custom_EnterRoomNameAlretPanel, active));
        }

        private IEnumerator customFadePanel(CanvasGroup cg, bool isFadein)
        {
            if (isFadein)
            {
                cg.gameObject.SetActive(true);
                cg.interactable = false;
                cg.alpha = 0;
                while (cg.alpha < 1)
                {

                    cg.alpha += 0.05f;
                    yield return fadeWait;
                }
                cg.alpha = 1;
                cg.interactable = true;
            }
            else
            {
                cg.interactable = false;
                cg.alpha = 1;
                while (cg.alpha > 0)
                {
                    cg.alpha -= 0.05f;
                    yield return fadeWait;
                }
                cg.alpha = 0;
                cg.gameObject.SetActive(false);
            }
        }

        public void CustomGameStartButton()
        {
            if (PhotonManager.instance.IsSceneReserved() && Custom_Player2NicknameText.text != null)
            {
                SoundManager.Instance.PlayMenu(MenuType.MatchStart);
                PhotonManager.instance.AlertStartGame();
                PhotonManager.instance.LoadLevel();
            }
        }

        public void CustomAlertOpponentExit(bool isOpen)
        {
            if (isOpen) SoundManager.Instance.PlayMenu(MenuType.MatchUnable);
            else SoundManager.Instance.PlayMenu(MenuType.SelectButton);

            Custom_OpponentExitAlretPanel.gameObject.SetActive(isOpen);
        }

        public void CustomOnPlayerEntered(string opponentNickName)
        {
            SoundManager.Instance.PlayMenu(MenuType.MatchStart);
            Custom_Player2NicknameText.text = opponentNickName;
            Custom_CustomGameStartButton.interactable = true;
            Custom_CustomLoading.SetActive(false);

            int gameMode = Custom_CreateIsClassic ? 1 : 0;
            int p1Role = Custom_CreateRole;
            string p1Name = DataManager.Instance.GetNickName();
            string roomName = Custom_CreateCustomRoomName.text;

            PhotonManager.Instance.SendRoomData(roomName, p1Name, gameMode, p1Role);
        }

        public void CustomDirectJoinButton()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Custom_SearchGameCanvasGroup.interactable = false;
            Custom_SearchGameCanvasGroup.alpha = 0;
            Custom_SearchGameCanvasGroup.gameObject.SetActive(false);
            StartCoroutine(customFadePanel(Custom_DirectJoinCanvasGroup, true));
        }

        public void CustomJoinRoomButton()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Debug.Log("JoinCustomRoom()");

            if (JoinCustomRoomName.text == "")
            {
                Custom_EnterRoomNameAlretPanel2Text.text = "방 제목을 입력하세요.";
                CustomCloseAlert(true);
            }
            else
            {
                PhotonManager.Instance.JoinCustomRoom(JoinCustomRoomName.text, JoinCustomRoomPassword.text);
            }
        }

        public void CustomFailToJoin()
        {
            SoundManager.Instance.PlayMenu(MenuType.MatchUnable);
            Custom_EnterRoomNameAlretPanel2Text.text = "방 입장에 실패했습니다.";
            if (!Custom_IsUpdatingRoomList)
            {
                PhotonManager.Instance.PullRoomList();
                StartCoroutine(CustomMapRoomList());
            }
            CustomCloseAlert(true);
        }

        public void CustomCloseAlert(bool active)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            StartCoroutine(customFadePanel(Custom_EnterRoomNameAlretPanel2, active));
        }

        public void CustomOnJoinedCustomRoom()
        {
            StartCoroutine(customJoinerEnterWaitRoom());
        }

        private IEnumerator customJoinerEnterWaitRoom()
        {
            while (!PhotonManager.Instance.IsGotRoomData && prevMenu == Menu.Custom)
            {
                yield return wait;
            }
            Debug.Log("Reach");
            if (prevMenu == Menu.Custom)
            {
                SoundManager.Instance.PlayMenu(MenuType.EnterWaitRoom);

                // Hide & Fade
                Custom_IconTargetObject.GetComponent<Image>().sprite = Custom_Sprite;
                Custom_SearchGameCanvasGroup.interactable = false;
                Custom_SearchGameCanvasGroup.alpha = 0;
                Custom_SearchGameCanvasGroup.gameObject.SetActive(false);

                Custom_CustomLoading.SetActive(false);
                Custom_WaitRoomCanvasGroup.gameObject.SetActive(true);
                Custom_WaitRoomCanvasGroup.alpha = 0;
                Custom_WaitRoomCanvasGroup.interactable = false;

                while (Custom_WaitRoomCanvasGroup.alpha < 1)
                {
                    Custom_WaitRoomCanvasGroup.alpha += 0.05f;
                    yield return fadeWait;
                }
                Custom_WaitRoomCanvasGroup.alpha = 1;
                Custom_WaitRoomCanvasGroup.interactable = true;
            }
        }

        public void CustomSetWaitRoomData(string roomName, string p1Name, int gameMode, int p1Role)
        {
            // Map Data
            Custom_RoomNameText.text = roomName;
            Custom_Player1NicknameText.text = p1Name;
            Custom_Player2NicknameText.text = DataManager.Instance.GetNickName();
            Custom_GameModeText.text = gameMode == 1 ? "클래식" : "하드코어";

            if (p1Role == 2)
            {
                Custom_Player1RoleText.text = "랜덤 역할";
                Custom_Player2RoleText.text = "랜덤 역할";
                Custom_RoleModeText.text = "랜덤 역할";
            }
            else if (p1Role == 1)
            {
                Custom_Player1RoleText.text = "응시자";
                Custom_Player2RoleText.text = "출제자";
                Custom_RoleModeText.text = "지정 역할";
            }
            else if (p1Role == 0)
            {
                Custom_Player1RoleText.text = "출제자";
                Custom_Player2RoleText.text = "응시자";
                Custom_RoleModeText.text = "지정 역할";
            }
        }

        public void CustomGetRoomList()
        {
            if (!Custom_IsUpdatingRoomList)
            {
                SoundManager.Instance.PlayMenu(MenuType.SelectButton);
                PhotonManager.Instance.PullRoomList();
                StartCoroutine(CustomMapRoomList());
            }
            else
            {
                SoundManager.Instance.PlayMenu(MenuType.MatchUnable);
            }

        }

        private IEnumerator CustomMapRoomList()
        {
            Custom_IsUpdatingRoomList = true;
            float timeOut = 2f;
            float passedTime = 0;

            for(int i=0;i< Custom_RoomListPanel.transform.childCount; i++)
            {
                Custom_RoomListPanel.transform.GetChild(i).gameObject.SetActive(false);
            }

            while (PhotonManager.Instance.CurrentRoomList == null && passedTime < timeOut)
            {
                passedTime += 0.025f;
                yield return fadeWait;
            }

            if (passedTime < timeOut)
            {
                for (int i = 0; i < PhotonManager.Instance.CurrentRoomList.Count; i++)
                {
                    Custom_RoomPrefab.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = PhotonManager.Instance.CurrentRoomList[i].Name;
                    Custom_RoomPrefab.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = (int)PhotonManager.Instance.CurrentRoomList[i].CustomProperties["GameMode"] == 1 ? "클래식" : "하드코어";
                    Custom_RoomPrefab.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (int)PhotonManager.Instance.CurrentRoomList[i].CustomProperties["RandomRole"] == 1 ? "랜덤 역할" : "지정 역할";
                    Instantiate(Custom_RoomPrefab, Custom_RoomListPanel.transform);
                }
            }
            else
            {
                Debug.LogError("TimeOut!");
            }

            Custom_IsUpdatingRoomList = false;
        }

        public void CustomSelectRoom(string RoomName)
        {
            Custom_SelectedRoomName = RoomName;
        }

        public void CustomEnterSelectRoom()
        {
            if (Custom_SelectedRoomName == null || Custom_SelectedRoomName == "")
            {
                CustomFailToJoin();
                return;
            }
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            PhotonManager.Instance.JoinCustomRoom(Custom_SelectedRoomName, "");
        }

        #endregion

        #region Stat
        // TODO : Implement
        #endregion

        #region Profile
        public void ProfileSetData()
        {
            Profile_PortraitImage.sprite = DataManager.Instance.GetPortrait();
            Profile_NickNameInputField.text = DataManager.Instance.GetNickName();
            Profile_NickNameText.text = DataManager.Instance.GetNickName();
            Profile_ShownTitleText.text = DataManager.Instance.GetTitle();
            Profile_ScoreText.text = "점수 : " + DataManager.Instance.GetScore().ToString();
            Profile_CreditText.text = DataManager.Instance.GetCredit().ToString();
            Profile_CardSkinImage.sprite = DataManager.Instance.GetCardSkin();
        }

        public void ProfileShowInsideCanvasGroup(int index)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            if (Profile_InsideCanvasGroup[index].gameObject.activeSelf)
            {
                for (int i = 0; i < Profile_OutsideCanvasGroup.Length; i++)
                {
                    Profile_OutsideCanvasGroup[i].interactable = true;
                }
                Profile_InsideCanvasGroup[index].gameObject.SetActive(false);
            }
            else
            {
                for (int i = 0; i < Profile_OutsideCanvasGroup.Length; i++)
                {
                    Profile_OutsideCanvasGroup[i].interactable = false;
                }

                Profile_OutsideCanvasGroup[index].interactable = true;
                Profile_InsideCanvasGroup[index].gameObject.SetActive(true);

                if (index == 0)
                {
                    List<Shop> list = DataManager.Instance.GetShopList("Portrait", 1);
                    for (int i = 0; i < 4; i++)
                    {
                        Profile_PortraitSelectImage[i].gameObject.SetActive(false);
                    }

                    int firstIndex = -1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Name == Profile_PortraitImage.sprite.name)
                        {
                            firstIndex = i;
                        }
                    }

                    int len = list.Count < 4 ? list.Count : 4;
                    for (int i = 0; i < len; i++)
                    {
                        Profile_PortraitSelectImage[i].gameObject.SetActive(true);
                        Profile_PortraitSelectImage[i].sprite = list[(i + firstIndex) % list.Count].sprite;
                    }
                }
                else if (index == 2)
                {
                    List<Shop> list = DataManager.Instance.GetShopList("Title", 1);
                    for (int i = 0; i < Profile_TitleText.Length; i++)
                    {
                        if (i < list.Count)
                        {
                            Profile_TitleText[i].gameObject.SetActive(true);
                            Profile_TitleText[i].text = list[i].Name;
                        }
                        else Profile_TitleText[i].gameObject.SetActive(false);
                    }
                }
                else if (index == 7)
                {
                    List<Shop> list = DataManager.Instance.GetShopList("CardSkin", 1);
                    for (int i = 0; i < 4; i++)
                    {
                        Profile_CardSkinSelectImage[i].gameObject.SetActive(false);
                    }

                    int firstIndex = -1;
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].Name == Profile_CardSkinImage.sprite.name)
                        {
                            firstIndex = i;
                        }
                    }

                    int len = list.Count < 4 ? list.Count : 4;
                    for (int i = 0; i < len; i++)
                    {
                        Profile_CardSkinSelectImage[i].gameObject.SetActive(true);
                        Profile_CardSkinSelectImage[i].sprite = list[(i + firstIndex) % list.Count].sprite;
                    }
                }
            }
        }

        public void ProfileResetCanvasGroup()
        {
            for (int i = 0; i < Profile_OutsideCanvasGroup.Length; i++)
            {
                Profile_OutsideCanvasGroup[i].interactable = true;
                Profile_InsideCanvasGroup[i].gameObject.SetActive(false);
            }
        }

        public void ProfileSetPortrait(int index)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Profile_PortraitImage.sprite = Profile_PortraitSelectImage[index].sprite;
            DataManager.Instance.SetPortrait(Profile_PortraitImage.sprite.name);
            ProfileResetCanvasGroup();
        }

        public void ProfileRoll(bool isRight)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            List<Shop> list = DataManager.Instance.GetShopList("Portrait", 1);
            if (list.Count > 4)
            {
                int firstIndex = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Name == Profile_PortraitSelectImage[0].sprite.name)
                    {
                        firstIndex = i;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (isRight)
                        Profile_PortraitSelectImage[i].sprite = list[(list.Count + i + firstIndex + 1) % list.Count].sprite;
                    else
                        Profile_PortraitSelectImage[i].sprite = list[(list.Count + i + firstIndex - 1) % list.Count].sprite;
                }
            }
        }

        public void ProfileSetCardSkin(int index)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Profile_CardSkinImage.sprite = Profile_CardSkinSelectImage[index].sprite;
            DataManager.Instance.SetCardSkin(Profile_CardSkinImage.sprite.name);
            ProfileResetCanvasGroup();
        }

        public void ProfileCardSkinRoll(bool isRight)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            List<Shop> list = DataManager.Instance.GetShopList("CardSkin", 1);
            if (list.Count > 4)
            {
                int firstIndex = -1;
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].Name == Profile_CardSkinSelectImage[0].sprite.name)
                    {
                        firstIndex = i;
                    }
                }

                for (int i = 0; i < 4; i++)
                {
                    if (isRight)
                        Profile_CardSkinSelectImage[i].sprite = list[(list.Count + i + firstIndex + 1) % list.Count].sprite;
                    else
                        Profile_CardSkinSelectImage[i].sprite = list[(list.Count + i + firstIndex - 1) % list.Count].sprite;
                }
            }
        }

        public void ProfileSetNickName()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            DataManager.Instance.SetNickName(Profile_NickNameInputField.text);
            Profile_NickNameText.text = Profile_NickNameInputField.text;
            ProfileResetCanvasGroup();
        }

        public void ProfileSetTitle(int index)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            DataManager.Instance.SetTitle(Profile_TitleText[index].text);
            Profile_ShownTitleText.text = Profile_TitleText[index].text;
            ProfileResetCanvasGroup();
        }

        #endregion

        #region Shop
        private void ShopUpdate()
        {
            // Credit
            Shop_CreditText.text = DataManager.Instance.GetCredit().ToString();

            // CardSkins
            List<Shop> list = DataManager.Instance.GetShopList("CardSkin", 0);
            for (int i = 0; i < Shop_CardSkinsFrame.transform.childCount; i++)
            {
                Shop_CardSkinsFrame.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < list.Count; i++)
            {
                Shop_CardSkinsFrame.transform.GetChild(i).GetComponent<Image>().sprite = list[i].sprite;
                Shop_CardSkinsFrame.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = list[i].Price.ToString();
                Shop_CardSkinsFrame.transform.GetChild(i).gameObject.SetActive(true);
            }

            // Portraits
            list = DataManager.Instance.GetShopList("Portrait", 0);
            for (int i = 0; i < Shop_PortraitsFrame.transform.childCount; i++)
            {
                Shop_PortraitsFrame.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < list.Count; i++)
            {
                Shop_PortraitsFrame.transform.GetChild(i).GetComponent<Image>().sprite = list[i].sprite;
                Shop_PortraitsFrame.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = list[i].Price.ToString();
                Shop_PortraitsFrame.transform.GetChild(i).gameObject.SetActive(true);
            }

            // Titles
            list = DataManager.Instance.GetShopList("Title", 0);
            for (int i = 0; i < Shop_TitlesFrame.transform.childCount; i++)
            {
                Shop_TitlesFrame.transform.GetChild(i).gameObject.SetActive(false);
            }
            for (int i = 0; i < list.Count; i++)
            {
                Shop_TitlesFrame.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = list[i].Name;
                Shop_TitlesFrame.transform.GetChild(i).GetChild(1).GetComponent<TextMeshProUGUI>().text = list[i].Price.ToString();
                Shop_TitlesFrame.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public void ShopBuyItem()
        {
            if (DataManager.Instance.BuyItem(shop_tempType, shop_tempName))
            {
                SoundManager.Instance.PlayMenu(MenuType.Purchase);
                ShopUpdate();
                ShopClosePanel();
            }
            else
            {
                SoundManager.Instance.PlayMenu(MenuType.MatchUnable);
                ShopNotEnoughMoney();
            }
        }

        public void ShopCheckBuyItem(string itemType, string itemName)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Shop_CheckPurchaseCanvasGroup.gameObject.SetActive(true);
            shop_tempType = itemType;
            shop_tempName = itemName;
        }

        public void ShopNotEnoughMoney()
        {
            Shop_NotEnoughMoneyCanvasGroup.gameObject.SetActive(true);
        }

        public void ShopClosePanel()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Shop_CheckPurchaseCanvasGroup.gameObject.SetActive(false);
            Shop_NotEnoughMoneyCanvasGroup.gameObject.SetActive(false);
        }

        #endregion

        #region Exit
        public void ExitButton(bool isExit)
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Exit_CheckPanel.SetActive(isExit);
        }

        public void ExitGame()
        {
            SoundManager.Instance.PlayMenu(MenuType.SelectButton);
            Debug.Log("ExitGame()");
            Application.Quit();
        }
        #endregion

    }
}