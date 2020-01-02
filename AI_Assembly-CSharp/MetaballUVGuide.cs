// Decompiled with JetBrains decompiler
// Type: MetaballUVGuide
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class MetaballUVGuide : MonoBehaviour
{
  public float uScale;
  public float vScale;

  public MetaballUVGuide()
  {
    base.\u002Ector();
  }

  private void Start()
  {
  }

  private void Update()
  {
  }

  private void OnDrawGizmosSelected()
  {
    Matrix4x4 matrix = Gizmos.get_matrix();
    Gizmos.set_matrix(((Component) this).get_transform().get_localToWorldMatrix());
    Gizmos.set_color(Color.get_white());
    Gizmos.DrawWireCube(new Vector3(this.uScale * 0.5f, this.vScale * 0.5f, 0.0f), new Vector3(this.uScale, this.vScale, 15f));
    Gizmos.set_matrix(matrix);
  }
}
