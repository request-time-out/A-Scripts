// Decompiled with JetBrains decompiler
// Type: UIDebugDraw
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class UIDebugDraw : MonoBehaviour
{
  public static UIDebugDraw Instance;
  private Texture m_texUpdateGeometry;
  private Texture m_texUpdateAlpha;
  private List<UIHighlightWidget> m_highlightedWidgets;
  private Dictionary<string, int> m_widgetHighlightCount;

  public UIDebugDraw()
  {
    base.\u002Ector();
  }

  public void StartStats()
  {
    if (((Behaviour) this).get_enabled())
      return;
    ((Behaviour) this).set_enabled(true);
    this.m_widgetHighlightCount.Clear();
  }

  public void StopStats()
  {
    if (!((Behaviour) this).get_enabled())
      return;
    ((Behaviour) this).set_enabled(false);
    List<KeyValuePair<string, int>> list = this.m_widgetHighlightCount.ToList<KeyValuePair<string, int>>();
    list.Sort((Comparison<KeyValuePair<string, int>>) ((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)));
    List<string> stringList = new List<string>();
    foreach (KeyValuePair<string, int> keyValuePair in list)
      stringList.Add(string.Format("{0} {1}", (object) keyValuePair.Key, (object) keyValuePair.Value));
    File.WriteAllLines(Path.Combine(Application.get_persistentDataPath(), string.Format("TestTools/ui_stats_{0}_{1}.log", (object) SysUtil.FormatDateAsFileNameString(DateTime.Now), (object) SysUtil.FormatTimeAsFileNameString(DateTime.Now))), stringList.ToArray());
  }

  private void Start()
  {
    Texture2D texture2D1 = new Texture2D(1, 1);
    Color magenta = Color.get_magenta();
    magenta.a = (__Null) 0.200000002980232;
    texture2D1.SetPixel(0, 0, magenta);
    texture2D1.Apply();
    this.m_texUpdateGeometry = (Texture) texture2D1;
    Texture2D texture2D2 = new Texture2D(1, 1);
    Color green = Color.get_green();
    green.a = (__Null) 0.200000002980232;
    texture2D2.SetPixel(0, 0, green);
    texture2D2.Apply();
    this.m_texUpdateAlpha = (Texture) texture2D2;
  }

  private void Update()
  {
  }

  private void OnGUI()
  {
    if (!((Behaviour) this).get_enabled())
      return;
    Color color1 = GUI.get_color();
    Color color2 = GUI.get_color();
    for (int index = 0; index < this.m_highlightedWidgets.Count; ++index)
    {
      color2.a = (__Null) (1.0 - ((double) Time.get_time() - (double) this.m_highlightedWidgets[index].timeChanged) * 2.0);
      GUI.set_color(color2);
      Vector3[] screenPos = this.m_highlightedWidgets[index].screenPos;
      float num = (float) (screenPos[2].y - screenPos[0].y);
      Rect rect;
      ((Rect) ref rect).\u002Ector((float) screenPos[0].x, (float) Screen.get_height() - (float) screenPos[0].y - num, (float) (screenPos[2].x - screenPos[0].x), num);
      switch (this.m_highlightedWidgets[index].type)
      {
        case UIHighlightType.HType_UpdateGeometry:
          GUI.DrawTexture(rect, this.m_texUpdateGeometry, (ScaleMode) 0);
          break;
        case UIHighlightType.HType_Invalidate:
          GUI.DrawTexture(rect, this.m_texUpdateAlpha, (ScaleMode) 0);
          break;
      }
      ((Rect) ref rect).set_width(Math.Max(((Rect) ref rect).get_width(), 200f));
      ((Rect) ref rect).set_height(Math.Max(((Rect) ref rect).get_height(), 50f));
      GUI.Label(rect, this.m_highlightedWidgets[index].name);
    }
    GUI.set_color(color1);
    this.m_highlightedWidgets.RemoveAll((Predicate<UIHighlightWidget>) (w => (double) Time.get_time() - (double) w.timeChanged > 0.5));
  }

  private void OnDrawGizmos()
  {
  }
}
