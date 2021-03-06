﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Cosmos.Validation.Internals;
using Cosmos.Validation.Internals.Rules;
using Cosmos.Validation.Objects;

namespace Cosmos.Validation.Strategies
{
    public abstract class ValidationStrategy<T> : IValidationStrategy<T>, ICorrectStrategy<T>
    {
        private readonly List<CorrectValueRuleBuilder<T>> _memberValueRuleBuilders;
        private readonly object _builderLockObj = new();
        private readonly VerifiableObjectContract _contract;

        protected ValidationStrategy()
        {
            SourceType = typeof(T);

            _memberValueRuleBuilders = new List<CorrectValueRuleBuilder<T>>();
            _contract = VerifiableObjectContractManager.Resolve<T>();
        }

        public Type SourceType { get; }

        IEnumerable<CorrectValueRuleBuilder<T>> ICorrectStrategy<T>.GetValueRuleBuilders()
        {
            return _memberValueRuleBuilders;
        }

        protected IValueRuleBuilder<T> ForMember(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            lock (_builderLockObj)
            {
                var builder = _memberValueRuleBuilders.FirstOrDefault(b => b.MemberName == name);
                if (builder is null)
                {
                    builder = new CorrectValueRuleBuilder<T>(_contract.GetMemberContract(name));
                    _memberValueRuleBuilders.Add(builder);
                }

                return builder;
            }
        }

        protected IValueRuleBuilder<T> ForMember(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
                throw new ArgumentNullException(nameof(propertyInfo));
            return ForMember(propertyInfo.Name);
        }

        protected IValueRuleBuilder<T> ForMember(FieldInfo fieldInfo)
        {
            if (fieldInfo is null)
                throw new ArgumentNullException(nameof(fieldInfo));
            return ForMember(fieldInfo.Name);
        }

        protected IValueRuleBuilder<T> ForMember<TVal>(Expression<Func<T, TVal>> expression)
        {
            if (expression is null)
                throw new ArgumentNullException(nameof(expression));

            lock (_builderLockObj)
            {
                var name = PropertySelector.GetPropertyName(expression);
                var builder = _memberValueRuleBuilders.FirstOrDefault(b => b.MemberName == name);
                if (builder is null)
                {
                    builder = new CorrectValueRuleBuilder<T, TVal>(_contract.GetMemberContract(name));
                    _memberValueRuleBuilders.Add(builder);
                }

                return builder;
            }
        }
    }
}