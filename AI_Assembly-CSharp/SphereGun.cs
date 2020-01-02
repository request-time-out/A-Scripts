// Decompiled with JetBrains decompiler
// Type: SphereGun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

public class SphereGun : Weapon
{
  public ParticleSystem hitPS;

  protected override void DoShoot(DungeonControl2 dungeon, Vector3 from, Vector3 to)
  {
    this.weaponBody.get_transform().set_position(to);
    dungeon.Attack(this.brush);
    Object.Instantiate<GameObject>((M0) ((Component) this.hitPS).get_gameObject(), to, Quaternion.get_identity());
  }
}
