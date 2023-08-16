using DynamicRoles.Core;
using DynamicRoles.CoreTests.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DynamicRoles.CoreTests;
public class PolicyNameHelperTests
{

    [TestCase(TestPermissions.Permission_1, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "1")]
    [TestCase(TestPermissions.Permission_2, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "2")]
    [TestCase(TestPermissions.Permission_3, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "4")]
    [TestCase(TestPermissions.Permission_4, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "8")]
    [TestCase(TestPermissions.Permission_5, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "16")]
    [TestCase(TestPermissions.Permission_6, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "32")]
    [TestCase(TestPermissions.Permission_7, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "64")]
    [TestCase(TestPermissions.Permission_8, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "128")]

    [TestCase(TestPermissions.Permission_1 | TestPermissions.Permission_3, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "5")]
    [TestCase(TestPermissions.Permission_2 | TestPermissions.Permission_3, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "6")]
    [TestCase(TestPermissions.Permission_5 | TestPermissions.Permission_2, ExpectedResult = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "18")]
    public string ShouldGeneratePolicyName(TestPermissions permissions)
    {
        return PolicyNameHelper.GeneratePolicyNameFor(permissions);
    }

    [TestCase(TestPermissions.None)]
    [TestCase(TestPermissions.All)]
    public void ShouldThrowExceptionOnGeneratePolicyName(TestPermissions permissions)
    {
        FluentActions.Invoking(() => PolicyNameHelper.GeneratePolicyNameFor(permissions))
            .Should().Throw<PermissionsException>();
    }

    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "1", ExpectedResult = TestPermissions.Permission_1)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "2", ExpectedResult = TestPermissions.Permission_2)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "4", ExpectedResult = TestPermissions.Permission_3)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "8", ExpectedResult = TestPermissions.Permission_4)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "16", ExpectedResult = TestPermissions.Permission_5)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "32", ExpectedResult = TestPermissions.Permission_6)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "64", ExpectedResult = TestPermissions.Permission_7)]
    [TestCase(Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "128", ExpectedResult = TestPermissions.Permission_8)]
    public TestPermissions ShouldGetPermissionFromPolicyName(string policyName)
    {
        return PolicyNameHelper.GetPermission<TestPermissions>(policyName);
    }

    public void ShouldGetPermissionsUniqueName()
    {
        string policy = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "1";

        string result = PolicyNameHelper.GetPermissionsUniqueName(policy);

        result.Should().Be(Constants.TestPermissionsName);
    }

    public void ShouldGetPermissionValue()
    {
        string policy = Constants.TestPermissionsName + PolicyNameHelper.SEPARATOR + "1";

        int result = PolicyNameHelper.GetPermissionValueFrom(policy);

        result.Should().Be(1);
    }
}
