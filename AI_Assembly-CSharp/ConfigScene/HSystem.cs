// Decompiled with JetBrains decompiler
// Type: ConfigScene.HSystem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace ConfigScene
{
  public class HSystem : BaseSystem
  {
    public bool Visible = true;
    public bool Son = true;
    public bool Cloth = true;
    public bool Accessory = true;
    public bool Shoes = true;
    public int Siru = 1;
    public bool Gloss = true;
    public bool FeelingGauge = true;
    public bool ActionGuide = true;
    public bool MenuIcon = true;
    public bool FinishButton = true;
    public bool InitCamera = true;
    public bool Urine;
    public bool Sio;
    public bool EyeDir0;
    public bool NeckDir0;
    public bool EyeDir1;
    public bool NeckDir1;

    public HSystem(string elementName)
      : base(elementName)
    {
    }

    public override void Init()
    {
      this.Visible = true;
      this.Son = true;
      this.Cloth = true;
      this.Accessory = true;
      this.Shoes = true;
      this.Siru = 1;
      this.Urine = false;
      this.Sio = false;
      this.Gloss = true;
      this.FeelingGauge = true;
      this.ActionGuide = true;
      this.MenuIcon = true;
      this.FinishButton = true;
      this.InitCamera = true;
      this.EyeDir0 = false;
      this.NeckDir0 = false;
      this.EyeDir1 = false;
      this.NeckDir1 = false;
    }
  }
}
