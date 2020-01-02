// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.Vector2IntExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace AIProject.Animal
{
  public static class Vector2IntExtensions
  {
    public static int RandomRange(this Vector2Int vec)
    {
      return Random.Range(((Vector2Int) ref vec).get_x(), ((Vector2Int) ref vec).get_y() + 1);
    }
  }
}
