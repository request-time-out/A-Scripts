// Decompiled with JetBrains decompiler
// Type: IMBrushSphere
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class IMBrushSphere : IMBrush
{
  public float radius = 1f;

  protected override void DoDraw()
  {
    this.im.AddSphere(((Component) this).get_transform(), this.radius, this.PowerScale, this.fadeRadius);
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.set_matrix(((Component) this).get_transform().get_localToWorldMatrix());
    Gizmos.set_color(Color.get_green());
    Gizmos.DrawWireSphere(Vector3.get_zero(), this.radius);
    if (!Object.op_Inequality((Object) this.im, (Object) null))
      return;
    Gizmos.set_color(!this.bSubtract ? Color.get_white() : Color.get_red());
    Gizmos.DrawWireSphere(Vector3.get_zero(), Mathf.Lerp(this.radius, this.radius - this.fadeRadius, this.im.powerThreshold));
  }
}
