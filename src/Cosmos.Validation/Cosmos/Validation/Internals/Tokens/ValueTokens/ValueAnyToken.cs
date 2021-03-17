﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cosmos.Validation.Internals.Tokens.ValueTokens.Basic;
using Cosmos.Validation.Objects;

namespace Cosmos.Validation.Internals.Tokens.ValueTokens
{
    internal class ValueAnyToken : ValueCollBasicToken
    {
        // ReSharper disable once InconsistentNaming
        public const string NAME = "ValueAnyToken";

        public ValueAnyToken(VerifiableMemberContract contract, Func<object, bool> func) : base(contract, func, NAME) { }

        protected override bool IsValidImpl(ICollection collection, Func<object, bool> func)
        {
            return collection.Cast<object>().Any(one => func!.Invoke(one));
        }
    }

    internal class ValueAnyToken<TVal, TItem> : ValueCollBasicToken<TVal, TItem>
        where TVal : IEnumerable<TItem>
    {
        // ReSharper disable once InconsistentNaming
        public const string NAME = "GenericValueAnyToken";

        public ValueAnyToken(VerifiableMemberContract contract, Func<TItem, bool> func) : base(contract, func, NAME) { }

        protected override bool IsValidImpl(ICollection collection, Func<TItem, bool> func)
        {
            return collection.Cast<TItem>().Any(one => func!.Invoke(one));
        }
    }
}