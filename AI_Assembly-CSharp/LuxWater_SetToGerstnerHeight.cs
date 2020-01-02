// Decompiled with JetBrains decompiler
// Type: LuxWater_SetToGerstnerHeight
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

public class LuxWater_SetToGerstnerHeight : MonoBehaviour
{
  public Material WaterMaterial;
  public Vector3 Damping;
  public float TimeOffset;
  public bool UpdateWaterMaterialPerFrame;
  [Space(8f)]
  public bool AddCircleAnim;
  public float Radius;
  public float Speed;
  [Space(8f)]
  public Transform[] ManagedWaterProjectors;
  [Header("Debug")]
  public float MaxDisp;
  private Transform trans;
  private LuxWaterUtils.GersterWavesDescription Description;
  private bool ObjectIsVisible;
  private Vector3 Offset;

  public LuxWater_SetToGerstnerHeight()
  {
    base.\u002Ector();
  }

  private void Start()
  {
    this.trans = ((Component) this).get_transform();
    LuxWaterUtils.GetGersterWavesDescription(ref this.Description, this.WaterMaterial);
  }

  private void OnBecameVisible()
  {
    this.ObjectIsVisible = true;
  }

  private void OnBecameInvisible()
  {
    this.ObjectIsVisible = false;
  }

  private void LateUpdate()
  {
    if (!this.ObjectIsVisible && !this.AddCircleAnim || Object.op_Equality((Object) this.WaterMaterial, (Object) null))
      return;
    if (this.UpdateWaterMaterialPerFrame)
      LuxWaterUtils.GetGersterWavesDescription(ref this.Description, this.WaterMaterial);
    Vector3 WorldPosition = Vector3.op_Subtraction(this.trans.get_position(), this.Offset);
    if (this.AddCircleAnim)
    {
      ref Vector3 local1 = ref WorldPosition;
      local1.x = (__Null) (local1.x + (double) Mathf.Sin(Time.get_time() * this.Speed) * (double) Time.get_deltaTime() * (double) this.Radius);
      ref Vector3 local2 = ref WorldPosition;
      local2.z = (__Null) (local2.z + (double) Mathf.Cos(Time.get_time() * this.Speed) * (double) Time.get_deltaTime() * (double) this.Radius);
    }
    int length = this.ManagedWaterProjectors.Length;
    if (length > 0)
    {
      for (int index = 0; index != length; ++index)
      {
        Vector3 position = this.ManagedWaterProjectors[index].get_position();
        position.x = WorldPosition.x;
        position.z = WorldPosition.z;
        this.ManagedWaterProjectors[index].set_position(position);
      }
    }
    this.Offset = LuxWaterUtils.GetGestnerDisplacement(WorldPosition, this.Description, this.TimeOffset);
    ref Vector3 local3 = ref this.Offset;
    local3.x = local3.x + this.Offset.x * this.Damping.x;
    ref Vector3 local4 = ref this.Offset;
    local4.y = local4.y + this.Offset.y * this.Damping.y;
    ref Vector3 local5 = ref this.Offset;
    local5.z = local5.z + this.Offset.z * this.Damping.z;
    this.trans.set_position(Vector3.op_Addition(WorldPosition, this.Offset));
  }
}
