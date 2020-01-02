// Decompiled with JetBrains decompiler
// Type: GUIDrawer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GUIDrawer
{
  public static void Draw(float x, float y, string str, float baseW = 7f, float baseH = 15f, bool isLeftUp = true)
  {
    GUIDrawer.Draw(ref x, ref y, str, baseW, baseH, true);
  }

  public static void Draw(
    ref float x,
    float y,
    string str,
    float baseW = 7f,
    float baseH = 15f,
    bool isLeftUp = true)
  {
    GUIDrawer.Draw(ref x, ref y, str, baseW, baseH, true);
  }

  public static void Draw(
    float x,
    ref float y,
    string str,
    float baseW = 7f,
    float baseH = 15f,
    bool isLeftUp = true)
  {
    GUIDrawer.Draw(ref x, ref y, str, baseW, baseH, true);
  }

  public static void Draw(
    ref float x,
    ref float y,
    string str,
    float baseW = 7f,
    float baseH = 15f,
    bool isLeftUp = true)
  {
    if (str == string.Empty)
      return;
    string[] strArray = str.Replace(Environment.NewLine, "\n").Split('\n');
    float num1 = (float) ((IEnumerable<string>) strArray).Max<string>((Func<string, int>) (max => max.Length)) * baseW;
    float num2 = (float) strArray.Length * baseH;
    GUI.Box(new Rect(x, y, num1 + 10f, num2 + 5f), string.Empty);
    GUI.Label(new Rect(x + 5f, y, (float) Screen.get_width(), (float) Screen.get_height()), str);
    x += num1;
    if (isLeftUp)
      y += num2 + baseH;
    else
      y -= num2 + baseH;
  }

  public static float GetWidth(string str, float baseW = 7f)
  {
    if (str == string.Empty)
      return 0.0f;
    return (float) ((IEnumerable<string>) str.Replace(Environment.NewLine, "\n").Split('\n')).Max<string>((Func<string, int>) (max => max.Length)) * baseW;
  }

  public static float GetHeight(string str, float baseH = 15f)
  {
    if (str == string.Empty)
      return 0.0f;
    return (float) str.Replace(Environment.NewLine, "\n").Split('\n').Length * baseH + baseH;
  }

  public class Window
  {
    private static Vector2 buttonSize = new Vector2(20f, 20f);
    public GUIDrawer.Window.Type type = GUIDrawer.Window.Type.Layout;
    public string title;
    public Rect rect;
    public Action<int> DoWindow;
    public Action HideEvent;
    private bool _isHide;
    public Action CloseEvent;
    private bool _isClose;
    private string defaultTitle;
    private Rect? backupRect;

    public Window()
    {
      this.type = GUIDrawer.Window.Type.None;
      this.defaultTitle = this.title = string.Empty;
    }

    public Window(string title)
    {
      this.title = title ?? string.Empty;
      this.defaultTitle = this.title;
      this.rect = new Rect(0.0f, 0.0f, 300f, 0.0f);
    }

    public Window(string title, Rect rect)
    {
      this.title = title ?? string.Empty;
      this.defaultTitle = this.title;
      this.rect = rect;
    }

    public void Open()
    {
      this.isClose = false;
    }

    public void Close()
    {
      this.isClose = true;
    }

    public void Hide()
    {
      this.isHide = true;
    }

    public void View()
    {
      this.isHide = false;
    }

    public bool isHide
    {
      get
      {
        return this._isHide;
      }
      set
      {
        this._isHide = value;
        if (!this._isHide)
          return;
        this.HideEvent.Call();
      }
    }

    public bool isClose
    {
      get
      {
        return this._isClose;
      }
      set
      {
        this._isClose = value;
        if (!this._isClose)
          return;
        this.CloseEvent.Call();
        this.title = this.defaultTitle;
      }
    }

    public void Draw(int windowID)
    {
      if (!this.backupRect.HasValue)
        this.backupRect = new Rect?(this.rect);
      if (this.isClose)
        return;
      this.CloseButton();
      GUIDrawer.Window.Type type = this.type;
      // ISSUE: object of a compiler-generated type is created
      // ISSUE: variable of a compiler-generated type
      GUIDrawer.Window.\u003CDraw\u003Ec__AnonStorey0 drawCAnonStorey0 = new GUIDrawer.Window.\u003CDraw\u003Ec__AnonStorey0();
      // ISSUE: reference to a compiler-generated field
      drawCAnonStorey0.\u0024this = this;
      switch (type)
      {
        case GUIDrawer.Window.Type.None:
          this.DoWindow(-1);
          break;
        case GUIDrawer.Window.Type.Normal:
        case GUIDrawer.Window.Type.Layout:
          // ISSUE: reference to a compiler-generated field
          // ISSUE: reference to a compiler-generated method
          drawCAnonStorey0.doWindow = new Action<int>(drawCAnonStorey0.\u003C\u003Em__0);
          if (this.type == GUIDrawer.Window.Type.Normal || this.isHide)
          {
            if (this.isHide)
              ((Rect) ref this.rect).set_height(50f);
            // ISSUE: method pointer
            this.rect = GUI.Window(windowID, this.rect, new GUI.WindowFunction((object) drawCAnonStorey0, __methodptr(\u003C\u003Em__1)), this.title);
          }
          else
          {
            // ISSUE: method pointer
            this.rect = GUILayout.Window(windowID, this.rect, new GUI.WindowFunction((object) drawCAnonStorey0, __methodptr(\u003C\u003Em__2)), this.title, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
          }
          this.HideButton();
          break;
        case GUIDrawer.Window.Type.Custom:
          Rect rect = this.rect;
          using (new GUILayout.AreaScope(rect))
          {
            GUIContent guiContent = new GUIContent(this.title);
            using (new GUILayout.VerticalScope(GUIStyle.op_Implicit("box"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>()))
            {
              if (GUILayout.RepeatButton(guiContent, new GUILayoutOption[2]
              {
                GUILayout.ExpandWidth(true),
                GUILayout.ExpandHeight(false)
              }))
              {
                float y = (float) new GUIStyle().CalcSize(guiContent).y;
                ((Rect) ref this.rect).set_x((float) (Input.get_mousePosition().x - (double) ((Rect) ref rect).get_width() * 0.5));
                ((Rect) ref this.rect).set_y((float) ((double) Screen.get_height() - Input.get_mousePosition().y - (double) y * 0.5));
              }
              this.DoWindow(-1);
              break;
            }
          }
      }
    }

    private void HideButton()
    {
      Rect rect1 = this.rect;
      float x = (float) GUIDrawer.Window.buttonSize.x;
      float y = (float) GUIDrawer.Window.buttonSize.y;
      ((Rect) ref rect1).set_x((float) ((double) ((Rect) ref rect1).get_x() + (double) ((Rect) ref rect1).get_width() - (double) x * 2.0));
      ref Rect local1 = ref rect1;
      ((Rect) ref local1).set_y(((Rect) ref local1).get_y() - y);
      ((Rect) ref rect1).set_width(x);
      ((Rect) ref rect1).set_height(y);
      if (!GUI.Button(rect1, !this.isHide ? "-" : "□"))
        return;
      this.isHide = !this.isHide;
      if (this.isHide)
        return;
      ref Rect local2 = ref this.rect;
      Rect rect2 = this.backupRect.Value;
      double height = (double) ((Rect) ref rect2).get_height();
      ((Rect) ref local2).set_height((float) height);
    }

    private void CloseButton()
    {
      Rect rect = this.rect;
      float x = (float) GUIDrawer.Window.buttonSize.x;
      float y = (float) GUIDrawer.Window.buttonSize.y;
      ((Rect) ref rect).set_x(((Rect) ref rect).get_x() + ((Rect) ref rect).get_width() - x);
      ref Rect local = ref rect;
      ((Rect) ref local).set_y(((Rect) ref local).get_y() - y);
      ((Rect) ref rect).set_width(x);
      ((Rect) ref rect).set_height(y);
      if (!GUI.Button(rect, "X"))
        return;
      this.isClose = true;
    }

    public enum Type
    {
      None,
      Normal,
      Layout,
      Custom,
    }
  }
}
