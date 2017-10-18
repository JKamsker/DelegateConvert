using System;
using DelegateConvert;

namespace JSocket.Utilities.Generic.Delegate.DelegateConvert
{
    public class ActionConvert
    {
        /// <summary>
        /// Converts any generic Action to the given return type
        /// </summary>
        /// <param name="sourceDelegate">Should be Action<TX,...></param>
        /// <returns></returns>
        public static Action<object[]> Convert(System.Delegate sourceDelegate)
        {
            if (sourceDelegate.Method.ReturnType != typeof(void))
                throw new Exception("Invalid source delegate Type");

            return (Action<object[]>)DelegateConverter.Convert(sourceDelegate);
        }
    }
}