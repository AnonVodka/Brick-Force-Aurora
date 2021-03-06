using System.Collections.Generic;
using UnityEngine;

public class TeamMatchSituation : MonoBehaviour
{
	public GUIDepth.LAYER guiDepth = GUIDepth.LAYER.GAME_CONTROL;

	public Texture2D redTeam;

	public Texture2D blueTeam;

	public Vector2 crdFrame = new Vector2(492f, 520f);

	public Vector2 crdRoomTitle = new Vector2(10f, 5f);

	public Rect crdRedTeamTitle = new Rect(0f, 0f, 472f, 22f);

	public Rect crdBlueTeamTitle = new Rect(0f, 192f, 472f, 22f);

	public Rect crdRedTeamIcon = new Rect(0f, 0f, 86f, 34f);

	public Rect crdBlueTeamIcon = new Rect(0f, 192f, 86f, 34f);

	public Rect crdRedFirstLow = new Rect(0f, 0f, 472f, 19f);

	public Rect crdBlueFirstLow = new Rect(0f, 192f, 472f, 19f);

	public float redResultY = 14f;

	public float blueResultY = 307f;

	public Vector2 clanMarkSize = new Vector2(16f, 16f);

	public Vector2 badgeSize = new Vector2(34f, 17f);

	public float redY = 88f;

	public float blueY = 337f;

	public float markX = 53f;

	public float badgeX = 97f;

	public float nickX = 176f;

	public float killX = 295f;

	public float scoreX = 380f;

	public float pingX = 439f;

	public float offset = 21f;

	public float myRowX = 11f;

	public Vector2 myRowSize = new Vector2(472f, 18f);

	public Rect crdRedTeamSituation = new Rect(0f, 0f, 472f, 192f);

	public Rect crdBlueTeamSituation = new Rect(0f, 0f, 472f, 192f);

	public UIFlickerColor redTeamMyTeam;

	public UIFlickerColor blueTeamMyTeam;

	private bool on;

	private LocalController localController;

	private void Start()
	{
	}

	private void VerifyLocalController()
	{
		if (null == localController)
		{
			GameObject gameObject = GameObject.Find("Me");
			if (null != gameObject)
			{
				localController = gameObject.GetComponent<LocalController>();
			}
		}
	}

	private void DrawClanMark(Rect rc, int mark)
	{
		if (mark >= 0)
		{
			Texture2D bg = ClanMarkManager.Instance.GetBg(mark);
			Color colorValue = ClanMarkManager.Instance.GetColorValue(mark);
			Texture2D amblum = ClanMarkManager.Instance.GetAmblum(mark);
			if (null != bg)
			{
				TextureUtil.DrawTexture(rc, bg);
			}
			Color color = GUI.color;
			GUI.color = colorValue;
			if (null != amblum)
			{
				TextureUtil.DrawTexture(rc, amblum);
			}
			GUI.color = color;
		}
	}

	private float GridOut(int clanMark, int xp, int rank, string nickname, int kill, int death, int assist, int score, float avgPing, int status, bool isDead, float y, bool isHost)
	{
		DrawClanMark(new Rect(markX - clanMarkSize.x / 2f, y - clanMarkSize.y / 2f + 1f, clanMarkSize.x, clanMarkSize.y), clanMark);
		Texture2D badge = XpManager.Instance.GetBadge(XpManager.Instance.GetLevel(xp), rank);
		TextureUtil.DrawTexture(new Rect(badgeX - badgeSize.x / 2f, y - badgeSize.y / 2f + 1f, badgeSize.x, badgeSize.y), badge);
		if (isDead)
		{
			LabelUtil.TextOut(new Vector2(nickX, y), nickname, "MiniLabel", Color.grey, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
		}
		else
		{
			LabelUtil.TextOut(new Vector2(nickX, y), nickname, "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
		}
		LabelUtil.TextOut(new Vector2(killX, y), kill.ToString() + "/" + assist.ToString() + "/" + death.ToString(), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
		LabelUtil.TextOut(new Vector2(scoreX, y), score.ToString(), "MiniLabel", new Color(0.83f, 0.49f, 0.29f), GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
		if (status == 4)
		{
			if (avgPing < float.PositiveInfinity)
			{
				if (avgPing > 0.3f)
				{
					LabelUtil.TextOut(new Vector2(pingX, y), avgPing.ToString("0.###"), "MiniLabel", Color.red, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
				}
				else if (avgPing > 0.1f)
				{
					LabelUtil.TextOut(new Vector2(pingX, y), avgPing.ToString("0.###"), "MiniLabel", Color.yellow, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
				}
				else
				{
					LabelUtil.TextOut(new Vector2(pingX, y), avgPing.ToString("0.###"), "MiniLabel", Color.green, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
				}
			}
			else
			{
				LabelUtil.TextOut(new Vector2(pingX, y), "x", "MiniLabel", Color.red, Color.black, TextAnchor.MiddleCenter);
			}
		}
		else
		{
			string text = StringMgr.Instance.Get("PLAYER_IS_WAITING");
			if (status == 2 || status == 3)
			{
				text = StringMgr.Instance.Get("PLAYER_IS_LOADING");
			}
			LabelUtil.TextOut(new Vector2(pingX, y), text, "MiniLabel", Color.blue, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
		}
		return y + offset;
	}

	private void OnGUI()
	{
		if (MyInfoManager.Instance.isGuiOn && on)
		{
			VerifyLocalController();
			GUI.skin = GUISkinFinder.Instance.GetGUISkin();
			GUI.depth = (int)guiDepth;
			GUI.enabled = !DialogManager.Instance.IsModal;
			GUI.BeginGroup(new Rect(((float)Screen.width - crdFrame.x) / 2f, ((float)Screen.height - crdFrame.y) / 2f, crdFrame.x, crdFrame.y));
			GUI.Box(new Rect(0f, 0f, crdFrame.x, crdFrame.y), string.Empty, "BoxResult");
			Room room = RoomManager.Instance.GetRoom(RoomManager.Instance.CurrentRoom);
			if (room != null)
			{
				LabelUtil.TextOut(crdRoomTitle, room.GetString(), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.UpperLeft);
			}
			GUI.Box(crdRedTeamTitle, string.Empty, "BoxRed02");
			GUI.Box(crdRedFirstLow, string.Empty, "BoxResultText");
			TextureUtil.DrawTexture(crdRedTeamIcon, redTeam, ScaleMode.StretchToFill);
			LabelUtil.TextOut(new Vector2(markX, redResultY), StringMgr.Instance.Get("MARK"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(badgeX, redResultY), StringMgr.Instance.Get("BADGE"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(nickX, redResultY), StringMgr.Instance.Get("CHARACTER"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(killX, redResultY), StringMgr.Instance.Get("KILL") + "/" + StringMgr.Instance.Get("ASSIST") + "/" + StringMgr.Instance.Get("DEATH"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(scoreX, redResultY), StringMgr.Instance.Get("SCORE"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(pingX, redResultY), StringMgr.Instance.Get("PING"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			float num = redY;
			GUI.Box(crdRedTeamSituation, string.Empty, "BoxResult01");
			bool flag = false;
			List<KeyValuePair<int, BrickManDesc>> list = BrickManManager.Instance.ToSortedList();
			for (int i = 0; i < list.Count; i++)
			{
				BrickManDesc value = list[i].Value;
				if (value != null && value.Slot < 8)
				{
					GameObject gameObject = BrickManManager.Instance.Get(value.Seq);
					if (!(null == gameObject))
					{
						TPController component = gameObject.GetComponent<TPController>();
						if (!(null == component))
						{
							if (MyInfoManager.Instance.Slot < 8 && !flag && (MyInfoManager.Instance.Score > value.Score || (MyInfoManager.Instance.Score == value.Score && MyInfoManager.Instance.Kill > value.Kill)))
							{
								flag = true;
								float num2 = num;
								num = GridOut(MyInfoManager.Instance.ClanMark, MyInfoManager.Instance.Xp, MyInfoManager.Instance.Rank, MyInfoManager.Instance.Nickname, MyInfoManager.Instance.Kill, MyInfoManager.Instance.Death, MyInfoManager.Instance.Assist, MyInfoManager.Instance.Score, MyInfoManager.Instance.PingTime, MyInfoManager.Instance.Status, localController.IsDead, num, MyInfoManager.Instance.Seq == RoomManager.Instance.Master);
								GUI.Box(new Rect(myRowX, num2 - myRowSize.y / 2f + 2f, myRowSize.x, myRowSize.y), string.Empty, "BoxMyInfo");
							}
							Peer peer = P2PManager.Instance.Get(value.Seq);
							num = GridOut(value.ClanMark, value.Xp, value.Rank, value.Nickname, value.Kill, value.Death, value.Assist, value.Score, peer?.PingTime ?? float.PositiveInfinity, value.Status, component.IsDead, num, value.Seq == RoomManager.Instance.Master);
						}
					}
				}
			}
			if (MyInfoManager.Instance.Slot < 8 && !flag)
			{
				GridOut(MyInfoManager.Instance.ClanMark, MyInfoManager.Instance.Xp, MyInfoManager.Instance.Rank, MyInfoManager.Instance.Nickname, MyInfoManager.Instance.Kill, MyInfoManager.Instance.Death, MyInfoManager.Instance.Assist, MyInfoManager.Instance.Score, MyInfoManager.Instance.PingTime, MyInfoManager.Instance.Status, localController.IsDead, num, MyInfoManager.Instance.Seq == RoomManager.Instance.Master);
				GUI.Box(new Rect(myRowX, num - myRowSize.y / 2f + 2f, myRowSize.x, myRowSize.y), string.Empty, "BoxMyInfo");
			}
			GUI.Box(crdBlueTeamTitle, string.Empty, "BoxBlue02");
			GUI.Box(crdBlueFirstLow, string.Empty, "BoxResultText");
			TextureUtil.DrawTexture(crdBlueTeamIcon, blueTeam, ScaleMode.StretchToFill);
			LabelUtil.TextOut(new Vector2(markX, blueResultY), StringMgr.Instance.Get("MARK"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(badgeX, blueResultY), StringMgr.Instance.Get("BADGE"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(nickX, blueResultY), StringMgr.Instance.Get("CHARACTER"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(killX, blueResultY), StringMgr.Instance.Get("KILL") + "/" + StringMgr.Instance.Get("ASSIST") + "/" + StringMgr.Instance.Get("DEATH"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(scoreX, blueResultY), StringMgr.Instance.Get("SCORE"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			LabelUtil.TextOut(new Vector2(pingX, blueResultY), StringMgr.Instance.Get("PING"), "MiniLabel", Color.white, GlobalVars.txtEmptyColor, TextAnchor.MiddleCenter);
			num = blueY;
			GUI.Box(crdBlueTeamSituation, string.Empty, "BoxResult01");
			for (int j = 0; j < list.Count; j++)
			{
				BrickManDesc value2 = list[j].Value;
				if (value2 != null && value2.Slot >= 8)
				{
					GameObject gameObject2 = BrickManManager.Instance.Get(value2.Seq);
					if (!(null == gameObject2))
					{
						TPController component2 = gameObject2.GetComponent<TPController>();
						if (!(null == component2))
						{
							if (MyInfoManager.Instance.Slot >= 8 && !flag && (MyInfoManager.Instance.Score > value2.Score || (MyInfoManager.Instance.Score == value2.Score && MyInfoManager.Instance.Kill > value2.Kill)))
							{
								flag = true;
								float num3 = num;
								num = GridOut(MyInfoManager.Instance.ClanMark, MyInfoManager.Instance.Xp, MyInfoManager.Instance.Rank, MyInfoManager.Instance.Nickname, MyInfoManager.Instance.Kill, MyInfoManager.Instance.Death, MyInfoManager.Instance.Assist, MyInfoManager.Instance.Score, MyInfoManager.Instance.PingTime, MyInfoManager.Instance.Status, localController.IsDead, num, MyInfoManager.Instance.Seq == RoomManager.Instance.Master);
								GUI.Box(new Rect(myRowX, num3 - myRowSize.y / 2f + 2f, myRowSize.x, myRowSize.y), string.Empty, "BoxMyInfo");
							}
							Peer peer2 = P2PManager.Instance.Get(value2.Seq);
							num = GridOut(value2.ClanMark, value2.Xp, value2.Rank, value2.Nickname, value2.Kill, value2.Death, value2.Assist, value2.Score, peer2?.PingTime ?? float.PositiveInfinity, value2.Status, component2.IsDead, num, value2.Seq == RoomManager.Instance.Master);
						}
					}
				}
			}
			if (MyInfoManager.Instance.Slot >= 8 && !flag)
			{
				GridOut(MyInfoManager.Instance.ClanMark, MyInfoManager.Instance.Xp, MyInfoManager.Instance.Rank, MyInfoManager.Instance.Nickname, MyInfoManager.Instance.Kill, MyInfoManager.Instance.Death, MyInfoManager.Instance.Assist, MyInfoManager.Instance.Score, MyInfoManager.Instance.PingTime, MyInfoManager.Instance.Status, localController.IsDead, num, MyInfoManager.Instance.Seq == RoomManager.Instance.Master);
				GUI.Box(new Rect(myRowX, num - myRowSize.y / 2f + 2f, myRowSize.x, myRowSize.y), string.Empty, "BoxMyInfo");
			}
			if (MyInfoManager.Instance.IsRedTeam())
			{
				redTeamMyTeam.Draw();
			}
			else
			{
				blueTeamMyTeam.Draw();
			}
			GUI.EndGroup();
			GUI.enabled = true;
		}
	}

	private void Update()
	{
		on = (!DialogManager.Instance.IsModal && custom_inputs.Instance.GetButton("K_SITUATION"));
		redTeamMyTeam.Update();
		blueTeamMyTeam.Update();
	}
}
