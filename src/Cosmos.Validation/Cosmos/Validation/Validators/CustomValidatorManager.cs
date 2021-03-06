﻿using System;
using System.Collections.Generic;
using System.Linq;
using Cosmos.Collections;

namespace Cosmos.Validation.Validators
{
    public class CustomValidatorManager : ICustomValidatorManager
    {
        //For all normal CustomValidator/CustomValidator<T>
        private readonly Dictionary<string, CustomValidator> _validators = new();

        #region Register

        public void Register<TValidator>() where TValidator : CustomValidator, new()
        {
            Register(new TValidator());
        }

        public void Register<TValidator, T>() where TValidator : CustomValidator<T>, new()
        {
            Register(new TValidator());
        }

        public void Register(CustomValidator validator)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));
            RegisterImpl(validator);
        }

        public void Register<T>(CustomValidator<T> validator)
        {
            if (validator is null)
                throw new ArgumentNullException(nameof(validator));
            RegisterImpl(validator);
        }

        #endregion

        #region RegisterImpl

        private void RegisterImpl(CustomValidator validator)
        {
            if (_validators.ContainsKey(validator.Name))
                return;
            _validators.AddValueIfNotExist(validator.Name, validator);
        }
        
        #endregion

        #region Resolve

        public CustomValidator Resolve(string name)
        {
            if (_validators.TryGetValue(name, out var validator))
                return validator;
            return SealedValidator.Instance;
        }

        public CustomValidator<T> Resolve<T>(string name)
        {
            if (_validators.TryGetValue(name, out var v) && v is CustomValidator<T> validator)
                return validator;
            return SealedValidator<T>.Instance;
        }

        public IEnumerable<CustomValidator> ResolveAll() => _validators.Values;

        public IEnumerable<CustomValidator> ResolveEmpty() => Enumerable.Empty<CustomValidator>();

        public IEnumerable<CustomValidator> ResolveBy(Func<CustomValidator, bool> filter) => _validators.Values.Where(filter);

        #endregion
    }
}