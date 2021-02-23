﻿using System;
using System.Collections.Generic;
using Cosmos.Reflection;
using Cosmos.Validation.Internals;
using Cosmos.Validation.Objects;
using Cosmos.Validation.Projects;
using Cosmos.Validation.Validators;

namespace Cosmos.Validation
{
    public class ValidationProvider : IValidationProvider, ICorrectProvider
    {
        private readonly IValidationProjectManager _projectManager;
        private readonly IValidationObjectResolver _objectResolver;
        private readonly CustomValidatorManager _customValidatorManager;

        private ValidationOptions _options;

        public ValidationProvider(
            IValidationProjectManager projectManager,
            IValidationObjectResolver objectResolver,
            ValidationOptions options)
        {
            _projectManager = projectManager ?? throw new ArgumentNullException(nameof(projectManager));
            _objectResolver = objectResolver ?? throw new ArgumentNullException(nameof(objectResolver));
            _customValidatorManager = new CustomValidatorManager();
            _options = options ?? new ValidationOptions();
        }

        internal const string DefaultName = "Default Validation Provider";
        internal const string MainName = "Main Validation Provider";

        internal static bool IsDefault(string name) => name == DefaultName;

        string ICorrectProvider.Name { get; set; } = DefaultName;

        public IValidator Resolve(Type type)
        {
            var d = typeof(AggregationValidator<>);
            var v = d.MakeGenericType(type);           
            return TypeVisit.CreateInstance<IValidator>(v, _projectManager, _objectResolver, _options);
        }

        public IValidator Resolve(Type type, string name)
        {
            var d = typeof(AggregationValidator<>);
            var v = d.MakeGenericType(type);            
            return TypeVisit.CreateInstance<IValidator>(v, name, _projectManager, _objectResolver, _options);
        }

        public IValidator<T> Resolve<T>() => (IValidator<T>)Resolve(typeof(T));

        public IValidator<T> Resolve<T>(string name) =>  (IValidator<T>)Resolve(typeof(T), name);

        IValidationProjectManager ICorrectProvider.ExposeProjectManager()
        {
            return _projectManager;
        }

        IValidationObjectResolver ICorrectProvider.ExposeObjectResolver()
        {
            return _objectResolver;
        }

        CustomValidatorManager ICorrectProvider.ExposeCustomValidatorManager()
        {
            return _customValidatorManager;
        }

        ValidationOptions ICorrectProvider.ExposeValidationOptions()
        {
            return _options;
        }

        void ICorrectProvider.RegisterValidator<TValidator>()
        {
            _customValidatorManager.Register<TValidator>();
        }

        void ICorrectProvider.RegisterValidator<TValidator, T>()
        {
            _customValidatorManager.Register<TValidator, T>();
        }

        void ICorrectProvider.RegisterValidator(CustomValidator validator)
        {
            _customValidatorManager.Register(validator);
        }

        void ICorrectProvider.RegisterValidator<T>(CustomValidator<T> validator)
        {
            _customValidatorManager.Register(validator);
        }
        
        

        public void UpdateOptions(ValidationOptions options)
        {
            if (options is not null)
                _options = options;
        }

        public void UpdateOptions(Action<ValidationOptions> optionAct)
        {
            optionAct?.Invoke(_options);
        }
    }
}