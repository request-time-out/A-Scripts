// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.BehaviourExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public static class BehaviourExtensions
  {
    public static void SetEnableSelf(this Behaviour beh, bool enabled)
    {
      if (!Object.op_Inequality((Object) beh, (Object) null) || beh.get_enabled() == enabled)
        return;
      beh.set_enabled(enabled);
    }
  }
}
