// Decompiled with JetBrains decompiler
// Type: AudioSetting
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;
using UnityEngine;

[RequireComponent(typeof (AudioSource))]
public sealed class AudioSetting : BaseLoader
{
  public int no = -1;
  private bool isInit;

  private void Start()
  {
    if (this.isInit)
      return;
    this.Init();
  }

  public void Init()
  {
    this.isInit = true;
    if (this.no >= 0)
    {
      AudioSource component = (AudioSource) ((Component) this).GetComponent<AudioSource>();
      float delayTime = 0.0f;
      if (Singleton<Sound>.IsInstance())
        delayTime = Singleton<Sound>.Instance.AudioSettingData(component, this.no).delayTime;
      if (component.get_playOnAwake())
      {
        component.PlayDelayed(delayTime);
        if (!component.get_loop() && Object.op_Inequality((Object) component.get_clip(), (Object) null))
          component.PlayEndDestroy(delayTime);
      }
      else
        component.Stop();
    }
    Object.Destroy((Object) this);
  }
}
