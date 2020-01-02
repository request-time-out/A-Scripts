// Decompiled with JetBrains decompiler
// Type: Exploder.ExploderSlowMotion
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Exploder.Utils;
using UnityEngine;

namespace Exploder
{
  public class ExploderSlowMotion : MonoBehaviour
  {
    public float slowMotionTime;
    private ExploderObject Exploder;
    private float slowMotionSpeed;
    private bool slowmo;

    public ExploderSlowMotion()
    {
      base.\u002Ector();
    }

    private void Start()
    {
      this.Exploder = ExploderSingleton.Instance;
    }

    public void EnableSlowMotion(bool status)
    {
      this.slowmo = status;
      if (this.slowmo)
      {
        this.slowMotionSpeed = 0.05f;
        if (Object.op_Implicit((Object) this.Exploder))
          this.Exploder.FragmentOptions.MeshColliders = true;
      }
      else
      {
        this.slowMotionSpeed = 1f;
        if (Object.op_Implicit((Object) this.Exploder))
          this.Exploder.FragmentOptions.MeshColliders = false;
      }
      this.slowMotionTime = this.slowMotionSpeed;
    }

    public void Update()
    {
      this.slowMotionSpeed = this.slowMotionTime;
      Time.set_timeScale(this.slowMotionSpeed);
      Time.set_fixedDeltaTime(this.slowMotionSpeed * 0.02f);
      if (!Input.GetKeyDown((KeyCode) 116))
        return;
      this.slowmo = !this.slowmo;
      this.EnableSlowMotion(this.slowmo);
    }
  }
}
