// Decompiled with JetBrains decompiler
// Type: Exploder.Demo.UseObject
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder.Demo
{
  public abstract class UseObject : MonoBehaviour
  {
    public float UseRadius;
    public string HelperText;
    public AudioClip UseClip;

    protected UseObject()
    {
      base.\u002Ector();
    }

    public virtual void Use()
    {
      AudioSource component = (AudioSource) ((Component) this).GetComponent<AudioSource>();
      if (!Object.op_Implicit((Object) component) || !Object.op_Implicit((Object) this.UseClip))
        return;
      component.PlayOneShot(this.UseClip);
    }

    private void OnDrawGizmos()
    {
      Gizmos.set_color(Color.get_yellow());
      Gizmos.DrawWireSphere(((Component) this).get_transform().get_position(), this.UseRadius);
    }
  }
}
