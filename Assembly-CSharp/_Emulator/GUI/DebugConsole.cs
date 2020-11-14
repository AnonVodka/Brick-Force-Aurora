﻿using System.Collections.Generic;
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

        public KeyCode toggleKey = KeyCode.BackQuote;

        List<Log> logs = new List<Log>();
        Vector2 scrollPosition;
        bool hidden = true;
        bool collapse = true;

        static readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>()
    {
        { LogType.Assert, Color.white },
        { LogType.Error, Color.red },
        { LogType.Exception, Color.red },
        { LogType.Log, Color.white },
        { LogType.Warning, Color.yellow },
    };

        const int margin = 20;

        Rect windowRect = new Rect(margin, margin, Screen.width - (margin * 2), Screen.height - (margin * 2));
        Rect titleBarRect = new Rect(0, 0, 10000, 20);
        GUIContent clearLabel = new GUIContent("Clear", "Clear the contents of the console.");
        GUIContent collapseLabel = new GUIContent("Collapse", "Hide repeated messages.");

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

        public void OnGUI()
        {
            if (!hidden)
                windowRect = GUILayout.Window(123456, windowRect, ConsoleWindow, "Console");
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
        }
    }
}