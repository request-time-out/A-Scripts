// Decompiled with JetBrains decompiler
// Type: ADV.Commands.Base.Vector
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace ADV.Commands.Base
{
  public class Vector : CommandBase
  {
    public override string[] ArgsLabel
    {
      get
      {
        return new string[5]
        {
          "Variable",
          "Type",
          "X",
          "Y",
          "Z"
        };
      }
    }

    public override string[] ArgsDefault
    {
      get
      {
        return new string[5]
        {
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty,
          string.Empty
        };
      }
    }

    public override void Do()
    {
      base.Do();
      int num1 = 0;
      string[] args1 = this.args;
      int index1 = num1;
      int num2 = index1 + 1;
      string key = args1[index1];
      string[] args2 = this.args;
      int index2 = num2;
      int index3 = index2 + 1;
      string str = args2[index2];
      Vector3 vector3_1 = Vector3.get_zero();
      Dictionary<string, Vector3> v3Dic = this.scenario.commandController.V3Dic;
      int num3;
      if (str == "=")
      {
        if (!v3Dic.TryGetValue(this.args[index3], out vector3_1))
        {
          for (int index4 = 0; index4 < 3; ++index4)
          {
            int index5 = index4 + index3;
            float result;
            if (!this.args.SafeGet<string>(index5).IsNullOrEmpty() && float.TryParse(this.args[index5], out result))
              ((Vector3) ref vector3_1).set_Item(index4, result);
          }
        }
      }
      else if (str == "+=")
      {
        if (v3Dic.TryGetValue(key, out vector3_1))
        {
          float result;
          if (float.TryParse(this.args[index3], out result))
          {
            for (int index4 = 0; index4 < 3; ++index4)
            {
              int index5 = index4 + index3;
              if (!this.args.SafeGet<string>(index5).IsNullOrEmpty() && float.TryParse(this.args[index5], out result))
              {
                // ISSUE: variable of a reference type
                Vector3& local;
                int num4;
                ((Vector3) (local = ref vector3_1)).set_Item(num4 = index4, ((Vector3) ref local).get_Item(num4) + result);
              }
            }
          }
          else
          {
            Vector3 vector3_2 = vector3_1;
            Dictionary<string, Vector3> dictionary = v3Dic;
            string[] args3 = this.args;
            int index4 = index3;
            num3 = index4 + 1;
            string index5 = args3[index4];
            Vector3 vector3_3 = dictionary[index5];
            vector3_1 = Vector3.op_Addition(vector3_2, vector3_3);
          }
        }
      }
      else if (str == "-=" && v3Dic.TryGetValue(key, out vector3_1))
      {
        float result;
        if (float.TryParse(this.args[index3], out result))
        {
          for (int index4 = 0; index4 < 3; ++index4)
          {
            int index5 = index4 + index3;
            if (!this.args.SafeGet<string>(index5).IsNullOrEmpty() && float.TryParse(this.args[index5], out result))
            {
              // ISSUE: variable of a reference type
              Vector3& local;
              int num4;
              ((Vector3) (local = ref vector3_1)).set_Item(num4 = index4, ((Vector3) ref local).get_Item(num4) - result);
            }
          }
        }
        else
        {
          Vector3 vector3_2 = vector3_1;
          Dictionary<string, Vector3> dictionary = v3Dic;
          string[] args3 = this.args;
          int index4 = index3;
          num3 = index4 + 1;
          string index5 = args3[index4];
          Vector3 vector3_3 = dictionary[index5];
          vector3_1 = Vector3.op_Subtraction(vector3_2, vector3_3);
        }
      }
      this.scenario.commandController.V3Dic[key] = vector3_1;
    }
  }
}
