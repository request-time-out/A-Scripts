// Decompiled with JetBrains decompiler
// Type: AIProject.Definitions.State
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

namespace AIProject.Definitions
{
  public static class State
  {
    private static State.Type[] _commandableTypes = new State.Type[8]
    {
      State.Type.Sleep,
      State.Type.Toilet,
      State.Type.Bath,
      State.Type.Search,
      State.Type.Cook,
      State.Type.Collapse,
      State.Type.Cold,
      State.Type.Hurt
    };

    public static bool ContainsCommandableTypes(State.Type source)
    {
      foreach (State.Type commandableType in State._commandableTypes)
      {
        if (source == commandableType)
          return true;
      }
      return false;
    }

    public enum Type
    {
      Immobility,
      Normal,
      Greet,
      Commun,
      Sleep,
      Toilet,
      Bath,
      Search,
      Cook,
      Collapse,
      Cold,
      Hurt,
    }
  }
}
