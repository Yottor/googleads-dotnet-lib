// Copyright 2016, Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Ads.Dfp.Lib;
using Google.Api.Ads.Dfp.Util.v201605;
using Google.Api.Ads.Dfp.v201605;

using System;
using System.Collections.Generic;
namespace Google.Api.Ads.Dfp.Examples.CSharp.v201605 {
  /// <summary>
  /// This example gets predefined custom targeting keys and values.
  /// </summary>
  public class GetPredefinedCustomTargetingKeysAndValues : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This example gets predefined custom targeting keys and values.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main() {
      GetPredefinedCustomTargetingKeysAndValues codeExample =
          new GetPredefinedCustomTargetingKeysAndValues();
      Console.WriteLine(codeExample.Description);

      codeExample.Run(new DfpUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser user) {
      CustomTargetingService customTargetingService =
          (CustomTargetingService) user.GetService(DfpService.v201605.CustomTargetingService);

      List<long> customTargetingKeyIds = getPredefinedCustomTargetingKeyIds(user);

              // Create a statement to select custom targeting values.
        StatementBuilder statementBuilder = new StatementBuilder()
            .Where("customTargetingKeyId = :customTargetingKeyId")
            .OrderBy("id ASC")
            .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT);

      foreach (long customTargetingKeyId in customTargetingKeyIds) {
        // Set the custom targeting key ID to select from.
        statementBuilder.AddValue("customTargetingKeyId", customTargetingKeyId);

        // Retrieve a small amount of custom targeting values at a time, paging through
        // until all custom targeting values have been retrieved.
        CustomTargetingValuePage page = new CustomTargetingValuePage();
        try {
          do {
            page = customTargetingService.getCustomTargetingValuesByStatement(
                statementBuilder.ToStatement());

            if (page.results != null) {
              // Print out some information for each custom targeting value.
              int i = page.startIndex;
              foreach (CustomTargetingValue customTargetingValue in page.results) {
                Console.WriteLine("{0}) Custom targeting value with ID \"{1}\", name \"{2}\", "
                    + "display name \"{3}\", and custom targeting key ID \"{4}\" was found.",
                    i++,
                    customTargetingValue.id,
                    customTargetingValue.name,
                    customTargetingValue.displayName,
                    customTargetingValue.customTargetingKeyId);
              }
            }

            statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
          } while (statementBuilder.GetOffset() < page.totalResultSetSize);

          Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
        } catch (Exception e) {
          Console.WriteLine("Failed to get custom targeting values. Exception says \"{0}\"",
              e.Message);
        }
      }
    }

    private static List<long> getPredefinedCustomTargetingKeyIds(DfpUser user) {
      List<long> customTargetingKeyIds = new List<long>();

      CustomTargetingService customTargetingService = 
          (CustomTargetingService) user.GetService(DfpService.v201605.CustomTargetingService);

      StatementBuilder statementBuilder = new StatementBuilder()
          .Where("type = :type")
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT)
          .AddValue("type", CustomTargetingKeyType.PREDEFINED.ToString());

      CustomTargetingKeyPage page = new CustomTargetingKeyPage();
      do {
        page = customTargetingService.getCustomTargetingKeysByStatement(
            statementBuilder.ToStatement());

        if (page.results != null) {
         // Print out some information for each custom targeting value.
          int i = page.startIndex;
          foreach (CustomTargetingKey customTargetingKey in page.results) {
            Console.WriteLine("{0}) Custom targeting value with ID \"{1}\", name \"{2}\", "
                + "and display name \"{3}\" was found.",
                i++,
                customTargetingKey.id,
                customTargetingKey.name,
                customTargetingKey.displayName);
            customTargetingKeyIds.Add(customTargetingKey.id);
          }
        }

        statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
      } while (statementBuilder.GetOffset() < page.totalResultSetSize);

      return customTargetingKeyIds;
    }
  }
}
