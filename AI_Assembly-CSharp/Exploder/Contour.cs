// Decompiled with JetBrains decompiler
// Type: Exploder.Contour
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace Exploder
{
  public class Contour
  {
    public List<Dictionary<int, int>> contour;
    private ArrayDictionary<Contour.MidPoint> midPoints;
    private LSHash lsHash;

    public Contour(int trianglesNum)
    {
      this.AllocateBuffers(trianglesNum);
    }

    public void AllocateBuffers(int trianglesNum)
    {
      if (this.lsHash == null || this.lsHash.Capacity() < trianglesNum * 2)
      {
        this.midPoints = new ArrayDictionary<Contour.MidPoint>(trianglesNum * 2);
        this.contour = new List<Dictionary<int, int>>();
        this.lsHash = new LSHash(1f / 1000f, trianglesNum * 2);
      }
      else
      {
        this.lsHash.Clear();
        foreach (Dictionary<int, int> dictionary in this.contour)
          dictionary.Clear();
        this.contour.Clear();
        if (this.midPoints.Size < trianglesNum * 2)
          this.midPoints = new ArrayDictionary<Contour.MidPoint>(trianglesNum * 2);
        else
          this.midPoints.Clear();
      }
    }

    public int MidPointsCount { get; private set; }

    public void AddTriangle(int triangleID, int id0, int id1, Vector3 v0, Vector3 v1)
    {
      int hash0;
      int hash1;
      this.lsHash.Hash(v0, v1, out hash0, out hash1);
      if (hash0 == hash1)
        return;
      Contour.MidPoint midPoint;
      if (this.midPoints.TryGetValue(hash0, out midPoint))
      {
        if (midPoint.idNext == int.MaxValue && midPoint.idPrev != hash1)
          midPoint.idNext = hash1;
        else if (midPoint.idPrev == int.MaxValue && midPoint.idNext != hash1)
          midPoint.idPrev = hash1;
        this.midPoints[hash0] = midPoint;
      }
      else
        this.midPoints.Add(hash0, new Contour.MidPoint()
        {
          id = hash0,
          vertexId = id0,
          idNext = hash1,
          idPrev = int.MaxValue
        });
      if (this.midPoints.TryGetValue(hash1, out midPoint))
      {
        if (midPoint.idNext == int.MaxValue && midPoint.idPrev != hash0)
          midPoint.idNext = hash0;
        else if (midPoint.idPrev == int.MaxValue && midPoint.idNext != hash0)
          midPoint.idPrev = hash0;
        this.midPoints[hash1] = midPoint;
      }
      else
        this.midPoints.Add(hash1, new Contour.MidPoint()
        {
          id = hash1,
          vertexId = id1,
          idPrev = hash0,
          idNext = int.MaxValue
        });
      this.MidPointsCount = this.midPoints.Count;
    }

    public bool FindContours()
    {
      if (this.midPoints.Count == 0)
        return false;
      Dictionary<int, int> dictionary = new Dictionary<int, int>(this.midPoints.Count);
      int num = this.midPoints.Count * 2;
      Contour.MidPoint firstValue = this.midPoints.GetFirstValue();
      dictionary.Add(firstValue.id, firstValue.vertexId);
      this.midPoints.Remove(firstValue.id);
      int key = firstValue.idNext;
      while (this.midPoints.Count > 0)
      {
        if (key == int.MaxValue)
          return false;
        Contour.MidPoint midPoint;
        if (!this.midPoints.TryGetValue(key, out midPoint))
        {
          this.contour.Clear();
          return false;
        }
        dictionary.Add(midPoint.id, midPoint.vertexId);
        this.midPoints.Remove(midPoint.id);
        if (dictionary.ContainsKey(midPoint.idNext))
        {
          if (dictionary.ContainsKey(midPoint.idPrev))
          {
            this.contour.Add(new Dictionary<int, int>((IDictionary<int, int>) dictionary));
            dictionary.Clear();
            if (this.midPoints.Count != 0)
            {
              firstValue = this.midPoints.GetFirstValue();
              dictionary.Add(firstValue.id, firstValue.vertexId);
              this.midPoints.Remove(firstValue.id);
              key = firstValue.idNext;
              continue;
            }
            break;
          }
          key = midPoint.idPrev;
        }
        else
          key = midPoint.idNext;
        --num;
        if (num == 0)
        {
          this.contour.Clear();
          return false;
        }
      }
      return true;
    }

    private struct MidPoint
    {
      public int id;
      public int vertexId;
      public int idNext;
      public int idPrev;
    }
  }
}
