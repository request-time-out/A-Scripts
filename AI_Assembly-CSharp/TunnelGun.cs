// Decompiled with JetBrains decompiler
// Type: TunnelGun
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class TunnelGun : Weapon
{
  public ParticleSystem hitPS;
  public ParticleSystem shootPS;

  protected override void DoShoot(DungeonControl2 dungeon, Vector3 from, Vector3 to)
  {
    this.weaponBody.get_transform().set_position(from);
    this.weaponBody.get_transform().LookAt(to, Vector3.get_up());
    dungeon.Attack(this.brush);
    Object.Instantiate<GameObject>((M0) ((Component) this.hitPS).get_gameObject(), to, Quaternion.get_identity());
    Object.Instantiate<GameObject>((M0) ((Component) this.shootPS).get_gameObject(), this.weaponBody.get_transform().get_position(), this.weaponBody.get_transform().get_rotation());
  }
}
