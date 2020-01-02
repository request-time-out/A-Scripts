// Decompiled with JetBrains decompiler
// Type: Weapon
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
  public GameObject weaponBody;
  public IMBrush brush;
  public Animator animator;
  public AudioSource audio;
  public AudioClip equipAudio;
  public AudioClip shotAudio;

  protected Weapon()
  {
    base.\u002Ector();
  }

  protected abstract void DoShoot(DungeonControl2 dungeon, Vector3 from, Vector3 to);

  public void Shoot(DungeonControl2 dungeon, Vector3 from, Vector3 to)
  {
    if (Object.op_Inequality((Object) this.audio, (Object) null) && Object.op_Inequality((Object) this.shotAudio, (Object) null))
      this.audio.PlayOneShot(this.shotAudio);
    this.DoShoot(dungeon, from, to);
  }

  public void OnEquip()
  {
    this.animator.SetBool("EQUIP", true);
    if (!Object.op_Inequality((Object) this.audio, (Object) null) || !Object.op_Inequality((Object) this.equipAudio, (Object) null))
      return;
    this.audio.PlayOneShot(this.equipAudio);
  }

  public void OnRemove()
  {
    this.animator.SetBool("EQUIP", false);
  }
}
