// Decompiled with JetBrains decompiler
// Type: Illusion.Extensions.VectorExtensions
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using System;
using UnityEngine;

namespace Illusion.Extensions
{
  public static class VectorExtensions
  {
    private static string[] FormatRemoveSplit(string str)
    {
      return VectorExtensions.FormatRemove(str).Split(',');
    }

    private static string FormatRemove(string str)
    {
      return str.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty);
    }

    public static string Convert(this Vector2 self, bool isDefault = true)
    {
      int num1 = 0;
      string format = !isDefault ? "{0},{1}" : "({0}, {1})";
      ref Vector2 local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      // ISSUE: variable of a boxed type
      __Boxed<float> local2 = (ValueType) ((Vector2) ref local1).get_Item(num2);
      ref Vector2 local3 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      // ISSUE: variable of a boxed type
      __Boxed<float> local4 = (ValueType) ((Vector2) ref local3).get_Item(num4);
      return string.Format(format, (object) local2, (object) local4);
    }

    public static string Convert(this Vector3 self, bool isDefault = true)
    {
      int num1 = 0;
      string format = !isDefault ? "{0},{1},{2}" : "({0}, {1}, {2})";
      ref Vector3 local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      // ISSUE: variable of a boxed type
      __Boxed<float> local2 = (ValueType) ((Vector3) ref local1).get_Item(num2);
      ref Vector3 local3 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      // ISSUE: variable of a boxed type
      __Boxed<float> local4 = (ValueType) ((Vector3) ref local3).get_Item(num4);
      ref Vector3 local5 = ref self;
      int num6 = num5;
      int num7 = num6 + 1;
      // ISSUE: variable of a boxed type
      __Boxed<float> local6 = (ValueType) ((Vector3) ref local5).get_Item(num6);
      return string.Format(format, (object) local2, (object) local4, (object) local6);
    }

    public static string Convert(this Vector4 self, bool isDefault = true)
    {
      int num1 = 0;
      string format = !isDefault ? "{0},{1},{2},{3}" : "({0}, {1}, {2}, {3})";
      object[] objArray = new object[4];
      ref Vector4 local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      objArray[0] = (object) ((Vector4) ref local1).get_Item(num2);
      ref Vector4 local2 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      objArray[1] = (object) ((Vector4) ref local2).get_Item(num4);
      ref Vector4 local3 = ref self;
      int num6 = num5;
      int num7 = num6 + 1;
      objArray[2] = (object) ((Vector4) ref local3).get_Item(num6);
      ref Vector4 local4 = ref self;
      int num8 = num7;
      int num9 = num8 + 1;
      objArray[3] = (object) ((Vector4) ref local4).get_Item(num8);
      return string.Format(format, objArray);
    }

    public static Vector2 Convert(this Vector2 _, string str)
    {
      string[] strArray = VectorExtensions.FormatRemoveSplit(str);
      Vector2 zero = Vector2.get_zero();
      for (int index = 0; index < strArray.Length && index < 2; ++index)
      {
        float result;
        if (float.TryParse(strArray[index], out result))
          ((Vector2) ref zero).set_Item(index, result);
      }
      return zero;
    }

    public static Vector3 Convert(this Vector3 _, string str)
    {
      string[] strArray = VectorExtensions.FormatRemoveSplit(str);
      Vector3 zero = Vector3.get_zero();
      for (int index = 0; index < strArray.Length && index < 3; ++index)
      {
        float result;
        if (float.TryParse(strArray[index], out result))
          ((Vector3) ref zero).set_Item(index, result);
      }
      return zero;
    }

    public static Vector4 Convert(this Vector4 _, string str)
    {
      string[] strArray = VectorExtensions.FormatRemoveSplit(str);
      Vector4 zero = Vector4.get_zero();
      for (int index = 0; index < strArray.Length && index < 4; ++index)
      {
        float result;
        if (float.TryParse(strArray[index], out result))
          ((Vector4) ref zero).set_Item(index, result);
      }
      return zero;
    }

    public static Vector2 Convert(this Vector2 self, float[] fArray)
    {
      int num1 = 0;
      float[] numArray1 = fArray;
      int index1 = num1;
      int num2 = index1 + 1;
      double num3 = (double) numArray1[index1];
      float[] numArray2 = fArray;
      int index2 = num2;
      int num4 = index2 + 1;
      double num5 = (double) numArray2[index2];
      return new Vector2((float) num3, (float) num5);
    }

    public static Vector3 Convert(this Vector3 self, float[] fArray)
    {
      int num1 = 0;
      float[] numArray1 = fArray;
      int index1 = num1;
      int num2 = index1 + 1;
      double num3 = (double) numArray1[index1];
      float[] numArray2 = fArray;
      int index2 = num2;
      int num4 = index2 + 1;
      double num5 = (double) numArray2[index2];
      float[] numArray3 = fArray;
      int index3 = num4;
      int num6 = index3 + 1;
      double num7 = (double) numArray3[index3];
      return new Vector3((float) num3, (float) num5, (float) num7);
    }

    public static Vector4 Convert(this Vector4 self, float[] fArray)
    {
      int num1 = 0;
      float[] numArray1 = fArray;
      int index1 = num1;
      int num2 = index1 + 1;
      double num3 = (double) numArray1[index1];
      float[] numArray2 = fArray;
      int index2 = num2;
      int num4 = index2 + 1;
      double num5 = (double) numArray2[index2];
      float[] numArray3 = fArray;
      int index3 = num4;
      int num6 = index3 + 1;
      double num7 = (double) numArray3[index3];
      float[] numArray4 = fArray;
      int index4 = num6;
      int num8 = index4 + 1;
      double num9 = (double) numArray4[index4];
      return new Vector4((float) num3, (float) num5, (float) num7, (float) num9);
    }

    public static float[] ToArray(this Vector2 self)
    {
      int num1 = 0;
      float[] numArray = new float[2];
      ref Vector2 local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      numArray[0] = ((Vector2) ref local1).get_Item(num2);
      ref Vector2 local2 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      numArray[1] = ((Vector2) ref local2).get_Item(num4);
      return numArray;
    }

    public static float[] ToArray(this Vector3 self)
    {
      int num1 = 0;
      float[] numArray = new float[3];
      ref Vector3 local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      numArray[0] = ((Vector3) ref local1).get_Item(num2);
      ref Vector3 local2 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      numArray[1] = ((Vector3) ref local2).get_Item(num4);
      ref Vector3 local3 = ref self;
      int num6 = num5;
      int num7 = num6 + 1;
      numArray[2] = ((Vector3) ref local3).get_Item(num6);
      return numArray;
    }

    public static float[] ToArray(this Vector4 self)
    {
      int num1 = 0;
      float[] numArray = new float[4];
      ref Vector4 local1 = ref self;
      int num2 = num1;
      int num3 = num2 + 1;
      numArray[0] = ((Vector4) ref local1).get_Item(num2);
      ref Vector4 local2 = ref self;
      int num4 = num3;
      int num5 = num4 + 1;
      numArray[1] = ((Vector4) ref local2).get_Item(num4);
      ref Vector4 local3 = ref self;
      int num6 = num5;
      int num7 = num6 + 1;
      numArray[2] = ((Vector4) ref local3).get_Item(num6);
      ref Vector4 local4 = ref self;
      int num8 = num7;
      int num9 = num8 + 1;
      numArray[3] = ((Vector4) ref local4).get_Item(num8);
      return numArray;
    }

    public static Vector2 Get(this Vector2 self, Vector2 set, bool x = true, bool y = true)
    {
      return new Vector2(!x ? (float) self.x : (float) set.x, !y ? (float) self.y : (float) set.y);
    }

    public static Vector3 Get(this Vector3 self, Vector3 set, bool x = true, bool y = true, bool z = true)
    {
      return new Vector3(!x ? (float) self.x : (float) set.x, !y ? (float) self.y : (float) set.y, !z ? (float) self.z : (float) set.z);
    }

    public static Vector4 Get(
      this Vector4 self,
      Vector4 set,
      bool x = true,
      bool y = true,
      bool z = true,
      bool w = true)
    {
      return new Vector4(!x ? (float) self.x : (float) set.x, !y ? (float) self.y : (float) set.y, !z ? (float) self.z : (float) set.z, !w ? (float) self.w : (float) set.w);
    }

    public static bool isNaN(this Vector2 self)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (float.IsNaN(((Vector2) ref self).get_Item(index)))
          return true;
      }
      return false;
    }

    public static bool isNaN(this Vector3 self)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (float.IsNaN(((Vector3) ref self).get_Item(index)))
          return true;
      }
      return false;
    }

    public static bool isNaN(this Vector4 self)
    {
      for (int index = 0; index < 4; ++index)
      {
        if (float.IsNaN(((Vector4) ref self).get_Item(index)))
          return true;
      }
      return false;
    }

    public static bool isInfinity(this Vector2 self)
    {
      for (int index = 0; index < 2; ++index)
      {
        if (float.IsInfinity(((Vector2) ref self).get_Item(index)))
          return true;
      }
      return false;
    }

    public static bool isInfinity(this Vector3 self)
    {
      for (int index = 0; index < 3; ++index)
      {
        if (float.IsInfinity(((Vector3) ref self).get_Item(index)))
          return true;
      }
      return false;
    }

    public static bool isInfinity(this Vector4 self)
    {
      for (int index = 0; index < 4; ++index)
      {
        if (float.IsInfinity(((Vector4) ref self).get_Item(index)))
          return true;
      }
      return false;
    }
  }
}
