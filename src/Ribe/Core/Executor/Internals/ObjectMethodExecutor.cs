using Ribe.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Ribe.Core.Executor.Internals
{
    public class ObjectMethodExecutor : IObjectMethodExecutor
    {
        private Func<object, object[], object> _methodExecutor { get; }

        public Type ServiceType { get; }

        public ServiceMethod ServiceMethod { get; }

        public ObjectMethodExecutor(Type serviceType, ServiceMethod serviceMethod)
        {
            ServiceType = serviceType;
            ServiceMethod = serviceMethod;

            _methodExecutor = CreateExecuteDelegate(serviceType, serviceMethod);
        }

        public object Execute(object service, object[] paramterValues)
        {
            return _methodExecutor(service, paramterValues);
        }

        //TODO: private
        public static Func<object, object[], object> CreateExecuteDelegate(Type serviceType, ServiceMethod serviceMethod)
        {
            var serviceParamter = Expression.Parameter(typeof(object), "service");
            var parametersParameter = Expression.Parameter(typeof(object[]), "parameters");

            var parameters = new List<Expression>();
            var parameterInfos = serviceMethod.Parameters;
            var returnType = serviceMethod.Method.ReturnType;

            for (int i = 0; i < parameterInfos.Length; i++)
            {
                var item = parameterInfos[i];
                var paramterValue = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                var castedValue = Expression.Convert(paramterValue, item.ParameterType);

                parameters.Add(castedValue);
            }

            var methodCall = Expression.Convert(
                Expression.Call(
                    Expression.Convert(serviceParamter, serviceType),
                    serviceMethod.Method,
                    parameters
                ),
                returnType == typeof(void) ? typeof(void) : typeof(object)
            );

            var createdDelegate = Expression.Lambda(methodCall, serviceParamter, parametersParameter).Compile();

            if (returnType != typeof(void))
            {
                return (Func<object, object[], object>)createdDelegate;
            }

            //Wrap return void
            return (obj, paramterValues) =>
            {
                ((Action<object, object[]>)createdDelegate)(obj, paramterValues);
                return null;
            };
        }
    }
}
