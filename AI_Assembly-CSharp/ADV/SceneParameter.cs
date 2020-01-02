// Decompiled with JetBrains decompiler
// Type: ADV.SceneParameter
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ADV
{
  public abstract class SceneParameter
  {
    public SceneParameter(IData data)
    {
      this.data = data;
    }

    public static ADVScene advScene { get; set; }

    public IData data { get; }

    public virtual void Init()
    {
    }

    public virtual void Release()
    {
    }

    public virtual void WaitEndProc()
    {
    }
  }
}
