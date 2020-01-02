// Decompiled with JetBrains decompiler
// Type: AIProject.MerchantBehavior.SetAbsentPoint
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using BehaviorDesigner.Runtime.Tasks;
using System.Collections;
using System.Diagnostics;

namespace AIProject.MerchantBehavior
{
  [TaskCategory("商人")]
  public class SetAbsentPoint : MerchantSetPoint
  {
    [DebuggerHidden]
    protected override IEnumerator NextPointSettingCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new SetAbsentPoint.\u003CNextPointSettingCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
