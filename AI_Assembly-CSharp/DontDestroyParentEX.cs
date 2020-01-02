// Decompiled with JetBrains decompiler
// Type: DontDestroyParentEX
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngineExtra;

internal static class DontDestroyParentEX
{
  public static void DontDestroyOnNextLoad(this GameObject self, GameObject target)
  {
    DontDestroyParent.Register(target);
  }

  public static void DontDestroyOnNextLoad(this GameObject self, MonoBehaviour target)
  {
    DontDestroyParent.Register(target);
  }

  public static void DontDestroyOnNextLoad(this MonoBehaviour self, GameObject target)
  {
    DontDestroyParent.Register(target);
  }

  public static void DontDestroyOnNextLoad(this MonoBehaviour self, MonoBehaviour target)
  {
    DontDestroyParent.Register(target);
  }
}
