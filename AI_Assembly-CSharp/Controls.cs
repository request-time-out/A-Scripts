// Decompiled with JetBrains decompiler
// Type: Controls
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

[ExecuteInEditMode]
public class Controls : MonoBehaviour
{
  public AmplifyOcclusionEffect occlusion;
  private const AmplifyOcclusionBase.ApplicationMethod POST = AmplifyOcclusionBase.ApplicationMethod.PostEffect;
  private const AmplifyOcclusionBase.ApplicationMethod DEFERRED = AmplifyOcclusionBase.ApplicationMethod.Deferred;
  private const AmplifyOcclusionBase.ApplicationMethod DEBUG = AmplifyOcclusionBase.ApplicationMethod.Debug;

  public Controls()
  {
    base.\u002Ector();
  }

  private void OnGUI()
  {
    GUILayout.BeginArea(new Rect(0.0f, 0.0f, (float) Screen.get_width(), (float) Screen.get_height()));
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(5f);
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    ((Behaviour) this.occlusion).set_enabled(GUILayout.Toggle(((Behaviour) this.occlusion).get_enabled(), " Amplify Occlusion Enabled", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()));
    GUILayout.Space(5f);
    this.occlusion.ApplyMethod = !GUILayout.Toggle(this.occlusion.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.PostEffect, " Standard Post-effect", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()) ? this.occlusion.ApplyMethod : AmplifyOcclusionBase.ApplicationMethod.PostEffect;
    this.occlusion.ApplyMethod = !GUILayout.Toggle(this.occlusion.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.Deferred, " Deferred Injection", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()) ? this.occlusion.ApplyMethod : AmplifyOcclusionBase.ApplicationMethod.Deferred;
    this.occlusion.ApplyMethod = !GUILayout.Toggle(this.occlusion.ApplyMethod == AmplifyOcclusionBase.ApplicationMethod.Debug, " Debug Mode", (GUILayoutOption[]) Array.Empty<GUILayoutOption>()) ? this.occlusion.ApplyMethod : AmplifyOcclusionBase.ApplicationMethod.Debug;
    GUILayout.EndVertical();
    GUILayout.FlexibleSpace();
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(5f);
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label("Intensity     ", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    this.occlusion.Intensity = GUILayout.HorizontalSlider(this.occlusion.Intensity, 0.0f, 1f, new GUILayoutOption[1]
    {
      GUILayout.Width(100f)
    });
    GUILayout.Space(5f);
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label(" " + this.occlusion.Intensity.ToString("0.00"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    GUILayout.Space(5f);
    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label("Power Exp. ", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    this.occlusion.PowerExponent = GUILayout.HorizontalSlider(this.occlusion.PowerExponent, 0.0001f, 6f, new GUILayoutOption[1]
    {
      GUILayout.Width(100f)
    });
    GUILayout.Space(5f);
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label(" " + this.occlusion.PowerExponent.ToString("0.00"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    GUILayout.Space(5f);
    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label("Radius        ", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    this.occlusion.Radius = GUILayout.HorizontalSlider(this.occlusion.Radius, 0.1f, 10f, new GUILayoutOption[1]
    {
      GUILayout.Width(100f)
    });
    GUILayout.Space(5f);
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label(" " + this.occlusion.Radius.ToString("0.00"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    GUILayout.Space(5f);
    GUILayout.EndHorizontal();
    GUILayout.BeginHorizontal((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label("Quality        ", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    this.occlusion.SampleCount = (AmplifyOcclusionBase.SampleCountLevel) GUILayout.HorizontalSlider((float) this.occlusion.SampleCount, 0.0f, 3f, new GUILayoutOption[1]
    {
      GUILayout.Width(100f)
    });
    GUILayout.Space(5f);
    GUILayout.BeginVertical((GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.Space(-3f);
    GUILayout.Label("        ", (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
    GUILayout.EndVertical();
    GUILayout.Space(5f);
    GUILayout.EndHorizontal();
    GUILayout.EndVertical();
    GUILayout.EndHorizontal();
    GUILayout.EndArea();
  }
}
