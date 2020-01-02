// Decompiled with JetBrains decompiler
// Type: DungeonControl2
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class DungeonControl2 : MonoBehaviour
{
  public Camera myCamera;
  public IncrementalModeling metaball;
  public AudioSource audioSource;

  public DungeonControl2()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    MeshCollider component = (MeshCollider) ((Component) this.metaball).GetComponent<MeshCollider>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.set_sharedMesh(this.metaball.Mesh);
  }

  private void Update()
  {
  }

  public void Attack(IMBrush brush)
  {
    this.audioSource.Play();
    brush.Draw();
    MeshCollider component = (MeshCollider) ((Component) this.metaball).GetComponent<MeshCollider>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.set_sharedMesh(this.metaball.Mesh);
  }
}
