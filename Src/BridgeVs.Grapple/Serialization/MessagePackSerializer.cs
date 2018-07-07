﻿#region License
// Copyright (c) 2013 - 2018 Coding Adventures
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

using System;
using System.IO;

namespace BridgeVs.Grapple.Serialization
{
    public class MessagePackSerializer : IServiceSerializer
    {
        public T Deserialize<T>(Stream aStream)
        {
            return (T)MessagePack.MessagePackSerializer.Typeless.Deserialize(aStream);
        }

        public T Deserialize<T>(byte[] objToDeserialize)
        {
            return (T)Deserialize(objToDeserialize);
        }

        public object Deserialize(byte[] objToDeserialize, Type type = null)
        {
            return MessagePack.MessagePackSerializer.Typeless.Deserialize(objToDeserialize);
        }

        public void Serialize<T>(Stream aStream, T objToSerialize)
        {
            byte[] serializedObj = Serialize(objToSerialize);
            aStream.Write(serializedObj, 0, serializedObj.Length);
        }

        public byte[] Serialize<T>(T objToSerialize)
        {
            return MessagePack.MessagePackSerializer.Typeless.Serialize(objToSerialize);
        }

        public override string ToString()
        {
            return "Message Pack Serializer";
        }
    }
}
