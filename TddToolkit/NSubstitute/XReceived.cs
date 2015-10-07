﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using NSubstitute.Core;
using NSubstitute.Core.SequenceChecking;
using NSubstitute.Exceptions;

namespace TddEbook.TddToolkit.NSubstitute
{
  public class XReceived
  {
    public static void Only(Action action)
    {
      new SequenceExclusiveAssertion().Assert(SubstitutionContext.Current.RunQuery(action));
    }
  }

  public class SequenceExclusiveAssertion
  {
    public void Assert(IQueryResults queryResult)
    {
      var querySpec = QuerySpecificationFrom(queryResult);
      var allReceivedCalls = AllReceivedCallsWithExceptionOfPropertyGettersByTargetsOf(querySpec);

      var callsSpecifiedButNotReceived = GetCallsExpectedButNoReceived(querySpec, allReceivedCalls);
      var callsReceivedButNotSpecified = GetCallsReceivedButNotExpected(querySpec, allReceivedCalls);

      if (callsSpecifiedButNotReceived.Length > 0 || callsReceivedButNotSpecified.Length > 0)
      {
        throw new CallSequenceNotFoundException(GetExceptionMessage(querySpec, allReceivedCalls,
          callsSpecifiedButNotReceived, callsReceivedButNotSpecified));
      }
    }

    private ICall[] GetCallsReceivedButNotExpected(CallSpecAndTarget[] querySpec, ICall[] allReceivedCalls)
    {
      var copyOfQuerySpec = querySpec.ToList();

      var notMatchedCalls = new List<ICall>();

      foreach (var call in allReceivedCalls)
      {
        var matchingSpec = copyOfQuerySpec.FirstOrDefault(spec => Matches(call, spec));
        if (matchingSpec != null)
        {
          copyOfQuerySpec.Remove(matchingSpec);
        }
        else
        {
          notMatchedCalls.Add(call);
        }
      }

      return notMatchedCalls.ToArray();
    }

    private CallSpecAndTarget[] GetCallsExpectedButNoReceived(CallSpecAndTarget[] querySpec, ICall[] allReceivedCalls)
    {
      var copyOfAllReceivedCalls = allReceivedCalls.ToList();
      var notMatchedSpecs = new List<CallSpecAndTarget>();

      foreach (var spec in querySpec)
      {
        var matchingCall = copyOfAllReceivedCalls.FirstOrDefault(call => Matches(call, spec));
        if (matchingCall != null)
        {
          copyOfAllReceivedCalls.Remove(matchingCall);
        }
        else
        {
          notMatchedSpecs.Add(spec);
        }
      }
      return notMatchedSpecs.ToArray();
    }

    private ICall[] AllReceivedCallsWithExceptionOfPropertyGettersByTargetsOf(CallSpecAndTarget[] querySpec)
    {
      return querySpec.Select(s => s.Target).Distinct()
        .SelectMany(target => target.ReceivedCalls())
        .Where(x => this.IsNotPropertyGetterCall(x.GetMethodInfo()))
        .ToArray();
    }

    private CallSpecAndTarget[] QuerySpecificationFrom(IQueryResults queryResult)
    {
      return
        queryResult.QuerySpecification()
          .Where(x => this.IsNotPropertyGetterCall(x.CallSpecification.GetMethodInfo()))
          .ToArray();
    }

    private static bool Matches(ICall call, CallSpecAndTarget specAndTarget)
    {
      if (object.ReferenceEquals(call.Target(), specAndTarget.Target))
        return specAndTarget.CallSpecification.IsSatisfiedBy(call);
      return false;
    }

    private bool IsNotPropertyGetterCall(MethodInfo methodInfo)
    {
      return methodInfo.GetPropertyFromGetterCallOrNull() == null;
    }

    private static string GetExceptionMessage(
      CallSpecAndTarget[] querySpec, ICall[] receivedCalls,
      CallSpecAndTarget[] callsSpecifiedButNotReceived, ICall[] callsReceivedButNotSpecified)
    {
      var sequenceFormatter = new SequenceFormatter("\n    ", querySpec, receivedCalls);

      var sequenceFormatterForUnexpectedAndExcessiveCalls = new SequenceFormatter("\n    ", callsSpecifiedButNotReceived,
        callsReceivedButNotSpecified);

      return string.Format("\nExpected to receive only these calls:\n{0}{1}\n\n"
                           + "Actually received the following calls:\n{0}{2}\n\n"
                           + "Calls expected but not received:\n{0}{3}\n\n"
                           + "Calls received but not expected:\n{0}{4}\n\n"
                           + "{5}\n\n"

        , (object) "\n    "
        , sequenceFormatter.FormatQuery()
        , sequenceFormatter.FormatActualCalls()
        , sequenceFormatterForUnexpectedAndExcessiveCalls.FormatQuery()
        , sequenceFormatterForUnexpectedAndExcessiveCalls.FormatActualCalls()
        , "*** Note: calls to property getters are not considered part of the query. ***");
    }

  }
}
