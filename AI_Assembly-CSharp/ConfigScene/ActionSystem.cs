// Decompiled with JetBrains decompiler
// Type: ConfigScene.ActionSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

namespace ConfigScene
{
  public class ActionSystem : BaseSystem
  {
    public bool Look = true;
    public const int TPS_SENSITIVITY_X = 0;
    public const int TPS_SENSITIVITY_Y = 0;
    public const int FPS_SENSITIVITY_X = 0;
    public const int FPS_SENSITIVITY_Y = 0;
    public int TPSSensitivityX;
    public int TPSSensitivityY;
    public int FPSSensitivityX;
    public int FPSSensitivityY;
    public bool InvertMoveX;
    public bool InvertMoveY;

    public ActionSystem(string elementName)
      : base(elementName)
    {
    }

    public override void Init()
    {
      this.TPSSensitivityX = 0;
      this.TPSSensitivityY = 0;
      this.FPSSensitivityX = 0;
      this.FPSSensitivityY = 0;
      this.InvertMoveX = false;
      this.InvertMoveY = false;
      this.Look = true;
    }
  }
}
