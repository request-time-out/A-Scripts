// Decompiled with JetBrains decompiler
// Type: MotionIKData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using AIChara;
using Correct;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MotionIKData
{
  private List<string> row = new List<string>();
  private ExcelData data;
  private MotionIKData.State[] _states;

  public MotionIKData Copy()
  {
    MotionIKData motionIkData = new MotionIKData();
    int length1 = this.states.Length;
    motionIkData.states = new MotionIKData.State[length1];
    for (int index1 = 0; index1 < length1; ++index1)
    {
      MotionIKData.State state1 = new MotionIKData.State();
      MotionIKData.State state2 = this.states[index1];
      state1.name = state2.name;
      int length2 = state1.parts.Length;
      for (int index2 = 0; index2 < length2; ++index2)
      {
        MotionIKData.Parts part1 = state1.parts[index2];
        MotionIKData.Parts part2 = state2.parts[index2];
        part1.param2.sex = part2.param2.sex;
        part1.param2.target = part2.param2.target;
        part1.param2.weightPos = part2.param2.weightPos;
        part1.param2.weightAng = part2.param2.weightAng;
        part1.param2.blendInfos[0] = new List<MotionIKData.BlendWeightInfo>((IEnumerable<MotionIKData.BlendWeightInfo>) part2.param2.blendInfos[0]);
        part1.param2.blendInfos[1] = new List<MotionIKData.BlendWeightInfo>((IEnumerable<MotionIKData.BlendWeightInfo>) part2.param2.blendInfos[1]);
        part1.param3.chein = part2.param3.chein;
        part1.param3.weight = part2.param3.weight;
        part1.param3.blendInfos = new List<MotionIKData.BlendWeightInfo>((IEnumerable<MotionIKData.BlendWeightInfo>) part2.param3.blendInfos);
      }
      int length3 = state2.frames.Length;
      state1.frames = new MotionIKData.Frame[length3];
      for (int index2 = 0; index2 < length3; ++index2)
      {
        MotionIKData.Frame frame1 = new MotionIKData.Frame();
        MotionIKData.Frame frame2 = state2.frames[index2];
        frame1.frameNo = frame2.frameNo;
        frame1.editNo = frame2.editNo;
        int length4 = frame2.shapes.Length;
        frame1.shapes = new MotionIKData.Shape[length4];
        for (int index3 = 0; index3 < length4; ++index3)
        {
          MotionIKData.Shape shape1 = new MotionIKData.Shape();
          MotionIKData.Shape shape2 = frame2.shapes[index3];
          shape1.shapeNo = shape2.shapeNo;
          for (int index4 = 0; index4 < 3; ++index4)
          {
            shape1[index4] = new MotionIKData.PosAng?(new MotionIKData.PosAng());
            MotionIKData.PosAng posAng = shape1[index4].Value;
            for (int index5 = 0; index5 < 3; ++index5)
              ((Vector3) ref posAng.pos).set_Item(index5, ((Vector3) ref shape2[index4].Value.pos).get_Item(index5));
            for (int index5 = 0; index5 < 3; ++index5)
              ((Vector3) ref posAng.ang).set_Item(index5, ((Vector3) ref shape2[index4].Value.ang).get_Item(index5));
            shape1[index4] = new MotionIKData.PosAng?(posAng);
          }
          frame1.shapes[index3] = shape1;
        }
        state1.frames[index2] = frame1;
      }
      motionIkData.states[index1] = state1;
    }
    return motionIkData;
  }

  public MotionIKData.State InitState(string stateName, int sex)
  {
    int index1 = -1;
    if (this.states != null)
    {
      int index2 = -1;
      do
        ;
      while (++index2 < this.states.Length && !(this.states[index2].name == stateName));
      index1 = index2 < this.states.Length ? index2 : -1;
    }
    if (index1 == -1)
    {
      MotionIKData.State[] stateArray = new MotionIKData.State[1]
      {
        new MotionIKData.State() { name = stateName }
      };
      this.states = this.states != null ? ((IEnumerable<MotionIKData.State>) this.states).Concat<MotionIKData.State>((IEnumerable<MotionIKData.State>) stateArray).ToArray<MotionIKData.State>() : stateArray;
      index1 = this.states.Length - 1;
    }
    MotionIKData.State state = this.states[index1];
    MotionIKData.InitFrame(state, sex);
    return state;
  }

  public void Release()
  {
    this.states = (MotionIKData.State[]) null;
  }

  public static void InitFrame(MotionIKData.State state, int sex)
  {
    int ikLen = MotionIK.IKTargetPair.IKTargetLength;
    IEnumerable<MotionIKData.Frame> source = Enumerable.Range(0, ikLen).Select<int, MotionIKData.Frame>((Func<int, MotionIKData.Frame>) (i => new MotionIKData.Frame()
    {
      frameNo = i
    })).Concat<MotionIKData.Frame>(Enumerable.Range(0, FrameCorrect.FrameNames.Length).Select<int, MotionIKData.Frame>((Func<int, MotionIKData.Frame>) (i => new MotionIKData.Frame()
    {
      frameNo = i + ikLen
    })));
    state.frames = source.ToArray<MotionIKData.Frame>();
    for (int index = 0; index < state.frames.Length; ++index)
      MotionIKData.InitShape(ref state.frames[index]);
  }

  public static void InitShape(ref MotionIKData.Frame frame)
  {
    frame.shapes = Enumerable.Range(0, ChaFileDefine.cf_bodyshapename.Length).Select<int, MotionIKData.Shape>((Func<int, MotionIKData.Shape>) (i => new MotionIKData.Shape()
    {
      shapeNo = i
    })).ToArray<MotionIKData.Shape>();
    for (int index = 0; index < frame.shapes.Length; ++index)
    {
      MotionIKData.Shape shape = frame.shapes[index];
      shape.small = new MotionIKData.PosAng();
      shape.mediam = new MotionIKData.PosAng();
      shape.large = new MotionIKData.PosAng();
    }
  }

  public MotionIKData.State[] states
  {
    get
    {
      return this._states;
    }
    set
    {
      this._states = value;
    }
  }

  public void AIRead(string abName, string assetName, bool add = false)
  {
    if (!GlobalMethod.AssetFileExist(abName, assetName, string.Empty))
      return;
    this.data = (ExcelData) null;
    this.data = CommonLib.LoadAsset<ExcelData>(abName, assetName, false, string.Empty);
    Singleton<HSceneManager>.Instance.hashUseAssetBundle.Add(abName);
    if (Object.op_Equality((Object) this.data, (Object) null))
      return;
    int length = this.data.list[1].list.Count - 4;
    int num1 = 0;
    int maxCell = this.data.MaxCell;
    MotionIKData.State[] stateArray;
    if (!add)
    {
      stateArray = new MotionIKData.State[length];
    }
    else
    {
      num1 = this.states.Length;
      stateArray = new MotionIKData.State[length + num1];
      for (int index = 0; index < num1; ++index)
        stateArray[index] = new MotionIKData.State(this.states[index]);
    }
    string empty1 = string.Empty;
    string empty2 = string.Empty;
    string empty3 = string.Empty;
    for (int index1 = 0; index1 < length; ++index1)
    {
      int num2 = index1;
      int index2 = num2;
      if (add)
        index2 += num1;
      stateArray[index2] = new MotionIKData.State();
      stateArray[index2].frames = new MotionIKData.Frame[FrameCorrect.FrameNames.Length + 8];
      int index3 = -1;
      int index4 = -1;
      int index5 = -1;
      bool flag1 = true;
      int num3 = 1;
      while (num3 < maxCell)
      {
        this.row = this.data.list[num3++].list;
        int count = this.row.Count;
        int index6 = 4 + num2;
        string str = index6 >= count ? (string) null : this.row[index6];
        bool flag2 = str.IsNullOrEmpty();
        int num4;
        if (num3 < 28)
        {
          string key = index6 >= count ? (string) null : this.row[3];
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (MotionIKData.\u003C\u003Ef__switch\u0024mapB == null)
            {
              // ISSUE: reference to a compiler-generated field
              MotionIKData.\u003C\u003Ef__switch\u0024mapB = new Dictionary<string, int>(7)
              {
                {
                  "anime name",
                  0
                },
                {
                  "sex",
                  1
                },
                {
                  "target",
                  2
                },
                {
                  "chain",
                  3
                },
                {
                  "weight pos",
                  4
                },
                {
                  "weight rot",
                  5
                },
                {
                  "weight",
                  6
                }
              };
            }
            // ISSUE: reference to a compiler-generated field
            if (MotionIKData.\u003C\u003Ef__switch\u0024mapB.TryGetValue(key, out num4))
            {
              switch (num4)
              {
                case 0:
                  stateArray[index2].name = str;
                  continue;
                case 1:
                  ++index3;
                  if ((index6 >= count ? (string) null : this.row[0]) != string.Empty)
                  {
                    int.TryParse(str, out stateArray[index2].parts[index3].param2.sex);
                    continue;
                  }
                  continue;
                case 2:
                  stateArray[index2].parts[index3].param2.target = str;
                  continue;
                case 3:
                  stateArray[index2].parts[index3].param3.chein = str;
                  continue;
                case 4:
                  float.TryParse(str, out stateArray[index2].parts[index3].param2.weightPos);
                  continue;
                case 5:
                  float.TryParse(str, out stateArray[index2].parts[index3].param2.weightAng);
                  continue;
                case 6:
                  float.TryParse(str, out stateArray[index2].parts[index3].param3.weight);
                  continue;
                default:
                  continue;
              }
            }
          }
        }
        else if (num3 < 86)
        {
          ++index4;
          if (!flag2)
            int.TryParse(str, out stateArray[index2].frames[index4].editNo);
          stateArray[index2].frames[index4].frameNo = index4;
          stateArray[index2].frames[index4].shapes = new MotionIKData.Shape[ChaFileDefine.cf_bodyshapename.Length];
          index5 = -1;
        }
        else
        {
          string key = count <= 3 ? (string) null : this.row[3];
          if (key != null)
          {
            // ISSUE: reference to a compiler-generated field
            if (MotionIKData.\u003C\u003Ef__switch\u0024mapC == null)
            {
              // ISSUE: reference to a compiler-generated field
              MotionIKData.\u003C\u003Ef__switch\u0024mapC = new Dictionary<string, int>(9)
              {
                {
                  "Sx",
                  0
                },
                {
                  "Sy",
                  1
                },
                {
                  "Sz",
                  2
                },
                {
                  "Mx",
                  3
                },
                {
                  "My",
                  4
                },
                {
                  "Mz",
                  5
                },
                {
                  "Lx",
                  6
                },
                {
                  "Ly",
                  7
                },
                {
                  "Lz",
                  8
                }
              };
            }
            // ISSUE: reference to a compiler-generated field
            if (MotionIKData.\u003C\u003Ef__switch\u0024mapC.TryGetValue(key, out num4))
            {
              switch (num4)
              {
                case 0:
                  if (flag1)
                  {
                    if ((count <= 0 ? (string) null : this.row[0]) != string.Empty)
                    {
                      ++index4;
                      index5 = -1;
                    }
                    ++index5;
                    stateArray[index2].frames[index4].shapes[index5].shapeNo = index5;
                  }
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].small.pos.x);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].small.ang.x);
                    continue;
                  }
                  continue;
                case 1:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].small.pos.y);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].small.ang.y);
                    continue;
                  }
                  continue;
                case 2:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].small.pos.z);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].small.ang.z);
                    continue;
                  }
                  continue;
                case 3:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].mediam.pos.x);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].mediam.ang.x);
                    continue;
                  }
                  continue;
                case 4:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].mediam.pos.y);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].mediam.ang.y);
                    continue;
                  }
                  continue;
                case 5:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].mediam.pos.z);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].mediam.ang.z);
                    continue;
                  }
                  continue;
                case 6:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].large.pos.x);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].large.ang.x);
                    continue;
                  }
                  continue;
                case 7:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].large.pos.y);
                      continue;
                    }
                    // ISSUE: cast to a reference type
                    float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].large.ang.y);
                    continue;
                  }
                  continue;
                case 8:
                  if (!flag2)
                  {
                    if (flag1)
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].large.pos.z);
                    }
                    else
                    {
                      // ISSUE: cast to a reference type
                      float.TryParse(str, (float&) ref stateArray[index2].frames[index4].shapes[index5].large.ang.z);
                    }
                  }
                  flag1 = ((flag1 ? 1 : 0) ^ 1) != 0;
                  continue;
              }
            }
          }
          if ((count <= 0 ? (string) null : this.row[0]) == "フレーム")
            index4 = -1;
        }
      }
    }
    this.states = new MotionIKData.State[stateArray.Length];
    for (int index = 0; index < stateArray.Length; ++index)
      this.states[index] = stateArray[index];
  }

  public class State
  {
    public string name = string.Empty;
    public MotionIKData.Parts leftHand = new MotionIKData.Parts();
    public MotionIKData.Parts rightHand = new MotionIKData.Parts();
    public MotionIKData.Parts leftFoot = new MotionIKData.Parts();
    public MotionIKData.Parts rightFoot = new MotionIKData.Parts();
    public MotionIKData.Frame[] frames;

    public State()
    {
    }

    public State(MotionIKData.State src)
    {
      this.name = src.name;
      this.leftHand = src.leftHand;
      this.rightHand = src.rightHand;
      this.leftFoot = src.leftFoot;
      this.rightFoot = src.rightFoot;
      this.frames = src.frames;
    }

    public MotionIKData.Parts this[int index]
    {
      get
      {
        return this.parts[index];
      }
    }

    public MotionIKData.Parts[] parts
    {
      get
      {
        return new MotionIKData.Parts[4]
        {
          this.leftHand,
          this.rightHand,
          this.leftFoot,
          this.rightFoot
        };
      }
    }
  }

  public class Parts
  {
    public MotionIKData.Param2 param2 = new MotionIKData.Param2();
    public MotionIKData.Param3 param3 = new MotionIKData.Param3();
  }

  public class Param2
  {
    public string target = string.Empty;
    public List<MotionIKData.BlendWeightInfo>[] blendInfos = new List<MotionIKData.BlendWeightInfo>[2]
    {
      new List<MotionIKData.BlendWeightInfo>(),
      new List<MotionIKData.BlendWeightInfo>()
    };
    public int sex;
    public float weightPos;
    public float weightAng;

    public object this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return (object) this.sex;
          case 1:
            return (object) this.target;
          case 2:
            return (object) this.weightPos;
          case 3:
            return (object) this.weightAng;
          default:
            return (object) null;
        }
      }
      set
      {
        switch (index)
        {
          case 0:
            this.sex = !(value is string) ? (int) value : int.Parse((string) value);
            break;
          case 1:
            this.target = (string) value;
            break;
          case 2:
            this.weightPos = !(value is string) ? (float) value : float.Parse((string) value);
            break;
          case 3:
            this.weightAng = !(value is string) ? (float) value : float.Parse((string) value);
            break;
        }
      }
    }
  }

  public class Param3
  {
    public string chein = string.Empty;
    public List<MotionIKData.BlendWeightInfo> blendInfos = new List<MotionIKData.BlendWeightInfo>();
    public float weight;

    public object this[int index]
    {
      get
      {
        if (index == 0)
          return (object) this.chein;
        return index == 1 ? (object) this.weight : (object) null;
      }
      set
      {
        switch (index)
        {
          case 0:
            this.chein = (string) value;
            break;
          case 1:
            this.weight = !(value is string) ? (float) value : float.Parse((string) value);
            break;
        }
      }
    }
  }

  public struct Frame
  {
    public int frameNo;
    public int editNo;
    public MotionIKData.Shape[] shapes;
  }

  public struct Shape
  {
    public int shapeNo;
    public MotionIKData.PosAng small;
    public MotionIKData.PosAng mediam;
    public MotionIKData.PosAng large;

    public MotionIKData.PosAng? this[int index]
    {
      get
      {
        switch (index)
        {
          case 0:
            return new MotionIKData.PosAng?(this.small);
          case 1:
            return new MotionIKData.PosAng?(this.mediam);
          case 2:
            return new MotionIKData.PosAng?(this.large);
          default:
            return new MotionIKData.PosAng?();
        }
      }
      set
      {
        if (!value.HasValue)
          return;
        switch (index)
        {
          case 0:
            this.small = value.Value;
            break;
          case 1:
            this.mediam = value.Value;
            break;
          case 2:
            this.large = value.Value;
            break;
        }
      }
    }
  }

  public struct PosAng
  {
    public Vector3 pos;
    public Vector3 ang;

    public Vector3 this[int index]
    {
      get
      {
        if (index == 0)
          return this.pos;
        return index == 1 ? this.ang : Vector3.get_zero();
      }
      set
      {
        if (index != 0)
        {
          if (index != 1)
            return;
          this.ang = value;
        }
        else
          this.pos = value;
      }
    }

    public float[] posArray
    {
      get
      {
        return new float[3]
        {
          (float) this.pos.x,
          (float) this.pos.y,
          (float) this.pos.z
        };
      }
    }

    public float[] angArray
    {
      get
      {
        return new float[3]
        {
          (float) this.ang.x,
          (float) this.ang.y,
          (float) this.ang.z
        };
      }
    }
  }

  public struct BlendWeightInfo
  {
    public int pattern;
    public float StartKey;
    public float EndKey;
    public MotionIKData.Shape shape;
  }
}
