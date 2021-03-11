﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cosmos.Collections;
using Cosmos.Validation.Objects;

namespace Cosmos.Validation.Internals.Tokens.ValueTokens
{
    internal class ValueNotInToken : ValueToken
    {
        // ReSharper disable once InconsistentNaming
        public const string NAME = "ValueNotInToken";

        private readonly ICollection<object> _objects;

        public ValueNotInToken(VerifiableMemberContract contract, ICollection<object> objects) : base(contract)
        {
            _objects = objects ?? Arrays.Empty<object>();
        }

        public override CorrectValueOps Ops => CorrectValueOps.NotIn;

        public override string TokenName => NAME;

        public override bool MutuallyExclusive => false;

        public override int[] MutuallyExclusiveFlags => NoMutuallyExclusiveFlags;

        public override CorrectVerifyVal Valid(VerifiableObjectContext context)
        {
            var verifyVal = CreateVerifyVal();

            var value = GetValueFrom(context);

            if (!IsValidImpl(value))
            {
                UpdateVal(verifyVal, value);
            }

            return verifyVal;
        }

        public override CorrectVerifyVal Valid(VerifiableMemberContext context)
        {
            var verifyVal = CreateVerifyVal();

            var value = GetValueFrom(context);

            if (!IsValidImpl(value))
            {
                UpdateVal(verifyVal, value);
            }

            return verifyVal;
        }

        private bool IsValidImpl(object value)
        {
            if (value is ICollection collection)
            {
                if (collection.Cast<object>().Any(item => _objects.Contains(item)))
                {
                    return false;
                }
            }

            return !_objects.Contains(value);
        }

        private void UpdateVal(CorrectVerifyVal val, object obj)
        {
            val.IsSuccess = false;
            val.VerifiedValue = obj;
            val.ErrorMessage = MergeMessage("The value is contained in the given value array or collection.");
        }

        public override string ToString() => NAME;
    }
}