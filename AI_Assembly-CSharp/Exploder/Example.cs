// Decompiled with JetBrains decompiler
// Type: Exploder.Example
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public class Example : MonoBehaviour
  {
    public ExploderObject Exploder;

    public Example()
    {
      base.\u002Ector();
    }

    public void ExplodeObject(GameObject obj)
    {
      this.Exploder.ExplodeObject(obj, new ExploderObject.OnExplosion(this.OnExplosion));
    }

    private void OnExplosion(float time, ExploderObject.ExplosionState state)
    {
      if (state != ExploderObject.ExplosionState.ExplosionFinished)
        ;
    }

    private void CrackAndExplodeObject(GameObject obj)
    {
      this.Exploder.CrackObject(obj, new ExploderObject.OnExplosion(this.OnCracked));
    }

    private void OnCracked(float time, ExploderObject.ExplosionState state)
    {
      this.Exploder.ExplodeCracked(new ExploderObject.OnExplosion(this.OnExplosion));
    }
  }
}
