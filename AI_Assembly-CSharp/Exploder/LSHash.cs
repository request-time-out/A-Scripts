// Decompiled with JetBrains decompiler
// Type: Exploder.LSHash
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using UnityEngine;

namespace Exploder
{
  public class LSHash
  {
    private readonly Vector3[] buckets;
    private readonly float bucketSize2;
    private int count;

    public LSHash(float bucketSize, int allocSize)
    {
      this.bucketSize2 = bucketSize * bucketSize;
      this.buckets = new Vector3[allocSize];
      this.count = 0;
    }

    public int Capacity()
    {
      return this.buckets.Length;
    }

    public void Clear()
    {
      for (int index = 0; index < this.buckets.Length; ++index)
        this.buckets[index] = Vector3.get_zero();
      this.count = 0;
    }

    public int Hash(Vector3 p)
    {
      for (int index = 0; index < this.count; ++index)
      {
        Vector3 bucket = this.buckets[index];
        float num1 = (float) (p.x - bucket.x);
        float num2 = (float) (p.y - bucket.y);
        float num3 = (float) (p.z - bucket.z);
        if ((double) num1 * (double) num1 + (double) num2 * (double) num2 + (double) num3 * (double) num3 < (double) this.bucketSize2)
          return index;
      }
      if (this.count >= this.buckets.Length)
        return this.count - 1;
      this.buckets[this.count++] = p;
      return this.count - 1;
    }

    public void Hash(Vector3 p0, Vector3 p1, out int hash0, out int hash1)
    {
      hash0 = this.Hash(p0);
      hash1 = this.Hash(p1);
    }
  }
}
