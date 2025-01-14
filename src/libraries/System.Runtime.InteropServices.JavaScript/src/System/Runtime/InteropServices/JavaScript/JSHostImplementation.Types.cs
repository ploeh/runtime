﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Threading;
using System.Threading.Tasks;

namespace System.Runtime.InteropServices.JavaScript
{
    internal static partial class JSHostImplementation
    {
        internal unsafe delegate void ToManagedCallback(JSMarshalerArgument* arguments_buffer);

        public sealed class PromiseHolder
        {
            public nint GCVHandle;
            public ToManagedCallback? Callback;
#if FEATURE_WASM_THREADS
            // the JavaScript object could only exist on the single web worker and can't migrate to other workers
            internal int OwnerThreadId;
            internal SynchronizationContext? SynchronizationContext;
#endif

            public PromiseHolder(nint gcvHandle)
            {
                this.GCVHandle = gcvHandle;
#if FEATURE_WASM_THREADS
                this.OwnerThreadId = Thread.CurrentThread.ManagedThreadId;
                this.SynchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
#endif
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct IntPtrAndHandle
        {
            [FieldOffset(0)]
            internal IntPtr ptr;

            [FieldOffset(0)]
            internal RuntimeMethodHandle methodHandle;

            [FieldOffset(0)]
            internal RuntimeTypeHandle typeHandle;
        }
    }
}
