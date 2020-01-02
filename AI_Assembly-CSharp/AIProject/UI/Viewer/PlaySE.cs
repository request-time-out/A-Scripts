// Decompiled with JetBrains decompiler
// Type: AIProject.UI.Viewer.PlaySE
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using Manager;

namespace AIProject.UI.Viewer
{
  public class PlaySE
  {
    public PlaySE()
      : this(true)
    {
    }

    public PlaySE(bool use)
    {
      this.use = use;
    }

    public bool use { private get; set; }

    public void Play(SoundPack.SystemSE se)
    {
      if (!this.use)
        return;
      Singleton<Resources>.Instance.SoundPack.Play(se);
    }
  }
}
