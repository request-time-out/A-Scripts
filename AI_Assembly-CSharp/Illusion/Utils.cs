// Decompiled with JetBrains decompiler
// Type: Illusion.Utils
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using Illusion.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Illusion
{
  public static class Utils
  {
    public static class Animator
    {
      public static string GetControllerName(UnityEngine.Animator animator)
      {
        return Utils.Animator.GetControllerName(animator.get_runtimeAnimatorController());
      }

      public static string GetControllerName(
        RuntimeAnimatorController runtimeAnimatorController)
      {
        return Object.op_Equality((Object) runtimeAnimatorController, (Object) null) ? (string) null : ((Object) runtimeAnimatorController).get_name();
      }

      public static AnimatorControllerParameter[] GetAnimeParams(
        UnityEngine.Animator animator)
      {
        return Enumerable.Range(0, animator.get_parameterCount()).Select<int, AnimatorControllerParameter>(new Func<int, AnimatorControllerParameter>(animator.GetParameter)).ToArray<AnimatorControllerParameter>();
      }

      public static AnimatorControllerParameter GetAnimeParam(
        string name,
        UnityEngine.Animator animator)
      {
        return Utils.Animator.GetAnimeParam(name, Utils.Animator.GetAnimeParams(animator));
      }

      public static AnimatorControllerParameter GetAnimeParam(
        string name,
        AnimatorControllerParameter[] param)
      {
        return ((IEnumerable<AnimatorControllerParameter>) param).FirstOrDefault<AnimatorControllerParameter>((Func<AnimatorControllerParameter, bool>) (item => item.get_name() == name));
      }

      public static bool AnimeParamFindSet(UnityEngine.Animator animator, Tuple<string, object> nameValue)
      {
        return Utils.Animator.AnimeParamFindSet(animator, nameValue.Item1, nameValue.Item2, Utils.Animator.GetAnimeParams(animator));
      }

      public static bool AnimeParamFindSet(UnityEngine.Animator animator, string name, object value)
      {
        return Utils.Animator.AnimeParamFindSet(animator, name, value, Utils.Animator.GetAnimeParams(animator));
      }

      public static bool AnimeParamFindSet(UnityEngine.Animator animator, Tuple<string, object>[] nameValues)
      {
        return Utils.Animator.AnimeParamFindSet(animator, nameValues, Utils.Animator.GetAnimeParams(animator));
      }

      public static bool AnimeParamFindSet(
        UnityEngine.Animator animator,
        Tuple<string, object> nameValue,
        AnimatorControllerParameter[] animParams)
      {
        return Utils.Animator.AnimeParamFindSet(animator, nameValue.Item1, nameValue.Item2, animParams);
      }

      public static bool AnimeParamFindSet(
        UnityEngine.Animator animator,
        string name,
        object value,
        AnimatorControllerParameter[] animParams)
      {
        return ((IEnumerable<AnimatorControllerParameter>) animParams).FirstOrDefault<AnimatorControllerParameter>((Func<AnimatorControllerParameter, bool>) (p => p.get_name() == name)).SafeProc<AnimatorControllerParameter>((Action<AnimatorControllerParameter>) (param => Utils.Animator.AnimeParamSet(animator, name, value, param.get_type())));
      }

      public static bool AnimeParamFindSet(
        UnityEngine.Animator animator,
        Tuple<string, object>[] nameValues,
        AnimatorControllerParameter[] animParams)
      {
        bool flag = false;
        using (IEnumerator<\u003C\u003E__AnonType33<AnimatorControllerParameterType, Tuple<string, object>>> enumerator = ((IEnumerable<Tuple<string, object>>) nameValues).Select<Tuple<string, object>, \u003C\u003E__AnonType33<AnimatorControllerParameterType, Tuple<string, object>>>((Func<Tuple<string, object>, \u003C\u003E__AnonType33<AnimatorControllerParameterType, Tuple<string, object>>>) (v =>
        {
          AnimatorControllerParameter controllerParameter = ((IEnumerable<AnimatorControllerParameter>) animParams).FirstOrDefault<AnimatorControllerParameter>((Func<AnimatorControllerParameter, bool>) (p => p.get_name() == v.Item1));
          // ISSUE: object of a compiler-generated type is created
          return controllerParameter == null ? (\u003C\u003E__AnonType33<AnimatorControllerParameterType, Tuple<string, object>>) null : new \u003C\u003E__AnonType33<AnimatorControllerParameterType, Tuple<string, object>>(controllerParameter.get_type(), v);
        })).GetEnumerator())
        {
          while (((IEnumerator) enumerator).MoveNext())
          {
            // ISSUE: variable of a compiler-generated type
            \u003C\u003E__AnonType33<AnimatorControllerParameterType, Tuple<string, object>> current = enumerator.Current;
            flag |= Utils.Animator.AnimeParamSet(animator, current.value, current.type);
          }
        }
        return flag;
      }

      public static bool AnimeParamSet(UnityEngine.Animator animator, Tuple<string, object> nameValue)
      {
        return Utils.Animator.AnimeParamSet(animator, nameValue.Item1, nameValue.Item2);
      }

      public static bool AnimeParamSet(UnityEngine.Animator animator, string name, object value)
      {
        switch (value)
        {
          case float num:
            animator.SetFloat(name, num);
            break;
          case int num:
            animator.SetInteger(name, num);
            break;
          case bool flag:
            animator.SetBool(name, flag);
            break;
          default:
            return false;
        }
        return true;
      }

      public static bool AnimeParamSet(
        UnityEngine.Animator animator,
        Tuple<string, object> nameValue,
        AnimatorControllerParameterType type)
      {
        return Utils.Animator.AnimeParamSet(animator, nameValue.Item1, nameValue.Item2, type);
      }

      public static bool AnimeParamSet(
        UnityEngine.Animator animator,
        string name,
        object value,
        AnimatorControllerParameterType type)
      {
        switch (type - 1)
        {
          case 0:
            animator.SetFloat(name, (float) value);
            break;
          case 2:
            animator.SetInteger(name, (int) value);
            break;
          case 3:
            animator.SetBool(name, (bool) value);
            break;
          default:
            if (type != 9)
              return false;
            switch (value)
            {
              case null:
                animator.ResetTrigger(name);
                break;
              case bool flag:
                if (flag)
                {
                  animator.SetTrigger(name);
                  break;
                }
                animator.ResetTrigger(name);
                break;
              case int num:
                if (num != 0)
                {
                  animator.SetTrigger(name);
                  break;
                }
                animator.ResetTrigger(name);
                break;
              default:
                animator.SetTrigger(name);
                break;
            }
            break;
        }
        return true;
      }

      public static AnimatorOverrideController SetupAnimatorOverrideController(
        RuntimeAnimatorController src,
        RuntimeAnimatorController over)
      {
        if (Object.op_Equality((Object) src, (Object) null) || Object.op_Equality((Object) over, (Object) null))
          return (AnimatorOverrideController) null;
        AnimatorOverrideController overrideController = new AnimatorOverrideController(src);
        foreach (AnimationClip animationClip in ((RuntimeAnimatorController) new AnimatorOverrideController(over)).get_animationClips())
          overrideController.set_Item(((Object) animationClip).get_name(), animationClip);
        ((Object) overrideController).set_name(((Object) over).get_name());
        return overrideController;
      }
    }

    public static class Comparer
    {
      public static readonly string[] STR = new string[6]
      {
        "==",
        "!=",
        ">=",
        "<=",
        ">",
        "<"
      };
      public static readonly string[] LABEL = new string[6]
      {
        "一致",
        "不一致",
        "以上",
        "以下",
        "より大きい",
        "より小さい"
      };

      public static bool Check<T>(T v1, string compStr, T v2) where T : IComparable
      {
        return Utils.Comparer.Check<T>(v1, (Utils.Comparer.Type) Utils.Comparer.STR.Check<string>((Func<string, bool>) (s => s == compStr)), v2);
      }

      public static bool Check<T>(T v1, Utils.Comparer.Type compEnum, T v2) where T : IComparable
      {
        int num = v1.CompareTo((object) v2);
        switch (compEnum)
        {
          case Utils.Comparer.Type.Equal:
            return num == 0;
          case Utils.Comparer.Type.NotEqual:
            return num != 0;
          case Utils.Comparer.Type.Over:
            return num >= 0;
          case Utils.Comparer.Type.Under:
            return num <= 0;
          case Utils.Comparer.Type.Greater:
            return num > 0;
          case Utils.Comparer.Type.Lesser:
            return num < 0;
          default:
            return false;
        }
      }

      public static Utils.Comparer.Type Cast(string str, out string v)
      {
        int findIndex = -1;
        int index = Utils.Comparer.STR.Check<string>((Func<string, bool>) (s =>
        {
          findIndex = str.IndexOf(s);
          return findIndex != -1;
        }));
        v = str.Substring(findIndex + Utils.Comparer.STR[index].Length);
        return (Utils.Comparer.Type) index;
      }

      public static Tuple<Utils.Comparer.Type, string>[] Cast(params string[] strs)
      {
        string v;
        return Enumerable.Range(0, strs.Length).Select<int, Tuple<Utils.Comparer.Type, string>>((Func<int, Tuple<Utils.Comparer.Type, string>>) (i => Tuple.Create<Utils.Comparer.Type, string>(Utils.Comparer.Cast(strs[i], out v), v))).ToArray<Tuple<Utils.Comparer.Type, string>>();
      }

      public enum Type
      {
        Equal,
        NotEqual,
        Over,
        Under,
        Greater,
        Lesser,
      }
    }

    public static class Crypto
    {
      private const string AesInitVector = "1234567890abcdefghujklmnopqrstuv";
      private const string AesKey = "piyopiyopiyopiyopiyopiyopiyopiyo";
      private const int BlockSize = 256;
      private const int KeySize = 256;

      public static byte[] Encrypt(byte[] binData)
      {
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Padding = PaddingMode.Zeros;
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 256;
        byte[] bytes1 = Encoding.UTF8.GetBytes("piyopiyopiyopiyopiyopiyopiyopiyo");
        byte[] bytes2 = Encoding.UTF8.GetBytes("1234567890abcdefghujklmnopqrstuv");
        ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes1, bytes2);
        MemoryStream memoryStream = new MemoryStream();
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
        {
          byte[] buffer = binData;
          cryptoStream.Write(buffer, 0, buffer.Length);
        }
        memoryStream.Close();
        return memoryStream.ToArray();
      }

      public static byte[] Decrypt(byte[] binData)
      {
        RijndaelManaged rijndaelManaged = new RijndaelManaged();
        rijndaelManaged.Padding = PaddingMode.Zeros;
        rijndaelManaged.Mode = CipherMode.CBC;
        rijndaelManaged.KeySize = 256;
        rijndaelManaged.BlockSize = 256;
        byte[] bytes1 = Encoding.UTF8.GetBytes("piyopiyopiyopiyopiyopiyopiyopiyo");
        byte[] bytes2 = Encoding.UTF8.GetBytes("1234567890abcdefghujklmnopqrstuv");
        ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes1, bytes2);
        byte[] buffer1 = binData;
        byte[] buffer2 = new byte[buffer1.Length];
        using (MemoryStream memoryStream = new MemoryStream(buffer1))
        {
          using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
            cryptoStream.Read(buffer2, 0, buffer2.Length);
        }
        return buffer2;
      }
    }

    public static class Enum<T> where T : struct
    {
      public static string[] Names
      {
        get
        {
          return Enum.GetNames(typeof (T));
        }
      }

      public static Array Values
      {
        get
        {
          return Enum.GetValues(typeof (T));
        }
      }

      public static int Length
      {
        get
        {
          return Utils.Enum<T>.Values.Length;
        }
      }

      public static void Each(Action<T> act)
      {
        foreach (T obj in Utils.Enum<T>.Values)
          act(obj);
      }

      [Conditional("UNITY_ASSERTIONS")]
      private static void Check(bool condition, string message)
      {
      }

      public static bool Contains(string key, bool ignoreCase = false)
      {
        return Utils.Enum<T>.FindIndex(key, ignoreCase) != -1;
      }

      public static int FindIndex(string key, bool ignoreCase = false)
      {
        string[] names = Utils.Enum<T>.Names;
        for (int index = 0; index < names.Length; ++index)
        {
          if (string.Compare(names[index], key, ignoreCase) == 0)
            return index;
        }
        return -1;
      }

      public static T Cast(string key)
      {
        return (T) Enum.Parse(typeof (T), key);
      }

      public static T Cast(int no)
      {
        return (T) Enum.ToObject(typeof (T), no);
      }

      public static T Cast(uint sum)
      {
        return (T) Enum.ToObject(typeof (T), sum);
      }

      public static T Cast(ulong sum)
      {
        return (T) Enum.ToObject(typeof (T), sum);
      }

      public static Utils.Enum<T>.EnumerateParameter Enumerate()
      {
        return new Utils.Enum<T>.EnumerateParameter();
      }

      public static T Nothing
      {
        get
        {
          return default (T);
        }
      }

      public static T Everything
      {
        get
        {
          ulong sum = 0;
          Utils.Enum<T>.Each((Action<T>) (o => sum += Convert.ToUInt64((object) o)));
          return Utils.Enum<T>.Cast(sum);
        }
      }

      public static T Normalize(T value)
      {
        return Utils.Enum<T>.Cast((ulong) (Convert.ToInt64((object) value) & Convert.ToInt64((object) Utils.Enum<T>.Everything)));
      }

      public static string ToString(T value)
      {
        StringBuilder text = new StringBuilder();
        Utils.Enum<T>.Each((Action<T>) (e =>
        {
          ulong uint64 = Convert.ToUInt64((object) e);
          if (((long) Convert.ToUInt64((object) value) & (long) uint64) != (long) uint64)
            return;
          text.AppendFormat("{0} | ", (object) e);
        }));
        return text.ToString();
      }

      public class EnumerateParameter
      {
        [DebuggerHidden]
        public IEnumerator<T> GetEnumerator()
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          Utils.Enum<T>.EnumerateParameter.\u003CGetEnumerator\u003Ec__Iterator0 enumeratorCIterator0 = new Utils.Enum<T>.EnumerateParameter.\u003CGetEnumerator\u003Ec__Iterator0();
          return (IEnumerator<T>) enumeratorCIterator0;
        }
      }
    }

    public static class File
    {
      public static string[] Gets(string filePath, string searchFile)
      {
        List<string> stringList1 = new List<string>();
        if (Directory.Exists(filePath))
        {
          foreach (string directory in Directory.GetDirectories(filePath))
          {
            List<string> stringList2 = stringList1;
            string[] files = Directory.GetFiles(directory, searchFile);
            // ISSUE: reference to a compiler-generated field
            if (Utils.File.\u003C\u003Ef__mg\u0024cache0 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Utils.File.\u003C\u003Ef__mg\u0024cache0 = new Func<string, string>(Utils.File.ConvertPath);
            }
            // ISSUE: reference to a compiler-generated field
            Func<string, string> fMgCache0 = Utils.File.\u003C\u003Ef__mg\u0024cache0;
            IEnumerable<string> collection = ((IEnumerable<string>) files).Select<string, string>(fMgCache0);
            stringList2.AddRange(collection);
          }
        }
        return stringList1.ToArray();
      }

      public static void GetAllFiles(string folder, string searchPattern, ref List<string> files)
      {
        if (!Directory.Exists(folder))
          return;
        List<string> stringList = files;
        string[] files1 = Directory.GetFiles(folder, searchPattern);
        // ISSUE: reference to a compiler-generated field
        if (Utils.File.\u003C\u003Ef__mg\u0024cache1 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Utils.File.\u003C\u003Ef__mg\u0024cache1 = new Func<string, string>(Utils.File.ConvertPath);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, string> fMgCache1 = Utils.File.\u003C\u003Ef__mg\u0024cache1;
        IEnumerable<string> collection = ((IEnumerable<string>) files1).Select<string, string>(fMgCache1);
        stringList.AddRange(collection);
        foreach (string directory in Directory.GetDirectories(folder))
          Utils.File.GetAllFiles(directory, searchPattern, ref files);
      }

      public static List<string> GetPaths(string[] paths, string ext, SearchOption option)
      {
        List<string> stringList1 = new List<string>();
        string[] strArray = paths;
        // ISSUE: reference to a compiler-generated field
        if (Utils.File.\u003C\u003Ef__mg\u0024cache2 == null)
        {
          // ISSUE: reference to a compiler-generated field
          Utils.File.\u003C\u003Ef__mg\u0024cache2 = new Func<string, bool>(Directory.Exists);
        }
        // ISSUE: reference to a compiler-generated field
        Func<string, bool> fMgCache2 = Utils.File.\u003C\u003Ef__mg\u0024cache2;
        foreach (IGrouping<bool, string> source1 in ((IEnumerable<string>) strArray).GroupBy<string, bool>(fMgCache2))
        {
          if (source1.Key)
          {
            foreach (string path in (IEnumerable<string>) source1)
            {
              List<string> stringList2 = stringList1;
              string[] files = Directory.GetFiles(path, "*" + ext, option);
              // ISSUE: reference to a compiler-generated field
              if (Utils.File.\u003C\u003Ef__mg\u0024cache3 == null)
              {
                // ISSUE: reference to a compiler-generated field
                Utils.File.\u003C\u003Ef__mg\u0024cache3 = new Func<string, string>(Utils.File.ConvertPath);
              }
              // ISSUE: reference to a compiler-generated field
              Func<string, string> fMgCache3 = Utils.File.\u003C\u003Ef__mg\u0024cache3;
              IEnumerable<string> collection = ((IEnumerable<string>) files).Select<string, string>(fMgCache3);
              stringList2.AddRange(collection);
            }
          }
          else
          {
            List<string> stringList2 = stringList1;
            IEnumerable<string> source2 = source1.Where<string>((Func<string, bool>) (path => Path.GetExtension(path) == ext));
            // ISSUE: reference to a compiler-generated field
            if (Utils.File.\u003C\u003Ef__mg\u0024cache4 == null)
            {
              // ISSUE: reference to a compiler-generated field
              Utils.File.\u003C\u003Ef__mg\u0024cache4 = new Func<string, string>(Utils.File.ConvertPath);
            }
            // ISSUE: reference to a compiler-generated field
            Func<string, string> fMgCache4 = Utils.File.\u003C\u003Ef__mg\u0024cache4;
            IEnumerable<string> collection = source2.Select<string, string>(fMgCache4);
            stringList2.AddRange(collection);
          }
        }
        return stringList1;
      }

      public static string ConvertPath(string path)
      {
        return path.Replace("\\", "/");
      }

      public static object LoadFromBinaryFile(string path)
      {
        object obj = (object) null;
        Utils.File.OpenRead(path, (Action<FileStream>) (fs => obj = new BinaryFormatter().Deserialize((Stream) fs)));
        return obj;
      }

      public static void SaveToBinaryFile(object obj, string path)
      {
        Utils.File.OpenWrite(path, false, (Action<FileStream>) (fs => new BinaryFormatter().Serialize((Stream) fs, obj)));
      }

      public static bool OpenRead(string filePath, Action<FileStream> act)
      {
        if (!System.IO.File.Exists(filePath))
          return false;
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
          act(fileStream);
        return true;
      }

      public static void OpenWrite(string filePath, bool isAppend, Action<FileStream> act)
      {
        if (!isAppend)
        {
          using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            act(fileStream);
        }
        else
        {
          using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            act(fileStream);
        }
      }

      public static bool Read(string filePath, Action<StreamReader> act)
      {
        return Utils.File.OpenRead(filePath, (Action<FileStream>) (fs =>
        {
          using (StreamReader streamReader = new StreamReader((Stream) fs))
            act(streamReader);
        }));
      }

      public static void Write(string filePath, bool isAppend, Action<StreamWriter> act)
      {
        Utils.File.OpenWrite(filePath, isAppend, (Action<FileStream>) (fs =>
        {
          using (StreamWriter streamWriter = new StreamWriter((Stream) fs))
            act(streamWriter);
        }));
      }
    }

    public static class Gizmos
    {
      public static void Axis(Vector3 pos, Quaternion rot, float len = 0.25f)
      {
        UnityEngine.Gizmos.set_color(Color.get_red());
        UnityEngine.Gizmos.DrawRay(pos, Vector3.op_Multiply(Quaternion.op_Multiply(rot, Vector3.get_right()), len));
        UnityEngine.Gizmos.set_color(Color.get_green());
        UnityEngine.Gizmos.DrawRay(pos, Vector3.op_Multiply(Quaternion.op_Multiply(rot, Vector3.get_up()), len));
        UnityEngine.Gizmos.set_color(Color.get_blue());
        UnityEngine.Gizmos.DrawRay(pos, Vector3.op_Multiply(Quaternion.op_Multiply(rot, Vector3.get_forward()), len));
      }

      public static void Axis(Transform transform, float len = 0.25f)
      {
        Utils.Gizmos.Axis(transform.get_position(), transform.get_rotation(), len);
      }

      public static void PointLine(Vector3[] route, bool isLink = false)
      {
        if (!((IEnumerable<Vector3>) route).Any<Vector3>())
          return;
        ((IEnumerable<Vector3>) route).Aggregate<Vector3>((Func<Vector3, Vector3, Vector3>) ((prev, current) =>
        {
          UnityEngine.Gizmos.DrawLine(prev, current);
          return current;
        }));
        if (!isLink)
          return;
        UnityEngine.Gizmos.DrawLine(((IEnumerable<Vector3>) route).Last<Vector3>(), ((IEnumerable<Vector3>) route).First<Vector3>());
      }
    }

    public static class Hash
    {
      public static bool Equals(byte[] arg1, byte[] arg2)
      {
        if (arg1.Length != arg2.Length)
          return false;
        int index = -1;
        do
          ;
        while (++index < arg1.Length && (int) arg1[index] == (int) arg2[index]);
        return index == arg1.Length;
      }

      public static byte[] ComputeMD5(string source)
      {
        return new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(source));
      }

      public static byte[] ComputeSHA1(string source)
      {
        return new SHA1CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(source));
      }

      public static int Convert(byte[] bytes)
      {
        if (BitConverter.IsLittleEndian)
          Array.Reverse((Array) bytes);
        return BitConverter.ToInt32(bytes, 0);
      }

      public static string Cast(byte[] source)
      {
        StringBuilder stringBuilder = new StringBuilder(source.Length);
        for (int index = 0; index < source.Length - 1; ++index)
          stringBuilder.Append(source[index].ToString("X2"));
        return stringBuilder.ToString();
      }
    }

    public static class Math
    {
      public static Vector3 MoveSpeedPositionEnter(Vector3[] points, float moveSpeed)
      {
        for (int index = 0; index < points.Length - 1; ++index)
        {
          Vector3 point1 = points[index];
          Vector3 point2 = points[index + 1];
          float num = Vector3.Distance(point1, point2);
          if ((double) moveSpeed <= (double) num)
            return Vector3.Lerp(point1, point2, Mathf.InverseLerp(num, 0.0f, num - moveSpeed));
          moveSpeed -= num;
        }
        return points[points.Length - 1];
      }

      public static int MinDistanceRouteIndex(Vector3[] route, Vector3 pos)
      {
        int num1 = -1;
        float num2 = float.MaxValue;
        for (int index = 0; index < route.Length; ++index)
        {
          Vector3 vector3 = Vector3.op_Subtraction(route[index], pos);
          float sqrMagnitude = ((Vector3) ref vector3).get_sqrMagnitude();
          if ((double) num2 > (double) sqrMagnitude)
          {
            num2 = sqrMagnitude;
            num1 = index;
          }
        }
        return num1;
      }

      public static void TargetFor(Transform from, Transform target, bool isHeight = false)
      {
        Vector3 position = target.get_position();
        if (!isHeight)
          position.y = from.get_position().y;
        from.LookAt(position);
      }

      public static float NewtonMethod(
        Utils.Math.Func func,
        Utils.Math.Func derive,
        float initX,
        int maxLoop)
      {
        float x = initX;
        for (int index = 0; index < maxLoop; ++index)
        {
          float num = func(x);
          if ((double) num >= 9.99999974737875E-06 || (double) num <= -9.99999974737875E-06)
            x -= num / derive(x);
          else
            break;
        }
        return x;
      }

      public static class Cast
      {
        public static Vector2 ToVector2(float[] f)
        {
          return new Vector2(f[0], f[1]);
        }

        public static Vector3 ToVector3(float[] f)
        {
          return new Vector3(f[0], f[1], f[2]);
        }

        public static float[] ToArray(Vector2 v2)
        {
          return new float[2]{ (float) v2.x, (float) v2.y };
        }

        public static float[] ToArray(Vector3 v3)
        {
          return new float[3]
          {
            (float) v3.x,
            (float) v3.y,
            (float) v3.z
          };
        }

        public static string ToString(Vector3 v3)
        {
          return string.Format("({0},{1},{2})", (object) (float) v3.x, (object) (float) v3.y, (object) (float) v3.z);
        }
      }

      public static class Fuzzy
      {
        public static float Grade(float _value, float _x0, float _x1)
        {
          float num = _value;
          return (double) num > (double) _x0 ? ((double) num < (double) _x1 ? (float) ((double) num / ((double) _x1 - (double) _x0) - (double) _x0 / ((double) _x1 - (double) _x0)) : 1f) : 0.0f;
        }

        public static float ReverseGrade(float _value, float _x0, float _x1)
        {
          float num = _value;
          return (double) num > (double) _x0 ? ((double) num < (double) _x1 ? (float) (-(double) num / ((double) _x1 - (double) _x0) + (double) _x1 / ((double) _x1 - (double) _x0)) : 0.0f) : 1f;
        }

        public static float Triangle(float _value, float _x0, float _x1, float _x2)
        {
          float num = _value;
          return (double) num > (double) _x0 ? ((double) num != (double) _x1 ? ((double) num <= (double) _x0 || (double) num >= (double) _x1 ? (float) (-(double) num / ((double) _x2 - (double) _x1) + (double) _x2 / ((double) _x2 - (double) _x1)) : (float) ((double) num / ((double) _x1 - (double) _x0) - (double) _x0 / ((double) _x1 - (double) _x0))) : 1f) : 0.0f;
        }

        public static float Trapezoid(float _value, float _x0, float _x1, float _x2, float _x3)
        {
          float num = _value;
          return (double) num > (double) _x0 ? ((double) num < (double) _x1 || (double) num > (double) _x2 ? ((double) num <= (double) _x0 || (double) num >= (double) _x1 ? (float) (-(double) num / ((double) _x3 - (double) _x2) + (double) _x3 / ((double) _x3 - (double) _x2)) : (float) ((double) num / ((double) _x1 - (double) _x0) - (double) _x0 / ((double) _x1 - (double) _x0))) : 1f) : 0.0f;
        }

        public static float AND(float _a, float _b)
        {
          return Mathf.Min(_a, _b);
        }

        public static float OR(float _a, float _b)
        {
          return Mathf.Max(_a, _b);
        }

        public static float NOT(float _a)
        {
          return 1f - _a;
        }
      }

      public delegate float Func(float x);
    }

    public static class Mesh
    {
      private static float ToRad(float angle, int index)
      {
        return (float) ((double) angle * (double) index * (System.Math.PI / 180.0));
      }

      public static IEnumerable<Vector3> CalculateVertices(int verticesNum)
      {
        if (verticesNum <= 0)
          return Enumerable.Empty<Vector3>();
        float angle = 360f / (float) verticesNum;
        return Enumerable.Range(0, verticesNum).Select<int, float>((Func<int, float>) (i => Utils.Mesh.ToRad(angle, i))).Select<float, Vector3>((Func<float, Vector3>) (r => new Vector3(Mathf.Sin(r), Mathf.Cos(r))));
      }

      public static void Create(GameObject go, IEnumerable<Vector3> vertices)
      {
        if (vertices == null || vertices.Count<Vector3>() < 3)
          return;
        MeshFilter meshFilter = (MeshFilter) go.GetComponent<MeshFilter>();
        if (Object.op_Equality((Object) meshFilter, (Object) null))
          meshFilter = (MeshFilter) go.AddComponent<MeshFilter>();
        UnityEngine.Mesh mesh = meshFilter.get_mesh();
        mesh.Clear();
        mesh.set_vertices(vertices.ToArray<Vector3>());
        int[] numArray = new int[(mesh.get_vertices().Length - 2) * 3];
        int index = 0;
        int num = 0;
        while (index < numArray.Length)
        {
          numArray[index] = 0;
          numArray[index + 1] = num + 1;
          numArray[index + 2] = num + 2;
          index += 3;
          ++num;
        }
        mesh.set_triangles(numArray);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        meshFilter.set_sharedMesh(mesh);
      }

      public static void RendererSet(GameObject go, Color color, string matName = "Sprites-Default.mat")
      {
        MeshRenderer meshRenderer = (MeshRenderer) go.GetComponent<MeshRenderer>();
        if (Object.op_Equality((Object) meshRenderer, (Object) null))
          meshRenderer = (MeshRenderer) go.AddComponent<MeshRenderer>();
        ((Renderer) meshRenderer).set_material((Material) Resources.GetBuiltinResource<Material>(matName));
        ((Renderer) meshRenderer).get_material().set_color(color);
      }
    }

    public static class NavMesh
    {
      public static GameObject CreateDrawObject(Color? color)
      {
        NavMeshTriangulation triangulation = UnityEngine.AI.NavMesh.CalculateTriangulation();
        UnityEngine.Mesh mesh = new UnityEngine.Mesh();
        mesh.set_vertices((Vector3[]) triangulation.vertices);
        mesh.set_triangles((int[]) triangulation.indices);
        GameObject gameObject = new GameObject("NavMeshDrawObject");
        ((MeshFilter) gameObject.AddComponent<MeshFilter>()).set_mesh(mesh);
        ((Renderer) gameObject.AddComponent<MeshRenderer>()).get_material().set_color(!color.HasValue ? Color.get_white() : color.Value);
        return gameObject;
      }

      public static bool GetRandomPosition(
        Vector3 center,
        out Vector3 result,
        float range = 10f,
        int count = 30,
        float maxDistance = 1f,
        bool isY = true,
        int area = -1)
      {
        Func<Vector3> func = !isY ? (Func<Vector3>) (() =>
        {
          Vector2 vector2 = Vector2.op_Multiply(Random.get_insideUnitCircle(), range);
          return new Vector3((float) vector2.x, 0.0f, (float) vector2.y);
        }) : (Func<Vector3>) (() => Vector3.op_Multiply(Random.get_insideUnitSphere(), range));
        for (int index = 0; index < count; ++index)
        {
          NavMeshHit navMeshHit;
          if (UnityEngine.AI.NavMesh.SamplePosition(Vector3.op_Addition(center, func()), ref navMeshHit, maxDistance, area))
          {
            result = ((NavMeshHit) ref navMeshHit).get_position();
            return true;
          }
        }
        result = Vector3.get_zero();
        return false;
      }
    }

    public static class Network
    {
      public static string MACAddress
      {
        get
        {
          string empty = string.Empty;
          NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
          if (networkInterfaces != null)
          {
            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
              PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
              if (physicalAddress != null && physicalAddress.GetAddressBytes().Length == 6)
              {
                string str = physicalAddress.ToString();
                empty += str;
                Debug.Log((object) str);
              }
            }
          }
          return empty;
        }
      }
    }

    public static class ProbabilityCalclator
    {
      public static bool DetectFromPercent(float percent)
      {
        int num1 = 0;
        string str = percent.ToString();
        if (str.IndexOf(".") > 0)
          num1 = str.Split('.')[1].Length;
        int num2 = (int) Mathf.Pow(10f, (float) num1);
        return Random.Range(0, 100 * num2) < (int) ((double) num2 * (double) percent);
      }

      public static T DetermineFromDict<T>(Dictionary<T, int> targetDict)
      {
        if (!targetDict.Any<KeyValuePair<T, int>>())
          return default (T);
        float num = Random.Range(0.0f, (float) targetDict.Values.Sum());
        foreach (KeyValuePair<T, int> keyValuePair in targetDict)
        {
          num -= (float) keyValuePair.Value;
          if ((double) num < 0.0)
            return keyValuePair.Key;
        }
        Debug.LogError((object) "抽選ができませんでした");
        return targetDict.Keys.First<T>();
      }

      public static T DetermineFromDict<T>(Dictionary<T, float> targetDict)
      {
        if (!targetDict.Any<KeyValuePair<T, float>>())
          return default (T);
        float num = Random.Range(0.0f, targetDict.Values.Sum());
        foreach (KeyValuePair<T, float> keyValuePair in targetDict)
        {
          num -= keyValuePair.Value;
          if ((double) num < 0.0)
            return keyValuePair.Key;
        }
        Debug.LogError((object) "抽選ができませんでした");
        return targetDict.Keys.First<T>();
      }
    }

    public static class String
    {
      public static string GetPropertyName<T>(Expression<Func<T>> e)
      {
        return ((MemberExpression) e.Body).Member.Name;
      }
    }

    public static class Type
    {
      public static System.Type Get(string dllName, string typeName)
      {
        return Assembly.Load(dllName).GetType(typeName);
      }

      public static System.Type Get(string typeName)
      {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (type.Name == typeName)
              return type;
          }
        }
        return (System.Type) null;
      }

      public static System.Type GetFull(string typeFullName)
      {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (type.FullName == typeFullName)
              return type;
          }
        }
        return (System.Type) null;
      }

      public static string GetAssemblyQualifiedName(string typeName)
      {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (type.Name == typeName)
              return type.AssemblyQualifiedName;
          }
        }
        return (string) null;
      }

      public static string GetFullAssemblyQualifiedName(string typeFullName)
      {
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
          foreach (System.Type type in assembly.GetTypes())
          {
            if (type.FullName == typeFullName)
              return type.AssemblyQualifiedName;
          }
        }
        return (string) null;
      }

      public static FieldInfo[] GetPublicFields(System.Type type)
      {
        return type.GetFields(BindingFlags.Instance | BindingFlags.Public);
      }

      public static PropertyInfo[] GetPublicProperties(System.Type type)
      {
        return type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
      }
    }

    public static class uGUI
    {
      public static bool isMouseHit
      {
        get
        {
          return Utils.uGUI.HitList(Input.get_mousePosition()).Count > 0;
        }
      }

      public static List<RaycastResult> HitList(Vector3 position)
      {
        List<RaycastResult> raycastResultList1 = new List<RaycastResult>();
        EventSystem current = EventSystem.get_current();
        PointerEventData pointerEventData1 = new PointerEventData(EventSystem.get_current());
        pointerEventData1.set_position(Vector2.op_Implicit(position));
        PointerEventData pointerEventData2 = pointerEventData1;
        List<RaycastResult> raycastResultList2 = raycastResultList1;
        current.RaycastAll(pointerEventData2, raycastResultList2);
        return raycastResultList1;
      }
    }

    public static class Value
    {
      public static int Check(int len, Func<int, bool> func)
      {
        int num = -1;
        do
          ;
        while (++num < len && !func(num));
        return num >= len ? -1 : num;
      }
    }
  }
}
