﻿namespace Cosmos.Validation
{
    public class ValidationOptions
    {
        public bool AnnotationEnabled { get; set; } = true;

        public bool CustomValidatorEnabled { get; set; } = true;

        public bool FailureIfInstanceIsNull { get; set; } = true;

        public bool FailureIfProjectNotMatch { get; set; } = false;
    }
}