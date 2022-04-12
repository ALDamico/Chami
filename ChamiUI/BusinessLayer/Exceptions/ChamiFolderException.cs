using System;
using System.Runtime.Serialization;
using ChamiUI.Localization;

namespace ChamiUI.BusinessLayer.Exceptions
{
    [Serializable]
    public class ChamiFolderException : Exception
    {
        public ChamiFolderException(string message) : base(message)
        {
        }

        protected ChamiFolderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}