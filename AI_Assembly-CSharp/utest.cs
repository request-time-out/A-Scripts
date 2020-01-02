// Decompiled with JetBrains decompiler
// Type: utest
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class utest : IDisposable
{
  public float m_renderOrdinaryPercentage = 100f;
  public float m_renderParticlePercentage = 100f;
  public bool m_enable;

  public void Dispose()
  {
  }

  public void OnLevelWasLoaded()
  {
    Scene activeScene = SceneManager.GetActiveScene();
    if (((Scene) ref activeScene).get_name() == "Loading0")
      return;
    GameUtil.Log("on_level loaded.", (object[]) Array.Empty<object>());
    GameInterface.Instance.Init();
  }

  public void OnGUI()
  {
    using (new utest.FontSetter())
    {
      if (this.m_enable)
      {
        GUILayout.BeginArea(new Rect(200f, 50f, (float) (Screen.get_width() - 400), (float) (Screen.get_height() - 100)), nameof (utest), GUI.get_skin().get_window());
        GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        if (this.Gui_ShowCloseButton())
          this.m_enable = false;
        GUILayout.EndHorizontal();
        GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        this.Gui_ShowToggles();
        if (this.Gui_ChangeByPercentSlider(ref this.m_renderOrdinaryPercentage, GameInterface.Instance.DisabledRenderers.Count) || this.Gui_ChangeByPercentSlider(ref this.m_renderParticlePercentage, GameInterface.Instance.DisabledParticleSystems.Count))
          GameInterface.Instance.FilterVisibleObjects(this.m_renderOrdinaryPercentage * 0.01f, this.m_renderParticlePercentage * 0.01f);
        this.Gui_ShowLogs();
        GUILayout.EndVertical();
        GUILayout.EndArea();
      }
      else
      {
        if (!GUI.Button(new Rect(50f, (float) ((double) Screen.get_height() * 0.5 - 40.0), 80f, 80f), nameof (utest)))
          return;
        this.m_enable = !this.m_enable;
      }
    }
  }

  private bool Gui_ShowCloseButton()
  {
    return GUILayout.Button("×", new GUILayoutOption[2]
    {
      GUILayout.MinWidth((float) (GUI.get_skin().get_label().CalcSize(new GUIContent("M")).x * 1.5)),
      GUILayout.ExpandWidth(false)
    });
  }

  private void Gui_ShowToggles()
  {
    using (Dictionary<string, GameObject>.Enumerator enumerator = GameInterface.Instance.KeyNodes.GetEnumerator())
    {
      while (enumerator.MoveNext())
      {
        KeyValuePair<string, GameObject> current = enumerator.Current;
        if (!current.Value.get_activeInHierarchy() && current.Value.get_activeSelf())
          GUI.set_enabled(false);
        bool flag = GUILayout.Toggle(current.Value.get_activeSelf(), current.Key, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
        if (current.Value.get_activeSelf() != flag)
          current.Value.SetActive(flag);
        GUI.set_enabled(true);
      }
    }
  }

  private void Gui_ShowLogs()
  {
    GUILayout.Box("Log", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GameUtil._logPosition = GUILayout.BeginScrollView(GameUtil._logPosition, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Label(GameUtil._log, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndScrollView();
  }

  private bool Gui_ChangeByPercentSlider(ref float percentage, int disabledCount)
  {
    bool flag = false;
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    float num = GUILayout.HorizontalSlider(percentage, 0.0f, 100f, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    if ((double) num != (double) percentage)
    {
      percentage = num;
      flag = true;
    }
    GUILayout.Label(percentage.ToString("0.00"), new GUILayoutOption[2]
    {
      GUILayout.MinWidth((float) (GUI.get_skin().get_label().CalcSize(new GUIContent("100.00")).x * 1.5)),
      GUILayout.ExpandWidth(false)
    });
    GUILayout.Label(disabledCount.ToString(), new GUILayoutOption[2]
    {
      GUILayout.MinWidth((float) (GUI.get_skin().get_label().CalcSize(new GUIContent("000")).x * 1.5)),
      GUILayout.ExpandWidth(false)
    });
    GUILayout.EndHorizontal();
    return flag;
  }

  public class FontSetter : IDisposable
  {
    private int m_oldSize = GUI.get_skin().get_button().get_fontSize();

    public FontSetter()
    {
      GUI.get_skin().get_button().set_fontSize(GameUtil.FontSize);
    }

    public void Dispose()
    {
      GUI.get_skin().get_button().set_fontSize(this.m_oldSize);
    }
  }
}
