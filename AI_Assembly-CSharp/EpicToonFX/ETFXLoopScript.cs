// Decompiled with JetBrains decompiler
// Type: EpicToonFX.ETFXLoopScript
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace EpicToonFX
{
  public class ETFXLoopScript : MonoBehaviour
  {
    public GameObject chosenEffect;
    public float loopTimeLimit;
    [Header("Spawn without")]
    public bool spawnWithoutLight;
    public bool spawnWithoutSound;

    public ETFXLoopScript()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.PlayEffect();
    }

    public void PlayEffect()
    {
      this.StartCoroutine("EffectLoop");
    }

    [DebuggerHidden]
    private IEnumerator EffectLoop()
    {
      // ISSUE: object of a compiler-generated type is created
      return (IEnumerator) new ETFXLoopScript.\u003CEffectLoop\u003Ec__Iterator0()
      {
        \u0024this = this
      };
    }
  }
}
