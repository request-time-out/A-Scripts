// Decompiled with JetBrains decompiler
// Type: LuxWater_WaterVolumeListener
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using LuxWater;
using UnityEngine;

public class LuxWater_WaterVolumeListener : MonoBehaviour
{
  public LuxWater_WaterVolumeListener()
  {
    base.\u002Ector();
  }

  private void OnEnable()
  {
    LuxWater_WaterVolume.OnEnterWaterVolume += new LuxWater_WaterVolume.TriggerEnter(this.Enter);
    LuxWater_WaterVolume.OnExitWaterVolume += new LuxWater_WaterVolume.TriggerExit(this.Exit);
  }

  private void OnDisable()
  {
    LuxWater_WaterVolume.OnEnterWaterVolume -= new LuxWater_WaterVolume.TriggerEnter(this.Enter);
    LuxWater_WaterVolume.OnExitWaterVolume -= new LuxWater_WaterVolume.TriggerExit(this.Exit);
  }

  private void Enter()
  {
    Debug.Log((object) "Entered.");
  }

  private void Exit()
  {
    Debug.Log((object) "Exited.");
  }
}
