// Decompiled with JetBrains decompiler
// Type: TentacleControl
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class TentacleControl : MonoBehaviour
{
  public SkinnedMetaballSeed seed;

  public TentacleControl()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.SetupPhysicsBones();
  }

  private void Update()
  {
  }

  private void SetupPhysicsBones()
  {
    MetaballCellObject componentInChildren = (MetaballCellObject) ((Component) this.seed.boneRoot).GetComponentInChildren<MetaballCellObject>();
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null))
      return;
    this.SetupPhysicsBonesRecursive(componentInChildren, true);
  }

  private void SetupPhysicsBonesRecursive(MetaballCellObject obj, bool bRoot = false)
  {
    Rigidbody rigidbody = (Rigidbody) ((Component) obj).GetComponent<Rigidbody>();
    if (Object.op_Equality((Object) rigidbody, (Object) null))
      rigidbody = (Rigidbody) ((Component) obj).get_gameObject().AddComponent<Rigidbody>();
    rigidbody.set_useGravity(false);
    if (bRoot)
    {
      FixedJoint fixedJoint = (FixedJoint) ((Component) obj).GetComponent<FixedJoint>();
      if (Object.op_Equality((Object) fixedJoint, (Object) null))
        fixedJoint = (FixedJoint) ((Component) obj).get_gameObject().AddComponent<FixedJoint>();
      ((Joint) fixedJoint).set_connectedBody((Rigidbody) ((Component) this.seed).GetComponent<Rigidbody>());
    }
    else
    {
      HingeJoint hingeJoint1 = (HingeJoint) ((Component) obj).GetComponent<HingeJoint>();
      if (Object.op_Equality((Object) hingeJoint1, (Object) null))
        hingeJoint1 = (HingeJoint) ((Component) obj).get_gameObject().AddComponent<HingeJoint>();
      ((Joint) hingeJoint1).set_connectedBody((Rigidbody) ((Component) ((Component) obj).get_transform().get_parent()).GetComponent<Rigidbody>());
      hingeJoint1.set_useLimits(true);
      HingeJoint hingeJoint2 = hingeJoint1;
      JointLimits jointLimits1 = (JointLimits) null;
      ((JointLimits) ref jointLimits1).set_max(30f);
      ((JointLimits) ref jointLimits1).set_min(-30f);
      JointLimits jointLimits2 = jointLimits1;
      hingeJoint2.set_limits(jointLimits2);
    }
    for (int index = 0; index < ((Component) obj).get_transform().get_childCount(); ++index)
    {
      MetaballCellObject component = (MetaballCellObject) ((Component) ((Component) obj).get_transform().GetChild(index)).GetComponent<MetaballCellObject>();
      if (Object.op_Inequality((Object) component, (Object) null))
        this.SetupPhysicsBonesRecursive(component, false);
    }
  }
}
