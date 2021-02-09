using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;
using _Emulator;

public class Login : MonoBehaviour
{
	public enum LOGIN_STEP
	{
		NONE,
		WAITING_SERVER,
		WAITING_SEED,
		WAITING_LOGIN
	}

    private struct HostIP
    {
        public string ip;
        public string name;
        public GUIContent content;

        public HostIP(string _ip, string _name)
        {
            ip = _ip;
            name = _name;
            content = new GUIContent(_name);
        }
    }

	public GUIDepth.LAYER guiDepth = GUIDepth.LAYER.BOTTOM;

    public string id = string.Empty;

    public string pswd = string.Empty;

	public int maxId = 30;

	public int maxPswd = 30;

	public string[] WelcomeMessages;

    private LangOptManager.LANG_OPT[] languages;

	private Texture2D[] langTex;

	public Texture2D grbIcon;

	public Texture2D back;

	public Texture2D deco;

	private Rect crdBackground = new Rect(0f, 0f, 1024f, 768f);

	private Rect crdGrb = new Rect(900f, 68f, 97f, 55f);

	public Rect crdDeco = new Rect(434f, 630f, 180f, 26f);

	private Rect crdCurLangBtn = new Rect(400f, 590f, 223f, 28f);

    private Rect crdIdTxtFld = new Rect(400f, 635f, 224f, 26f);

    private Rect crdPswdTxtFld = new Rect(400f, 666f, 224f, 26f);

	private Rect crdStartBtn = new Rect(637f, 638f, 50f, 50f);

	private Rect crdExtBtn = new Rect(963f, 4f, 36f, 36f);

	private Vector2 crdVer = new Vector2(84f, 33f);

	private Vector2 crdStep = new Vector2(512f, 568f);

	private Rect crdRemember = new Rect(636f, 590f, 21f, 22f);

	private Rect crdLogo = new Rect(340f, 437f, 343f, 120f);

	private Vector2 crdAccount = new Vector2(388f, 648f);

	private Vector2 crdPassword = new Vector2(388f, 679f);

	private Vector2 crdServerIP = new Vector2(388f, 710f);

    private Vector2 crdCustomIP = new Vector2(386f, 741f);

    private Rect crdHostIPSel = new Rect(400f, 697f, 224f, 26f);

    private Rect crdHostButton = new Rect(631f, 691f, 62f, 38f);

    private Rect crdCustomIPSel = new Rect(400f, 728f, 224f, 26f);

    private Vector2 crdCopyRight = new Vector2(512f, 742f);

	private Vector2 crdPswdRequest = new Vector2(705f, 640f);

	private Vector2 crdRegisterRequest = new Vector2(705f, 670f);

    private int selectedIPIndex = 0;

    private ComboBox ipBox;

    private GUIContent selectedIP;

    private List<HostIP> IPs;

    private GUIContent[] listIPs;

    private LOGIN_STEP loginStep;

	private bool guiOnce;

	private bool returnPressed;

	private bool bFocusPswd;

	private float dtFocus;

	private string welcomeMessage = "BrickForce";

    private bool hostedServer = false;

	private void Reset()
	{
		loginStep = LOGIN_STEP.NONE;
		welcomeMessage = StringMgr.Instance.Get(WelcomeMessages[Random.Range(0, WelcomeMessages.Length)]);
		BuildOption.Instance.ResetSingletons();
		guiOnce = false;
		returnPressed = false;
		dtFocus = 0f;
		pswd = string.Empty;
        hostedServer = false;
        string @string = PlayerPrefs.GetString("myID", string.Empty);
		if (@string.Length <= 0)
		{
			GlobalVars.Instance.bRemember = false;
		}
		else
		{
			id = @string;
			GlobalVars.Instance.bRemember = true;
			bFocusPswd = true;
		}
	}

	private void Start()
	{
		Reset();
		languages = new LangOptManager.LANG_OPT[BuildOption.Instance.Props.supportLanguages.Length];
		langTex = new Texture2D[BuildOption.Instance.Props.supportLanguages.Length];
		for (int i = 0; i < languages.Length; i++)
		{
			languages[i] = BuildOption.Instance.Props.supportLanguages[i];
			langTex[i] = LangOptManager.Instance.languages[(int)languages[i]];
		}
		BuffManager.Instance.ExportBuffs();
		TItemManager.Instance.ExportItems();
		BrickManager.Instance.ExportBricks();

        CSVLoader csv = new CSVLoader();
        IPs = new List<HostIP>();
        if (csv.Load("Config\\ServerIPS.csv"))
        {
            List<string> _added = new List<string>();
            for (int row = 0; row < csv.Rows; row++)
            {
                if (!csv.ReadValue(0, row, "", out string ip))
                    continue;

                if (!csv.ReadValue(1, row, "", out string text))
                    continue;

                if (!_added.Contains(ip))
                {
                    IPs.Add(new HostIP(ip, text));
                    _added.Add(ip);
                }
            }
            IPs.Add(new HostIP(string.Empty, "Custom"));
        }
        else
        {
            // Create file and write default localhost shit to it
            File.AppendAllText("Config\\ServerIPS.csv", "IP\tString\n");
            File.AppendAllText("Config\\ServerIPS.csv", "127.0.0.1\tLocalhost");
            IPs.Add(new HostIP("127.0.0.1", "Localhost"));
            IPs.Add(new HostIP(string.Empty, "Custom"));
        }
    }

	private void OnLanguageChanged()
	{
		welcomeMessage = StringMgr.Instance.Get(WelcomeMessages[Random.Range(0, WelcomeMessages.Length)]);
	}

	private void OnGUI()
	{
        if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Return || Event.current.keyCode == KeyCode.KeypadEnter) && !DialogManager.Instance.IsModal)
		{
			returnPressed = true;
		}
		GUI.depth = (int)guiDepth;
		GUI.skin = GUISkinFinder.Instance.GetGUISkin();
		GUI.enabled = !DialogManager.Instance.IsModal;
		GlobalVars.Instance.BeginGUIWithBox(string.Empty, true);

        crdCopyRight = new Vector2(512f, 742f);

        TextureUtil.DrawTexture(crdBackground, VersionTextureManager.Instance.seasonTexture.texLoginBg, ScaleMode.StretchToFill);
		LabelUtil.TextOut(crdAccount, StringMgr.Instance.Get("ACCOUNT"), "BigLabel", GlobalVars.Instance.txtMainColor, GlobalVars.txtEmptyColor, TextAnchor.MiddleRight);
		LabelUtil.TextOut(crdPassword, StringMgr.Instance.Get("PASSWORD2"), "BigLabel", GlobalVars.Instance.txtMainColor, GlobalVars.txtEmptyColor, TextAnchor.MiddleRight);
        LabelUtil.TextOut(crdServerIP, "Server-IP", "BigLabel", GlobalVars.Instance.txtMainColor, GlobalVars.txtEmptyColor, TextAnchor.MiddleRight);

        if (ipBox == null)
        {
            ipBox = new ComboBox();

            ipBox.Initialize(false, new Vector2(crdHostIPSel.width, crdHostIPSel.height));
            ipBox.setBackground(Color.white, GlobalVars.Instance.txtMainColor);

            if (listIPs == null)
            {
                listIPs = new GUIContent[IPs.Count];

                for (int i = 0; i < IPs.Count; i++)
                    listIPs[i] = new GUIContent(IPs[i].name + (IPs[i].ip != string.Empty ? (" (" + IPs[i].ip + ")") : ""));

                selectedIP = listIPs[0];
                ipBox.SetSelectedItemIndex(0);
            }
        }

        Texture2D logo = BuildOption.Instance.Props.logo;
        if (null != logo)
		{
			TextureUtil.DrawTexture(crdLogo, logo, ScaleMode.StretchToFill);
		}

        if (BuildOption.Instance.Props.ShowGrb)
		{
			TextureUtil.DrawTexture(crdGrb, grbIcon, ScaleMode.StretchToFill);
		}

        if (!BuildOption.Instance.Props.LanguageSelectable)
		{
			TextureUtil.DrawTexture(crdDeco, deco, ScaleMode.StretchToFill);
		}
		else
		{
			int num = -1;
			int num2 = 0;
			while (num < 0 && num2 < languages.Length)
			{
				if (languages[num2] == (LangOptManager.LANG_OPT)LangOptManager.Instance.LangOpt)
				{
					num = num2;
				}
				num2++;
			}
			if (num < 0 || num >= langTex.Length)
			{
				Debug.LogError("Fail to find language options");
			}
			else if (GlobalVars.Instance.MyButton(crdCurLangBtn, langTex[num], "BtnBlue"))
			{
				((ChangeLangDialog)DialogManager.Instance.Popup(DialogManager.DIALOG_INDEX.CHANGE_LANG, exclusive: true))?.InitDialog(crdCurLangBtn.y - 5f);
			}
		}

        if (loginStep != 0)
		{
			GUI.SetNextControlName("IdInput");
			GUI.TextField(crdIdTxtFld, id);
			GUI.SetNextControlName("PswdInput");
			GUI.PasswordField(crdPswdTxtFld, pswd, '*');
        }
		else
		{
			string text = id;
			GUI.SetNextControlName("IdInput");
			id = GUI.TextField(crdIdTxtFld, id);
			if (id.Length > maxId)
            {
				id = text;
			}
			string text2 = pswd;
			GUI.SetNextControlName("PswdInput");
			pswd = GUI.PasswordField(crdPswdTxtFld, pswd, '*');
			if (pswd.Length > maxPswd)
			{
				pswd = text2;
			}
			if (bFocusPswd)
			{
				GUI.FocusControl("PswdInput");
				dtFocus += Time.deltaTime;
				if (dtFocus > 0.2f)
				{
					bFocusPswd = false;
				}
			}
		}

        GlobalVars.Instance.bRemember = GUI.Toggle(crdRemember, GlobalVars.Instance.bRemember, StringMgr.Instance.Get("REMEMBER_ME"));

        if (BuildOption.Instance.Props.PswdRequestURL.Length > 0)
		{
			string text3 = StringMgr.Instance.Get("PASSWORD_REQUEST");
			Vector2 vector = LabelUtil.CalcLength("InvisibleButton", text3);
			if (GUI.Button(new Rect(crdPswdRequest.x, crdPswdRequest.y, vector.x, vector.y), text3, "HyperLink"))
			{
				BuildOption.OpenURL(BuildOption.Instance.Props.PswdRequestURL);
			}
		}

        if (BuildOption.Instance.Props.RegisterURL.Length > 0)
		{
			string text4 = StringMgr.Instance.Get("REGISTER_REQUEST");
			Vector2 vector2 = LabelUtil.CalcLength("InvisibleButton", text4);
			if (GUI.Button(new Rect(crdRegisterRequest.x, crdRegisterRequest.y, vector2.x, vector2.y), text4, "HyperLink"))
			{
				BuildOption.OpenURL(BuildOption.Instance.Props.RegisterURL);
			}
		}

        if (GlobalVars.Instance.MyButton(crdStartBtn, string.Empty, "Start") || returnPressed)
		{
			returnPressed = false;
			if (loginStep == LOGIN_STEP.NONE)
			{
				if (!Application.CanStreamedLevelBeLoaded("Login"))
				{
					MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("STREAMING_WAIT"));
				}
				else
				{
					id.Trim();
					pswd.Trim();
					if (id.Length <= 0)
					{
						MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("INPUT_ACCOUNT"));
					}
					//else if (pswd.Length <= 0)
					//{
					//	MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("INPUT_PSWD"));
					//}
					else
					{
                        ClientExtension.instance.LoadServer();

                        //CSNetManager.Instance.SwitchAfter = new SockTcp();
                        //if (CSNetManager.Instance.SwitchAfter.Open(CSNetManager.Instance.RoundRobinIp, CSNetManager.Instance.RoundRobinPort))
                        //{
                        //	if (CSNetManager.Instance.Sock != null)
                        //	{
                        //		CSNetManager.Instance.Sock.Close();
                        //	}
                        //	loginStep = LOGIN_STEP.WAITING_SERVER;
                        //}
                        //else
                        //{
                        //	MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("NETWORK_FAIL"));
                        //}
                    }
				}
			}
		}

        if (GlobalVars.Instance.MyButton(crdExtBtn, string.Empty, "BtnClose") || GlobalVars.Instance.IsEscapePressed())
		{
			Application.Quit();
		}

        if (GlobalVars.Instance.MyButton3(crdHostButton, new GUIContent("HOST"), "BtnAction"))
        {
            if (!hostedServer)
            {
                hostedServer = true;
                ServerEmulator.instance.SetupServer();
                ClientExtension.instance.LoadServer();
            }
        }

        string curStepString = GetCurStepString();
		LabelUtil.TextOut(crdStep, curStepString, "MiniLabel", Color.gray, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
		Color byteColor2FloatColor = GlobalVars.Instance.GetByteColor2FloatColor(94, 79, 9);
		string str = "Copyright ";
		if (BuildOption.Instance.Props.copyRights != null)
		{
			for (int i = 0; i < BuildOption.Instance.Props.copyRights.Length; i++)
			{
				if (i > 0)
				{
					str += ", ";
				}
				str += BuildOption.Instance.Props.copyRights[i];
			}
		}
		str += " ALL RIGHTS RESERVED.";

        if (selectedIP.text == "Custom")
        {
            GUI.enabled = !ipBox.IsClickedComboButton();
		    LabelUtil.TextOut(crdCustomIP, "Custom-IP", "BigLabel", GlobalVars.Instance.txtMainColor, GlobalVars.txtEmptyColor, TextAnchor.MiddleRight);
            ClientExtension.instance.hostIP = GUI.TextField(crdCustomIPSel, ClientExtension.instance.hostIP);
            crdCopyRight.y += 24f;
        }
        else
        {
            ClientExtension.instance.hostIP = IPs[selectedIPIndex].ip;
        }

        LabelUtil.TextOut(crdCopyRight, str, "MiniLabel", byteColor2FloatColor, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);

        GUI.enabled = true;
        selectedIPIndex = ipBox.List(crdHostIPSel, selectedIP, listIPs);
        selectedIP = IPs[selectedIPIndex].content;


        if (!guiOnce)
		{
			guiOnce = true;
			GUI.FocusControl("IdInput");
		}

        GUI.enabled = true;
		GlobalVars.Instance.EndGUI();
	}

	private string GetCurStepString()
	{
		string result = welcomeMessage;
		switch (loginStep)
		{
		case LOGIN_STEP.WAITING_SERVER:
			result = StringMgr.Instance.Get("GETTING_SERVER_INFO");
			break;
		case LOGIN_STEP.WAITING_SEED:
			result = StringMgr.Instance.Get("SEEDING");
			break;
		case LOGIN_STEP.WAITING_LOGIN:
			result = StringMgr.Instance.Get("AUTHENTICATING");
			break;
		}
		return result;
	}

	private void OnDisable()
	{
		try
		{
			DialogManager.Instance.CloseAll();
		}
		catch
		{
		}
	}

	private void OnRoundRobin()
	{
		DialogManager.Instance.CloseAll();
		CSNetManager.Instance.SwitchAfter = new SockTcp();
		if (CSNetManager.Instance.SwitchAfter.Open(CSNetManager.Instance.BfServer, CSNetManager.Instance.BfPort))
		{
			if (CSNetManager.Instance.Sock != null)
			{
				CSNetManager.Instance.Sock.Close();
			}
			loginStep = LOGIN_STEP.WAITING_SEED;
		}
		else
		{
			loginStep = LOGIN_STEP.NONE;
			MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("NETWORK_FAIL"));
		}
	}

	private void OnServiceFail(int reason)
	{
		loginStep = LOGIN_STEP.NONE;
		switch (reason)
		{
		case 0:
			MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("NETWORK_FAIL"));
			break;
		case 1:
			MessageBoxMgr.Instance.AddMessage(StringMgr.Instance.Get("SERVICE_CROWDED"));
			break;
		}
	}

	private void OnSeed()
	{
		if (GlobalVars.Instance.bRemember)
		{
			GlobalVars.Instance.strMyID = id;
		}
		loginStep = LOGIN_STEP.WAITING_LOGIN;
		if (BuildOption.Instance.IsAxeso5)
		{
			using (MD5 md5Hash = MD5.Create())
			{
				string md5Hash2 = GetMd5Hash(md5Hash, pswd);
				CSNetManager.Instance.Sock.SendCS_LOGIN_TO_AXESO5_REQ(id, md5Hash2, BuildOption.Instance.Major, BuildOption.Instance.Minor);
			}
		}
		else
		{
			CSNetManager.Instance.Sock.SendCS_LOGIN_REQ(id, pswd, BuildOption.Instance.Major, BuildOption.Instance.Minor);
		}
	}

	private void OnLoginFail()
	{
		loginStep = LOGIN_STEP.NONE;
	}

	private void OnLoginFailMessage(string message)
	{
		MessageBoxMgr.Instance.AddMessage(message);
		loginStep = LOGIN_STEP.NONE;
	}

	private static string GetMd5Hash(MD5 md5Hash, string input)
	{
		byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}
}
