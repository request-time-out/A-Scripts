// Decompiled with JetBrains decompiler
// Type: AssetBundleLoadAssetOperationSimulation
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections;
using System.Linq;
using UnityEngine;

public class AssetBundleLoadAssetOperationSimulation : AssetBundleLoadAssetOperation
{
  private Object[] m_SimulatedObjects;

  public AssetBundleLoadAssetOperationSimulation(Object simulatedObject)
  {
    this.m_SimulatedObjects = new Object[1]
    {
      simulatedObject
    };
  }

  public AssetBundleLoadAssetOperationSimulation(Object[] simulatedObjects)
  {
    this.m_SimulatedObjects = simulatedObjects;
  }

  public override bool IsEmpty()
  {
    return this.m_SimulatedObjects == null || this.m_SimulatedObjects.Length == 0 || Object.op_Equality(this.m_SimulatedObjects[0], (Object) null);
  }

  public override T GetAsset<T>()
  {
    return this.m_SimulatedObjects[0] as T;
  }

  public override T[] GetAllAssets<T>()
  {
    return ((IEnumerable) this.m_SimulatedObjects).OfType<T>().ToArray<T>();
  }

  public override bool Update()
  {
    return false;
  }

  public override bool IsDone()
  {
    return true;
  }
}
