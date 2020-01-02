// Decompiled with JetBrains decompiler
// Type: AIProject.HPointDataSerializedValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\\AI_Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;
using UnityEx;

namespace AIProject
{
  [MessagePackObject(false)]
  public class HPointDataSerializedValue
  {
    public HPointDataSerializedValue()
    {
    }

    public HPointDataSerializedValue(AutoHPointData data)
    {
      this.HPointDataAreaID = new Dictionary<string, List<int>>();
      this.HPointDataPos = new Dictionary<string, List<Vector3>>();
      using (Dictionary<string, List<ValueTuple<int, Vector3>>>.Enumerator enumerator1 = data.Points.GetEnumerator())
      {
        while (enumerator1.MoveNext())
        {
          KeyValuePair<string, List<ValueTuple<int, Vector3>>> current1 = enumerator1.Current;
          this.HPointDataAreaID.Add(current1.Key, new List<int>());
          this.HPointDataPos.Add(current1.Key, new List<Vector3>());
          using (List<ValueTuple<int, Vector3>>.Enumerator enumerator2 = current1.Value.GetEnumerator())
          {
            while (enumerator2.MoveNext())
            {
              ValueTuple<int, Vector3> current2 = enumerator2.Current;
              this.HPointDataAreaID[current1.Key].Add((int) current2.Item1);
              this.HPointDataPos[current1.Key].Add((Vector3) current2.Item2);
            }
          }
        }
      }
    }

    [Key(0)]
    public Dictionary<string, List<int>> HPointDataAreaID { get; set; }

    [Key(1)]
    public Dictionary<string, List<Vector3>> HPointDataPos { get; set; }

    public static Task SaveAsync(string path, AutoHPointData data)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async0 saveAsyncCAsync0;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync0.path = path;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync0.data = data;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync0.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync0.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async0>((M0&) ref saveAsyncCAsync0);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static void Save(string path, byte[] bytes)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
        HPointDataSerializedValue.Save((Stream) fileStream, bytes);
    }

    public static void Save(Stream stream, byte[] bytes)
    {
      using (BinaryWriter writer = new BinaryWriter(stream))
        HPointDataSerializedValue.Save(writer, bytes);
    }

    public static void Save(BinaryWriter writer, byte[] bytes)
    {
      writer.Write(bytes);
    }

    public static void Load(string path, ref AutoHPointData data)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        HPointDataSerializedValue.Load((Stream) fileStream, ref data);
    }

    public static void Load(Stream stream, ref AutoHPointData data)
    {
      using (BinaryReader reader = new BinaryReader(stream))
        HPointDataSerializedValue.Load(reader, ref data);
    }

    public static void Load(BinaryReader reader, ref AutoHPointData data)
    {
      HPointDataSerializedValue dataSerializedValue = (HPointDataSerializedValue) MessagePackSerializer.Deserialize<HPointDataSerializedValue>(reader.ReadBytes((int) reader.BaseStream.Length));
      data.Allocation(dataSerializedValue.HPointDataAreaID, dataSerializedValue.HPointDataPos);
    }

    public static Task SaveAsync(string path, byte[] bytes)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async1 saveAsyncCAsync1;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync1.path = path;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync1.bytes = bytes;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync1.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync1.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async1>((M0&) ref saveAsyncCAsync1);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task SaveAsync(Stream stream, byte[] bytes)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async2 saveAsyncCAsync2;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync2.stream = stream;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync2.bytes = bytes;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync2.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync2.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async2>((M0&) ref saveAsyncCAsync2);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task SaveAsync(BinaryWriter writer, byte[] bytes)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async3 saveAsyncCAsync3;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync3.writer = writer;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync3.bytes = bytes;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync3.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync3.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<HPointDataSerializedValue.\u003CSaveAsync\u003Ec__async3>((M0&) ref saveAsyncCAsync3);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task<HPointDataSerializedValue> LoadAsync(string path)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CLoadAsync\u003Ec__async4 loadAsyncCAsync4;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync4.path = path;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync4.\u0024builder = AsyncTaskMethodBuilder<HPointDataSerializedValue>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<HPointDataSerializedValue> local = ref loadAsyncCAsync4.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<HPointDataSerializedValue>) ref local).Start<HPointDataSerializedValue.\u003CLoadAsync\u003Ec__async4>((M0&) ref loadAsyncCAsync4);
      return ((AsyncTaskMethodBuilder<HPointDataSerializedValue>) ref local).get_Task();
    }

    public static Task<HPointDataSerializedValue> LoadAsync(
      Stream stream)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CLoadAsync\u003Ec__async5 loadAsyncCAsync5;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync5.stream = stream;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync5.\u0024builder = AsyncTaskMethodBuilder<HPointDataSerializedValue>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<HPointDataSerializedValue> local = ref loadAsyncCAsync5.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<HPointDataSerializedValue>) ref local).Start<HPointDataSerializedValue.\u003CLoadAsync\u003Ec__async5>((M0&) ref loadAsyncCAsync5);
      return ((AsyncTaskMethodBuilder<HPointDataSerializedValue>) ref local).get_Task();
    }

    public static Task<HPointDataSerializedValue> LoadAsync(
      BinaryReader reader)
    {
      // ISSUE: variable of a compiler-generated type
      HPointDataSerializedValue.\u003CLoadAsync\u003Ec__async6 loadAsyncCAsync6;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync6.reader = reader;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync6.\u0024builder = AsyncTaskMethodBuilder<HPointDataSerializedValue>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<HPointDataSerializedValue> local = ref loadAsyncCAsync6.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<HPointDataSerializedValue>) ref local).Start<HPointDataSerializedValue.\u003CLoadAsync\u003Ec__async6>((M0&) ref loadAsyncCAsync6);
      return ((AsyncTaskMethodBuilder<HPointDataSerializedValue>) ref local).get_Task();
    }
  }
}
