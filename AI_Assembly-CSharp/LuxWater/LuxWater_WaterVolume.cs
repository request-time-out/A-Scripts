// Decompiled with JetBrains decompiler
// Type: LuxWater.LuxWater_WaterVolume
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.Rendering;

namespace LuxWater
{
  public class LuxWater_WaterVolume : MonoBehaviour
  {
    [Space(6f)]
    [LuxWater_HelpBtn("h.86taxuhovssb")]
    public Mesh WaterVolumeMesh;
    [Space(8f)]
    public bool SlidingVolume;
    public float GridSize;
    private LuxWater_UnderWaterRendering waterrendermanager;
    private bool readyToGo;
    private int ID;

    public LuxWater_WaterVolume()
    {
      base.\u002Ector();
    }

    public static event LuxWater_WaterVolume.TriggerEnter OnEnterWaterVolume;

    public static event LuxWater_WaterVolume.TriggerExit OnExitWaterVolume;

    private void OnEnable()
    {
      if (Object.op_Equality((Object) this.WaterVolumeMesh, (Object) null))
      {
        Debug.Log((object) "No WaterVolumeMesh assigned.");
      }
      else
      {
        this.ID = ((Object) this).GetInstanceID();
        this.Invoke("Register", 0.0f);
        Renderer component = (Renderer) ((Component) this).GetComponent<Renderer>();
        component.set_shadowCastingMode((ShadowCastingMode) 0);
        Material sharedMaterial = component.get_sharedMaterial();
        sharedMaterial.EnableKeyword("USINGWATERVOLUME");
        sharedMaterial.SetFloat("_WaterSurfaceYPos", (float) ((Component) this).get_transform().get_position().y);
      }
    }

    private void OnDisable()
    {
      if (Object.op_Implicit((Object) this.waterrendermanager))
        this.waterrendermanager.DeRegisterWaterVolume(this, this.ID);
      this.readyToGo = false;
      ((Renderer) ((Component) this).GetComponent<Renderer>()).get_sharedMaterial().DisableKeyword("USINGWATERVOLUME");
    }

    private void Register()
    {
      if (Object.op_Inequality((Object) LuxWater_UnderWaterRendering.instance, (Object) null))
      {
        this.waterrendermanager = LuxWater_UnderWaterRendering.instance;
        this.waterrendermanager.RegisterWaterVolume(this, this.ID, ((Renderer) ((Component) this).GetComponent<Renderer>()).get_isVisible(), this.SlidingVolume);
        this.readyToGo = true;
      }
      else
        this.Invoke(nameof (Register), 0.0f);
    }

    private void OnBecameVisible()
    {
      if (!this.readyToGo)
        return;
      this.waterrendermanager.SetWaterVisible(this.ID);
    }

    private void OnBecameInvisible()
    {
      if (!this.readyToGo)
        return;
      this.waterrendermanager.SetWaterInvisible(this.ID);
    }

    private void OnTriggerEnter(Collider other)
    {
      LuxWater_WaterVolumeTrigger component = (LuxWater_WaterVolumeTrigger) ((Component) other).GetComponent<LuxWater_WaterVolumeTrigger>();
      if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) this.waterrendermanager, (Object) null) || (!this.readyToGo || !component.active))
        return;
      this.waterrendermanager.EnteredWaterVolume(this, this.ID, component.cam, this.GridSize);
      if (LuxWater_WaterVolume.OnEnterWaterVolume == null)
        return;
      LuxWater_WaterVolume.OnEnterWaterVolume();
    }

    private void OnTriggerStay(Collider other)
    {
      LuxWater_WaterVolumeTrigger component = (LuxWater_WaterVolumeTrigger) ((Component) other).GetComponent<LuxWater_WaterVolumeTrigger>();
      if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) this.waterrendermanager, (Object) null) || (!this.readyToGo || !component.active))
        return;
      this.waterrendermanager.EnteredWaterVolume(this, this.ID, component.cam, this.GridSize);
    }

    private void OnTriggerExit(Collider other)
    {
      LuxWater_WaterVolumeTrigger component = (LuxWater_WaterVolumeTrigger) ((Component) other).GetComponent<LuxWater_WaterVolumeTrigger>();
      if (!Object.op_Inequality((Object) component, (Object) null) || !Object.op_Inequality((Object) this.waterrendermanager, (Object) null) || (!this.readyToGo || !component.active))
        return;
      this.waterrendermanager.LeftWaterVolume(this, this.ID, component.cam);
      if (LuxWater_WaterVolume.OnExitWaterVolume == null)
        return;
      LuxWater_WaterVolume.OnExitWaterVolume();
    }

    public delegate void TriggerEnter();

    public delegate void TriggerExit();
  }
}
