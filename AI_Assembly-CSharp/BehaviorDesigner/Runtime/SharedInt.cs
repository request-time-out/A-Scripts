// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedInt
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedInt : SharedVariable<int>
  {
    public SharedInt()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedInt(int value)
    {
      SharedInt sharedInt = new SharedInt();
      sharedInt.mValue = (__Null) value;
      return sharedInt;
    }
  }
}
