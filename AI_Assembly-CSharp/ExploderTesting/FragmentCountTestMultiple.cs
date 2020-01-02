// Decompiled with JetBrains decompiler
// Type: ExploderTesting.FragmentCountTestMultiple
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;

namespace ExploderTesting
{
  internal class FragmentCountTestMultiple : TestCase
  {
    private int count;
    private int target;

    public FragmentCountTestMultiple(int target, int count)
    {
      this.target = target;
      this.count = count;
    }

    public override string ToString()
    {
      return base.ToString() + " " + (object) this.target + " " + (object) this.count;
    }

    [DebuggerHidden]
    protected override IEnumerator RunTest()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new FragmentCountTestMultiple.\u003CRunTest\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
