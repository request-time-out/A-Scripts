// Decompiled with JetBrains decompiler
// Type: ConfigScene.DebugSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ConfigScene
{
  public class DebugSystem : BaseSystem
  {
    public bool FPS;

    public DebugSystem(string elementName)
      : base(elementName)
    {
    }

    public override void Init()
    {
      this.FPS = false;
    }
  }
}
