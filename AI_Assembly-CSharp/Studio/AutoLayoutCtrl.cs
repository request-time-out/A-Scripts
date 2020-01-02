// Decompiled with JetBrains decompiler
// Type: Studio.AutoLayoutCtrl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Studio
{
  public class AutoLayoutCtrl : MonoBehaviour
  {
    [SerializeField]
    private HorizontalOrVerticalLayoutGroup horizontalOrVerticalLayoutGroup;
    [SerializeField]
    private ContentSizeFitter contentSizeFitter;
    [SerializeField]
    private AutoLayoutCtrl.Func func;

    public AutoLayoutCtrl()
    {
      base.\u002Ector();
    }

    [DebuggerHidden]
    private IEnumerator WaitFuncCoroutine()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new AutoLayoutCtrl.\u003CWaitFuncCoroutine\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }

    private void Start()
    {
      this.StartCoroutine(this.WaitFuncCoroutine());
    }

    private enum Func
    {
      Disabled,
      Delete,
    }
  }
}
