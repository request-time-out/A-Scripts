// Decompiled with JetBrains decompiler
// Type: BehaviorDesigner.Runtime.SharedBool
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;

namespace BehaviorDesigner.Runtime
{
  [Serializable]
  public class SharedBool : SharedVariable<bool>
  {
    public SharedBool()
    {
      base.\u002Ector();
    }

    public static implicit operator SharedBool(bool value)
    {
      SharedBool sharedBool = new SharedBool();
      sharedBool.mValue = (__Null) (value ? 1 : 0);
      return sharedBool;
    }
  }
}
