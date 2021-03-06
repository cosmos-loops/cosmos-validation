﻿using System.Collections.Generic;

namespace Cosmos.Validation.Objects
{
    public interface ICustomVerifiableObjectContractImpl : IVerifiableObjectContract
    {
        Dictionary<string, VerifiableMemberContract> GetMemberContractMap();
    }
}