// Decompiled with JetBrains decompiler
// Type: MetaCollider
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

public class MetaCollider : MonoBehaviour
{
  [Tooltip("こいつに当たったらLayerの名前を変更したりジョイントを切り離す dragWaitの値になる")]
  public string[] nameJudgeTags;
  [Tooltip("変更するLayerの名")]
  public string nameChangeLayer;
  [Tooltip("こいつに当たったらdragHitの値になる 拘束判定も含む")]
  public string[] nameWaitTags;
  [Tooltip("こいつに当たったらdragHitの値になる\n離れた瞬間dragAirが入る\nウエイトのついてる当たり判定？\nnameJudgeTagsより判定強い")]
  public string nameBodytagName;
  [Tooltip("nameJudgeTagsのColliderに当たった瞬間")]
  public float dragWait;
  [Tooltip("nameWaitTagsのColliderに当たった瞬間")]
  public float dragHit;
  [Tooltip("nameWaitTagsのColliderから離れた瞬間")]
  public float dragAir;
  [Tooltip("当たったコライダーと拘束する？")]
  public bool isConstraint;
  [Tooltip("当たったコライダーと拘束するまでの時間")]
  public float constTime;
  [Tooltip("強制的にスリープにするまでの時間")]
  public float sleepTime;
  [Tooltip("後ろのオブジェクトのRigidBody")]
  public Rigidbody nextRigidBody;
  [Tooltip("Air時に重力付けて,それ以外の時は切る")]
  public bool isEndGravity;
  [Tooltip("追加のちから")]
  public float addNextForce;
  [Header("確認用")]
  [Tooltip("Air中にこの時間でDragが変化する dragAirの値になる")]
  public float timeDropDown;
  [SerializeField]
  [Tooltip("確認用")]
  private float timeLerpDrag;
  [SerializeField]
  [Tooltip("確認用")]
  private float dragTempAir;
  [SerializeField]
  [Tooltip("確認用 拘束されている先")]
  private Transform parentTransfrom;
  private HashSet<string> judgeTags;
  private HashSet<string> judgeWaitTags;
  [SerializeField]
  [Tooltip("確認用表示 拘束計算するまで")]
  private float stayTime;
  [SerializeField]
  [Tooltip("確認用表示 強制的にスリープにするまで")]
  private float sleepForceTime;
  [SerializeField]
  [Tooltip("確認用表示 拘束計算が終わった？")]
  private bool isConstraintNow;
  private Vector3 posConstLocal;
  private MetaCollider.Hit objhit;
  [SerializeField]
  [Tooltip("確認用表示")]
  private MetaCollider.Hit objnowhit;
  [SerializeField]
  [Tooltip("確認用表示")]
  private MetaCollider.Hit nowhit;
  [SerializeField]
  [Tooltip("確認用表示")]
  private bool isNextAddForce;
  private Rigidbody rigid;
  private bool isGroundHit;
  private MetaballJoint metajoint;
  private ConfigurableJoint joint;

  public MetaCollider()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.judgeTags = new HashSet<string>((IEnumerable<string>) this.nameJudgeTags);
    if (this.judgeTags == null)
      this.judgeTags = new HashSet<string>();
    this.judgeWaitTags = new HashSet<string>((IEnumerable<string>) this.nameWaitTags);
    if (this.judgeWaitTags == null)
      this.judgeWaitTags = new HashSet<string>();
    this.rigid = (Rigidbody) ((Component) this).GetComponent<Rigidbody>();
    this.metajoint = (MetaballJoint) ((Component) this).GetComponent<MetaballJoint>();
    this.joint = (ConfigurableJoint) ((Component) this).GetComponent<ConfigurableJoint>();
  }

  private void FixedUpdate()
  {
    if (this.isGroundHit || this.objnowhit != MetaCollider.Hit.air && this.objnowhit != MetaCollider.Hit.exit || this.nowhit != MetaCollider.Hit.air)
      return;
    this.lerpDrag();
  }

  private void Update()
  {
    if (this.isConstraintNow && Object.op_Inequality((Object) this.parentTransfrom, (Object) null))
    {
      Transform transform = ((Component) this).get_transform();
      Matrix4x4 localToWorldMatrix = this.parentTransfrom.get_localToWorldMatrix();
      Vector3 vector3 = ((Matrix4x4) ref localToWorldMatrix).MultiplyPoint3x4(this.posConstLocal);
      transform.set_position(vector3);
    }
    if (!Object.op_Implicit((Object) this.rigid) || this.rigid.get_isKinematic())
      return;
    this.sleepForceTime += Time.get_deltaTime();
    if ((double) this.sleepTime > (double) this.sleepForceTime)
      return;
    this.rigid.set_isKinematic(true);
    this.rigid.Sleep();
  }

  private void LateUpdate()
  {
    if (!this.isGroundHit)
    {
      if (this.objhit == MetaCollider.Hit.hit)
      {
        if (this.objnowhit == MetaCollider.Hit.air || this.objnowhit == MetaCollider.Hit.exit)
        {
          this.objnowhit = MetaCollider.Hit.hit;
          this.rigid.set_drag(this.dragHit);
        }
        else if (this.objnowhit == MetaCollider.Hit.hit)
          this.objnowhit = MetaCollider.Hit.stay;
      }
      else if (this.objnowhit == MetaCollider.Hit.stay || this.objnowhit == MetaCollider.Hit.hit)
      {
        this.objnowhit = MetaCollider.Hit.exit;
        this.dragTempAir = this.dragHit;
        this.timeLerpDrag = 0.0f;
      }
      if ((this.objnowhit == MetaCollider.Hit.air || this.objnowhit == MetaCollider.Hit.exit) && (this.nowhit == MetaCollider.Hit.air && this.isEndGravity) && (Object.op_Implicit((Object) this.rigid) && !this.rigid.get_useGravity()))
        this.rigid.set_useGravity(true);
    }
    this.objhit = MetaCollider.Hit.air;
  }

  private void OnCollisionEnter(Collision col)
  {
    if (col.get_gameObject().get_layer() == 13 || col.get_gameObject().get_layer() == 17)
      return;
    if (this.isEndGravity && Object.op_Implicit((Object) this.rigid) && this.rigid.get_useGravity())
      this.rigid.set_useGravity(false);
    if (this.judgeTags.Contains(col.get_gameObject().get_tag()))
    {
      this.isGroundHit = true;
      ((Component) this).get_gameObject().set_layer(LayerMask.NameToLayer(this.nameChangeLayer));
      if (Object.op_Implicit((Object) this.rigid))
      {
        this.rigid.set_mass(1f);
        this.rigid.set_useGravity(true);
        this.rigid.set_drag(this.dragWait);
      }
      if (Object.op_Implicit((Object) this.metajoint))
        Object.Destroy((Object) this.metajoint);
      if (Object.op_Implicit((Object) this.joint))
        Object.Destroy((Object) this.joint);
    }
    if (this.judgeWaitTags.Contains(col.get_gameObject().get_tag()))
    {
      this.nowhit = MetaCollider.Hit.hit;
      if (Object.op_Implicit((Object) this.rigid))
        this.rigid.set_drag(this.dragHit);
      this.Constraint(col);
    }
    this.ChangeJoint();
    this.NextAddForce();
    if (!(col.get_gameObject().get_tag() == this.nameBodytagName))
      return;
    this.objhit = MetaCollider.Hit.hit;
  }

  private void OnCollisionStay(Collision col)
  {
    if (!this.judgeWaitTags.Contains(col.get_gameObject().get_tag()))
      return;
    this.Constraint(col);
  }

  private void OnCollisionExit(Collision col)
  {
    if (!this.judgeWaitTags.Contains(col.get_gameObject().get_tag()))
      return;
    this.nowhit = MetaCollider.Hit.air;
    this.timeLerpDrag = 0.0f;
    this.dragTempAir = this.dragHit;
  }

  public bool ChangeJoint()
  {
    if (!Object.op_Implicit((Object) this.metajoint) || !Object.op_Implicit((Object) this.joint))
      return false;
    if (!((Behaviour) this.metajoint).get_isActiveAndEnabled())
      return true;
    ((Behaviour) this.metajoint).set_enabled(false);
    this.joint.set_xMotion((ConfigurableJointMotion) 1);
    this.joint.set_yMotion((ConfigurableJointMotion) 1);
    this.joint.set_zMotion((ConfigurableJointMotion) 1);
    return true;
  }

  private bool Constraint(Collision col)
  {
    if (!this.isConstraint || this.isConstraintNow)
      return true;
    this.stayTime += Time.get_deltaTime();
    if ((double) this.stayTime < (double) this.constTime)
      return true;
    if (Object.op_Implicit((Object) this.rigid))
    {
      this.rigid.set_isKinematic(true);
      this.rigid.Sleep();
    }
    this.isConstraintNow = true;
    return true;
  }

  private bool NextAddForce()
  {
    if (!Object.op_Implicit((Object) this.nextRigidBody) || this.isNextAddForce)
      return true;
    Vector3 vector3 = Vector3.op_Subtraction(((Component) this).get_transform().get_position(), ((Component) this.nextRigidBody).get_transform().get_position());
    this.nextRigidBody.set_velocity(Vector3.get_zero());
    this.nextRigidBody.AddForce(Vector3.op_Multiply(((Vector3) ref vector3).get_normalized(), this.addNextForce), (ForceMode) 1);
    this.isNextAddForce = true;
    return true;
  }

  private bool lerpDrag()
  {
    this.timeLerpDrag += Time.get_deltaTime();
    this.timeLerpDrag = Mathf.Clamp(this.timeLerpDrag, 0.0f, this.timeDropDown);
    float num = Mathf.InverseLerp(0.0f, this.timeDropDown, this.timeLerpDrag);
    if ((double) this.timeDropDown == 0.0)
      num = 1f;
    if (Object.op_Implicit((Object) this.rigid))
      this.rigid.set_drag(Mathf.Lerp(this.dragTempAir, this.dragAir, num));
    return true;
  }

  public enum Hit
  {
    air,
    hit,
    ground,
    stay,
    exit,
  }
}
