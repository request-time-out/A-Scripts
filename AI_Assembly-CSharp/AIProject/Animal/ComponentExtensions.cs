// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.ComponentExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public static class ComponentExtensions
  {
    public static void SetActiveSelf(this Component com, bool active)
    {
      if (Object.op_Equality((Object) com, (Object) null) || Object.op_Equality((Object) com.get_gameObject(), (Object) null) || com.get_gameObject().get_activeSelf() == active)
        return;
      com.get_gameObject().SetActive(active);
    }
  }
}
