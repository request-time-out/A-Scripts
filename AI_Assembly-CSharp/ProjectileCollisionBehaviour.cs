// Decompiled with JetBrains decompiler
// Type: ProjectileCollisionBehaviour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class ProjectileCollisionBehaviour : MonoBehaviour
{
  public float RandomMoveRadius;
  public float RandomMoveSpeed;
  public float RandomRange;
  public RandomMoveCoordinates RandomMoveCoordinates;
  public GameObject EffectOnHitObject;
  public GameObject GoLight;
  public AnimationCurve Acceleration;
  public float AcceleraionTime;
  public bool IsCenterLightPosition;
  public bool IsLookAt;
  public bool AttachAfterCollision;
  public bool IsRootMove;
  public bool IsLocalSpaceRandomMove;
  public bool IsDeviation;
  public bool SendCollisionMessage;
  public bool ResetParentPositionOnDisable;
  private EffectSettings effectSettings;
  private Transform tRoot;
  private Transform tTarget;
  private Transform t;
  private Transform tLight;
  private Vector3 forwardDirection;
  private Vector3 startPosition;
  private Vector3 startParentPosition;
  private RaycastHit hit;
  private Vector3 smootRandomPos;
  private Vector3 oldSmootRandomPos;
  private float deltaSpeed;
  private float startTime;
  private float randomSpeed;
  private float randomRadiusX;
  private float randomRadiusY;
  private int randomDirection1;
  private int randomDirection2;
  private int randomDirection3;
  private bool onCollision;
  private bool isInitializedOnStart;
  private Vector3 randomTargetOffsetXZVector;
  private bool frameDroped;

  public ProjectileCollisionBehaviour()
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
    this.t = ((Component) this).get_transform();
    this.GetEffectSettingsComponent(this.t);
    if (Object.op_Equality((Object) this.effectSettings, (Object) null))
      Debug.Log((object) "Prefab root or children have not script \"PrefabSettings\"");
    if (!this.IsRootMove)
      this.startParentPosition = ((Component) this).get_transform().get_parent().get_position();
    if (Object.op_Inequality((Object) this.GoLight, (Object) null))
      this.tLight = this.GoLight.get_transform();
    this.InitializeDefault();
    this.isInitializedOnStart = true;
  }

  private void OnEnable()
  {
    if (!this.isInitializedOnStart)
      return;
    this.InitializeDefault();
  }

  private void OnDisable()
  {
    if (!this.ResetParentPositionOnDisable || !this.isInitializedOnStart || this.IsRootMove)
      return;
    ((Component) this).get_transform().get_parent().set_position(this.startParentPosition);
  }

  private void InitializeDefault()
  {
    this.hit = (RaycastHit) null;
    this.onCollision = false;
    this.smootRandomPos = (Vector3) null;
    this.oldSmootRandomPos = (Vector3) null;
    this.deltaSpeed = 0.0f;
    this.startTime = 0.0f;
    this.randomSpeed = 0.0f;
    this.randomRadiusX = 0.0f;
    this.randomRadiusY = 0.0f;
    this.randomDirection1 = 0;
    this.randomDirection2 = 0;
    this.randomDirection3 = 0;
    this.frameDroped = false;
    this.tRoot = !this.IsRootMove ? ((Component) this).get_transform().get_parent() : ((Component) this.effectSettings).get_transform();
    this.startPosition = this.tRoot.get_position();
    if (Object.op_Inequality((Object) this.effectSettings.Target, (Object) null))
      this.tTarget = this.effectSettings.Target.get_transform();
    else if (!this.effectSettings.UseMoveVector)
      Debug.Log((object) "You must setup the the target or the motion vector");
    if ((double) this.effectSettings.EffectRadius > 0.001)
    {
      Vector2 vector2 = Vector2.op_Multiply(Random.get_insideUnitCircle(), this.effectSettings.EffectRadius);
      this.randomTargetOffsetXZVector = new Vector3((float) vector2.x, 0.0f, (float) vector2.y);
    }
    else
      this.randomTargetOffsetXZVector = Vector3.get_zero();
    if (!this.effectSettings.UseMoveVector)
    {
      Vector3 position = this.tRoot.get_position();
      Vector3 vector3_1 = Vector3.op_Subtraction(Vector3.op_Addition(this.tTarget.get_position(), this.randomTargetOffsetXZVector), this.tRoot.get_position());
      Vector3 vector3_2 = Vector3.op_Multiply(((Vector3) ref vector3_1).get_normalized(), this.effectSettings.MoveDistance);
      this.forwardDirection = Vector3.op_Addition(position, vector3_2);
      this.GetTargetHit();
    }
    else
      this.forwardDirection = Vector3.op_Addition(this.tRoot.get_position(), Vector3.op_Multiply(this.effectSettings.MoveVector, this.effectSettings.MoveDistance));
    if (this.IsLookAt)
    {
      if (!this.effectSettings.UseMoveVector)
        this.tRoot.LookAt(this.tTarget);
      else
        this.tRoot.LookAt(this.forwardDirection);
    }
    this.InitRandomVariables();
  }

  private void Update()
  {
    if (!this.frameDroped)
    {
      this.frameDroped = true;
    }
    else
    {
      if ((!this.effectSettings.UseMoveVector && Object.op_Equality((Object) this.tTarget, (Object) null) || this.onCollision) && this.frameDroped)
        return;
      Vector3 vector3_1 = this.effectSettings.UseMoveVector ? this.forwardDirection : (!this.effectSettings.IsHomingMove ? this.forwardDirection : this.tTarget.get_position());
      float num1 = Vector3.Distance(this.tRoot.get_position(), vector3_1);
      float num2 = this.effectSettings.MoveSpeed * Time.get_deltaTime();
      if ((double) num2 > (double) num1)
        num2 = num1;
      if ((double) num1 <= (double) this.effectSettings.ColliderRadius)
      {
        this.hit = (RaycastHit) null;
        this.CollisionEnter();
      }
      Vector3 vector3_2 = Vector3.op_Subtraction(vector3_1, this.tRoot.get_position());
      Vector3 normalized = ((Vector3) ref vector3_2).get_normalized();
      RaycastHit raycastHit;
      if (Physics.Raycast(this.tRoot.get_position(), normalized, ref raycastHit, num2 + this.effectSettings.ColliderRadius, LayerMask.op_Implicit(this.effectSettings.LayerMask)))
      {
        this.hit = raycastHit;
        vector3_1 = Vector3.op_Subtraction(((RaycastHit) ref raycastHit).get_point(), Vector3.op_Multiply(normalized, this.effectSettings.ColliderRadius));
        this.CollisionEnter();
      }
      if (this.IsCenterLightPosition && Object.op_Inequality((Object) this.GoLight, (Object) null))
        this.tLight.set_position(Vector3.op_Division(Vector3.op_Addition(this.startPosition, this.tRoot.get_position()), 2f));
      Vector3 vector3_3 = (Vector3) null;
      if (this.RandomMoveCoordinates != RandomMoveCoordinates.None)
      {
        this.UpdateSmootRandomhPos();
        vector3_3 = Vector3.op_Subtraction(this.smootRandomPos, this.oldSmootRandomPos);
      }
      float num3 = 1f;
      if (this.Acceleration.get_length() > 0)
        num3 = this.Acceleration.Evaluate((Time.get_time() - this.startTime) / this.AcceleraionTime);
      Vector3 vector3_4 = Vector3.MoveTowards(this.tRoot.get_position(), vector3_1, this.effectSettings.MoveSpeed * Time.get_deltaTime() * num3);
      Vector3 vector3_5 = Vector3.op_Addition(vector3_4, vector3_3);
      if (this.IsLookAt && this.effectSettings.IsHomingMove)
        this.tRoot.LookAt(vector3_5);
      if (this.IsLocalSpaceRandomMove && this.IsRootMove)
      {
        this.tRoot.set_position(vector3_4);
        Transform t = this.t;
        t.set_localPosition(Vector3.op_Addition(t.get_localPosition(), vector3_3));
      }
      else
        this.tRoot.set_position(vector3_5);
      this.oldSmootRandomPos = this.smootRandomPos;
    }
  }

  private void CollisionEnter()
  {
    if (Object.op_Inequality((Object) this.EffectOnHitObject, (Object) null) && Object.op_Inequality((Object) ((RaycastHit) ref this.hit).get_transform(), (Object) null))
    {
      Renderer componentInChildren = (Renderer) ((Component) ((RaycastHit) ref this.hit).get_transform()).GetComponentInChildren<Renderer>();
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.EffectOnHitObject);
      gameObject.get_transform().set_parent(((Component) componentInChildren).get_transform());
      gameObject.get_transform().set_localPosition(Vector3.get_zero());
      ((AddMaterialOnHit) gameObject.GetComponent<AddMaterialOnHit>()).UpdateMaterial(this.hit);
    }
    if (this.AttachAfterCollision)
      this.tRoot.set_parent(((RaycastHit) ref this.hit).get_transform());
    if (this.SendCollisionMessage)
    {
      CollisionInfo e = new CollisionInfo()
      {
        Hit = this.hit
      };
      this.effectSettings.OnCollisionHandler(e);
      if (Object.op_Inequality((Object) ((RaycastHit) ref this.hit).get_transform(), (Object) null))
      {
        ShieldCollisionBehaviour component = (ShieldCollisionBehaviour) ((Component) ((RaycastHit) ref this.hit).get_transform()).GetComponent<ShieldCollisionBehaviour>();
        if (Object.op_Inequality((Object) component, (Object) null))
          component.ShieldCollisionEnter(e);
      }
    }
    this.onCollision = true;
  }

  private void InitRandomVariables()
  {
    this.deltaSpeed = (float) ((double) this.RandomMoveSpeed * (double) Random.Range(1f, (float) (1000.0 * (double) this.RandomRange + 1.0)) / 1000.0 - 1.0);
    this.startTime = Time.get_time();
    this.randomRadiusX = Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
    this.randomRadiusY = Random.Range(this.RandomMoveRadius / 20f, this.RandomMoveRadius * 100f) / 100f;
    this.randomSpeed = Random.Range(this.RandomMoveSpeed / 20f, this.RandomMoveSpeed * 100f) / 100f;
    this.randomDirection1 = Random.Range(0, 2) <= 0 ? -1 : 1;
    this.randomDirection2 = Random.Range(0, 2) <= 0 ? -1 : 1;
    this.randomDirection3 = Random.Range(0, 2) <= 0 ? -1 : 1;
  }

  private void GetTargetHit()
  {
    Ray ray;
    ((Ray) ref ray).\u002Ector(this.tRoot.get_position(), Vector3.Normalize(Vector3.op_Subtraction(Vector3.op_Addition(this.tTarget.get_position(), this.randomTargetOffsetXZVector), this.tRoot.get_position())));
    Collider componentInChildren = (Collider) ((Component) this.tTarget).GetComponentInChildren<Collider>();
    RaycastHit raycastHit;
    if (!Object.op_Inequality((Object) componentInChildren, (Object) null) || !componentInChildren.Raycast(ray, ref raycastHit, this.effectSettings.MoveDistance))
      return;
    this.hit = raycastHit;
  }

  private void UpdateSmootRandomhPos()
  {
    float num1 = Time.get_time() - this.startTime;
    float num2 = num1 * this.randomSpeed;
    float num3 = num1 * this.deltaSpeed;
    float num4;
    float num5;
    if (this.IsDeviation)
    {
      float num6 = Vector3.Distance(this.tRoot.get_position(), ((RaycastHit) ref this.hit).get_point()) / this.effectSettings.MoveDistance;
      num4 = (float) this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX * num6;
      num5 = (float) this.randomDirection3 * Mathf.Sin(num2 + (float) ((double) this.randomDirection1 * 3.14159274101257 / 2.0) * num1 + Mathf.Sin(num3)) * this.randomRadiusY * num6;
    }
    else
    {
      num4 = (float) this.randomDirection2 * Mathf.Sin(num2) * this.randomRadiusX;
      num5 = (float) this.randomDirection3 * Mathf.Sin(num2 + (float) ((double) this.randomDirection1 * 3.14159274101257 / 2.0) * num1 + Mathf.Sin(num3)) * this.randomRadiusY;
    }
    if (this.RandomMoveCoordinates == RandomMoveCoordinates.XY)
      this.smootRandomPos = new Vector3(num4, num5, 0.0f);
    if (this.RandomMoveCoordinates == RandomMoveCoordinates.XZ)
      this.smootRandomPos = new Vector3(num4, 0.0f, num5);
    if (this.RandomMoveCoordinates == RandomMoveCoordinates.YZ)
      this.smootRandomPos = new Vector3(0.0f, num4, num5);
    if (this.RandomMoveCoordinates != RandomMoveCoordinates.XYZ)
      return;
    this.smootRandomPos = new Vector3(num4, num5, (float) (((double) num4 + (double) num5) / 2.0) * (float) this.randomDirection1);
  }
}
