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

namespace RGYB
{
    [Serializable]
    public enum Menu { Home, Matching, Custom, Stat, Profile, Shop, Option, Exit }

    public class MenuManager : MonoBehaviour
    {
        public static MenuManager Instance = null;
        private WaitForSeconds fadeWait = new WaitForSeconds(0.025f);
        // private WaitForSeconds normalWait = new WaitForSeconds(0.01f);

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

        [Header("Stat")]

        [Header("Profile")]
        [SerializeField] private CanvasGroup[] Profile_OutsideCanvasGroup;
        [SerializeField] private CanvasGroup[] Profile_InsideCanvasGroup;

        [SerializeField] private Image Profile_PortraitImage;
        [SerializeField] private Image[] Profile_PortraitSelectImage;
        [SerializeField] private GameObject[] Profile_PortraitButtons;

        [SerializeField] private TextMeshProUGUI Profile_NickNameText;
        [SerializeField] private TMP_InputField Profile_NickName;

        [SerializeField] private TextMeshProUGUI Profile_ShownTitleText;
        [SerializeField] private TextMeshProUGUI[] Profile_TitleText;

        [SerializeField] private TextMeshProUGUI Profile_ScoreText;


        [Header("Shop")]
        [SerializeField] private TextMeshProUGUI Shop_CreditText;
        //[Header("Option")]
        //[Header("Exit")]



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

            // Shop
            Shop_CreditText.text = DataManager.Instance.GetCredit().ToString();

        }

        public void SetFront(Menu menu)
        {
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
                Profile_PortraitImage.sprite = DataManager.Instance.GetPortrait();
                Profile_NickName.text = DataManager.Instance.GetNickName();
                Profile_NickNameText.text = DataManager.Instance.GetNickName();
                Profile_ShownTitleText.text = DataManager.Instance.GetTitle();
                Profile_ScoreText.text = "점수 : " + DataManager.Instance.GetScore().ToString();
            }
            if (prevMenu == Menu.Profile)
            {
                ProfileResetCanvasGroup();
            }

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
            }
            prevMenu = menu;
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
                PhotonManager.Instance.CreateRoom(Custom_SetClosedToggle.isOn, Custom_CreateCustomRoomName.text, Custom_CreateCustomRoomPassword.text, Custom_CreateRole == 2 ? true : false, Custom_CreateRole == 0 ? true : false);
                StartCoroutine(customEnterWaitRoom());
            }
        }

        private IEnumerator customEnterWaitRoom()
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
                PhotonManager.instance.SendAllow();
                PhotonManager.instance.LoadLevel();
            }
        }

        public void SetNickName(string opponentNickName)
        {
            Custom_Player2NicknameText.text = opponentNickName;
        }

        public void CustomDirectJoinButton()
        {
            Custom_SearchGameCanvasGroup.interactable = false;
            Custom_SearchGameCanvasGroup.alpha = 0;
            Custom_SearchGameCanvasGroup.gameObject.SetActive(false);
            StartCoroutine(customFadePanel(Custom_DirectJoinCanvasGroup, true));
        }

        public void CustomJoinRoomButton()
        {
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
            Custom_EnterRoomNameAlretPanel2Text.text = "방 입장에 실패했습니다.";
            CustomCloseAlert(true);
        }

        public void CustomCloseAlert(bool active)
        {
            StartCoroutine(customFadePanel(Custom_EnterRoomNameAlretPanel2, active));
        }

        #endregion

        #region Stat
        // TODO : Implement
        #endregion

        #region Profile
        public void ProfileShowInsideCanvasGroup(int index)
        {
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
                    List<Sprite> sprites = DataManager.Instance.GetMyPortraits();
                    for (int i = 0; i < sprites.Count; i++)
                    {
                        Profile_PortraitSelectImage[i].enabled = true;
                        Profile_PortraitSelectImage[i].sprite = sprites[i];
                    }
                    for (int i = 3; i > 0; i--)
                    {
                        if (i >= sprites.Count) Profile_PortraitSelectImage[i].enabled = false;
                    }
                }
                else if (index == 2)
                {
                    List<String> list = DataManager.Instance.GetMyTitles();
                    for (int i = 0; i < Profile_TitleText.Length; i++)
                    {
                        if (i < list.Count)
                        {
                            Profile_TitleText[i].gameObject.SetActive(true);
                            Profile_TitleText[i].text = list[i];
                        }
                        else Profile_TitleText[i].gameObject.SetActive(false);
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
            Profile_PortraitImage.sprite = Profile_PortraitSelectImage[index].sprite;
            DataManager.Instance.SetPortrait(Profile_PortraitImage.sprite.name);
        }

        public void ProfileSetNickName()
        {
            DataManager.Instance.SetNickName(Profile_NickName.text);
            Profile_NickNameText.text = Profile_NickName.text;
            ProfileResetCanvasGroup();
        }

        public void ProfileSetTitle(int index)
        {
            DataManager.Instance.SetTitle(Profile_TitleText[index].text);
            Profile_ShownTitleText.text = Profile_TitleText[index].text;
        }

        #endregion

        #region Shop

        #endregion
        public void ExitGame()
        {
            Debug.Log("ExitGame()");
            Application.Quit();
        }
    }
}