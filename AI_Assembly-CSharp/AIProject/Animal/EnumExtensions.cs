// Decompiled with JetBrains decompiler
// Type: AIProject.Animal.EnumExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.Animal
{
  public static class EnumExtensions
  {
    public static bool Contains(this ActionTypes source, ActionTypes type)
    {
      return source == (source | type) && type != ActionTypes.None;
    }
  }
}
