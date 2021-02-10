using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Emulator
{
    public class DebugConsole : MonoBehaviour
    {
        public static DebugConsole instance;
        struct Log
        {
            public string message;
            public string stackTrace;
            public LogType type;
        }

        List<Log> logs = new List<Log>();
        Vector2 scrollPosition;
        bool hidden = true;
        bool collapse = true;
        bool externConsole = false;


        static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
        {
            { LogType.Assert, Color.white },
            { LogType.Error, Color.red },
            { LogType.Exception, Color.red },
            { LogType.Log, Color.white },
            { LogType.Warning, Color.yellow },
        };

        const int margin = 20;

        //Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 4));
        Rect windowRect = new Rect(Screen.width * 0.5f + 125 - margin, margin, 840, 660);
        Rect titleBarRect = new Rect(0, 0, 10000, 20);
        GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

        Texture2D backgroundTexture = new Texture2D(840, 660, TextureFormat.RGBA32, true);

        IntPtr _out;
        IntPtr _in;

        void OnEnable()
        {
            Application.RegisterLogCallback(HandleLog);
        }

        void OnDisable()
        {
            Application.RegisterLogCallback(null);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.F8))
                hidden = !hidden;

            if (Input.GetKey(KeyCode.DownArrow))
                scrollPosition.y += 5f;

            if (Input.GetKey(KeyCode.UpArrow))
                scrollPosition.y -= 5f;
        }

        void Start()
        {
            if (externConsole)
            {
                Import.AllocConsole();
                Import.AttachConsole(Import.GetCurrentProcessId());
                Import.SetConsoleTitle("Debug Console");

                _out = Import.GetStdHandle(StdHandle.STD_OUTPUT_HANDLE);
                _in = Import.GetStdHandle(StdHandle.STD_INPUT_HANDLE);

                Import.SetConsoleMode(_out, 0x0001 | 0x0002);
                Import.SetConsoleMode(_in, 0x0020 | 0x0080 | 0x0001 | 0x0040);
            }

            //for (int x = 0; x <= (int)windowRect.width; x++)
            //{
            //    backgroundTexture.SetPixel(x, 0, Color.black);
            //    backgroundTexture.SetPixel(x - 2, 1, Color.black);

            //    backgroundTexture.SetPixel(x, (int)windowRect.yMax, Color.black);
            //    backgroundTexture.SetPixel(x - 2, (int)windowRect.yMax - 1, Color.black);
            //}

            //for (int y = 0; y <= (int)windowRect.height; y++)
            //{
            //    backgroundTexture.SetPixel(0, y, Color.black);
            //    backgroundTexture.SetPixel(1, y - 2, Color.black);

            //    backgroundTexture.SetPixel((int)windowRect.width - 1, y, Color.black);
            //    backgroundTexture.SetPixel((int)windowRect.width - 2, y - 4, Color.black);
            //}

            for (int x = 0; x <= (int)windowRect.width; x++)
                for (int y = 0; y <= (int)windowRect.height; y++)
                    backgroundTexture.SetPixel(x, y, Color.black);

            backgroundTexture.Apply();

        }

        public void OnGUI()
        {
            GUIStyle style = GUI.skin.GetStyle("Box");
            style.fontSize = 12;
            style.fontStyle = FontStyle.Bold;

            style.normal.background = backgroundTexture;

            if (!hidden)
                windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console", style);
        }

        void ConsoleWindow(int windowID)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition);

            for (int i = 0; i < logs.Count; i++)
            {
                var log = logs[i];

                if (collapse)
                {
                    var messageSameAsPrevious = i > 0 && log.message == logs[i - 1].message;

                    if (messageSameAsPrevious)
                    {
                        continue;
                    }
                }

                GUI.contentColor = logTypeColors[log.type];
                GUILayout.Label(log.message);
            }

            GUILayout.EndScrollView();

            GUI.contentColor = Color.white;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(clearLabel))
            {
                logs.Clear();
            }

            collapse = GUILayout.Toggle(collapse, collapseLabel, GUILayout.ExpandWidth(false));

            GUILayout.EndHorizontal();

            GUI.DragWindow(titleBarRect);
        }

        void HandleLog(string message, string stackTrace, LogType type)
        {
            logs.Add(new Log()
            {
                message = message,
                stackTrace = stackTrace,
                type = type,
            });

            if (externConsole)
            {
                string newMessage = message;
                uint color = (0x0001 | 0x0002 | 0x0004);
                switch (type)
                {
                    case LogType.Assert:
                        newMessage = "[ASSERT] " + message + "\n";
                        color = 0x0001 | 0x0002;
                        break;
                    case LogType.Error:
                        newMessage = "[ERROR] " + message + "\n";
                        color = 0x0004;
                        break;
                    case LogType.Exception:
                        newMessage = "[EXCEPTION] " + message + "\n";
                        color = 0x0004;
                        break;
                    case LogType.Log:
                        newMessage = "[LOG] " + message + "\n";
                        break;
                    case LogType.Warning:
                        newMessage = "[WARNING] " + message + "\n";
                        color = 0x0002 | 0x0004;
                        break;
                }
                Import.SetConsoleTextAttribute(_out, color);
                Import.WriteConsole(_out, newMessage, (ulong)newMessage.Length, (UIntPtr)null, (UIntPtr)null);
                Import.SetConsoleTextAttribute(_out, 0x0001 | 0x0002 | 0x0004);
            }
        }
    }
}