// Decompiled with JetBrains decompiler
// Type: ILSetUtility.TimeUtility.TimeUtility
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace ILSetUtility.TimeUtility
{
  public class TimeUtility : MonoBehaviour
  {
    private float fps;
    [Range(0.0f, 50f)]
    public float time_scale;
    private float deltaTime;
    private uint frame_cnt;
    private float time_cnt;
    public bool mode_mem;
    private float memTime;
    private GUIStyle style;
    private GUIStyleState styleState;
    public bool ForceDrawFPS;

    public TimeUtility()
    {
      base.\u002Ector();
    }

    private void Awake()
    {
      this.fps = 0.0f;
      this.time_scale = 1f;
      this.deltaTime = 0.0f;
      this.frame_cnt = 0U;
      this.time_cnt = 0.0f;
      this.mode_mem = false;
      this.memTime = 0.0f;
    }

    private void Start()
    {
      this.style = new GUIStyle();
      this.style.set_fontSize(20);
      this.styleState = new GUIStyleState();
      this.styleState.set_textColor(new Color(1f, 1f, 1f));
      this.style.set_normal(this.styleState);
      ((Behaviour) this).set_enabled(false);
    }

    private void Update()
    {
      if (!Input.GetKey((KeyCode) 303) || !Input.GetKeyDown((KeyCode) (int) sbyte.MaxValue))
        ;
      this.deltaTime = Time.get_deltaTime() * this.time_scale;
      if (this.mode_mem)
        this.memTime += Time.get_deltaTime();
      this.time_cnt += Time.get_deltaTime();
      ++this.frame_cnt;
      if (1.0 > (double) this.time_cnt)
        return;
      this.fps = (float) this.frame_cnt / this.time_cnt;
      this.frame_cnt = 0U;
      this.time_cnt = 0.0f;
    }

    private void OnGUI()
    {
      GUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.Label("FPS:" + this.fps.ToString("000.0"), this.style, (GUILayoutOption[]) Array.Empty<GUILayoutOption>());
      GUILayout.EndVertical();
    }

    public void SetTimeScale(float value)
    {
      this.time_scale = value;
    }

    public float GetTimeScale()
    {
      return this.time_scale;
    }

    public float GetFps()
    {
      return this.fps;
    }

    public float GetTime()
    {
      return this.deltaTime;
    }

    public void ChangeMemoryFlags(bool flags)
    {
      this.mode_mem = flags;
    }

    public float GetMemoryTime()
    {
      return this.memTime;
    }
  }
}
