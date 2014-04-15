﻿using TddEbook.TddToolkit.ImplementationDetails.Common;
using TddEbook.TddToolkit.ImplementationDetails.ConstraintAssertions;
using TddEbook.TddToolkit.ImplementationDetails.ConstraintAssertions.Reflection;
using TddEbook.TddToolkit.ImplementationDetails.CustomCollections.ConstraintAssertions;

namespace TddEbook.TddToolkit.Helpers.Constraints.EqualityOperator
{
  public class UnEqualityWithNullMustBeImplementedInTermsOfEqualityOperator : IConstraint
  {
    private readonly ValueObjectActivator _activator;

    public UnEqualityWithNullMustBeImplementedInTermsOfEqualityOperator(ValueObjectActivator activator)
    {
      _activator = activator;
    }

    public void CheckAndRecord(ConstraintsViolations violations)
    {
      object aNull = null;
         
      var instance1 = _activator.CreateInstanceAsValueObjectWithFreshParameters();
      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, instance1, aNull), 
          "a == null should return false", violations), 
        "a == null should return false", violations);

      RecordedAssertions.DoesNotThrow(() =>
        RecordedAssertions.False(Are.EqualInTermsOfEqualityOperator(_activator.TargetType, aNull, instance1),
          "null == a should return false", violations),
        "null == a should return false", violations);
    }
  }
}