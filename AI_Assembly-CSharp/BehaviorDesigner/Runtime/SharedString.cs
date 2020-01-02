// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedString
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedString : SharedVariable<string>
  {
    public SharedString()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedString(string value)
    {
      SharedString sharedString = new SharedString();
      sharedString.mValue = (__Null) value;
      return sharedString;
    }
  }
}
