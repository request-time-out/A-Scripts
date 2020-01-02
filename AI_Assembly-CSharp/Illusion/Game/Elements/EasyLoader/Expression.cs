// Decompiled with JetBrains decompiler
// Type: Illusion.Game.Elements.EasyLoader.Expression
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using AIChara;
using System;

namespace Illusion.Game.Elements.EasyLoader
{
  [Serializable]
  public class Expression
  {
    public int personality;
    public string name;

    public virtual void Setting(ChaControl chaCtrl, int personality, string name)
    {
    }

    public virtual void Setting(ChaControl chaCtrl)
    {
      this.Setting(chaCtrl, this.personality, this.name);
    }
  }
}
