// Decompiled with JetBrains decompiler
// Type: AIProject.WayPointDataSerializedValue
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 3FB45F06-6483-4AD8-97CB-A1C42CCDD6C3
// Assembly location: E:\GAME\illusion_AI\PluginDev\反编译阅读\AI_Assembly-CSharp.dll

using MessagePack;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

namespace AIProject
{
  [MessagePackObject(false)]
  public class WayPointDataSerializedValue
  {
    public WayPointDataSerializedValue()
    {
    }

    public WayPointDataSerializedValue(NavMeshWayPointData data)
    {
      this.MapID = data.MapID;
      this.WayData = ((IEnumerable<Vector3>) data.Points).ToArray<Vector3>();
    }

    [Key(0)]
    public int MapID { get; set; }

    [Key(1)]
    public Vector3[] WayData { get; set; }

    public static Task SaveAsync(string path, NavMeshWayPointData data)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async0 saveAsyncCAsync0;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync0.path = path;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync0.data = data;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync0.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync0.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async0>((M0&) ref saveAsyncCAsync0);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static void Save(string path, byte[] bytes)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Write))
        WayPointDataSerializedValue.Save((Stream) fileStream, bytes);
    }

    public static void Save(Stream stream, byte[] bytes)
    {
      using (BinaryWriter writer = new BinaryWriter(stream))
        WayPointDataSerializedValue.Save(writer, bytes);
    }

    public static void Save(BinaryWriter writer, byte[] bytes)
    {
      writer.Write(bytes);
    }

    public static void Load(string path, ref NavMeshWayPointData data)
    {
      using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        WayPointDataSerializedValue.Load((Stream) fileStream, ref data);
    }

    public static void Load(Stream stream, ref NavMeshWayPointData data)
    {
      using (BinaryReader reader = new BinaryReader(stream))
        WayPointDataSerializedValue.Load(reader, ref data);
    }

    public static void Load(BinaryReader reader, ref NavMeshWayPointData data)
    {
      WayPointDataSerializedValue dataSerializedValue = (WayPointDataSerializedValue) MessagePackSerializer.Deserialize<WayPointDataSerializedValue>(reader.ReadBytes((int) reader.BaseStream.Length));
      data.MapID = dataSerializedValue.MapID;
      data.Allocation(dataSerializedValue.WayData);
    }

    public static Task SaveAsync(string path, byte[] bytes)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async1 saveAsyncCAsync1;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync1.path = path;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync1.bytes = bytes;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync1.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync1.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async1>((M0&) ref saveAsyncCAsync1);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task SaveAsync(Stream stream, byte[] bytes)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async2 saveAsyncCAsync2;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync2.stream = stream;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync2.bytes = bytes;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync2.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync2.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async2>((M0&) ref saveAsyncCAsync2);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task SaveAsync(BinaryWriter writer, byte[] bytes)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async3 saveAsyncCAsync3;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync3.writer = writer;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync3.bytes = bytes;
      // ISSUE: reference to a compiler-generated field
      saveAsyncCAsync3.\u0024builder = AsyncTaskMethodBuilder.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder local = ref saveAsyncCAsync3.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder) ref local).Start<WayPointDataSerializedValue.\u003CSaveAsync\u003Ec__async3>((M0&) ref saveAsyncCAsync3);
      return ((AsyncTaskMethodBuilder) ref local).get_Task();
    }

    public static Task<WayPointDataSerializedValue> LoadAsync(
      string path)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CLoadAsync\u003Ec__async4 loadAsyncCAsync4;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync4.path = path;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync4.\u0024builder = AsyncTaskMethodBuilder<WayPointDataSerializedValue>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<WayPointDataSerializedValue> local = ref loadAsyncCAsync4.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<WayPointDataSerializedValue>) ref local).Start<WayPointDataSerializedValue.\u003CLoadAsync\u003Ec__async4>((M0&) ref loadAsyncCAsync4);
      return ((AsyncTaskMethodBuilder<WayPointDataSerializedValue>) ref local).get_Task();
    }

    public static Task<WayPointDataSerializedValue> LoadAsync(
      Stream stream)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CLoadAsync\u003Ec__async5 loadAsyncCAsync5;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync5.stream = stream;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync5.\u0024builder = AsyncTaskMethodBuilder<WayPointDataSerializedValue>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<WayPointDataSerializedValue> local = ref loadAsyncCAsync5.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<WayPointDataSerializedValue>) ref local).Start<WayPointDataSerializedValue.\u003CLoadAsync\u003Ec__async5>((M0&) ref loadAsyncCAsync5);
      return ((AsyncTaskMethodBuilder<WayPointDataSerializedValue>) ref local).get_Task();
    }

    public static Task<WayPointDataSerializedValue> LoadAsync(
      BinaryReader reader)
    {
      // ISSUE: variable of a compiler-generated type
      WayPointDataSerializedValue.\u003CLoadAsync\u003Ec__async6 loadAsyncCAsync6;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync6.reader = reader;
      // ISSUE: reference to a compiler-generated field
      loadAsyncCAsync6.\u0024builder = AsyncTaskMethodBuilder<WayPointDataSerializedValue>.Create();
      // ISSUE: reference to a compiler-generated field
      ref AsyncTaskMethodBuilder<WayPointDataSerializedValue> local = ref loadAsyncCAsync6.\u0024builder;
      // ISSUE: cast to a reference type
      ((AsyncTaskMethodBuilder<WayPointDataSerializedValue>) ref local).Start<WayPointDataSerializedValue.\u003CLoadAsync\u003Ec__async6>((M0&) ref loadAsyncCAsync6);
      return ((AsyncTaskMethodBuilder<WayPointDataSerializedValue>) ref local).get_Task();
    }
  }
}
