using System;
using System.Linq.Expressions;

namespace DelegateConvert
{
    public class ActionConvert
    {
        public static Action<object[]> ConvertToObjectParams(Delegate sourceDelegate)
        {
            if (sourceDelegate.Method.ReturnType != typeof(void))
                throw new Exception("Invalid source delegate Type");

            return (Action<object[]>)UniConvert.ConvertToObjectArrParams(sourceDelegate);
        }
    }
}