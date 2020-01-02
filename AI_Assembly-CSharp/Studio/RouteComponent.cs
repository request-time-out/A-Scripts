// Decompiled with JetBrains decompiler
// Type: Studio.RouteComponent
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Studio
{
  public class RouteComponent : MonoBehaviour
  {
    public RouteComponent.OnCompleteDel onComplete;

    public RouteComponent()
    {
      base.\u002Ector();
    }

    public void OnComplete()
    {
      if (this.onComplete == null)
        return;
      int num = this.onComplete() ? 1 : 0;
    }

    public delegate bool OnCompleteDel();
  }
}
