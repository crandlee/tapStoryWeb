using System;

namespace tapStoryWebApi.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ReferenceServiceFunction : Attribute
    {
        private string _sourceReference;
        private string _targetReference;
        private ReferenceServiceFunctionType _refFunctionType;

        public ReferenceServiceFunction(string sourceReference, string targetReference, ReferenceServiceFunctionType refFunctionType)
        {
            _sourceReference = sourceReference;
            _targetReference = targetReference;
            _refFunctionType = refFunctionType;
        }
        
    }

    public enum ReferenceServiceFunctionType
    {
        Add = 0,
        Delete = 1
    }

}