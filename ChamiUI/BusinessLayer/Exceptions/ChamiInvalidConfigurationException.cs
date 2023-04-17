using System;
using System.Runtime.Serialization;

namespace ChamiUI.BusinessLayer.Exceptions;

[Serializable]
public class ChamiInvalidConfigurationException : Exception
{
    public ChamiInvalidConfigurationException(string message) : base(message)
    {
    }

    protected ChamiInvalidConfigurationException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}