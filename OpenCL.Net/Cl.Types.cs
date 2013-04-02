﻿#region License and Copyright Notice
// Copyright (c) 2010 Ananth B.
// All rights reserved.
// 
// The contents of this file are made available under the terms of the
// Eclipse Public License v1.0 (the "License") which accompanies this
// distribution, and is available at the following URL:
// http://www.opensource.org/licenses/eclipse-1.0.php
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either expressed or implied. See the License for
// the specific language governing rights and limitations under the License.
// 
// By using this software in any fashion, you are agreeing to be bound by the
// terms of the License.
#endregion

using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace OpenCL.Net
{
    public static partial class Cl
    {
        public interface IHandle
        {
        }

        internal interface IHandleData
        {
            IntPtr Handle
            {
                get;
            }
        }

        internal interface IRefCountedHandle : IHandle, IHandleData, IDisposable
        {
            void Retain();
        }

        public static readonly InvalidHandle Invalid = new InvalidHandle();

        [StructLayout(LayoutKind.Sequential)]
        public struct InvalidHandle
        { 
            public static bool operator ==(IHandle handle, InvalidHandle invalidHandle)
            {
                return ((IHandleData)handle).Handle == IntPtr.Zero;
            }

            public static bool operator !=(IHandle handle, InvalidHandle invalidHandle)
            {
                return ((IHandleData)handle).Handle != IntPtr.Zero;
            }

            public static bool operator ==(InvalidHandle invalidHandle, IHandle handle)
            {
                return ((IHandleData)handle).Handle == IntPtr.Zero;
            }
            public static bool operator !=(InvalidHandle invalidHandle, IHandle handle)
            {
                return ((IHandleData)handle).Handle != IntPtr.Zero;
            }

            public override bool Equals(object obj)
            {
                return obj is IHandle ? 
                    ((IHandleData)obj).Handle == IntPtr.Zero :
                    false;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Platform : IHandle, IHandleData
        {
            private readonly IntPtr _handle;

            internal Platform(IntPtr handle)
            {
                _handle = handle;
            }

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            public static implicit operator IntPtr(Platform platform)
            {
                return platform._handle;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Device : IHandle, IHandleData
        {
            private readonly IntPtr _handle;

            internal Device(IntPtr handle)
            {
                _handle = handle;
            }

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct ImageFormat
        {
            [MarshalAs(UnmanagedType.U4)]
            private ChannelOrder _channelOrder;
            [MarshalAs(UnmanagedType.U4)]
            private ChannelType _channelType;

            public ImageFormat(ChannelOrder channelOrder, ChannelType channelType)
            {
                _channelOrder = channelOrder;
                _channelType = channelType;
            }

            public ChannelOrder ChannelOrder
            {
                get
                {
                    return _channelOrder;
                }
                set
                {
                    _channelOrder = value;
                }
            }

            public ChannelType ChannelType
            {
                get
                {
                    return _channelType;
                }
                set
                {
                    _channelType = value;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ContextProperty
        {
            private static readonly ContextProperty _zero = new ContextProperty(0);

            private readonly uint _propertyName;
            private readonly IntPtr _propertyValue;

            public ContextProperty(ContextProperties property, IntPtr value)
            {
                _propertyName = (uint)property;
                _propertyValue = value;
            }

            public ContextProperty(ContextProperties property)
            {
                _propertyName = (uint)property;
                _propertyValue = IntPtr.Zero;
            }

            public static ContextProperty Zero
            {
                get
                {
                    return _zero;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Context : IRefCountedHandle
        {
            private readonly IntPtr _handle;

            internal Context(IntPtr handle)
            {
                _handle = handle;
            }

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainContext(this);
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseContext(this);
            }

            #endregion

            public static readonly Context Zero = new Context(IntPtr.Zero);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Mem : IRefCountedHandle, IEquatable<Cl.Mem>
        {
            private readonly IntPtr _handle;

            internal Mem(IntPtr handle)
            {
                _handle = handle;
            }

            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainMemObject(this);
            }

            #endregion

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseMemObject(this);
            }

            #endregion

            public bool Equals(Mem other)
            {
                return _handle.ToInt64() == other._handle.ToInt64();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Program : IRefCountedHandle
        {
            private readonly IntPtr _handle;

            internal Program(IntPtr handle)
            {
                _handle = handle;
            }

            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainProgram(this);
            }

            #endregion

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseProgram(this);
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CommandQueue : IRefCountedHandle
        {
            private readonly IntPtr _handle;

            internal CommandQueue(IntPtr handle)
            {
                _handle = handle;
            }
            
            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainCommandQueue(this);
            }

            #endregion

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseCommandQueue(this);
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Kernel : IRefCountedHandle
        {
            private readonly IntPtr _handle;

            internal Kernel(IntPtr handle)
            {
                _handle = handle;
            }

            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainKernel(this);
            }

            #endregion

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseKernel(this);
            }

            #endregion

            public static readonly Cl.Kernel Zero = new Cl.Kernel();

            public static bool operator ==(Cl.Kernel a, Cl.Kernel b)
            {
                return a._handle == b._handle;
            }

            public static bool operator !=(Cl.Kernel a, Cl.Kernel b)
            {
                return a._handle != b._handle;
            }
        }
        
        [StructLayout(LayoutKind.Sequential)]
        public struct Event : IRefCountedHandle
        {
            private readonly IntPtr _handle;

            internal Event(IntPtr handle)
            {
                _handle = handle;
            }

            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainEvent(this);
            }

            #endregion

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseEvent(this);
            }

            #endregion
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Sampler : IRefCountedHandle
        {
            private readonly IntPtr _handle;

            internal Sampler(IntPtr handle)
            {
                _handle = handle;
            }

            #region IRefCountedHandle Members

            void IRefCountedHandle.Retain()
            {
                RetainSampler(this);
            }

            #endregion

            #region IHandleData Members

            IntPtr IHandleData.Handle
            {
                get
                {
                    return _handle;
                }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                ReleaseSampler(this);
            }

            #endregion
        }

        [Serializable]
        public class Exception : System.Exception
        {
            public Exception(ErrorCode error)
                : base(error.ToString())
            {
            }

            public Exception(ErrorCode error, string message)
                : base(string.Format("{0}: {1}", error, message))
            {
            }

            public Exception(ErrorCode error, string message, Exception inner)
                : base(string.Format("{0}: {1}", error, message), inner)
            {
            }

            protected Exception(
                SerializationInfo info,
                StreamingContext context)
                : base(info, context)
            {
            }
        }

        internal sealed class TypeSize<T>
        {
            public static readonly IntPtr Size = (IntPtr)Marshal.SizeOf(typeof(T));
        }
    }
}
