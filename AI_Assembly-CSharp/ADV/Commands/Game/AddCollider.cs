// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Game.AddCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using UnityEngine;

namespace ADV.Commands.Game
{
  public class AddCollider : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return (string[]) null;
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return (string[]) null;
      }
    }

    public override void Do()
    {
      base.Do();
      GameObject gameObject = ((Component) this.scenario.currentChara.transform).get_gameObject();
      CapsuleCollider capsuleCollider = (CapsuleCollider) gameObject.AddComponent<CapsuleCollider>();
      capsuleCollider.set_center(new Vector3(0.0f, 0.75f, 0.0f));
      capsuleCollider.set_radius(0.5f);
      capsuleCollider.set_height(1.5f);
      ((Collider) capsuleCollider).set_isTrigger(true);
      gameObject.AddComponent<Rigidbody>();
    }
  }
}
