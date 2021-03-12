﻿using System;
using System.Collections.Generic;
using Cosmos.Validation.Internals.Rules;
using Cosmos.Validation.Objects;

namespace Cosmos.Validation.Registrars
{
    internal class ValueValidationRegistrar<T, TVal> : ValueValidationRegistrar<T>, IValueFluentValidationRegistrar<T, TVal>
    {
        public ValueValidationRegistrar(
            VerifiableMemberContract verifiableMemberContract,
            List<CorrectValueRule> rules,
            ValueRuleMode mode,
            IFluentValidationRegistrar<T> parentRegistrar,
            IValidationRegistrar rootRegistrar)
            : base(verifiableMemberContract, rules, mode, parentRegistrar, rootRegistrar)
        {
            ValueRuleBuilder = new CorrectValueRuleBuilder<T, TVal>(verifiableMemberContract, mode);
        }

        #region ValueRuleBuilder
        
        private CorrectValueRuleBuilder<T, TVal> ValueRuleBuilderPtr => (CorrectValueRuleBuilder<T, TVal>) ValueRuleBuilder;
        
        internal CorrectValueRuleBuilder<T, TVal> ExposeValueRuleBuilder2() => ValueRuleBuilderPtr;
        
        #endregion

        #region WithConfig

        public IValueFluentValidationRegistrar<T, TVal> WithConfig(Func<IValueRuleBuilder<T, TVal>, IValueRuleBuilder<T, TVal>> func)
        {
            var builder = func?.Invoke(ValueRuleBuilderPtr);

            if (builder is not null)
                ValueRuleBuilder = (CorrectValueRuleBuilder<T>) builder;

            return this;
        }

        #endregion

        #region ValueRules`2
        
        public IValueFluentValidationRegistrar<T, TVal> Range(TVal from, TVal to, RangeOptions options = RangeOptions.OpenInterval)
        {
            ValueRuleBuilder.Range(from, to, options);
            return this;
        }

        public IValueFluentValidationRegistrar<T, TVal> RangeWithOpenInterval(TVal from, TVal to)
        {
            ValueRuleBuilder.RangeWithOpenInterval(from, to);
            return this;
        }

        public IValueFluentValidationRegistrar<T, TVal> RangeWithCloseInterval(TVal from, TVal to)
        {
            ValueRuleBuilder.RangeWithCloseInterval(from, to);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> Length(int min, int max)
        {
            ValueRuleBuilder.Length(min, max);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> MinLength(int min)
        {
            ValueRuleBuilder.MinLength(min);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> MaxLength(int max)
        {
            ValueRuleBuilder.MaxLength(max);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> AtLeast(int count)
        {
            ValueRuleBuilder.AtLeast(count);
            return this;
        }

        public IValueFluentValidationRegistrar<T, TVal> Func(Func<TVal, CustomVerifyResult> func)
        {
            ValueRuleBuilderPtr.Func(func);
            return this;
        }

        public IWaitForMessageValidationRegistrar<T, TVal> Func(Func<TVal, bool> func)
        {
            return new ValidationRegistrarWithMessage<T, TVal>(this, _rootRegistrar, func);
        }

        public IWaitForMessageValidationRegistrar<T, TVal> Predicate(Predicate<TVal> predicate)
        {
            return new ValidationRegistrarWithMessage<T, TVal>(this, _rootRegistrar, predicate);
        }

        public IValueFluentValidationRegistrar<T, TVal> Must(Func<TVal, CustomVerifyResult> func)
        {
            ValueRuleBuilderPtr.Must(func);
            return this;
        }

        public IWaitForMessageValidationRegistrar<T, TVal> Must(Func<TVal, bool> func)
        {
            return new ValidationRegistrarWithMessage<T, TVal>(this, _rootRegistrar, func);
        }

        public new IValueFluentValidationRegistrar<T, TVal> InEnum(Type enumType)
        {
            ValueRuleBuilder.InEnum(enumType);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> InEnum<TEnum>()
        {
            ValueRuleBuilder.InEnum<TEnum>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> IsEnumName(Type enumType, bool caseSensitive)
        {
            ValueRuleBuilder.IsEnumName(enumType, caseSensitive);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> IsEnumName<TEnum>(bool caseSensitive)
        {
            ValueRuleBuilder.IsEnumName<TEnum>(caseSensitive);
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> ScalePrecision(int scale, int precision, bool ignoreTrailingZeros = false)
        {
            ValueRuleBuilder.ScalePrecision(scale, precision, ignoreTrailingZeros);
            return this;
        }
        
        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1>()
        {
            ValueRuleBuilder.RequiredTypes<T1>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>();
            return this;
        }

        public new IValueFluentValidationRegistrar<T, TVal> RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>()
        {
            ValueRuleBuilder.RequiredTypes<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>();
            return this;
        }

        #endregion
    }
}