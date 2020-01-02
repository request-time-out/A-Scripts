// Decompiled with JetBrains decompiler
// Type: UIDebugStats
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class UIDebugStats : MonoBehaviour
{
  public static UIDebugStats Instance;
  private Texture m_debugTexture;
  private string m_debugInfo;
  private float m_lastUpdateTime;
  private GUIStyle guiStyle;
  private float m_startTime;
  private int m_startFrame;
  private Stopwatch m_panelSW;
  private Dictionary<int, UIPanelData> m_elapsedTicks;
  private Dictionary<int, UITimingDict> m_widgetTicks;
  private Dictionary<int, double> m_accumulated;
  private Dictionary<int, UIPanelData> m_accumulatedPanels;

  public UIDebugStats()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    Texture2D texture2D = new Texture2D(1, 1);
    Color color;
    ((Color) ref color).\u002Ector(0.2f, 0.2f, 0.2f, 0.4f);
    texture2D.SetPixel(0, 0, color);
    texture2D.Apply();
    this.m_debugTexture = (Texture) texture2D;
  }

  public string PrintDictDouble()
  {
    List<KeyValuePair<int, UIPanelData>> list = this.m_elapsedTicks.ToList<KeyValuePair<int, UIPanelData>>();
    list.Sort((Comparison<KeyValuePair<int, UIPanelData>>) ((pair1, pair2) => Math.Sign(pair2.Value.mElapsedTicks - pair1.Value.mElapsedTicks)));
    StringBuilder stringBuilder = new StringBuilder();
    foreach (KeyValuePair<int, UIPanelData> keyValuePair in list)
    {
      stringBuilder.AppendFormat("{0, -30} \t{1:0.00} \t{2:0.00} \t{3}/{4} \t{5}\n", (object) UIDebugCache.GetName(keyValuePair.Key), (object) keyValuePair.Value.mElapsedTicks, (object) (keyValuePair.Value.mElapsedTicks / (1.0 / (double) Time.get_deltaTime())), (object) keyValuePair.Value.mRebuildCount, (object) keyValuePair.Value.mCalls, (object) (keyValuePair.Value.mDrawCallNum / keyValuePair.Value.mCalls));
      if (UIDebugVariables.ShowWidgetStatsOnScreen)
      {
        UITimingDict uiTimingDict = (UITimingDict) null;
        if (this.m_widgetTicks.TryGetValue(keyValuePair.Key, out uiTimingDict))
          stringBuilder.AppendFormat("{0}\n", (object) uiTimingDict.PrintDict(5));
      }
    }
    return stringBuilder.ToString();
  }

  private void Update()
  {
    if ((double) Time.get_time() - (double) this.m_lastUpdateTime < 1.0)
      return;
    foreach (KeyValuePair<int, UIPanelData> elapsedTick in this.m_elapsedTicks)
    {
      if (this.m_accumulatedPanels.ContainsKey(elapsedTick.Key))
        this.m_accumulatedPanels[elapsedTick.Key].Enlarge(elapsedTick.Value);
      else
        this.m_accumulatedPanels.Add(elapsedTick.Key, elapsedTick.Value);
    }
    this.m_debugInfo = this.PrintDictDouble();
    this.m_elapsedTicks.Clear();
    this.m_widgetTicks.Clear();
    this.m_lastUpdateTime = Time.get_time();
  }

  private void OnGUI()
  {
    if (string.IsNullOrEmpty(this.m_debugInfo))
      return;
    Rect rect;
    ((Rect) ref rect).\u002Ector(250f, 60f, (float) (Screen.get_width() - 640), (float) Screen.get_height() - 120f);
    GUI.DrawTexture(rect, this.m_debugTexture);
    this.guiStyle.set_fontSize(20);
    this.guiStyle.get_normal().set_textColor(Color.get_white());
    GUI.Label(rect, this.m_debugInfo, this.guiStyle);
  }

  public void StartStats()
  {
    if (((Behaviour) this).get_enabled())
      return;
    ((Behaviour) this).set_enabled(true);
    this.m_accumulated.Clear();
    this.m_accumulatedPanels.Clear();
    this.m_lastUpdateTime = Time.get_time();
    this.m_startFrame = Time.get_frameCount();
    this.m_startTime = Time.get_time();
  }

  public void StopStats()
  {
    if (!((Behaviour) this).get_enabled())
      return;
    ((Behaviour) this).set_enabled(false);
    float num1 = Time.get_time() - this.m_startTime;
    List<string> stringList = new List<string>();
    int num2 = Time.get_frameCount() - this.m_startFrame;
    stringList.Add(string.Format("name \ttotalMS \tperFrameMS \trebuildCount \tupdateCount \tdrawcallCount/updateCount \t --- {0} frames ---", (object) num2));
    List<KeyValuePair<int, UIPanelData>> list1 = this.m_accumulatedPanels.ToList<KeyValuePair<int, UIPanelData>>();
    list1.Sort((Comparison<KeyValuePair<int, UIPanelData>>) ((pair1, pair2) => pair2.Value.mElapsedTicks.CompareTo(pair1.Value.mElapsedTicks)));
    foreach (KeyValuePair<int, UIPanelData> keyValuePair in list1)
    {
      string name = UIDebugCache.GetName(keyValuePair.Key);
      UIPanelData uiPanelData = keyValuePair.Value;
      stringList.Add(string.Format("{0}\t{1:0.00}\t{2:0.00}\t{3}\t{4}\t{5}", (object) name, (object) uiPanelData.mElapsedTicks, (object) (uiPanelData.mElapsedTicks / (double) num2), (object) (int) ((double) uiPanelData.mRebuildCount / (double) num1), (object) (int) ((double) uiPanelData.mCalls / (double) num1), (object) (uiPanelData.mDrawCallNum / uiPanelData.mCalls)));
    }
    List<KeyValuePair<int, double>> list2 = this.m_accumulated.ToList<KeyValuePair<int, double>>();
    list2.Sort((Comparison<KeyValuePair<int, double>>) ((pair1, pair2) => pair2.Value.CompareTo(pair1.Value)));
    foreach (KeyValuePair<int, double> keyValuePair in list2)
    {
      string str = UIDebugCache.GetName(keyValuePair.Key);
      string parentName = UIDebugCache.GetParentName(keyValuePair.Key);
      if (!string.IsNullOrEmpty(parentName))
        str = string.Format("{0}:{1}", (object) parentName, (object) str);
      stringList.Add(string.Format("{0}\t{1:0.00}\t{2:0.00}", (object) str, (object) keyValuePair.Value, (object) (keyValuePair.Value / (double) num2)));
    }
    File.WriteAllLines(Path.Combine(Application.get_persistentDataPath(), string.Format("TestTools/ui_stats_panels_{0}_{1}.log", (object) SysUtil.FormatDateAsFileNameString(DateTime.Now), (object) SysUtil.FormatTimeAsFileNameString(DateTime.Now))), stringList.ToArray());
  }

  public void StartPanelUpdate()
  {
    if (!((Behaviour) this).get_enabled())
      return;
    this.m_panelSW.Reset();
    this.m_panelSW.Start();
  }

  public void StopPanelUpdate(Object panel, bool bRebuild, int drawCallNum)
  {
    if (!((Behaviour) this).get_enabled())
      return;
    int instanceId = panel.GetInstanceID();
    this.m_panelSW.Stop();
    double num = (double) this.m_panelSW.ElapsedTicks / 10000.0;
    if (this.m_elapsedTicks.ContainsKey(instanceId))
    {
      UIPanelData elapsedTick = this.m_elapsedTicks[instanceId];
      elapsedTick.mElapsedTicks += num;
      ++elapsedTick.mCalls;
      elapsedTick.mRebuildCount += !bRebuild ? 0 : 1;
      elapsedTick.mDrawCallNum += drawCallNum;
    }
    else
    {
      this.m_elapsedTicks.Add(instanceId, new UIPanelData()
      {
        mElapsedTicks = num,
        mCalls = 1,
        mRebuildCount = !bRebuild ? 0 : 1,
        mDrawCallNum = drawCallNum
      });
      if (UIDebugCache.s_nameLut.ContainsKey(instanceId))
        return;
      UIDebugCache.s_nameLut.Add(instanceId, panel.get_name());
    }
  }

  public void StartPanelWidget(Object p)
  {
    if (!((Behaviour) this).get_enabled())
      return;
    int instanceId = p.GetInstanceID();
    UITimingDict uiTimingDict = (UITimingDict) null;
    if (!this.m_widgetTicks.TryGetValue(instanceId, out uiTimingDict))
    {
      uiTimingDict = new UITimingDict();
      this.m_widgetTicks.Add(instanceId, uiTimingDict);
    }
    uiTimingDict.StartTiming();
  }

  public void StopPanelWidget(Object p, Object w)
  {
    if (!((Behaviour) this).get_enabled())
      return;
    int instanceId1 = p.GetInstanceID();
    int instanceId2 = w.GetInstanceID();
    UITimingDict uiTimingDict = (UITimingDict) null;
    if (!this.m_widgetTicks.TryGetValue(instanceId1, out uiTimingDict))
      return;
    if (!UIDebugCache.s_parentNameLut.ContainsKey(w.GetInstanceID()))
      UIDebugCache.s_parentNameLut.Add(w.GetInstanceID(), p.get_name());
    double num = uiTimingDict.StopTiming(w);
    if (this.m_accumulated.ContainsKey(instanceId2))
    {
      Dictionary<int, double> accumulated;
      int index;
      (accumulated = this.m_accumulated)[index = instanceId2] = accumulated[index] + num;
    }
    else
      this.m_accumulated.Add(instanceId2, num);
  }
}
