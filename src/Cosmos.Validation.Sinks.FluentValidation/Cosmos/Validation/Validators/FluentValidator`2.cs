﻿using System;
using Cosmos.Validation.Internals;
using Cosmos.Validation.Objects;

namespace Cosmos.Validation.Validators
{
    public sealed class FluentValidator<TValidator, T> : CustomValidator<T>
        where TValidator : class, FluentValidation.IValidator<T>, new()
    {
        private readonly TValidator _validatorImpl;

        public FluentValidator() : base("FluentValidationWrappedValidator`2")
        {
            _validatorImpl = new TValidator();
        }

        #region Verify

        public override VerifyResult Verify(Type declaringType, object instance)
        {
            return FluentValidationCore.Verify(_validatorImpl, declaringType, instance, typeof(TValidator));
        }

        protected override VerifyResult VerifyImpl(VerifiableObjectContext context)
        {
            return FluentValidationCore.Verify(_validatorImpl, context, typeof(TValidator));
        }

        #endregion

        #region VerifyOne

        protected override VerifyResult VerifyOneImpl(VerifiableMemberContext context)
        {
            return FluentValidationCore.VerifyOne(_validatorImpl, context, typeof(TValidator));
        }

        #endregion
    }
}