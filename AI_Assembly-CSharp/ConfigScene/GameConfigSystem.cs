// Decompiled with JetBrains decompiler
// Type: ConfigScene.GameConfigSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ConfigScene
{
  public class GameConfigSystem : BaseSystem
  {
    public int FontSpeed = 40;
    public bool ReadSkip = true;
    public bool NextVoiceStop = true;
    public float AutoWaitTime = 3f;
    public bool TextWindowOption = true;
    public bool ActionGuide = true;
    public bool StoryHelp = true;
    public bool MiniMap = true;
    public bool CenterPointer = true;
    public bool ChoicesSkip;
    public bool ChoicesAuto;
    public bool ParameterLock;

    public GameConfigSystem(string elementName)
      : base(elementName)
    {
    }

    public override void Init()
    {
      this.FontSpeed = 40;
      this.ReadSkip = true;
      this.NextVoiceStop = true;
      this.AutoWaitTime = 3f;
      this.ChoicesSkip = false;
      this.ChoicesAuto = false;
      this.TextWindowOption = true;
      this.ActionGuide = true;
      this.StoryHelp = true;
      this.MiniMap = true;
      this.CenterPointer = true;
      this.ParameterLock = false;
    }
  }
}
