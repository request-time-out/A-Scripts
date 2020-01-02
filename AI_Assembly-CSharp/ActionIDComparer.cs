// Decompiled with JetBrains decompiler
// Type: ActionIDComparer
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;

public class ActionIDComparer : IEqualityComparer<ActionID>
{
  public bool Equals(ActionID _id0, ActionID _id1)
  {
    return _id0 == _id1;
  }

  public int GetHashCode(ActionID _id)
  {
    return (int) _id;
  }
}
