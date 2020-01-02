// Decompiled with JetBrains decompiler
// Type: DungeonControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class DungeonControl : MonoBehaviour
{
  public Camera myCamera;
  public StaticMetaballSeed metaball;
  public ParticleSystem hitPS;
  public AudioSource audioSource;

  public DungeonControl()
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

  public void AddCell(Vector3 position, float size)
  {
    this.audioSource.Play();
    GameObject gameObject = new GameObject("MetaballNode");
    gameObject.get_transform().set_parent(((Component) this.metaball.sourceRoot).get_transform());
    gameObject.get_transform().set_position(position);
    gameObject.get_transform().set_localScale(Vector3.get_one());
    gameObject.get_transform().set_localRotation(Quaternion.get_identity());
    ((MetaballNode) gameObject.AddComponent<MetaballNode>()).baseRadius = size;
    this.metaball.CreateMesh();
    MeshCollider component = (MeshCollider) ((Component) this.metaball).GetComponent<MeshCollider>();
    if (Object.op_Inequality((Object) component, (Object) null))
      component.set_sharedMesh(this.metaball.Mesh);
    Object.Instantiate<GameObject>((M0) ((Component) this.hitPS).get_gameObject(), position, Quaternion.get_identity());
  }
}
