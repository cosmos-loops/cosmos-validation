﻿using System;
using System.Collections.Generic;
using System.Reflection;
using Cosmos.Validation.Annotations;

namespace Cosmos.Validation.Objects
{
    public interface IVerifiableObjectContract
    {
        Type Type { get; }
        bool IsBasicType { get; }
        VerifiableObjectKind ObjectKind { get; }
        object GetValue(object instance, string memberName);
        object GetValue(object instance, int memberIndex);
        object GetValue(IDictionary<string, object> keyValueCollection, string memberName);
        object GetValue(IDictionary<string, object> keyValueCollection, int memberIndex);
        VerifiableMemberContract GetMemberContract(string memberName);
        VerifiableMemberContract GetMemberContract(PropertyInfo propertyInfo);
        VerifiableMemberContract GetMemberContract(FieldInfo fieldInfo);
        VerifiableMemberContract GetMemberContract(int memberIndex);
        bool ContainsMember(string memberName);
        IEnumerable<VerifiableMemberContract> GetMemberContracts();
        bool IncludeAnnotations { get; }
        IReadOnlyCollection<Attribute> Attributes { get; }
        IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute;
        IEnumerable<VerifiableParamsAttribute> GetParameterAnnotations();
        IEnumerable<IQuietVerifiableAnnotation> GetQuietVerifiableAnnotations();
        IEnumerable<IStrongVerifiableAnnotation> GetStrongVerifiableAnnotations();
    }
}