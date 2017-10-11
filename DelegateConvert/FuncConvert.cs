using System;
using System.Linq.Expressions;

namespace DelegateConvert
{
    public class FuncConvert
    {
        public static Func<object[], object> ConvertToObjectParams(Delegate sourceDelegate)
        {
            if (sourceDelegate.Method.ReturnType == typeof(void))
                throw new Exception("Invalid source delegate Type");

            return (Func<object[], object>)UniConvert.ConvertToObjectArrParams(sourceDelegate);
        }

        public static Func<object[], T> ConvertToObjectParams<T>(Delegate sourceDelegate)
        {
            if (sourceDelegate.Method.ReturnType == typeof(void))
                throw new Exception("Invalid source delegate Type");

            return (Func<object[], T>)UniConvert.ConvertToObjectArrParams(sourceDelegate, typeof(T));
        }
    }
}