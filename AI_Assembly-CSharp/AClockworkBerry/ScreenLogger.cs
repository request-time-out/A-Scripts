// Decompiled with JetBrains decompiler
// Type: AClockworkBerry.ScreenLogger
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AClockworkBerry
{
  public class ScreenLogger : MonoBehaviour
  {
    public static bool IsPersistent = true;
    private static bool instantiated = false;
    private static Queue<ScreenLogger.LogMessage> queue = new Queue<ScreenLogger.LogMessage>();
    private static ScreenLogger instance;
    public bool ShowLog;
    public bool ShowInEditor;
    [Tooltip("Height of the log area as a percentage of the screen height")]
    [Range(0.3f, 1f)]
    public float Height;
    [Tooltip("Width of the log area as a percentage of the screen width")]
    [Range(0.3f, 1f)]
    public float Width;
    public int Margin;
    public ScreenLogger.LogAnchor AnchorPosition;
    public int FontSize;
    [Range(0.0f, 1f)]
    public float BackgroundOpacity;
    public Color BackgroundColor;
    public bool LogMessages;
    public bool LogWarnings;
    public bool LogErrors;
    public Color MessageColor;
    public Color WarningColor;
    public Color ErrorColor;
    public Dictionary<string, Color> TagColors;
    public bool StackTraceMessages;
    public bool StackTraceWarnings;
    public bool StackTraceErrors;
    private GUIStyle styleContainer;
    private GUIStyle styleText;
    private int padding;
    private bool destroying;
    public TopNContainer SyncTopN;
    public TopNContainer AsyncTopN;

    public ScreenLogger()
    {
      Dictionary<string, Color> dictionary = new Dictionary<string, Color>();
      dictionary.Add("#LuaCache", Color.get_green());
      dictionary.Add("#LuaIO", Color.get_red());
      this.TagColors = dictionary;
      this.StackTraceErrors = true;
      this.padding = 20;
      this.SyncTopN = new TopNContainer();
      this.AsyncTopN = new TopNContainer();
      base.\u002Ector();
    }

    public static bool Instantiated
    {
      get
      {
        return ScreenLogger.instantiated;
      }
    }

    public static ScreenLogger Instance
    {
      get
      {
        if (ScreenLogger.instantiated)
          return ScreenLogger.instance;
        ScreenLogger.instance = Object.FindObjectOfType(typeof (ScreenLogger)) as ScreenLogger;
        if (Object.op_Equality((Object) ScreenLogger.instance, (Object) null))
        {
          ScreenLogger.instance = (ScreenLogger) new GameObject(nameof (ScreenLogger), new Type[1]
          {
            typeof (ScreenLogger)
          }).GetComponent<ScreenLogger>();
          if (Object.op_Equality((Object) ScreenLogger.instance, (Object) null))
            Debug.LogError((object) "Problem during the creation of ScreenLogger");
          else
            ScreenLogger.instantiated = true;
        }
        else
          ScreenLogger.instantiated = true;
        return ScreenLogger.instance;
      }
    }

    public void Awake()
    {
      if (((ScreenLogger[]) Object.FindObjectsOfType<ScreenLogger>()).Length > 1)
      {
        Debug.Log((object) "Destroying ScreenLogger, already exists...");
        this.destroying = true;
        Object.Destroy((Object) ((Component) this).get_gameObject());
      }
      else
      {
        this.InitStyles();
        if (!ScreenLogger.IsPersistent)
          return;
        Object.DontDestroyOnLoad((Object) this);
      }
    }

    private void InitStyles()
    {
      Texture2D texture2D = new Texture2D(1, 1);
      this.BackgroundColor.a = (__Null) (double) this.BackgroundOpacity;
      texture2D.SetPixel(0, 0, this.BackgroundColor);
      texture2D.Apply();
      this.styleContainer = new GUIStyle();
      this.styleContainer.get_normal().set_background(texture2D);
      this.styleContainer.set_wordWrap(true);
      this.styleContainer.set_padding(new RectOffset(this.padding, this.padding, this.padding, this.padding));
      this.styleText = new GUIStyle();
      this.styleText.set_fontSize(this.FontSize);
    }

    private void OnEnable()
    {
      if (!this.ShowInEditor && Application.get_isEditor())
        return;
      ScreenLogger.queue = new Queue<ScreenLogger.LogMessage>();
      // ISSUE: method pointer
      Application.add_logMessageReceived(new Application.LogCallback((object) this, __methodptr(HandleLog)));
    }

    private void OnDisable()
    {
      if (this.destroying)
        return;
      // ISSUE: method pointer
      Application.remove_logMessageReceived(new Application.LogCallback((object) this, __methodptr(HandleLog)));
    }

    private void Update()
    {
      if (!this.ShowInEditor && Application.get_isEditor())
        return;
      int num = (int) ((double) ((float) (Screen.get_height() - 2 * this.Margin) * this.Height - (float) (2 * this.padding)) / (double) this.styleText.get_lineHeight());
      while (ScreenLogger.queue.Count > num)
        ScreenLogger.queue.Dequeue();
    }

    private void OnGUI()
    {
      if (!this.ShowLog || !this.ShowInEditor && Application.get_isEditor())
        return;
      float num1 = (float) (Screen.get_width() - 2 * this.Margin) * this.Width;
      float num2 = (float) (Screen.get_height() - 2 * this.Margin) * this.Height;
      float num3 = 1f;
      float num4 = 1f;
      switch (this.AnchorPosition)
      {
        case ScreenLogger.LogAnchor.TopLeft:
          num3 = (float) this.Margin;
          num4 = (float) this.Margin;
          break;
        case ScreenLogger.LogAnchor.TopRight:
          num3 = (float) this.Margin + (float) (Screen.get_width() - 2 * this.Margin) * (1f - this.Width);
          num4 = (float) this.Margin;
          break;
        case ScreenLogger.LogAnchor.BottomLeft:
          num3 = (float) this.Margin;
          num4 = (float) this.Margin + (float) (Screen.get_height() - 2 * this.Margin) * (1f - this.Height);
          break;
        case ScreenLogger.LogAnchor.BottomRight:
          num3 = (float) this.Margin + (float) (Screen.get_width() - 2 * this.Margin) * (1f - this.Width);
          num4 = (float) this.Margin + (float) (Screen.get_height() - 2 * this.Margin) * (1f - this.Height);
          break;
      }
      GUILayout.BeginArea(new Rect(num3, num4, num1, num2), this.styleContainer);
      foreach (ScreenLogger.LogMessage logMessage in ScreenLogger.queue)
      {
        switch ((int) logMessage.Type)
        {
          case 0:
          case 1:
          case 4:
            this.styleText.get_normal().set_textColor(this.ErrorColor);
            break;
          case 2:
            this.styleText.get_normal().set_textColor(this.WarningColor);
            break;
          case 3:
            this.styleText.get_normal().set_textColor(this.MessageColor);
            break;
          default:
            this.styleText.get_normal().set_textColor(this.MessageColor);
            break;
        }
        if (logMessage.Message.Contains("#"))
        {
          using (Dictionary<string, Color>.Enumerator enumerator = this.TagColors.GetEnumerator())
          {
            while (enumerator.MoveNext())
            {
              KeyValuePair<string, Color> current = enumerator.Current;
              if (logMessage.Message.Contains(current.Key))
              {
                this.styleText.get_normal().set_textColor(current.Value);
                break;
              }
            }
          }
        }
        GUILayout.Label(logMessage.Message, this.styleText, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      }
      GUILayout.EndArea();
      float num5 = 550f;
      float num6 = (float) (Screen.get_height() - 2 * this.Margin - this.Margin) * (1f - this.Height);
      this.styleText.get_normal().set_textColor(Color.get_white());
      GUILayout.BeginArea(new Rect((float) this.Margin, (float) this.Margin, num5, num6), this.styleContainer);
      GUILayout.Label("Sync Top N", this.styleText, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      for (int i = 0; i < this.SyncTopN.TopN.Count; ++i)
        GUILayout.Label(this.SyncTopN.ItemToString(i), this.styleText, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.EndArea();
      GUILayout.BeginArea(new Rect((float) (this.Margin * 2) + num5, (float) this.Margin, num5, num6), this.styleContainer);
      GUILayout.Label("Async Top N", this.styleText, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      for (int i = 0; i < this.AsyncTopN.TopN.Count; ++i)
        GUILayout.Label(this.AsyncTopN.ItemToString(i), this.styleText, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.EndArea();
    }

    public void Clear()
    {
      this.SyncTopN.Clear();
      this.AsyncTopN.Clear();
    }

    private void HandleLog(string message, string stackTrace, LogType type)
    {
      if (type == 1 && !this.LogErrors || type == null && !this.LogErrors || (type == 4 && !this.LogErrors || type == 3 && !this.LogMessages) || type == 2 && !this.LogWarnings)
        return;
      string str1 = message;
      char[] chArray1 = new char[1]{ '\n' };
      foreach (string msg in str1.Split(chArray1))
        ScreenLogger.queue.Enqueue(new ScreenLogger.LogMessage(msg, type));
      if (type == 1 && !this.StackTraceErrors || type == null && !this.StackTraceErrors || (type == 4 && !this.StackTraceErrors || type == 3 && !this.StackTraceMessages) || type == 2 && !this.StackTraceWarnings)
        return;
      string str2 = stackTrace;
      char[] chArray2 = new char[1]{ '\n' };
      foreach (string str3 in str2.Split(chArray2))
      {
        if (str3.Length != 0)
          ScreenLogger.queue.Enqueue(new ScreenLogger.LogMessage("  " + str3, type));
      }
    }

    public void EnqueueDirectly(string message, LogType type)
    {
      ScreenLogger.queue.Enqueue(new ScreenLogger.LogMessage(message, type));
    }

    public void InspectorGUIUpdated()
    {
      this.InitStyles();
    }

    private class LogMessage
    {
      public string Message;
      public LogType Type;

      public LogMessage(string msg, LogType type)
      {
        this.Message = msg;
        this.Type = type;
      }
    }

    public enum LogAnchor
    {
      TopLeft,
      TopRight,
      BottomLeft,
      BottomRight,
    }
  }
}
