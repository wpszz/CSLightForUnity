using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf;
using ProtoBuf.Meta;
using CSLE;

public partial class CSLightMng : MonoBehaviour
{
    protected BetterList<CLS_Content.Value> m_emptyParams = new BetterList<CLS_Content.Value>();

    void GetSortMembers(SInstance sInstance, out List<CLS_Content.Value> values, out List<string> keywords)
    {
        if (sInstance.type.sortProtoFieldKeys == null)
        {
            List<string> tempKeys = new List<string>();
            foreach (var pair in sInstance.type.members)
            {
                if (pair.Value.sortIndex > 0)
                {
                    for (int i = tempKeys.Count; i < pair.Value.sortIndex; i++)
                    {
                        tempKeys.Add("");
                    }
                    tempKeys[pair.Value.sortIndex - 1] = pair.Key;
                }
            }

            sInstance.type.sortProtoFieldKeys = new List<string>();
            foreach (string key in tempKeys)
            {
                if (!string.IsNullOrEmpty(key))
                    sInstance.type.sortProtoFieldKeys.Add(key);
            }
        }

        keywords = sInstance.type.sortProtoFieldKeys;
        values = new List<CLS_Content.Value>();
        foreach (string key in keywords)
        {
            values.Add(sInstance.member[key]);
        }
    }

    Type GetItemType(Type type)
    {
        if (type.IsArray)
        {
            if (type.GetArrayRank() != 1)
            {
                throw new NotSupportedException("Multi-dimension arrays are supported");
            }
            Type itemType = type.GetElementType();
            if (itemType == typeof(byte))
            {
                return null;    // byte[] 不作为数组处理
            }
            else
            {
                return itemType;
            }
        }
        return TypeModel.GetListItemType(RuntimeTypeModel.Default, type); ;
    }

    public void Serialize(Stream destination, object clsObj)
    {
        SInstance sInstance = clsObj as SInstance;
        if (sInstance == null)
        {
            throw new ArgumentNullException("无效CSLight脚本对象: " + clsObj);
        }

        using (ProtoWriter writer = new ProtoWriter(destination, null, null))
        {
            WriteSInstance(writer, sInstance);
            writer.Close();
        }
    }

    void WriteSInstance(ProtoWriter writer, SInstance sInstance)
    {
        List<CLS_Content.Value> values;
        List<string> keywords;
        GetSortMembers(sInstance, out values, out keywords);

        // 写入流
        for (int i = 0, count = values.Count; i < count; i++)
        {
            int fieldNumber = i + 1;
            Type memberT = values[i].type;
            object memberV = values[i].value;

            if (memberV == null)
                continue;

            if (memberT == null)
                memberT = typeof(SInstance);

            Type itemType = GetItemType(memberT);
            if (itemType != null)
            {
                // 数组判断
                if (memberT.IsArray)
                {
                    IList arr = (IList)memberV;
                    for (int j = 0, len = arr.Count; j < len; j++)
                    {
                        WriteField(writer, arr[j], itemType, fieldNumber);
                    }
                }
                // 列表判断
                else
                {
                    IEnumerable list = (IEnumerable)memberV;
                    foreach (object subItem in list)
                    {
                        WriteField(writer, subItem, itemType, fieldNumber);
                    }
                }
            }
            else
            {
                WriteField(writer, memberV, memberT, fieldNumber);
            }
        }
    }

    void WriteField(ProtoWriter writer, object memberV, Type memberT, int fieldNumber)
    {
        if (memberT == typeof(int))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteInt32((int)memberV, writer);
        }
        else if (memberT == typeof(uint))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteUInt32((uint)memberV, writer);
        }
        else if (memberT == typeof(bool))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteBoolean((bool)memberV, writer);
        }
        else if (memberT == typeof(byte))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteByte((byte)memberV, writer);
        }
        else if (memberT == typeof(sbyte))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteSByte((sbyte)memberV, writer);
        }
        else if (memberT == typeof(float))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteSingle((float)memberV, writer);
        }
        else if (memberT == typeof(double))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteDouble((double)memberV, writer);
        }
        else if (memberT == typeof(short))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteInt16((short)memberV, writer);
        }
        else if (memberT == typeof(ushort))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteUInt16((ushort)memberV, writer);
        }
        else if (memberT == typeof(long))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteInt64((long)memberV, writer);
        }
        else if (memberT == typeof(ulong))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.Variant, writer);
            ProtoWriter.WriteUInt64((ulong)memberV, writer);
        }
        else if (memberT == typeof(string))
        {
            string str = (string)memberV;
            if (!string.IsNullOrEmpty(str))
            {
                ProtoWriter.WriteFieldHeader(fieldNumber, WireType.String, writer);
                ProtoWriter.WriteString(str, writer);
            }
        }
        else if (memberT == typeof(byte[]))
        {
            ProtoWriter.WriteFieldHeader(fieldNumber, WireType.String, writer);
            ProtoWriter.WriteBytes((byte[])memberV, writer);
        }
        else if (memberT == typeof(SInstance))
        {
            SInstance subSinstance = (SInstance)memberV;
            if (subSinstance != null)
            {
                ProtoWriter.WriteFieldHeader(fieldNumber, WireType.String, writer);
                SubItemToken st = ProtoWriter.StartSubItem(null, writer);
                WriteSInstance(writer, subSinstance);
                ProtoWriter.EndSubItem(st, writer);
            }
        }
        else
        {
            throw new NotImplementedException("未实现类型: " + memberT);
        }
    }

    public object Deserialize(Stream source, string className)
    {
        CLS_Type_Class sClass = m_clsEnv.GetTypeByKeywordQuiet(className) as CLS_Type_Class;
        if (sClass == null)
        {
            throw new NotImplementedException("未实现类型: " + className);
        }

        if (!sClass.compiled)
            RuntimeCompilerClass(className);

        CLS_Content.Value retVal = (sClass.function as SType).New(m_clsContent, m_emptyParams);
        SInstance sInstance = (SInstance)retVal.value;

        ProtoReader reader = null;
        try
        {
            reader = ProtoReader.Create(source, null, null, ProtoReader.TO_EOF);
            ReadSInstance(reader, sInstance, m_clsEnv);
            reader.CheckFullyConsumed();
            return sInstance;
        }
        finally
        {
            ProtoReader.Recycle(reader);
        }
    }

    void ReadSInstance(ProtoReader reader, SInstance sInstance, CLS_Environment environment)
    {
        List<CLS_Content.Value> values;
        List<string> keywords;
        GetSortMembers(sInstance, out values, out keywords);

        int fieldNumber = 0;
        while ((fieldNumber = reader.ReadFieldHeader()) > 0)
        {
            Type memberT = values[fieldNumber - 1].type;
            CLS_Content.Value memberV = values[fieldNumber - 1];
            string sClassName = keywords[fieldNumber - 1];

            if (memberT == null)
            {
                memberT = typeof(SInstance);
                sClassName = ((SType)memberV.type).Name;
            }

            Type itemType = GetItemType(memberT);
            if (itemType != null)
            {
                sClassName = sInstance.type.members[sClassName].type.keyword;

                // 数组判断
                if (memberT.IsArray)
                {
                    string itemClass = sClassName.Substring(0, sClassName.Length - 2);  // 从 xxx[] 中提取xxx
                    BasicList list = new BasicList();
                    do
                    {
                        list.Add(ReadField(reader, itemType, itemClass, environment));
                    } while (reader.TryReadFieldHeader(fieldNumber));
                    Array result = Array.CreateInstance(itemType, list.Count);
                    list.CopyTo(result, 0);
                    memberV.value = result;
                }
                // 列表判断
                else
                {
                    string itemClass = sClassName.Substring(5, sClassName.Length - 6);  // 从 List<xxx> 中提取xxx
                    ICLS_Type iType = environment.GetTypeByKeywordQuiet(sClassName);
                    CLS_Content content = CLS_Content.NewContent(environment);
                    memberV.value = iType.function.New(content, m_emptyParams).value;
                    CLS_Content.PoolContent(content);
                    IList list = (IList)memberV.value;
                    do
                    {
                        list.Add(ReadField(reader, itemType, itemClass, environment));
                    } while (reader.TryReadFieldHeader(fieldNumber));
                }
            }
            else
            {
                memberV.value = ReadField(reader, memberT, sClassName, environment);
            }
        }
    }

    object ReadField(ProtoReader reader, Type memberT, string sClassName, CLS_Environment environment)
    {
        if (memberT == typeof(int))
        {
            return reader.ReadInt32();
        }
        else if (memberT == typeof(uint))
        {
            return reader.ReadUInt32();
        }
        else if (memberT == typeof(bool))
        {
            return reader.ReadBoolean();
        }
        else if (memberT == typeof(byte))
        {
            return reader.ReadByte();
        }
        else if (memberT == typeof(sbyte))
        {
            return reader.ReadSByte();
        }
        else if (memberT == typeof(float))
        {
            return reader.ReadSingle();
        }
        else if (memberT == typeof(double))
        {
            return reader.ReadDouble();
        }
        else if (memberT == typeof(short))
        {
            return reader.ReadInt16();
        }
        else if (memberT == typeof(ushort))
        {
            return reader.ReadUInt16();
        }
        else if (memberT == typeof(long))
        {
            return reader.ReadInt64();
        }
        else if (memberT == typeof(ulong))
        {
            return reader.ReadUInt64();
        }
        else if (memberT == typeof(string))
        {
            return reader.ReadString();
        }
        else if (memberT == typeof(byte[]))
        {
            return ProtoReader.AppendBytes(null, reader);
        }
        else if (memberT == typeof(SInstance))
        {
            SubItemToken st = ProtoReader.StartSubItem(reader);
            CLS_Type_Class sClass = environment.GetTypeByKeywordQuiet(sClassName) as CLS_Type_Class;
            if (!sClass.compiled)
                RuntimeCompilerClass(sClassName);
            CLS_Content content = CLS_Content.NewContent(environment);
            CLS_Content.Value retVal = sClass.function.New(content, m_emptyParams);
            CLS_Content.PoolContent(content);
            SInstance sInstance = (SInstance)retVal.value;
            ReadSInstance(reader, sInstance, environment);
            ProtoReader.EndSubItem(st, reader);
            return sInstance;
        }
        else
        {
            throw new NotImplementedException("未实现类型: " + memberT);
        }
    }
}
