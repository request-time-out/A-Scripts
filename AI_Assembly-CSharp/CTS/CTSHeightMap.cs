// Decompiled with JetBrains decompiler
// Type: CTS.CTSHeightMap
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using System.Text;
using UnityEngine;

namespace CTS
{
  public class CTSHeightMap
  {
    protected byte[] m_metaData = new byte[0];
    protected int m_widthX;
    protected int m_depthZ;
    protected float[,] m_heights;
    protected bool m_isPowerOf2;
    protected float m_widthInvX;
    protected float m_depthInvZ;
    protected float m_statMinVal;
    protected float m_statMaxVal;
    protected double m_statSumVals;
    protected bool m_isDirty;

    public CTSHeightMap()
    {
      this.Reset();
    }

    public CTSHeightMap(int width, int depth)
    {
      this.m_widthX = width;
      this.m_depthZ = depth;
      this.m_widthInvX = 1f / (float) this.m_widthX;
      this.m_depthInvZ = 1f / (float) this.m_depthZ;
      this.m_heights = new float[this.m_widthX, this.m_depthZ];
      this.m_isPowerOf2 = this.Math_IsPowerOf2(this.m_widthX) && this.Math_IsPowerOf2(this.m_depthZ);
      this.m_statMinVal = this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      this.m_metaData = new byte[0];
      this.m_isDirty = false;
    }

    public CTSHeightMap(float[,] source)
    {
      this.m_widthX = source.GetLength(0);
      this.m_depthZ = source.GetLength(1);
      this.m_widthInvX = 1f / (float) this.m_widthX;
      this.m_depthInvZ = 1f / (float) this.m_depthZ;
      this.m_heights = new float[this.m_widthX, this.m_depthZ];
      this.m_isPowerOf2 = this.Math_IsPowerOf2(this.m_widthX) && this.Math_IsPowerOf2(this.m_depthZ);
      this.m_statMinVal = this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      this.m_metaData = new byte[0];
      Buffer.BlockCopy((Array) source, 0, (Array) this.m_heights, 0, this.m_widthX * this.m_depthZ * 4);
      this.m_isDirty = false;
    }

    public CTSHeightMap(float[,,] source, int slice)
    {
      this.m_widthX = source.GetLength(0);
      this.m_depthZ = source.GetLength(1);
      this.m_widthInvX = 1f / (float) this.m_widthX;
      this.m_depthInvZ = 1f / (float) this.m_depthZ;
      this.m_heights = new float[this.m_widthX, this.m_depthZ];
      this.m_isPowerOf2 = this.Math_IsPowerOf2(this.m_widthX) && this.Math_IsPowerOf2(this.m_depthZ);
      this.m_statMinVal = this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      this.m_metaData = new byte[0];
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = source[index1, index2, slice];
      }
      this.m_isDirty = false;
    }

    public CTSHeightMap(int[,] source)
    {
      this.m_widthX = source.GetLength(0);
      this.m_depthZ = source.GetLength(1);
      this.m_widthInvX = 1f / (float) this.m_widthX;
      this.m_depthInvZ = 1f / (float) this.m_depthZ;
      this.m_heights = new float[this.m_widthX, this.m_depthZ];
      this.m_isPowerOf2 = this.Math_IsPowerOf2(this.m_widthX) && this.Math_IsPowerOf2(this.m_depthZ);
      this.m_statMinVal = this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      this.m_metaData = new byte[0];
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = (float) source[index1, index2];
      }
      this.m_isDirty = false;
    }

    public CTSHeightMap(CTSHeightMap source)
    {
      this.Reset();
      this.m_widthX = source.m_widthX;
      this.m_depthZ = source.m_depthZ;
      this.m_widthInvX = 1f / (float) this.m_widthX;
      this.m_depthInvZ = 1f / (float) this.m_depthZ;
      this.m_heights = new float[this.m_widthX, this.m_depthZ];
      this.m_isPowerOf2 = source.m_isPowerOf2;
      this.m_metaData = new byte[source.m_metaData.Length];
      for (int index = 0; index < source.m_metaData.Length; ++index)
        this.m_metaData[index] = source.m_metaData[index];
      Buffer.BlockCopy((Array) source.m_heights, 0, (Array) this.m_heights, 0, this.m_widthX * this.m_depthZ * 4);
      this.m_isDirty = false;
    }

    public int Width()
    {
      return this.m_widthX;
    }

    public int Depth()
    {
      return this.m_depthZ;
    }

    public float MinVal()
    {
      return this.m_statMinVal;
    }

    public float MaxVal()
    {
      return this.m_statMaxVal;
    }

    public double SumVal()
    {
      return this.m_statSumVals;
    }

    public int GetBufferSize()
    {
      return this.m_widthX * this.m_depthZ;
    }

    public byte[] GetMetaData()
    {
      return this.m_metaData;
    }

    public bool IsDirty()
    {
      return this.m_isDirty;
    }

    public void SetDirty(bool dirty = true)
    {
      this.m_isDirty = dirty;
    }

    public void ClearDirty()
    {
      this.m_isDirty = false;
    }

    public void SetMetaData(byte[] metadata)
    {
      this.m_metaData = new byte[metadata.Length];
      Buffer.BlockCopy((Array) metadata, 0, (Array) this.m_metaData, 0, metadata.Length);
      this.m_isDirty = true;
    }

    public float[,] Heights()
    {
      return this.m_heights;
    }

    public float[] Heights1D()
    {
      float[] numArray = new float[this.m_widthX * this.m_depthZ];
      Buffer.BlockCopy((Array) this.m_heights, 0, (Array) numArray, 0, numArray.Length * 4);
      return numArray;
    }

    public void SetHeights(float[] heights)
    {
      int num = (int) Mathf.Sqrt((float) heights.Length);
      if (num != this.m_widthX || num != this.m_depthZ)
      {
        Debug.LogError((object) "SetHeights: Heights do not match. Aborting.");
      }
      else
      {
        Buffer.BlockCopy((Array) heights, 0, (Array) this.m_heights, 0, heights.Length * 4);
        this.m_isDirty = true;
      }
    }

    public void SetHeights(float[,] heights)
    {
      if (this.m_widthX != heights.GetLength(0) || this.m_depthZ != heights.GetLength(1))
      {
        Debug.LogError((object) "SetHeights: Sizes do not match. Aborting.");
      }
      else
      {
        int num = heights.GetLength(0) * heights.GetLength(1);
        Buffer.BlockCopy((Array) heights, 0, (Array) this.m_heights, 0, num * 4);
        this.m_isDirty = true;
      }
    }

    public float GetSafeHeight(int x, int z)
    {
      if (x < 0)
        x = 0;
      if (z < 0)
        z = 0;
      if (x >= this.m_widthX)
        x = this.m_widthX - 1;
      if (z >= this.m_depthZ)
        z = this.m_depthZ - 1;
      return this.m_heights[x, z];
    }

    public void SetSafeHeight(int x, int z, float height)
    {
      if (x < 0)
        x = 0;
      if (z < 0)
        z = 0;
      if (x >= this.m_widthX)
        x = this.m_widthX - 1;
      if (z >= this.m_depthZ)
        z = this.m_depthZ - 1;
      this.m_heights[x, z] = height;
      this.m_isDirty = true;
    }

    protected float GetInterpolatedHeight(float x, float z)
    {
      x *= (float) this.m_widthX - 1f;
      z *= (float) this.m_depthZ - 1f;
      int index1 = (int) x;
      int index2 = (int) z;
      int index3 = index1 + 1;
      int index4 = index2 + 1;
      if (index3 >= this.m_widthX)
        index3 = index1;
      if (index4 >= this.m_depthZ)
        index4 = index2;
      float num1 = x - (float) index1;
      float num2 = z - (float) index2;
      float num3 = 1f - num1;
      float num4 = 1f - num2;
      return (float) ((double) num3 * (double) num4 * (double) this.m_heights[index1, index2] + (double) num3 * (double) num2 * (double) this.m_heights[index1, index4] + (double) num1 * (double) num4 * (double) this.m_heights[index3, index2] + (double) num1 * (double) num2 * (double) this.m_heights[index3, index4]);
    }

    public float this[int x, int z]
    {
      get
      {
        return this.m_heights[x, z];
      }
      set
      {
        this.m_heights[x, z] = value;
        this.m_isDirty = true;
      }
    }

    public float this[float x, float z]
    {
      get
      {
        return this.GetInterpolatedHeight(x, z);
      }
      set
      {
        x *= (float) this.m_widthX - 1f;
        z *= (float) this.m_depthZ - 1f;
        this.m_heights[(int) x, (int) z] = value;
        this.m_isDirty = true;
      }
    }

    public CTSHeightMap SetHeight(float height)
    {
      float num = this.Math_Clamp(0.0f, 1f, height);
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = num;
      }
      this.m_isDirty = true;
      return this;
    }

    public void GetHeightRange(ref float minHeight, ref float maxHeight)
    {
      maxHeight = float.MinValue;
      minHeight = float.MaxValue;
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float height = this.m_heights[index1, index2];
          if ((double) height > (double) maxHeight)
            maxHeight = height;
          if ((double) height < (double) minHeight)
            minHeight = height;
        }
      }
    }

    public float GetSlope(int x, int z)
    {
      float height = this.m_heights[x, z];
      float num1 = this.m_heights[x + 1, z] - height;
      float num2 = this.m_heights[x, z + 1] - height;
      return (float) Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2);
    }

    public float GetSlope(float x, float z)
    {
      float num1 = this.GetInterpolatedHeight(x + this.m_widthInvX * 0.9f, z) - this.GetInterpolatedHeight(x - this.m_widthInvX * 0.9f, z);
      float num2 = this.GetInterpolatedHeight(x, z + this.m_depthInvZ * 0.9f) - this.GetInterpolatedHeight(x, z - this.m_depthInvZ * 0.9f);
      return this.Math_Clamp(0.0f, 90f, (float) (Math.Sqrt((double) num1 * (double) num1 + (double) num2 * (double) num2) * 10000.0));
    }

    public float GetSlope_a(float x, float z)
    {
      float interpolatedHeight = this.GetInterpolatedHeight(x, z);
      return (float) (((double) Math.Abs(this.GetInterpolatedHeight(x - this.m_widthInvX, z) - interpolatedHeight) + (double) Math.Abs(this.GetInterpolatedHeight(x + this.m_widthInvX, z) - interpolatedHeight) + (double) Math.Abs(this.GetInterpolatedHeight(x, z - this.m_depthInvZ) - interpolatedHeight) + (double) Math.Abs(this.GetInterpolatedHeight(x, z + this.m_depthInvZ) - interpolatedHeight)) / 4.0 * 400.0);
    }

    public float GetBaseLevel()
    {
      float num = 0.0f;
      for (int index = 0; index < this.m_widthX; ++index)
      {
        if ((double) this.m_heights[index, 0] > (double) num)
          num = this.m_heights[index, 0];
        if ((double) this.m_heights[index, this.m_depthZ - 1] > (double) num)
          num = this.m_heights[index, this.m_depthZ - 1];
      }
      for (int index = 0; index < this.m_depthZ; ++index)
      {
        if ((double) this.m_heights[0, index] > (double) num)
          num = this.m_heights[0, index];
        if ((double) this.m_heights[this.m_widthX - 1, index] > (double) num)
          num = this.m_heights[this.m_widthX - 1, index];
      }
      return num;
    }

    public bool HasData()
    {
      return this.m_widthX > 0 && this.m_depthZ > 0 && (this.m_heights != null && this.m_heights.GetLength(0) == this.m_widthX) && this.m_heights.GetLength(1) == this.m_depthZ;
    }

    public float[] GetRow(int rowX)
    {
      float[] numArray = new float[this.m_depthZ];
      for (int index = 0; index < this.m_depthZ; ++index)
        numArray[index] = this.m_heights[rowX, index];
      return numArray;
    }

    public void SetRow(int rowX, float[] values)
    {
      for (int index = 0; index < this.m_depthZ; ++index)
        this.m_heights[rowX, index] = values[index];
    }

    public float[] GetColumn(int columnZ)
    {
      float[] numArray = new float[this.m_widthX];
      for (int index = 0; index < this.m_widthX; ++index)
        numArray[index] = this.m_heights[index, columnZ];
      return numArray;
    }

    public void SetColumn(int columnZ, float[] values)
    {
      for (int index = 0; index < this.m_widthX; ++index)
        this.m_heights[index, columnZ] = values[index];
    }

    public void Reset()
    {
      this.m_widthX = this.m_depthZ = 0;
      this.m_widthInvX = this.m_depthInvZ = 0.0f;
      this.m_heights = (float[,]) null;
      this.m_statMinVal = this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      this.m_metaData = new byte[0];
      this.m_heights = new float[0, 0];
      this.m_isDirty = false;
    }

    public void UpdateStats()
    {
      this.m_statMinVal = 1f;
      this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float height = this.m_heights[index1, index2];
          if ((double) height < (double) this.m_statMinVal)
            this.m_statMinVal = height;
          if ((double) height > (double) this.m_statMaxVal)
            this.m_statMaxVal = height;
          this.m_statSumVals += (double) height;
        }
      }
    }

    public CTSHeightMap Smooth(int iterations)
    {
      for (int index = 0; index < iterations; ++index)
      {
        for (int x = 0; x < this.m_widthX; ++x)
        {
          for (int z = 0; z < this.m_depthZ; ++z)
            this.m_heights[x, z] = this.Math_Clamp(0.0f, 1f, (float) (((double) this.GetSafeHeight(x - 1, z) + (double) this.GetSafeHeight(x + 1, z) + (double) this.GetSafeHeight(x, z - 1) + (double) this.GetSafeHeight(x, z + 1)) / 4.0));
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap SmoothRadius(int radius)
    {
      radius = Mathf.Max(5, radius);
      CTSHeightMap ctsHeightMap = new CTSHeightMap(this.m_widthX, this.m_depthZ);
      float num1 = 1f / (float) ((2 * radius + 1) * (2 * radius + 1));
      for (int index1 = 0; index1 < this.m_depthZ; ++index1)
      {
        for (int index2 = 0; index2 < this.m_widthX; ++index2)
          ctsHeightMap[index2, index1] = num1 * this.m_heights[index2, index1];
      }
      for (int index1 = radius; index1 < this.m_widthX - radius; ++index1)
      {
        int num2 = radius;
        float num3 = 0.0f;
        for (int index2 = -radius; index2 < radius + 1; ++index2)
        {
          for (int index3 = -radius; index3 < radius + 1; ++index3)
            num3 += ctsHeightMap[index1 + index3, num2 + index2];
        }
        for (int index2 = num2 + 1; index2 < this.m_depthZ - radius; ++index2)
        {
          for (int index3 = -radius; index3 < radius + 1; ++index3)
            num3 = num3 - ctsHeightMap[index1 + index3, index2 - radius - 1] + ctsHeightMap[index1 + index3, index2 + radius];
          this.m_heights[index1, index2] = num3;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap GetSlopeMap()
    {
      CTSHeightMap ctsHeightMap = new CTSHeightMap(this);
      for (int x = 0; x < this.m_widthX; ++x)
      {
        for (int z = 0; z < this.m_depthZ; ++z)
          ctsHeightMap[x, z] = this.GetSlope(x, z);
      }
      return ctsHeightMap;
    }

    public CTSHeightMap Copy(CTSHeightMap CTSHeightMap)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not copy different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = CTSHeightMap.m_heights[index1, index2];
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap CopyClamped(CTSHeightMap CTSHeightMap, float min, float max)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not copy different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = CTSHeightMap.m_heights[index1, index2];
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Duplicate()
    {
      return new CTSHeightMap(this);
    }

    public CTSHeightMap Invert()
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = 1f - this.m_heights[index1, index2];
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Flip()
    {
      float[,] numArray = new float[this.m_depthZ, this.m_widthX];
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          numArray[index2, index1] = this.m_heights[index1, index2];
      }
      this.m_heights = numArray;
      this.m_widthX = numArray.GetLength(0);
      this.m_depthZ = numArray.GetLength(1);
      this.m_widthInvX = 1f / (float) this.m_widthX;
      this.m_depthInvZ = 1f / (float) this.m_depthZ;
      this.m_isPowerOf2 = this.Math_IsPowerOf2(this.m_widthX) && this.Math_IsPowerOf2(this.m_depthZ);
      this.m_statMinVal = this.m_statMaxVal = 0.0f;
      this.m_statSumVals = 0.0;
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Normalise()
    {
      float num1 = float.MinValue;
      float num2 = float.MaxValue;
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float height = this.m_heights[index1, index2];
          if ((double) height > (double) num1)
            num1 = height;
          if ((double) height < (double) num2)
            num2 = height;
        }
      }
      float num3 = num1 - num2;
      if ((double) num3 > 0.0)
      {
        for (int index1 = 0; index1 < this.m_widthX; ++index1)
        {
          for (int index2 = 0; index2 < this.m_depthZ; ++index2)
            this.m_heights[index1, index2] = (this.m_heights[index1, index2] - num2) / num3;
        }
        this.m_isDirty = true;
      }
      return this;
    }

    public CTSHeightMap Add(float value)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] += value;
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Add(CTSHeightMap CTSHeightMap)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not add different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] += CTSHeightMap.m_heights[index1, index2];
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap AddClamped(float value, float min, float max)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] + value;
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap AddClamped(CTSHeightMap CTSHeightMap, float min, float max)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not add different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] + CTSHeightMap.m_heights[index1, index2];
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Subtract(float value)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] -= value;
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Subtract(CTSHeightMap CTSHeightMap)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not subtract different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] -= CTSHeightMap.m_heights[index1, index2];
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap SubtractClamped(float value, float min, float max)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] - value;
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap SubtractClamped(
      CTSHeightMap CTSHeightMap,
      float min,
      float max)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not add different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] - CTSHeightMap.m_heights[index1, index2];
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Multiply(float value)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] *= value;
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Multiply(CTSHeightMap CTSHeightMap)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not multiply different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] *= CTSHeightMap.m_heights[index1, index2];
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap MultiplyClamped(float value, float min, float max)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] * value;
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap MultiplyClamped(
      CTSHeightMap CTSHeightMap,
      float min,
      float max)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not multiply different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] * CTSHeightMap.m_heights[index1, index2];
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Divide(float value)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] /= value;
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Divide(CTSHeightMap CTSHeightMap)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not divide different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] /= CTSHeightMap.m_heights[index1, index2];
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap DivideClamped(float value, float min, float max)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] / value;
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap DivideClamped(CTSHeightMap CTSHeightMap, float min, float max)
    {
      if (this.m_widthX != CTSHeightMap.m_widthX || this.m_depthZ != CTSHeightMap.m_depthZ)
      {
        Debug.LogError((object) "Can not divide different sized CTSHeightMap");
        return this;
      }
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          float num = this.m_heights[index1, index2] / CTSHeightMap.m_heights[index1, index2];
          if ((double) num < (double) min)
            num = min;
          else if ((double) num > (double) max)
            num = max;
          this.m_heights[index1, index2] = num;
        }
      }
      this.m_isDirty = true;
      return this;
    }

    public float Sum()
    {
      float num = 0.0f;
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          num += this.m_heights[index1, index2];
      }
      return num;
    }

    public float Average()
    {
      return this.Sum() / (float) (this.m_widthX * this.m_depthZ);
    }

    public CTSHeightMap Power(float exponent)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = Mathf.Pow(this.m_heights[index1, index2], exponent);
      }
      this.m_isDirty = true;
      return this;
    }

    public CTSHeightMap Contrast(float contrast)
    {
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
          this.m_heights[index1, index2] = (float) (((double) this.m_heights[index1, index2] - 0.5) * (double) contrast + 0.5);
      }
      this.m_isDirty = true;
      return this;
    }

    private bool Math_IsPowerOf2(int value)
    {
      return (value & value - 1) == 0;
    }

    private float Math_Clamp(float min, float max, float value)
    {
      if ((double) value < (double) min)
        return min;
      return (double) value > (double) max ? max : value;
    }

    public void DumpMap(float scaleValue, int precision, string spacer, bool flip)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string empty = string.Empty;
      string format;
      if (precision == 0)
      {
        format = "{0:0}";
      }
      else
      {
        string str = "{0:0.";
        for (int index = 0; index < precision; ++index)
          str += "0";
        format = str + "}";
      }
      if (!string.IsNullOrEmpty(spacer))
        format += spacer;
      for (int index1 = 0; index1 < this.m_widthX; ++index1)
      {
        for (int index2 = 0; index2 < this.m_depthZ; ++index2)
        {
          if (!flip)
            stringBuilder.AppendFormat(format, (object) (float) ((double) this.m_heights[index1, index2] * (double) scaleValue));
          else
            stringBuilder.AppendFormat(format, (object) (float) ((double) this.m_heights[index2, index1] * (double) scaleValue));
        }
        stringBuilder.AppendLine();
      }
      Debug.Log((object) stringBuilder.ToString());
    }

    public void DumpRow(int rowX, float scaleValue, int precision, string spacer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string empty = string.Empty;
      string format;
      if (precision == 0)
      {
        format = "{0:0}";
      }
      else
      {
        string str = "{0:0.";
        for (int index = 0; index < precision; ++index)
          str += "0";
        format = str + "}";
      }
      if (!string.IsNullOrEmpty(spacer))
        format += spacer;
      foreach (double num in this.GetRow(rowX))
        stringBuilder.AppendFormat(format, (object) (float) (num * (double) scaleValue));
      Debug.Log((object) stringBuilder.ToString());
    }

    public void DumpColumn(int columnZ, float scaleValue, int precision, string spacer)
    {
      StringBuilder stringBuilder = new StringBuilder();
      string empty = string.Empty;
      string format;
      if (precision == 0)
      {
        format = "{0:0}";
      }
      else
      {
        string str = "{0:0.";
        for (int index = 0; index < precision; ++index)
          str += "0";
        format = str + "}";
      }
      if (!string.IsNullOrEmpty(spacer))
        format += spacer;
      foreach (double num in this.GetColumn(columnZ))
        stringBuilder.AppendFormat(format, (object) (float) (num * (double) scaleValue));
      Debug.Log((object) stringBuilder.ToString());
    }
  }
}
