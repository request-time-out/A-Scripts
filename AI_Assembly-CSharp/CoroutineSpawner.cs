// Decompiled with JetBrains decompiler
// Type: CoroutineSpawner
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class CoroutineSpawner : MonoBehaviour
{
  public CoroutineSpawner()
  {
    base.\u002Ector();
  }

  [DebuggerHidden]
  public IEnumerator Co01_WaitForSeconds()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    CoroutineSpawner.\u003CCo01_WaitForSeconds\u003Ec__Iterator0 secondsCIterator0 = new CoroutineSpawner.\u003CCo01_WaitForSeconds\u003Ec__Iterator0();
    return (IEnumerator) secondsCIterator0;
  }

  [DebuggerHidden]
  public IEnumerator Co02_PerFrame_NULL()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    CoroutineSpawner.\u003CCo02_PerFrame_NULL\u003Ec__Iterator1 frameNullCIterator1 = new CoroutineSpawner.\u003CCo02_PerFrame_NULL\u003Ec__Iterator1();
    return (IEnumerator) frameNullCIterator1;
  }

  [DebuggerHidden]
  public IEnumerator Co03_PerFrame_EOF()
  {
    // ISSUE: object of a compiler-generated type is created
    // ISSUE: variable of a compiler-generated type
    CoroutineSpawner.\u003CCo03_PerFrame_EOF\u003Ec__Iterator2 frameEofCIterator2 = new CoroutineSpawner.\u003CCo03_PerFrame_EOF\u003Ec__Iterator2();
    return (IEnumerator) frameEofCIterator2;
  }

  [DebuggerHidden]
  public IEnumerator Co04_PerFrame_ARG(float argFloat)
  {
    // ISSUE: object of a compiler-generated type is created
    return (IEnumerator) new CoroutineSpawner.\u003CCo04_PerFrame_ARG\u003Ec__Iterator3()
    {
      argFloat = argFloat
    };
  }
}
