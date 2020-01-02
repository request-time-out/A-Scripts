// Decompiled with JetBrains decompiler
// Type: MetaballBuilder
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class MetaballBuilder
{
  private static bool __bCubePatternsInitialized = false;
  private static MetaballBuilder.MB3DCubePattern[] __cubePatterns = new MetaballBuilder.MB3DCubePattern[256];
  private static MetaballBuilder.MB3DCubePrimitivePattern[] __primitivePatterns = new MetaballBuilder.MB3DCubePrimitivePattern[15]
  {
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[0]
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[3]{ 0, 4, 8 }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[6]{ 1, 10, 0, 8, 0, 10 }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 1, (sbyte) 0),
      IndexBuf = new int[6]{ 0, 4, 8, 3, 5, 10 },
      IndexBufAlter = new int[12]
      {
        0,
        3,
        8,
        5,
        8,
        3,
        0,
        4,
        3,
        10,
        3,
        4
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 1),
      IndexBuf = new int[6]{ 0, 4, 8, 3, 11, 7 }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 0, (sbyte) 1, (sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[9]{ 10, 4, 0, 10, 0, 9, 10, 9, 11 }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 1),
      IndexBuf = new int[9]{ 1, 10, 0, 8, 0, 10, 3, 11, 7 },
      IndexBufAlter = new int[15]
      {
        3,
        10,
        7,
        10,
        8,
        7,
        8,
        0,
        7,
        0,
        1,
        7,
        1,
        11,
        7
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 0, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 1),
      IndexBuf = new int[9]{ 10, 4, 1, 2, 8, 5, 3, 11, 7 }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 1, (sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[6]{ 10, 8, 11, 9, 11, 8 }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[12]
      {
        2,
        7,
        8,
        8,
        7,
        4,
        4,
        7,
        11,
        4,
        11,
        1
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 1),
      IndexBuf = new int[12]
      {
        2,
        0,
        5,
        4,
        5,
        0,
        3,
        1,
        7,
        6,
        7,
        1
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0, (sbyte) 1),
      IndexBuf = new int[12]
      {
        8,
        9,
        4,
        4,
        9,
        3,
        7,
        3,
        9,
        1,
        4,
        3
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 0, (sbyte) 1, (sbyte) 1, (sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[12]
      {
        2,
        8,
        5,
        10,
        4,
        0,
        10,
        0,
        9,
        10,
        9,
        11
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 1, (sbyte) 0, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 1, (sbyte) 0),
      IndexBuf = new int[12]
      {
        0,
        4,
        8,
        3,
        5,
        10,
        2,
        7,
        9,
        11,
        1,
        6
      }
    },
    new MetaballBuilder.MB3DCubePrimitivePattern()
    {
      InOut = new MetaballBuilder.MB3DCubeInOut((sbyte) 0, (sbyte) 1, (sbyte) 1, (sbyte) 1, (sbyte) 0, (sbyte) 1, (sbyte) 0, (sbyte) 0),
      IndexBuf = new int[12]
      {
        2,
        4,
        0,
        2,
        11,
        4,
        7,
        11,
        2,
        10,
        4,
        11
      }
    }
  };
  private const int MB3D_PATTERN_COUNT = 15;
  private const int _maxGridCellCount = 1000000;
  private const int _maxVertexCount = 300000;
  private static MetaballBuilder _instance;

  static MetaballBuilder()
  {
    MetaballBuilder.__InitCubePatterns();
  }

  private static void __InitCubePatterns()
  {
    for (int index = 0; index < 256; ++index)
    {
      MetaballBuilder.__cubePatterns[index] = new MetaballBuilder.MB3DCubePattern();
      MetaballBuilder.__cubePatterns[index].Init((int) (sbyte) index);
    }
    MetaballBuilder.__bCubePatternsInitialized = true;
  }

  private static float CalcPower(Vector3 relativePos, float radius, float density)
  {
    float num = ((Vector3) ref relativePos).get_magnitude() / radius;
    return (double) num > 1.0 ? 0.0f : density * Mathf.Max((float) ((1.0 - (double) num) * (1.0 - (double) num)), 0.0f);
  }

  public static int MaxGridCellCount
  {
    get
    {
      return 1000000;
    }
  }

  public static int MaxVertexCount
  {
    get
    {
      return 300000;
    }
  }

  public static MetaballBuilder Instance
  {
    get
    {
      if (MetaballBuilder._instance == null)
        MetaballBuilder._instance = new MetaballBuilder();
      return MetaballBuilder._instance;
    }
  }

  public string CreateMesh(
    MetaballCellClusterInterface rootCell,
    Transform root,
    float powerThreshold,
    float gridCellSize,
    Vector3 uDir,
    Vector3 vDir,
    Vector3 uvOffset,
    out Mesh out_mesh,
    MetaballCellObject cellObjPrefab = null,
    bool bReverse = false,
    Bounds? fixedBounds = null,
    bool bAutoGridSize = false,
    float autoGridQuarity = 0.2f)
  {
    Mesh mesh = new Mesh();
    Bounds bounds;
    MetaballBuilder.MetaballPointInfo[] ballPoints;
    this.AnalyzeCellCluster(rootCell, root, out bounds, out ballPoints, cellObjPrefab);
    if (fixedBounds.HasValue)
      bounds = fixedBounds.Value;
    if (bAutoGridSize)
    {
      int num = (int) (1000000.0 * (double) Mathf.Clamp01(autoGridQuarity));
      gridCellSize = Mathf.Pow((float) (((Bounds) ref bounds).get_size().x * ((Bounds) ref bounds).get_size().y * ((Bounds) ref bounds).get_size().z) / (float) num, 0.3333333f);
    }
    float num1 = (float) ((int) (((Bounds) ref bounds).get_size().x / (double) gridCellSize) * (int) (((Bounds) ref bounds).get_size().y / (double) gridCellSize) * (int) (((Bounds) ref bounds).get_size().z / (double) gridCellSize));
    if ((double) num1 > 1000000.0)
    {
      out_mesh = mesh;
      return "Too many grid cells for building mesh (" + num1.ToString() + " > " + 1000000.ToString() + " )." + Environment.NewLine + "Make the area smaller or set larger (MetaballSeedBase.gridSize)";
    }
    this.BuildMetaballMesh(mesh, ((Bounds) ref bounds).get_center(), ((Bounds) ref bounds).get_extents(), gridCellSize, ballPoints, powerThreshold, bReverse, uDir, vDir, uvOffset);
    out_mesh = mesh;
    return (string) null;
  }

  public string CreateMeshWithSkeleton(
    SkinnedMetaballCell rootCell,
    Transform root,
    float powerThreshold,
    float gridCellSize,
    Vector3 uDir,
    Vector3 vDir,
    Vector3 uvOffset,
    out Mesh out_mesh,
    out Transform[] out_bones,
    MetaballCellObject cellObjPrefab = null,
    bool bReverse = false,
    Bounds? fixedBounds = null,
    bool bAutoGridSize = false,
    float autoGridQuarity = 0.2f)
  {
    Mesh mesh = new Mesh();
    Bounds bounds;
    Transform[] bones;
    Matrix4x4[] bindPoses;
    MetaballBuilder.MetaballPointInfo[] ballPoints;
    this.AnalyzeCellClusterWithSkeleton(rootCell, root, out bounds, out bones, out bindPoses, out ballPoints, cellObjPrefab);
    if (fixedBounds.HasValue)
      bounds = fixedBounds.Value;
    if (bAutoGridSize)
    {
      int num = (int) (1000000.0 * (double) Mathf.Clamp01(autoGridQuarity));
      gridCellSize = Mathf.Pow((float) (((Bounds) ref bounds).get_size().x * ((Bounds) ref bounds).get_size().y * ((Bounds) ref bounds).get_size().z) / (float) num, 0.3333333f);
    }
    mesh.set_bindposes(bindPoses);
    float num1 = (float) ((int) (((Bounds) ref bounds).get_size().x / (double) gridCellSize) * (int) (((Bounds) ref bounds).get_size().y / (double) gridCellSize) * (int) (((Bounds) ref bounds).get_size().z / (double) gridCellSize));
    if ((double) num1 > 1000000.0)
    {
      out_mesh = mesh;
      out_bones = bones;
      return "Too many grid cells for building mesh (" + num1.ToString() + " > " + 1000000.ToString() + " )." + Environment.NewLine + "Make the area smaller or set larger (MetaballSeedBase.gridSize)";
    }
    this.BuildMetaballMesh(mesh, ((Bounds) ref bounds).get_center(), ((Bounds) ref bounds).get_extents(), gridCellSize, ballPoints, powerThreshold, bReverse, uDir, vDir, uvOffset);
    out_mesh = mesh;
    out_bones = bones;
    return (string) null;
  }

  private void AnalyzeCellCluster(
    MetaballCellClusterInterface cellCluster,
    Transform root,
    out Bounds bounds,
    out MetaballBuilder.MetaballPointInfo[] ballPoints,
    MetaballCellObject cellObjPrefab = null)
  {
    int cellCount = cellCluster.CellCount;
    Bounds tmpBounds = new Bounds(Vector3.get_zero(), Vector3.get_zero());
    MetaballBuilder.MetaballPointInfo[] tmpBallPoints = new MetaballBuilder.MetaballPointInfo[cellCount];
    int cellIdx = 0;
    cellCluster.DoForeachCell((ForeachCellDeleg) (c =>
    {
      for (int index = 0; index < 3; ++index)
      {
        double num1 = (double) ((Vector3) ref c.modelPosition).get_Item(index) - (double) c.radius;
        Vector3 min1 = ((Bounds) ref tmpBounds).get_min();
        double num2 = (double) ((Vector3) ref min1).get_Item(index);
        if (num1 < num2)
        {
          Vector3 min2 = ((Bounds) ref tmpBounds).get_min();
          ((Vector3) ref min2).set_Item(index, ((Vector3) ref c.modelPosition).get_Item(index) - c.radius);
          ((Bounds) ref tmpBounds).set_min(min2);
        }
        double num3 = (double) ((Vector3) ref c.modelPosition).get_Item(index) + (double) c.radius;
        Vector3 max1 = ((Bounds) ref tmpBounds).get_max();
        double num4 = (double) ((Vector3) ref max1).get_Item(index);
        if (num3 > num4)
        {
          Vector3 max2 = ((Bounds) ref tmpBounds).get_max();
          ((Vector3) ref max2).set_Item(index, ((Vector3) ref c.modelPosition).get_Item(index) + c.radius);
          ((Bounds) ref tmpBounds).set_max(max2);
        }
      }
      GameObject gameObject;
      if (Object.op_Implicit((Object) cellObjPrefab))
      {
        gameObject = (GameObject) Object.Instantiate<GameObject>((M0) ((Component) cellObjPrefab).get_gameObject());
        ((MetaballCellObject) gameObject.GetComponent<MetaballCellObject>()).Setup(c);
      }
      else
        gameObject = new GameObject();
      if (!string.IsNullOrEmpty(c.tag))
        ((Object) gameObject).set_name(c.tag + "_Bone");
      else
        ((Object) gameObject).set_name("Bone");
      Transform transform = gameObject.get_transform();
      transform.set_parent(root);
      transform.set_localPosition(c.modelPosition);
      transform.set_localRotation(c.modelRotation);
      transform.set_localScale(Vector3.get_one());
      tmpBallPoints[cellIdx] = new MetaballBuilder.MetaballPointInfo()
      {
        center = c.modelPosition,
        radius = c.radius,
        density = c.density
      };
      ++cellIdx;
    }));
    bounds = tmpBounds;
    ballPoints = tmpBallPoints;
  }

  private void AnalyzeCellClusterWithSkeleton(
    SkinnedMetaballCell rootCell,
    Transform root,
    out Bounds bounds,
    out Transform[] bones,
    out Matrix4x4[] bindPoses,
    out MetaballBuilder.MetaballPointInfo[] ballPoints,
    MetaballCellObject cellObjPrefab = null)
  {
    int cellCount = rootCell.CellCount;
    Transform[] tmpBones = new Transform[cellCount];
    Matrix4x4[] tmpBindPoses = new Matrix4x4[cellCount];
    Bounds tmpBounds = new Bounds(Vector3.get_zero(), Vector3.get_zero());
    MetaballBuilder.MetaballPointInfo[] tmpBallPoints = new MetaballBuilder.MetaballPointInfo[cellCount];
    Dictionary<SkinnedMetaballCell, int> boneDictionary = new Dictionary<SkinnedMetaballCell, int>();
    int cellIdx = 0;
    rootCell.DoForeachSkinnedCell((SkinnedMetaballCell.ForeachSkinnedCellDeleg) (c =>
    {
      for (int index = 0; index < 3; ++index)
      {
        double num1 = (double) ((Vector3) ref c.modelPosition).get_Item(index) - (double) c.radius;
        Vector3 min1 = ((Bounds) ref tmpBounds).get_min();
        double num2 = (double) ((Vector3) ref min1).get_Item(index);
        if (num1 < num2)
        {
          Vector3 min2 = ((Bounds) ref tmpBounds).get_min();
          ((Vector3) ref min2).set_Item(index, ((Vector3) ref c.modelPosition).get_Item(index) - c.radius);
          ((Bounds) ref tmpBounds).set_min(min2);
        }
        double num3 = (double) ((Vector3) ref c.modelPosition).get_Item(index) + (double) c.radius;
        Vector3 max1 = ((Bounds) ref tmpBounds).get_max();
        double num4 = (double) ((Vector3) ref max1).get_Item(index);
        if (num3 > num4)
        {
          Vector3 max2 = ((Bounds) ref tmpBounds).get_max();
          ((Vector3) ref max2).set_Item(index, ((Vector3) ref c.modelPosition).get_Item(index) + c.radius);
          ((Bounds) ref tmpBounds).set_max(max2);
        }
      }
      GameObject gameObject;
      if (Object.op_Implicit((Object) cellObjPrefab))
      {
        gameObject = (GameObject) Object.Instantiate<GameObject>((M0) ((Component) cellObjPrefab).get_gameObject());
        ((MetaballCellObject) gameObject.GetComponent<MetaballCellObject>()).Setup((MetaballCell) c);
      }
      else
        gameObject = new GameObject();
      if (!string.IsNullOrEmpty(c.tag))
        ((Object) gameObject).set_name(c.tag + "_Bone");
      else
        ((Object) gameObject).set_name("Bone");
      Transform transform1 = gameObject.get_transform();
      if (c.IsRoot)
      {
        transform1.set_parent(root);
        transform1.set_localPosition(Vector3.get_zero());
        transform1.set_localRotation(c.modelRotation);
        transform1.set_localScale(Vector3.get_one());
      }
      else
      {
        Transform transform2 = tmpBones[boneDictionary[c.parent]];
        transform1.set_parent(root);
        transform1.set_localPosition(c.parent.modelPosition);
        transform1.set_localRotation(c.modelRotation);
        transform1.set_localScale(Vector3.get_one());
        transform1.set_parent(transform2);
      }
      tmpBones[cellIdx] = transform1;
      tmpBindPoses[cellIdx] = Matrix4x4.op_Multiply(tmpBones[cellIdx].get_worldToLocalMatrix(), root.get_localToWorldMatrix());
      boneDictionary.Add(c, cellIdx);
      tmpBallPoints[cellIdx] = new MetaballBuilder.MetaballPointInfo()
      {
        center = c.modelPosition,
        radius = c.radius,
        density = c.density
      };
      ++cellIdx;
    }));
    bounds = tmpBounds;
    bones = tmpBones;
    bindPoses = tmpBindPoses;
    ballPoints = tmpBallPoints;
  }

  public Mesh CreateImplicitSurfaceMesh(
    int countX,
    int countY,
    int countZ,
    Vector3[] positionMap,
    float[] powerMap,
    bool bReverse,
    float threshold,
    Vector3 uDir,
    Vector3 vDir,
    Vector3 uvOffset)
  {
    if (!MetaballBuilder.__bCubePatternsInitialized)
      MetaballBuilder.__InitCubePatterns();
    int length = countX * countY * countZ;
    Vector3[] vector3Array = new Vector3[length];
    int[] numArray1 = new int[length * 3];
    bool[] flagArray = new bool[length];
    int num1 = countX;
    int num2 = countX * countY;
    int num3 = countX * countY * countZ;
    for (int index = 0; index < length * 3; ++index)
      numArray1[index] = -1;
    for (int index = 0; index < length; ++index)
    {
      float num4 = powerMap[index] - threshold;
      flagArray[index] = (double) num4 >= 0.0;
      if (flagArray[index] && (double) num4 < 1.0 / 1000.0)
        powerMap[index] = threshold + 1f / 1000f;
    }
    for (int index1 = 1; index1 < countZ - 1; ++index1)
    {
      for (int index2 = 1; index2 < countY - 1; ++index2)
      {
        for (int index3 = 1; index3 < countX - 1; ++index3)
        {
          int index4 = index3 + index2 * num1 + index1 * num2;
          Vector3 vector3;
          vector3.x = (__Null) ((double) powerMap[index4 + 1] - (double) powerMap[index4 - 1]);
          vector3.y = (__Null) ((double) powerMap[index4 + num1] - (double) powerMap[index4 - num1]);
          vector3.z = (__Null) ((double) powerMap[index4 + num2] - (double) powerMap[index4 - num2]);
          if ((double) ((Vector3) ref vector3).get_sqrMagnitude() > 1.0 / 1000.0)
            ((Vector3) ref vector3).Normalize();
          vector3Array[index4] = vector3;
        }
      }
    }
    int num5 = 0;
    List<Vector3> vector3List1 = new List<Vector3>();
    List<Vector3> vector3List2 = new List<Vector3>();
    List<Vector2> vector2List = new List<Vector2>();
    for (int index1 = 0; index1 < countZ && num5 < 299999; ++index1)
    {
      for (int index2 = 0; index2 < countY && num5 < 299999; ++index2)
      {
        for (int index3 = 0; index3 < countX && num5 < 299999; ++index3)
        {
          for (int index4 = 0; index4 < 3 && num5 < 299999; ++index4)
          {
            int num4 = index4 != 0 ? 0 : 1;
            int num6 = index4 != 1 ? 0 : 1;
            int num7 = index4 != 2 ? 0 : 1;
            if (index1 + num7 < countZ && index2 + num6 < countY && index3 + num4 < countX)
            {
              int index5 = index3 + index2 * num1 + index1 * num2;
              int index6 = index3 + num4 + (index2 + num6) * num1 + (index1 + num7) * num2;
              float power1 = powerMap[index5];
              float power2 = powerMap[index6];
              if (((double) power1 - (double) threshold) * ((double) power2 - (double) threshold) < 0.0)
              {
                float num8 = (float) (((double) threshold - (double) power1) / ((double) power2 - (double) power1));
                Vector3 vector3_1 = Vector3.op_Addition(Vector3.op_Multiply(positionMap[index6], num8), Vector3.op_Multiply(positionMap[index5], 1f - num8));
                vector3List1.Add(vector3_1);
                Vector3 vector3_2 = Vector3.op_Addition(vector3_1, uvOffset);
                vector2List.Add(new Vector2(Vector3.Dot(vector3_2, uDir), Vector3.Dot(vector3_2, vDir)));
                Vector3 vector3_3 = Vector3.op_Addition(Vector3.op_Multiply(vector3Array[index6], num8), Vector3.op_Multiply(vector3Array[index5], 1f - num8));
                Vector3 vector3_4 = Vector3.op_UnaryNegation(((Vector3) ref vector3_3).get_normalized());
                vector3List2.Add(!bReverse ? vector3_4 : Vector3.op_UnaryNegation(vector3_4));
                numArray1[index4 * num3 + index5] = num5;
                ++num5;
              }
            }
          }
        }
      }
    }
    int[] numArray2 = new int[15];
    int num9 = 0;
    List<int> intList = new List<int>();
    if (num5 > 3)
    {
      for (int index1 = 0; index1 < countZ - 1; ++index1)
      {
        for (int index2 = 0; index2 < countY - 1; ++index2)
        {
          for (int index3 = 0; index3 < countX - 1; ++index3)
          {
            byte num4 = 0;
            for (int index4 = 0; index4 < 2; ++index4)
            {
              for (int index5 = 0; index5 < 2; ++index5)
              {
                for (int index6 = 0; index6 < 2; ++index6)
                {
                  if (flagArray[index3 + index6 + (index2 + index5) * num1 + (index1 + index4) * num2])
                    num4 |= (byte) (1 << index4 * 4 + index5 * 2 + index6);
                }
              }
            }
            int[] numArray3 = new int[12];
            for (int index4 = 0; index4 < 3; ++index4)
            {
              for (int index5 = 0; index5 < 2; ++index5)
              {
                for (int index6 = 0; index6 < 2; ++index6)
                {
                  int num6;
                  int num7;
                  int num8;
                  switch (index4)
                  {
                    case 0:
                      num6 = index3;
                      num7 = index2 + index5;
                      num8 = index1 + index6;
                      break;
                    case 1:
                      num6 = index3 + index6;
                      num7 = index2;
                      num8 = index1 + index5;
                      break;
                    case 2:
                      num6 = index3 + index5;
                      num7 = index2 + index6;
                      num8 = index1;
                      break;
                    default:
                      int num10;
                      num8 = num10 = -1;
                      num7 = num10;
                      num6 = num10;
                      break;
                  }
                  int index7 = index4 * 4 + index6 * 2 + index5;
                  numArray3[index7] = numArray1[index4 * num3 + num6 + num7 * num1 + num8 * num2];
                }
              }
            }
            int primaryPatternIndex = MetaballBuilder.__cubePatterns[(int) num4].MatchingInfo.PrimaryPatternIndex;
            ++numArray2[primaryPatternIndex];
            if (true)
            {
              for (int index4 = 0; index4 < MetaballBuilder.__cubePatterns[(int) num4].MatchingInfo.GetTargetPrimitiveIndexBuffer().Length; ++index4)
              {
                intList.Add(numArray3[MetaballBuilder.__cubePatterns[(int) num4].IndexBuf[index4]]);
                ++num9;
              }
            }
          }
        }
      }
    }
    Mesh mesh = new Mesh();
    mesh.set_vertices(vector3List1.ToArray());
    mesh.set_uv(vector2List.ToArray());
    mesh.set_normals(vector3List2.ToArray());
    if (!bReverse)
    {
      mesh.set_triangles(intList.ToArray());
    }
    else
    {
      intList.Reverse();
      mesh.set_triangles(intList.ToArray());
    }
    return mesh;
  }

  private void BuildMetaballMesh(
    Mesh mesh,
    Vector3 center,
    Vector3 extent,
    float cellSize,
    MetaballBuilder.MetaballPointInfo[] points,
    float powerThreshold,
    bool bReverse,
    Vector3 uDir,
    Vector3 vDir,
    Vector3 uvOffset)
  {
    if (!MetaballBuilder.__bCubePatternsInitialized)
      MetaballBuilder.__InitCubePatterns();
    int num1 = Mathf.CeilToInt((float) extent.x / cellSize) + 1;
    int num2 = Mathf.CeilToInt((float) extent.y / cellSize) + 1;
    int num3 = Mathf.CeilToInt((float) extent.z / cellSize) + 1;
    int num4 = num1 * 2;
    int num5 = num2 * 2;
    int num6 = num3 * 2;
    int num7 = num4;
    int num8 = num5;
    int num9 = num6;
    Vector3 vector3_1;
    ((Vector3) ref vector3_1).\u002Ector((float) num1 * cellSize, (float) num2 * cellSize, (float) num3 * cellSize);
    Vector3 vector3_2 = Vector3.op_Subtraction(center, vector3_1);
    float[] numArray1 = new float[num4 * num5 * num6];
    Vector3[] vector3Array1 = new Vector3[num4 * num5 * num6];
    Vector3[] vector3Array2 = new Vector3[num4 * num5 * num6];
    int[] numArray2 = new int[num4 * num5 * num6 * 3];
    bool[] flagArray = new bool[num4 * num5 * num6];
    BoneWeight[] boneWeightArray = new BoneWeight[num4 * num5 * num6];
    int num10 = num7;
    int num11 = num7 * num8;
    int num12 = num7 * num8 * num9;
    for (int index1 = 0; index1 < num9; ++index1)
    {
      for (int index2 = 0; index2 < num8; ++index2)
      {
        for (int index3 = 0; index3 < num7; ++index3)
          vector3Array1[index3 + index2 * num10 + index1 * num11] = Vector3.op_Addition(vector3_2, new Vector3(cellSize * (float) index3, cellSize * (float) index2, cellSize * (float) index1));
      }
    }
    for (int index = 0; index < 3 * num9 * num8 * num7; ++index)
      numArray2[index] = -1;
    int num13 = 0;
    for (int index1 = 0; index1 < points.Length; ++index1)
    {
      MetaballBuilder.MetaballPointInfo point = points[index1];
      int num14 = (int) Mathf.Floor(((float) (point.center.x - center.x) - point.radius) / cellSize) + num1;
      int num15 = (int) Mathf.Floor(((float) (point.center.y - center.y) - point.radius) / cellSize) + num2;
      int num16 = (int) Mathf.Floor(((float) (point.center.z - center.z) - point.radius) / cellSize) + num3;
      int num17 = Mathf.Max(0, num14);
      int num18 = Mathf.Max(0, num15);
      int num19 = Mathf.Max(0, num16);
      int num20 = (int) Mathf.Ceil(((float) (point.center.x - center.x) + point.radius) / cellSize) + num1;
      int num21 = (int) Mathf.Ceil(((float) (point.center.y - center.y) + point.radius) / cellSize) + num2;
      int num22 = (int) Mathf.Ceil(((float) (point.center.z - center.z) + point.radius) / cellSize) + num3;
      int num23 = Mathf.Min(num7 - 1, num20);
      int num24 = Mathf.Min(num8 - 1, num21);
      int num25 = Mathf.Min(num9 - 1, num22);
      for (int index2 = num19; index2 <= num25; ++index2)
      {
        for (int index3 = num18; index3 <= num24; ++index3)
        {
          for (int index4 = num17; index4 <= num23; ++index4)
          {
            int index5 = index4 + index3 * num10 + index2 * num11;
            float num26 = MetaballBuilder.CalcPower(Vector3.op_Subtraction(vector3Array1[index5], point.center), point.radius, point.density);
            numArray1[index5] += num26;
            if ((double) num26 > 0.0)
            {
              BoneWeight boneWeight = boneWeightArray[index5];
              if ((double) ((BoneWeight) ref boneWeight).get_weight0() < (double) num26 || (double) ((BoneWeight) ref boneWeight).get_weight1() < (double) num26)
              {
                if ((double) ((BoneWeight) ref boneWeight).get_weight0() < (double) ((BoneWeight) ref boneWeight).get_weight1())
                {
                  ((BoneWeight) ref boneWeight).set_weight0(num26);
                  ((BoneWeight) ref boneWeight).set_boneIndex0(num13);
                }
                else
                {
                  ((BoneWeight) ref boneWeight).set_weight1(num26);
                  ((BoneWeight) ref boneWeight).set_boneIndex1(num13);
                }
              }
              boneWeightArray[index5] = boneWeight;
            }
          }
        }
      }
      ++num13;
    }
    float num27 = powerThreshold;
    for (int index = 0; index < num7 * num8 * num9; ++index)
    {
      flagArray[index] = (double) numArray1[index] >= (double) num27;
      if (flagArray[index])
      {
        float num14 = 1f / 1000f;
        if ((double) Mathf.Abs(numArray1[index] - num27) < (double) num14)
          numArray1[index] = (double) numArray1[index] - (double) num27 < 0.0 ? num27 - num14 : num27 + num14;
      }
    }
    for (int index1 = 1; index1 < num9 - 1; ++index1)
    {
      for (int index2 = 1; index2 < num8 - 1; ++index2)
      {
        for (int index3 = 1; index3 < num7 - 1; ++index3)
        {
          vector3Array2[index3 + index2 * num10 + index1 * num11].x = (__Null) ((double) numArray1[index3 + 1 + index2 * num10 + index1 * num11] - (double) numArray1[index3 - 1 + index2 * num10 + index1 * num11]);
          vector3Array2[index3 + index2 * num10 + index1 * num11].y = (__Null) ((double) numArray1[index3 + (index2 + 1) * num10 + index1 * num11] - (double) numArray1[index3 + (index2 - 1) * num10 + index1 * num11]);
          vector3Array2[index3 + index2 * num10 + index1 * num11].z = (__Null) ((double) numArray1[index3 + index2 * num10 + (index1 + 1) * num11] - (double) numArray1[index3 + index2 * num10 + (index1 - 1) * num11]);
          int index4 = index3 + index2 * num10 + index1 * num11;
          if ((double) ((Vector3) ref vector3Array2[index4]).get_sqrMagnitude() > 1.0 / 1000.0)
            ((Vector3) ref vector3Array2[index4]).Normalize();
        }
      }
    }
    int num28 = 0;
    List<Vector3> vector3List1 = new List<Vector3>();
    List<Vector3> vector3List2 = new List<Vector3>();
    List<Vector2> vector2List = new List<Vector2>();
    for (int index1 = 0; index1 < num9 && num28 < 299999; ++index1)
    {
      for (int index2 = 0; index2 < num8 && num28 < 299999; ++index2)
      {
        for (int index3 = 0; index3 < num7 && num28 < 299999; ++index3)
        {
          for (int index4 = 0; index4 < 3 && num28 < 299999; ++index4)
          {
            int num14 = index4 != 0 ? 0 : 1;
            int num15 = index4 != 1 ? 0 : 1;
            int num16 = index4 != 2 ? 0 : 1;
            if (index1 + num16 < num9 && index2 + num15 < num8 && index3 + num14 < num7)
            {
              int index5 = index3 + index2 * num10 + index1 * num11;
              int index6 = index3 + num14 + (index2 + num15) * num10 + (index1 + num16) * num11;
              float num17 = numArray1[index5];
              float num18 = numArray1[index6];
              if (((double) num17 - (double) num27) * ((double) num18 - (double) num27) < 0.0)
              {
                float num19 = (float) (((double) num27 - (double) num17) / ((double) num18 - (double) num17));
                Vector3 vector3_3 = Vector3.op_Addition(Vector3.op_Multiply(vector3Array1[index6], num19), Vector3.op_Multiply(vector3Array1[index5], 1f - num19));
                vector3List1.Add(vector3_3);
                Vector3 vector3_4 = Vector3.op_Addition(vector3_3, uvOffset);
                vector2List.Add(new Vector2(Vector3.Dot(vector3_4, uDir), Vector3.Dot(vector3_4, vDir)));
                Vector3 vector3_5 = Vector3.op_Addition(Vector3.op_Multiply(vector3Array2[index6], num19), Vector3.op_Multiply(vector3Array2[index5], 1f - num19));
                Vector3 vector3_6 = Vector3.op_UnaryNegation(((Vector3) ref vector3_5).get_normalized());
                vector3List2.Add(!bReverse ? vector3_6 : Vector3.op_UnaryNegation(vector3_6));
                numArray2[index4 * num12 + index5] = num28;
                ++num28;
              }
            }
          }
        }
      }
    }
    int[] numArray3 = new int[15];
    int[] numArray4 = new int[12];
    List<int> intList = new List<int>();
    if (num28 > 3)
    {
      for (int index1 = 0; index1 < num9 - 1; ++index1)
      {
        for (int index2 = 0; index2 < num8 - 1; ++index2)
        {
          for (int index3 = 0; index3 < num7 - 1; ++index3)
          {
            byte num14 = 0;
            for (int index4 = 0; index4 < 2; ++index4)
            {
              for (int index5 = 0; index5 < 2; ++index5)
              {
                for (int index6 = 0; index6 < 2; ++index6)
                {
                  if (flagArray[index3 + index6 + (index2 + index5) * num10 + (index1 + index4) * num11])
                    num14 |= (byte) (1 << index4 * 4 + index5 * 2 + index6);
                }
              }
            }
            for (int index4 = 0; index4 < 3; ++index4)
            {
              for (int index5 = 0; index5 < 2; ++index5)
              {
                for (int index6 = 0; index6 < 2; ++index6)
                {
                  int num15;
                  int num16;
                  int num17;
                  switch (index4)
                  {
                    case 0:
                      num15 = index3;
                      num16 = index2 + index5;
                      num17 = index1 + index6;
                      break;
                    case 1:
                      num15 = index3 + index6;
                      num16 = index2;
                      num17 = index1 + index5;
                      break;
                    case 2:
                      num15 = index3 + index5;
                      num16 = index2 + index6;
                      num17 = index1;
                      break;
                    default:
                      int num18;
                      num17 = num18 = -1;
                      num16 = num18;
                      num15 = num18;
                      break;
                  }
                  int index7 = index4 * 4 + index6 * 2 + index5;
                  numArray4[index7] = numArray2[index4 * num12 + num15 + num16 * num10 + num17 * num11];
                }
              }
            }
            int primaryPatternIndex = MetaballBuilder.__cubePatterns[(int) num14].MatchingInfo.PrimaryPatternIndex;
            ++numArray3[primaryPatternIndex];
            for (int index4 = 0; index4 < MetaballBuilder.__cubePatterns[(int) num14].MatchingInfo.GetTargetPrimitiveIndexBuffer().Length; ++index4)
            {
              if (numArray4[MetaballBuilder.__cubePatterns[(int) num14].IndexBuf[index4]] < 0)
              {
                Debug.Log((object) ("(x,y,z)=" + (object) index3 + "," + (object) index2 + "," + (object) index1));
                Debug.Log((object) ("resolution=" + (object) num7 + "," + (object) num8 + "," + (object) num9));
                string str1 = string.Empty;
                for (int index5 = 0; index5 < 12; ++index5)
                  str1 = str1 + numArray4[index5].ToString() + ",";
                Debug.Log((object) ("localvtxmap=" + str1));
                Debug.Log((object) ("inout=" + Convert.ToString(num14, 2)));
                Debug.Log((object) ("idx=" + (object) index4));
                Debug.Log((object) ("primaryPatternIdx=" + (object) MetaballBuilder.__cubePatterns[(int) num14].MatchingInfo.PrimaryPatternIndex));
                Debug.Log((object) ("indexCount=" + (object) MetaballBuilder.__primitivePatterns[MetaballBuilder.__cubePatterns[(int) num14].MatchingInfo.PrimaryPatternIndex].IndexCount));
                string str2 = string.Empty;
                for (int index5 = 0; index5 < 2; ++index5)
                {
                  for (int index6 = 0; index6 < 2; ++index6)
                  {
                    for (int index7 = 0; index7 < 2; ++index7)
                    {
                      int index8 = index3 + index7 + (index2 + index6) * num10 + (index1 + index5) * num11;
                      str2 = str2 + numArray1[index8].ToString() + ",";
                    }
                  }
                }
                Debug.Log((object) ("powerMap=" + str2));
                throw new UnityException("vertex error");
              }
              intList.Add(numArray4[MetaballBuilder.__cubePatterns[(int) num14].IndexBuf[index4]]);
            }
          }
        }
      }
    }
    mesh.set_vertices(vector3List1.ToArray());
    mesh.set_uv(vector2List.ToArray());
    mesh.set_normals(vector3List2.ToArray());
    if (!bReverse)
    {
      mesh.set_triangles(intList.ToArray());
    }
    else
    {
      intList.Reverse();
      mesh.set_triangles(intList.ToArray());
    }
  }

  private class MB3DCubeVector
  {
    public sbyte[] e = new sbyte[3];
    public sbyte axisIdx = -1;

    public MB3DCubeVector()
    {
    }

    public MB3DCubeVector(sbyte x_, sbyte y_, sbyte z_)
    {
      this.x = x_;
      this.y = y_;
      this.z = z_;
      this.axisIdx = (sbyte) -1;
      this.CalcAxis();
    }

    public sbyte x
    {
      get
      {
        return this.e[0];
      }
      set
      {
        this.e[0] = value;
      }
    }

    public sbyte y
    {
      get
      {
        return this.e[1];
      }
      set
      {
        this.e[1] = value;
      }
    }

    public sbyte z
    {
      get
      {
        return this.e[2];
      }
      set
      {
        this.e[2] = value;
      }
    }

    public static MetaballBuilder.MB3DCubeVector operator +(
      MetaballBuilder.MB3DCubeVector lh,
      MetaballBuilder.MB3DCubeVector rh)
    {
      return new MetaballBuilder.MB3DCubeVector((sbyte) ((int) lh.x + (int) rh.x), (sbyte) ((int) lh.y + (int) rh.y), (sbyte) ((int) lh.z + (int) rh.z));
    }

    private void CalcAxis()
    {
      for (sbyte index = 0; index < (sbyte) 3; ++index)
      {
        if (this.e[(int) index] != (sbyte) 0)
        {
          if (this.axisIdx == (sbyte) -1)
          {
            this.axisIdx = index;
          }
          else
          {
            this.axisIdx = (sbyte) -1;
            break;
          }
        }
      }
    }

    public void SetAxisVector(sbyte axisIdx_, sbyte value)
    {
      sbyte num1 = 0;
      this.z = num1;
      sbyte num2 = num1;
      this.y = num2;
      this.x = num2;
      if (value == (sbyte) 0)
      {
        this.axisIdx = (sbyte) -1;
      }
      else
      {
        this.axisIdx = axisIdx_;
        this.e[(int) this.axisIdx] = value;
      }
    }

    public static MetaballBuilder.MB3DCubeVector operator *(
      MetaballBuilder.MB3DCubeVector lh,
      sbyte rh)
    {
      return new MetaballBuilder.MB3DCubeVector((sbyte) ((int) lh.x * (int) rh), (sbyte) ((int) lh.y * (int) rh), (sbyte) ((int) lh.z * (int) rh));
    }
  }

  private class MB3DCubeInOut
  {
    public sbyte[,,] bFill = new sbyte[2, 2, 2];
    public int inCount;

    public MB3DCubeInOut()
    {
    }

    public MB3DCubeInOut(sbyte patternIdx)
    {
      sbyte[] numArray = new sbyte[8];
      for (int index = 0; index < 8; ++index)
        numArray[index] = (sbyte) ((int) patternIdx >> index & 1);
      this.Init(numArray[0], numArray[1], numArray[2], numArray[3], numArray[4], numArray[5], numArray[6], numArray[7]);
    }

    public MB3DCubeInOut(
      sbyte a0,
      sbyte a1,
      sbyte a2,
      sbyte a3,
      sbyte a4,
      sbyte a5,
      sbyte a6,
      sbyte a7)
    {
      this.Init(a0, a1, a2, a3, a4, a5, a6, a7);
    }

    private void Init(
      sbyte a0,
      sbyte a1,
      sbyte a2,
      sbyte a3,
      sbyte a4,
      sbyte a5,
      sbyte a6,
      sbyte a7)
    {
      this.bFill[0, 0, 0] = a0;
      this.bFill[0, 0, 1] = a1;
      this.bFill[0, 1, 0] = a2;
      this.bFill[0, 1, 1] = a3;
      this.bFill[1, 0, 0] = a4;
      this.bFill[1, 0, 1] = a5;
      this.bFill[1, 1, 0] = a6;
      this.bFill[1, 1, 1] = a7;
      this.inCount = (int) a0 + (int) a1 + (int) a2 + (int) a3 + (int) a4 + (int) a5 + (int) a6 + (int) a7;
    }

    public sbyte At(MetaballBuilder.MB3DCubeVector point)
    {
      return this.bFill[(int) point.z, (int) point.y, (int) point.x];
    }
  }

  private struct MB3DCubePrimitivePattern
  {
    public MetaballBuilder.MB3DCubeInOut InOut;
    public int[] IndexBuf;
    public int[] IndexBufAlter;

    public int IndexCount
    {
      get
      {
        return this.IndexBuf != null ? this.IndexBuf.Length : 0;
      }
    }

    public int IndexCountAlter
    {
      get
      {
        return this.IndexBufAlter != null ? this.IndexBufAlter.Length : 0;
      }
    }
  }

  private class MB3DPatternMatchingInfo
  {
    public MetaballBuilder.MB3DCubeVector Origin = new MetaballBuilder.MB3DCubeVector();
    public MetaballBuilder.MB3DCubeVector[] Axis = new MetaballBuilder.MB3DCubeVector[3];
    public int PrimaryPatternIndex;
    public bool bFlipInOut;

    public void Match(MetaballBuilder.MB3DCubeInOut src)
    {
      this.PrimaryPatternIndex = -1;
      this.bFlipInOut = src.inCount > 4;
      for (int index = 0; index < 15; ++index)
      {
        MetaballBuilder.MB3DCubeInOut inOut = MetaballBuilder.__primitivePatterns[index].InOut;
        if (this.bFlipInOut)
        {
          if (8 - src.inCount != inOut.inCount)
            continue;
        }
        else if (src.inCount != inOut.inCount)
          continue;
        sbyte[] numArray = new sbyte[3];
        for (this.Origin.x = (sbyte) 0; this.Origin.x < (sbyte) 2; ++this.Origin.x)
        {
          numArray[0] = this.Origin.x == (sbyte) 0 ? (sbyte) 1 : (sbyte) -1;
          for (this.Origin.y = (sbyte) 0; this.Origin.y < (sbyte) 2; ++this.Origin.y)
          {
            numArray[1] = this.Origin.y == (sbyte) 0 ? (sbyte) 1 : (sbyte) -1;
            for (this.Origin.z = (sbyte) 0; this.Origin.z < (sbyte) 2; ++this.Origin.z)
            {
              numArray[2] = this.Origin.z == (sbyte) 0 ? (sbyte) 1 : (sbyte) -1;
              sbyte num = ((int) this.Origin.x + (int) this.Origin.y + (int) this.Origin.z) % 2 == 0 ? (sbyte) 1 : (sbyte) 2;
              for (sbyte axisIdx_ = 0; axisIdx_ < (sbyte) 3; ++axisIdx_)
              {
                this.Axis[0] = new MetaballBuilder.MB3DCubeVector();
                this.Axis[1] = new MetaballBuilder.MB3DCubeVector();
                this.Axis[2] = new MetaballBuilder.MB3DCubeVector();
                this.Axis[0].SetAxisVector(axisIdx_, numArray[(int) axisIdx_]);
                this.Axis[1].SetAxisVector((sbyte) (((int) axisIdx_ + (int) num) % 3), numArray[((int) axisIdx_ + (int) num) % 3]);
                this.Axis[2].SetAxisVector((sbyte) (((int) axisIdx_ + (int) num + (int) num) % 3), numArray[((int) axisIdx_ + (int) num + (int) num) % 3]);
                bool flag = true;
                for (sbyte x_ = 0; x_ < (sbyte) 2; ++x_)
                {
                  for (sbyte y_ = 0; y_ < (sbyte) 2; ++y_)
                  {
                    for (sbyte z_ = 0; z_ < (sbyte) 2; ++z_)
                    {
                      MetaballBuilder.MB3DCubeVector point = this.SampleVertex(new MetaballBuilder.MB3DCubeVector(x_, y_, z_));
                      if (this.bFlipInOut == ((int) src.At(point) == (int) inOut.bFill[(int) z_, (int) y_, (int) x_]))
                        flag = false;
                    }
                  }
                }
                if (flag)
                {
                  this.PrimaryPatternIndex = index;
                  return;
                }
              }
            }
          }
        }
      }
    }

    public int[] GetTargetPrimitiveIndexBuffer()
    {
      return this.bFlipInOut && MetaballBuilder.__primitivePatterns[this.PrimaryPatternIndex].IndexCountAlter > 0 ? MetaballBuilder.__primitivePatterns[this.PrimaryPatternIndex].IndexBufAlter : MetaballBuilder.__primitivePatterns[this.PrimaryPatternIndex].IndexBuf;
    }

    public MetaballBuilder.MB3DCubeVector SampleVertex(
      MetaballBuilder.MB3DCubeVector primaryPos)
    {
      return this.Origin + this.Axis[0] * primaryPos.x + this.Axis[1] * primaryPos.y + this.Axis[2] * primaryPos.z;
    }

    public void SampleSegment(
      sbyte primarySegmentID,
      out sbyte out_axis,
      out sbyte out_a_1,
      out sbyte out_a_2)
    {
      sbyte num1 = (sbyte) ((int) primarySegmentID / 4);
      sbyte num2 = (sbyte) ((int) primarySegmentID % 2);
      sbyte num3 = (sbyte) ((int) primarySegmentID / 2 % 2);
      out_axis = this.Axis[(int) num1].axisIdx;
      MetaballBuilder.MB3DCubeVector mb3DcubeVector = this.Origin + this.Axis[((int) num1 + 1) % 3] * num2 + this.Axis[((int) num1 + 2) % 3] * num3;
      sbyte num4 = (sbyte) (((int) out_axis + 1) % 3);
      sbyte num5 = (sbyte) (((int) out_axis + 2) % 3);
      out_a_1 = mb3DcubeVector.e[(int) num4];
      out_a_2 = mb3DcubeVector.e[(int) num5];
    }
  }

  private class MB3DCubePattern
  {
    public MetaballBuilder.MB3DPatternMatchingInfo MatchingInfo = new MetaballBuilder.MB3DPatternMatchingInfo();
    public int[] IndexBuf = new int[15];
    public int PatternIdx;

    public void Init(int patternIdx)
    {
      this.PatternIdx = patternIdx;
      this.MatchingInfo.Match(new MetaballBuilder.MB3DCubeInOut((sbyte) patternIdx));
      int[] primitiveIndexBuffer = this.MatchingInfo.GetTargetPrimitiveIndexBuffer();
      for (int index = 0; index < primitiveIndexBuffer.Length; ++index)
      {
        sbyte out_axis;
        sbyte out_a_1;
        sbyte out_a_2;
        this.MatchingInfo.SampleSegment((sbyte) primitiveIndexBuffer[index], out out_axis, out out_a_1, out out_a_2);
        this.IndexBuf[!this.MatchingInfo.bFlipInOut ? index : primitiveIndexBuffer.Length - index - 1] = (int) out_axis * 4 + (int) out_a_2 * 2 + (int) out_a_1;
      }
    }
  }

  public class MetaballPointInfo
  {
    public Vector3 center;
    public float radius;
    public float density;
  }
}
