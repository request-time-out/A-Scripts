// Decompiled with JetBrains decompiler
// Type: IMBrushBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class IMBrushBox : IMBrush
{
  public Vector3 extents = Vector3.get_one();

  protected override void DoDraw()
  {
    this.im.AddBox(((Component) this).get_transform(), this.extents, this.PowerScale, this.fadeRadius);
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.set_matrix(((Component) this).get_transform().get_localToWorldMatrix());
    Gizmos.set_color(Color.get_green());
    Gizmos.DrawWireCube(Vector3.get_zero(), Vector3.op_Multiply(this.extents, 2f));
    if (!Object.op_Inequality((Object) this.im, (Object) null))
      return;
    Gizmos.set_color(!this.bSubtract ? Color.get_white() : Color.get_red());
    float powerThreshold = this.im.powerThreshold;
    Vector3 vector3;
    ((Vector3) ref vector3).\u002Ector(Mathf.Lerp((float) this.extents.x, (float) this.extents.x - this.fadeRadius, powerThreshold), Mathf.Lerp((float) this.extents.y, (float) this.extents.y - this.fadeRadius, powerThreshold), Mathf.Lerp((float) this.extents.z, (float) this.extents.z - this.fadeRadius, powerThreshold));
    Gizmos.DrawWireCube(Vector3.get_zero(), Vector3.op_Multiply(vector3, 2f));
  }
}
