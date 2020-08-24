// Copyright (c) Huy Hoang. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Runtime.Serialization;

namespace Dash.Exceptions
{
    [Serializable]
    public class InvalidDataTypeException : Exception
    {
        public InvalidDataTypeException(string specifiedDashDataType) : base($"The specified datatype '{specifiedDashDataType}' is invalid")
        {
        }

        protected InvalidDataTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
