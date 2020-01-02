// Decompiled with JetBrains decompiler
// Type: LineRendererBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LineRendererBehaviour : MonoBehaviour
{
  public bool IsVertical;
  public float LightHeightOffset;
  public float ParticlesHeightOffset;
  public float TimeDestroyLightAfterCollision;
  public float TimeDestroyThisAfterCollision;
  public float TimeDestroyRootAfterCollision;
  public GameObject EffectOnHitObject;
  public GameObject Explosion;
  public GameObject StartGlow;
  public GameObject HitGlow;
  public GameObject Particles;
  public GameObject GoLight;
  private EffectSettings effectSettings;
  private Transform tRoot;
  private Transform tTarget;
  private bool isInitializedOnStart;
  private LineRenderer line;
  private int currentShaderIndex;
  private RaycastHit hit;

  public LineRendererBehaviour()
  {
    base.\u002Ector();
  }

  private void GetEffectSettingsComponent(Transform tr)
  {
    Transform parent = tr.get_parent();
    if (!Object.op_Inequality((Object) parent, (Object) null))
      return;
    this.effectSettings = (EffectSettings) ((Component) parent).GetComponentInChildren<EffectSettings>();
    if (!Object.op_Equality((Object) this.effectSettings, (Object) null))
      return;
    this.GetEffectSettingsComponent(((Component) parent).get_transform());
  }

  private void Start()
  {
    this.GetEffectSettingsComponent(((Component) this).get_transform());
    if (Object.op_Equality((Object) this.effectSettings, (Object) null))
      Debug.Log((object) "Prefab root have not script \"PrefabSettings\"");
    this.tRoot = ((Component) this.effectSettings).get_transform();
    this.line = (LineRenderer) ((Component) this).GetComponent<LineRenderer>();
    this.InitializeDefault();
    this.isInitializedOnStart = true;
  }

  private void InitializeDefault()
  {
    ((Renderer) ((Component) this).GetComponent<Renderer>()).get_material().SetFloat("_Chanel", (float) this.currentShaderIndex);
    ++this.currentShaderIndex;
    if (this.currentShaderIndex == 3)
      this.currentShaderIndex = 0;
    this.line.SetPosition(0, this.tRoot.get_position());
    if (this.IsVertical)
    {
      if (Physics.Raycast(this.tRoot.get_position(), Vector3.get_down(), ref this.hit))
      {
        this.line.SetPosition(1, ((RaycastHit) ref this.hit).get_point());
        if (Object.op_Inequality((Object) this.StartGlow, (Object) null))
          this.StartGlow.get_transform().set_position(this.tRoot.get_position());
        if (Object.op_Inequality((Object) this.HitGlow, (Object) null))
          this.HitGlow.get_transform().set_position(((RaycastHit) ref this.hit).get_point());
        if (Object.op_Inequality((Object) this.GoLight, (Object) null))
          this.GoLight.get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref this.hit).get_point(), new Vector3(0.0f, this.LightHeightOffset, 0.0f)));
        if (Object.op_Inequality((Object) this.Particles, (Object) null))
          this.Particles.get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref this.hit).get_point(), new Vector3(0.0f, this.ParticlesHeightOffset, 0.0f)));
        if (Object.op_Inequality((Object) this.Explosion, (Object) null))
          this.Explosion.get_transform().set_position(Vector3.op_Addition(((RaycastHit) ref this.hit).get_point(), new Vector3(0.0f, this.ParticlesHeightOffset, 0.0f)));
      }
    }
    else
    {
      if (Object.op_Inequality((Object) this.effectSettings.Target, (Object) null))
        this.tTarget = this.effectSettings.Target.get_transform();
      else if (!this.effectSettings.UseMoveVector)
        Debug.Log((object) "You must setup the the target or the motion vector");
      Vector3 vector3_1;
      if (!this.effectSettings.UseMoveVector)
      {
        Vector3 vector3_2 = Vector3.op_Subtraction(this.tTarget.get_position(), this.tRoot.get_position());
        vector3_1 = ((Vector3) ref vector3_2).get_normalized();
      }
      else
        vector3_1 = Vector3.op_Addition(this.tRoot.get_position(), Vector3.op_Multiply(this.effectSettings.MoveVector, this.effectSettings.MoveDistance));
      Vector3 vector3_3 = Vector3.op_Addition(this.tRoot.get_position(), Vector3.op_Multiply(vector3_1, this.effectSettings.MoveDistance));
      if (Physics.Raycast(this.tRoot.get_position(), vector3_1, ref this.hit, this.effectSettings.MoveDistance + 1f, LayerMask.op_Implicit(this.effectSettings.LayerMask)))
      {
        Vector3 vector3_2 = Vector3.op_Addition(this.tRoot.get_position(), Vector3.op_Multiply(Vector3.Normalize(Vector3.op_Subtraction(((RaycastHit) ref this.hit).get_point(), this.tRoot.get_position())), this.effectSettings.MoveDistance + 1f));
        vector3_3 = ((Vector3) ref vector3_2).get_normalized();
      }
      this.line.SetPosition(1, Vector3.op_Subtraction(((RaycastHit) ref this.hit).get_point(), Vector3.op_Multiply(this.effectSettings.ColliderRadius, vector3_3)));
      Vector3 vector3_4 = Vector3.op_Subtraction(((RaycastHit) ref this.hit).get_point(), Vector3.op_Multiply(vector3_3, this.ParticlesHeightOffset));
      if (Object.op_Inequality((Object) this.StartGlow, (Object) null))
        this.StartGlow.get_transform().set_position(this.tRoot.get_position());
      if (Object.op_Inequality((Object) this.HitGlow, (Object) null))
        this.HitGlow.get_transform().set_position(vector3_4);
      if (Object.op_Inequality((Object) this.GoLight, (Object) null))
        this.GoLight.get_transform().set_position(Vector3.op_Subtraction(((RaycastHit) ref this.hit).get_point(), Vector3.op_Multiply(vector3_3, this.LightHeightOffset)));
      if (Object.op_Inequality((Object) this.Particles, (Object) null))
        this.Particles.get_transform().set_position(vector3_4);
      if (Object.op_Inequality((Object) this.Explosion, (Object) null))
      {
        this.Explosion.get_transform().set_position(vector3_4);
        this.Explosion.get_transform().LookAt(Vector3.op_Addition(vector3_4, ((RaycastHit) ref this.hit).get_normal()));
      }
    }
    CollisionInfo e = new CollisionInfo()
    {
      Hit = this.hit
    };
    this.effectSettings.OnCollisionHandler(e);
    if (!Object.op_Inequality((Object) ((RaycastHit) ref this.hit).get_transform(), (Object) null))
      return;
    ShieldCollisionBehaviour component = (ShieldCollisionBehaviour) ((Component) ((RaycastHit) ref this.hit).get_transform()).GetComponent<ShieldCollisionBehaviour>();
    if (!Object.op_Inequality((Object) component, (Object) null))
      return;
    component.ShieldCollisionEnter(e);
  }

  private void OnEnable()
  {
    if (!this.isInitializedOnStart)
      return;
    this.InitializeDefault();
  }
}
