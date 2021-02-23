﻿using System.Linq;
using Cosmos.Validation.Objects;
using CosmosValidationUT.Models;
using Shouldly;
using Xunit;

namespace CosmosValidationUT.ObjectUT
{
    [Trait("ObjectUT", "ObjectContract")]
    public class ObjectContractTests
    {
        [Fact(DisplayName = "Create ObjectContract by direct type")]
        public void DirectTypeCreateObjectContractTest()
        {
            var contract = ObjectContractManager.Resolve(typeof(NiceBoat));

            contract.ShouldNotBeNull();
            contract.Type.ShouldBe(typeof(NiceBoat));
            contract.ObjectKind.ShouldBe(ObjectKind.StructureType);
            contract.IsBasicType().ShouldBeFalse();

            //annotations/attributes - class level
            contract.Attributes.Count.ShouldBe(0);
            contract.IncludeAnnotations.ShouldBeTrue();

            //value-contract
            contract.GetAllValueContracts().Count().ShouldBe(5);

            contract.GetValueContract("Name").MemberName.ShouldBe("Name");
            contract.GetValueContract("Length").MemberName.ShouldBe("Length");
            contract.GetValueContract("Width").MemberName.ShouldBe("Width");
            contract.GetValueContract("CreateTime").MemberName.ShouldBe("CreateTime");
            contract.GetValueContract("Email").MemberName.ShouldBe("Email");

            contract.GetValueContract(0).MemberName.ShouldBe("Name");
            contract.GetValueContract(1).MemberName.ShouldBe("Length");
            contract.GetValueContract(2).MemberName.ShouldBe("Width");
            contract.GetValueContract(3).MemberName.ShouldBe("Email"); //Property first
            contract.GetValueContract(4).MemberName.ShouldBe("CreateTime");
        }

        [Fact(DisplayName = "Create ObjectContract by generic type")]
        public void GenericTypeCreateObjectContractTest()
        {
            var contract = ObjectContractManager.Resolve<NiceBoat>();

            contract.ShouldNotBeNull();
            contract.Type.ShouldBe(typeof(NiceBoat));
            contract.ObjectKind.ShouldBe(ObjectKind.StructureType);
            contract.IsBasicType().ShouldBeFalse();

            //annotations/attributes - class level
            contract.Attributes.Count.ShouldBe(0);
            contract.IncludeAnnotations.ShouldBeTrue();

            //value-contract
            contract.GetAllValueContracts().Count().ShouldBe(5);

            contract.GetValueContract("Name").MemberName.ShouldBe("Name");
            contract.GetValueContract("Length").MemberName.ShouldBe("Length");
            contract.GetValueContract("Width").MemberName.ShouldBe("Width");
            contract.GetValueContract("CreateTime").MemberName.ShouldBe("CreateTime");
            contract.GetValueContract("Email").MemberName.ShouldBe("Email");

            contract.GetValueContract(0).MemberName.ShouldBe("Name");
            contract.GetValueContract(1).MemberName.ShouldBe("Length");
            contract.GetValueContract(2).MemberName.ShouldBe("Width");
            contract.GetValueContract(3).MemberName.ShouldBe("Email"); //Property first
            contract.GetValueContract(4).MemberName.ShouldBe("CreateTime");
        }
    }
}