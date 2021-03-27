﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cosmos.Validation.Objects;

namespace Cosmos.Validation.Annotations.Core
{
    internal static partial class AnnotationVerificationEngine
    {
        public static bool Verify(VerifiableMemberContext context, IEnumerable<IFlagAnnotation> annotations, out VerifyFailure failure)
        {
            if (context is null)
                throw new ArgumentNullException(nameof(context));

            var errors = new List<VerifyError>();

            foreach (var annotation in annotations)
            {
                VerifyAnnotation(context, annotation, errors);
            }

            if (errors.Any())
            {
                failure = new VerifyFailure(context.MemberName, $"There are multiple errors in this Member '{context.MemberName}'.", context.Value);
                failure.Details.AddRange(errors);
            }
            else
            {
                failure = null;
            }

            return failure is null;
        }

        private static void VerifyAnnotation(VerifiableMemberContext context, IFlagAnnotation annotation, List<VerifyError> errors)
        {
            switch (annotation)
            {
                // #region Custom annotations for specific purposes
                //
                // case ValidEmailValueAttribute attr:
                //     VerifyImpls.ValidEmailValue(context, attr, errors);
                //     break;
                //
                // #endregion

                #region General annotations

                case ValidationParameterAttribute attr:
                    attr.IsValid(context)
                        .IfFalseThenInvoke(() => CreateAndUpdateErrors(attr.ErrorMessage, attr.Name, errors));
                    break;

                #endregion

                #region CustomAnnotations

                // 对自定义（或第三方）验证注解的检查
                case CustomAnnotationAttribute attr:
                    var valid = attr.IsValidInternal(context);
                    if (!valid)
                        CreateAndUpdateErrors(attr.ErrorMessage, attr.Name, errors, ValidatorType.Custom);
                    break;

                #endregion
            }
        }

        private static void CreateAndUpdateErrors(string errorMessage, string validatorName, List<VerifyError> errors, ValidatorType type = ValidatorType.BuildIn)
        {
            var error = new VerifyError
            {
                ErrorMessage = errorMessage,
                ValidatorName = validatorName,
                ViaValidatorType = type
            };

            errors.Add(error);
        }
    }
}