// Decompiled with JetBrains decompiler
// Type: ADV.ValData
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using ADV.Commands.Base;
using Illusion;
using System;
using UnityEngine;

namespace ADV
{
  [Serializable]
  public class ValData
  {
    public ValData(object o)
    {
      this.o = o;
    }

    public object o { get; private set; }

    public object Convert(object val)
    {
      return ValData.Convert(val, this.o.GetType());
    }

    public static object Convert(object val, System.Type type)
    {
      return Convert.ChangeType(val, type);
    }

    public static object Cast(object o, System.Type type)
    {
      if (o == null)
        return ValData.Convert(o, type);
      if (type == typeof (int))
      {
        int? nullable = new int?();
        switch (o)
        {
          case int _:
          case float _:
            nullable = new int?((int) o);
            break;
          case bool flag:
            nullable = new int?(!flag ? 0 : 1);
            break;
          default:
            int result;
            if (int.TryParse(o.ToString(), out result))
            {
              nullable = new int?(result);
              break;
            }
            break;
        }
        return (object) (!nullable.HasValue ? 0 : nullable.Value);
      }
      if (type == typeof (float))
      {
        float? nullable = new float?();
        if (o is float num)
        {
          nullable = new float?(num);
        }
        else
        {
          float result;
          if (float.TryParse(o.ToString(), out result))
            nullable = new float?(result);
        }
        return (object) (float) (!nullable.HasValue ? 0.0 : (double) nullable.Value);
      }
      if (!(type == typeof (bool)))
        return (object) o.ToString();
      bool? nullable1 = new bool?();
      switch (o)
      {
        case bool flag:
          nullable1 = new bool?(flag);
          break;
        case int _:
        case float _:
          nullable1 = new bool?((int) o > 0);
          break;
        default:
          bool result1;
          if (bool.TryParse(o.ToString(), out result1))
          {
            nullable1 = new bool?(result1);
            break;
          }
          break;
      }
      return (object) (bool) (!nullable1.HasValue ? 0 : (nullable1.Value ? 1 : 0));
    }

    public static bool operator <(ValData a, ValData b)
    {
      return ValData.IF(Utils.Comparer.Type.Lesser, a.o, b.o);
    }

    public static bool operator >(ValData a, ValData b)
    {
      return ValData.IF(Utils.Comparer.Type.Greater, a.o, b.o);
    }

    public static bool operator <=(ValData a, ValData b)
    {
      return ValData.IF(Utils.Comparer.Type.Under, a.o, b.o);
    }

    public static bool operator >=(ValData a, ValData b)
    {
      return ValData.IF(Utils.Comparer.Type.Over, a.o, b.o);
    }

    public static ValData operator +(ValData a, ValData b)
    {
      return ValData.Calculate(Calc.Formula1.PlusEqual, a.o, b.o);
    }

    public static ValData operator -(ValData a, ValData b)
    {
      return ValData.Calculate(Calc.Formula1.MinusEqual, a.o, b.o);
    }

    public static ValData operator *(ValData a, ValData b)
    {
      return ValData.Calculate(Calc.Formula1.AstaEqual, a.o, b.o);
    }

    public static ValData operator /(ValData a, ValData b)
    {
      return ValData.Calculate(Calc.Formula1.SlashEqual, a.o, b.o);
    }

    private static bool IF(Utils.Comparer.Type type, object a, object b)
    {
      return Utils.Comparer.Check<IComparable>((IComparable) a, type, (IComparable) b);
    }

    private static ValData Calculate(Calc.Formula1 numerical, object a, object b)
    {
      switch (a)
      {
        case int num3:
          int num1 = (int) ValData.Cast(b, typeof (int));
          switch (numerical)
          {
            case Calc.Formula1.PlusEqual:
              return new ValData((object) (num3 + num1));
            case Calc.Formula1.MinusEqual:
              return new ValData((object) (num3 - num1));
            case Calc.Formula1.AstaEqual:
              return new ValData((object) (num3 * num1));
            case Calc.Formula1.SlashEqual:
              return new ValData((object) (num3 / num1));
          }
          break;
        case float num3:
          float num2 = (float) ValData.Cast(b, typeof (float));
          switch (numerical)
          {
            case Calc.Formula1.PlusEqual:
              return new ValData((object) (float) ((double) num3 + (double) num2));
            case Calc.Formula1.MinusEqual:
              return new ValData((object) (float) ((double) num3 - (double) num2));
            case Calc.Formula1.AstaEqual:
              return new ValData((object) (float) ((double) num3 * (double) num2));
            case Calc.Formula1.SlashEqual:
              return new ValData((object) (float) ((double) num3 / (double) num2));
          }
          break;
        case bool flag2:
          bool flag1 = (bool) ValData.Cast(b, typeof (bool));
          switch (numerical)
          {
            case Calc.Formula1.PlusEqual:
              return new ValData((object) ((!flag2 ? 0 : 1) + (!flag1 ? 0 : 1) > 0));
            case Calc.Formula1.MinusEqual:
              return new ValData((object) ((!flag2 ? 0 : 1) - (!flag1 ? 0 : 1) > 0));
            case Calc.Formula1.AstaEqual:
              return new ValData((object) (flag2 | flag1));
            case Calc.Formula1.SlashEqual:
              return new ValData((object) (flag2 & flag1));
          }
          break;
        default:
          string str = a.ToString();
          string oldValue = b.ToString();
          if (numerical == Calc.Formula1.PlusEqual)
            return new ValData((object) (str + oldValue));
          if (numerical == Calc.Formula1.MinusEqual)
            return new ValData((object) str.Replace(oldValue, string.Empty));
          break;
      }
      Debug.LogError((object) (numerical.ToString() + a + b));
      return new ValData((object) null);
    }
  }
}
