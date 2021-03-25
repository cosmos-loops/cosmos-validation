﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Cosmos.Validation.Internals;
using Cosmos.Validation.Internals.Extensions;
using Cosmos.Validation.Objects;
using Cosmos.Validation.Projects;

namespace Cosmos.Validation.Validators
{
    public class AggregationValidator<T> : IValidator<T>, ICorrectValidator<T>
    {
        private readonly IValidationProjectManager _projectManager;
        private readonly IVerifiableObjectResolver _objectResolver;
        private readonly ICustomValidatorManager _customValidatorManager;
        private readonly Type _type;
        private readonly string _name;

        private readonly ValidationOptions _options;
        private readonly AnnotationValidator _annotationValidator;

        public AggregationValidator(
            IValidationProjectManager projectManager,
            IVerifiableObjectResolver objectResolver,
            ICustomValidatorManager customValidatorManager,
            ValidationOptions options)
        {
            _projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
            _objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
            _customValidatorManager = customValidatorManager ?? throw new ArgumentNullException(nameof(customValidatorManager));
            _type = typeof(T);
            _name = string.Empty;
            _options = options;

            _annotationValidator = options.AnnotationEnabled ? AnnotationValidator.GetInstance(objectResolver, options) : null;
        }

        public AggregationValidator(
            string name,
            IValidationProjectManager projectManager,
            IVerifiableObjectResolver objectResolver,
            ICustomValidatorManager customValidatorManager,
            ValidationOptions options)
        {
            _projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
            _objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
            _customValidatorManager = customValidatorManager ?? throw new ArgumentNullException(nameof(customValidatorManager));
            _type = typeof(T);
            _name = name;
            _options = options;

            _annotationValidator = options.AnnotationEnabled ? AnnotationValidator.GetInstance(objectResolver, options) : null;
        }

        public string Name => string.IsNullOrEmpty(_name) ? "Anonymous Validator" : _name;

        public bool IsAnonymous => string.IsNullOrEmpty(_name);

        bool ICorrectValidator.IsTypeBinding => true;

        Type ICorrectValidator.SourceType => typeof(T);

        #region Verify

        public VerifyResult Verify(T instance)
        {
            if (instance is null)
                return _options.ReturnNullReferenceOrSuccess();

            VerifyResult result1 = null, result2 = null, result3 = null;
            var context = _objectResolver.Resolve(instance);

            if (_projectManager.TryResolve(_type, _name, out var project))
                result1 = project.Verify(context);

            if (_options.CustomValidatorEnabled)
                result2 = CorrectEngine.ValidViaCustomValidators(context, _customValidatorManager.ResolveAll());

            if (_options.AnnotationEnabled)
                result3 = _annotationValidator.Verify(context);

            if (result1 is null && result2 is null && result3 is null)
                return _options.ReturnUnregisterProjectForSuchTypeOrSuccess();

            return result1 is null
                ? VerifyResult.MakeTogether(result2, result3)
                : VerifyResult.Merge(result1, result2, result3);
        }

        VerifyResult IValidator.Verify(Type declaringType, object instance)
        {
            if (instance is null)
                return _options.ReturnNullReferenceOrSuccess();

            VerifyResult result1 = null, result2 = null, result3 = null;
            var context = _objectResolver.Resolve(declaringType, instance);

            if (_projectManager.TryResolve(_type, _name, out var project))
                result1 = project.Verify(context);

            if (_options.CustomValidatorEnabled)
                result2 = CorrectEngine.ValidViaCustomValidators(context, _customValidatorManager.ResolveAll());

            if (_options.AnnotationEnabled)
                result3 = _annotationValidator.Verify(context);

            if (result1 is null && result2 is null && result3 is null)
                return _options.ReturnUnregisterProjectForSuchTypeOrSuccess();

            return result1 is null
                ? VerifyResult.MakeTogether(result2, result3)
                : VerifyResult.Merge(result1, result2, result3);
        }

        #endregion

        #region VerifyOne

        public VerifyResult VerifyOne(object memberValue, string memberName)
        {
            if (memberValue is null)
                return _options.ReturnNullReferenceOrSuccess();
            var memberContract = VerifiableObjectContractManager.Resolve<T>()?.GetMemberContract(memberName);
            if (memberContract is null)
                return VerifyResult.MemberIsNotExists(memberName);
            return VerifyOneInternal(VerifiableMemberContext.Create(memberValue, memberContract));
        }

        public VerifyResult VerifyOne<TVal>(Expression<Func<T, TVal>> expression, TVal memberValue)
        {
            if (expression is null)
                return _options.ReturnNullReferenceOrSuccess();
            var memberName = PropertySelector.GetPropertyName(expression);
            var memberContract = VerifiableObjectContractManager.Resolve<T>()?.GetMemberContract(memberName);
            if (memberContract is null)
                return VerifyResult.MemberIsNotExists(memberName);
            return VerifyOneInternal(VerifiableMemberContext.Create(memberValue, memberContract));
        }

        VerifyResult IValidator.VerifyOne(Type declaringType, object memberValue, string memberName)
        {
            if (memberValue is null)
                return _options.ReturnNullReferenceOrSuccess();
            var memberContract = VerifiableObjectContractManager.Resolve(declaringType)?.GetMemberContract(memberName);
            if (memberContract is null)
                return VerifyResult.MemberIsNotExists(memberName);
            return VerifyOneInternal(VerifiableMemberContext.Create(memberValue, memberContract));
        }

        public VerifyResult VerifyOneWithInstance(object memberValue, string memberName, T instance)
        {
            if (memberValue is null)
                return _options.ReturnNullReferenceOrSuccess();
            var parentContract = VerifiableObjectContractManager.Resolve<T>();
            var memberContract = parentContract?.GetMemberContract(memberName);
            if (memberContract is null)
                return VerifyResult.MemberIsNotExists(memberName);
            return VerifyOneInternal(VerifiableMemberContext.Create(memberValue, memberContract, parentContract.WithInstance(instance)));
        }

        public VerifyResult VerifyOneWithInstance<TVal>(Expression<Func<T, TVal>> expression, TVal memberValue, T instance)
        {
            if (expression is null)
                return _options.ReturnNullReferenceOrSuccess();
            var parentContract = VerifiableObjectContractManager.Resolve<T>();
            var memberName = PropertySelector.GetPropertyName(expression);
            var memberContract = parentContract?.GetMemberContract(memberName);
            if (memberContract is null)
                return VerifyResult.MemberIsNotExists(memberName);
            return VerifyOneInternal(VerifiableMemberContext.Create(memberValue, memberContract, parentContract.WithInstance(instance)));
        }

        VerifyResult IValidator.VerifyOneWithInstance(Type declaringType, object memberValue, string memberName, object instance)
        {
            if (memberValue is null)
                return _options.ReturnNullReferenceOrSuccess();
            var parentContract = VerifiableObjectContractManager.Resolve(declaringType);
            var memberContract = parentContract?.GetMemberContract(memberName);
            if (memberContract is null)
                return VerifyResult.MemberIsNotExists(memberName);
            return VerifyOneInternal(VerifiableMemberContext.Create(memberValue, memberContract, parentContract.WithInstance(instance)));
        }

        private VerifyResult VerifyOneInternal(VerifiableMemberContext memberContext)
        {
            VerifyResult result1 = null, result2 = null, result3 = null;
            if (_projectManager.TryResolve(_type, _name, out var project))
                result1 = project.VerifyOne(memberContext);
            if (_options.CustomValidatorEnabled)
                result2 = CorrectEngine.ValidViaCustomValidators(memberContext, _customValidatorManager.ResolveAll());
            if (_options.AnnotationEnabled)
                result3 = _annotationValidator.VerifyOne(memberContext);
            if (result1 is null && result2 is null && result3 is null)
                return _options.ReturnUnregisterProjectForSuchTypeOrSuccess();
            return result1 is null
                ? VerifyResult.MakeTogether(result2, result3)
                : VerifyResult.Merge(result1, result2, result3);
        }

        #endregion

        #region VerifyMany

        public VerifyResult VerifyMany(IDictionary<string, object> keyValueCollections)
        {
            if (keyValueCollections is null)
                return _options.ReturnNullReferenceOrSuccess();

            VerifyResult result1 = null, result2 = null, result3 = null;
            var context = _objectResolver.Resolve<T>(keyValueCollections);

            if (_projectManager.TryResolve(_type, _name, out var project))
                result1 = project.VerifyMany(context.GetValueMap());

            if (_options.CustomValidatorEnabled)
                result2 = CorrectEngine.ValidViaCustomValidators(context.GetValueMap(), _customValidatorManager.ResolveAll());

            if (_options.AnnotationEnabled)
                result3 = _annotationValidator.Verify(context);

            if (result1 is null && result2 is null && result3 is null)
                return _options.ReturnUnregisterProjectForSuchTypeOrSuccess();

            return result1 is null
                ? VerifyResult.MakeTogether(result2, result3)
                : VerifyResult.Merge(result1, result2, result3);
        }

        VerifyResult IValidator.VerifyMany(Type declaringType, IDictionary<string, object> keyValueCollections)
        {
            if (declaringType is null)
                return _options.ReturnNullReferenceOrSuccess();

            if (keyValueCollections is null)
                return _options.ReturnNullReferenceOrSuccess();

            VerifyResult result1 = null, result2 = null, result3 = null;
            var context = _objectResolver.Resolve(declaringType, keyValueCollections);

            if (_projectManager.TryResolve(_type, _name, out var project))
                result1 = project.VerifyMany(context.GetValueMap());

            if (_options.CustomValidatorEnabled)
                result2 = CorrectEngine.ValidViaCustomValidators(context.GetValueMap(), _customValidatorManager.ResolveAll());

            if (_options.AnnotationEnabled)
                result3 = _annotationValidator.Verify(context);

            if (result1 is null && result2 is null && result3 is null)
                return _options.ReturnUnregisterProjectForSuchTypeOrSuccess();

            return result1 is null
                ? VerifyResult.MakeTogether(result2, result3)
                : VerifyResult.Merge(result1, result2, result3);
        }

        #endregion
    }
}