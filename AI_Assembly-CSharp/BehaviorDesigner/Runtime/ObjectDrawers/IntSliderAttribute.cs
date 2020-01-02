// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.ObjectDrawers.IntSliderAttribute
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;

namespace BehaviorDesigner.Runtime.ObjectDrawers
{
  public class IntSliderAttribute : ObjectDrawerAttribute
  {
    public int min;
    public int max;

    public IntSliderAttribute(int min, int max)
    {
      this.\u002Ector();
      this.min = min;
      this.max = max;
    }
  }
}
