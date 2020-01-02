// Decompiled with JetBrains decompiler
// Type: RandomInstancing
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class RandomInstancing : MonoBehaviour
{
  public GameObject m_Prefab;
  public int m_PoolSize;
  public int m_InstancesPerTile;
  public bool m_RandomPosition;
  public bool m_RandomOrientation;
  public float m_Height;
  public int m_BaseHash;
  public float m_Size;
  private List<Transform> m_Instances;
  private int m_Used;
  private int m_LocX;
  private int m_LocZ;

  public RandomInstancing()
  {
    base.\u002Ector();
  }

  private void Awake()
  {
    for (int index = 0; index < this.m_PoolSize; ++index)
    {
      GameObject gameObject = (GameObject) Object.Instantiate<GameObject>((M0) this.m_Prefab, Vector3.get_zero(), Quaternion.get_identity());
      gameObject.SetActive(false);
      this.m_Instances.Add(gameObject.get_transform());
    }
  }

  private void OnEnable()
  {
    this.m_LocX = -1;
    this.m_LocZ = -1;
    this.UpdateInstances();
  }

  private void OnDestroy()
  {
    for (int index = 0; index < this.m_Instances.Count; ++index)
    {
      if (Object.op_Implicit((Object) this.m_Instances[index]))
        Object.Destroy((Object) ((Component) this.m_Instances[index]).get_gameObject());
    }
    this.m_Instances.Clear();
  }

  private void Update()
  {
    this.UpdateInstances();
  }

  private void UpdateInstances()
  {
    int num1 = (int) Mathf.Floor((float) ((Component) this).get_transform().get_position().x / this.m_Size);
    int num2 = (int) Mathf.Floor((float) ((Component) this).get_transform().get_position().z / this.m_Size);
    if (num1 == this.m_LocX && num2 == this.m_LocZ)
      return;
    this.m_LocX = num1;
    this.m_LocZ = num2;
    this.m_Used = 0;
    for (int i = num1 - 2; i <= num1 + 2; ++i)
    {
      for (int j = num2 - 2; j <= num2 + 2; ++j)
      {
        if (this.UpdateTileInstances(i, j) != this.m_InstancesPerTile)
          return;
      }
    }
    for (int used = this.m_Used; used < this.m_PoolSize && ((Component) this.m_Instances[used]).get_gameObject().get_activeSelf(); ++used)
      ((Component) this.m_Instances[used]).get_gameObject().SetActive(false);
  }

  private int UpdateTileInstances(int i, int j)
  {
    int seed = RandomInstancing.Hash2(i, j) ^ this.m_BaseHash;
    int num1 = Math.Min(this.m_InstancesPerTile, this.m_PoolSize - this.m_Used);
    for (int index = this.m_Used + num1; this.m_Used < index; ++this.m_Used)
    {
      float num2 = 0.0f;
      float num3 = 0.0f;
      if (this.m_RandomPosition)
      {
        num2 = RandomInstancing.Random(ref seed);
        num3 = RandomInstancing.Random(ref seed);
      }
      Vector3 vector3;
      ((Vector3) ref vector3).\u002Ector(((float) i + num2) * this.m_Size, this.m_Height, ((float) j + num3) * this.m_Size);
      if (this.m_RandomOrientation)
        this.m_Instances[this.m_Used].set_rotation(Quaternion.AngleAxis(360f * RandomInstancing.Random(ref seed), Vector3.get_up()));
      this.m_Instances[this.m_Used].set_position(vector3);
      ((Component) this.m_Instances[this.m_Used]).get_gameObject().SetActive(true);
    }
    if (num1 < this.m_InstancesPerTile)
      Debug.LogWarning((object) "Pool exhausted", (Object) this);
    return num1;
  }

  private static int Hash2(int i, int j)
  {
    return i * 73856093 ^ j * 19349663;
  }

  private static float Random(ref int seed)
  {
    seed ^= 123459876;
    int num1 = seed / 127773;
    seed = 16807 * (seed - num1 * 127773) - 2836 * num1;
    if (seed < 0)
      seed += int.MaxValue;
    float num2 = (float) ((double) seed * 1.0 / 2147483648.0);
    seed ^= 123459876;
    return num2;
  }
}
