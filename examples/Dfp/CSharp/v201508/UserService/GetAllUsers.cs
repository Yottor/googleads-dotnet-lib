// Copyright 2015, Google Inc. All Rights Reserved.
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
using Google.Api.Ads.Dfp.Util.v201508;
using Google.Api.Ads.Dfp.v201508;

using System;

namespace Google.Api.Ads.Dfp.Examples.CSharp.v201508 {
  /// <summary>
  /// This code example gets all users. To create users, run CreateUsers.cs.
  /// </summary>
  public class GetAllUsers : SampleBase {
    /// <summary>
    /// Returns a description about the code example.
    /// </summary>
    public override string Description {
      get {
        return "This code example gets all users. To create users, run CreateUsers.cs.";
      }
    }

    /// <summary>
    /// Main method, to run this code example as a standalone application.
    /// </summary>
    /// <param name="args">The command line arguments.</param>
    public static void Main() {
      GetAllUsers codeExample = new GetAllUsers();
      Console.WriteLine(codeExample.Description);
      codeExample.Run(new DfpUser());
    }

    /// <summary>
    /// Run the code example.
    /// </summary>
    /// <param name="user">The DFP user object running the code example.</param>
    public void Run(DfpUser user) {
      // Get the UserService.
      UserService userService = (UserService) user.GetService(DfpService.v201508.UserService);

      // Create a statement to get all users.
      StatementBuilder statementBuilder = new StatementBuilder()
          .OrderBy("id ASC")
          .Limit(StatementBuilder.SUGGESTED_PAGE_LIMIT);

      // Sets defaults for page and statement.
      UserPage page = new UserPage();

      try {
        do {
          // Get users by statement.
          page = userService.getUsersByStatement(statementBuilder.ToStatement());

          if (page.results != null && page.results.Length > 0) {
            int i = page.startIndex;
            foreach (User usr in page.results) {
              Console.WriteLine("{0}) User with ID = '{1}', email = '{2}', and role = '{3}'" +
                  " was found.", i, usr.id, usr.email, usr.roleName);
              i++;
            }
          }
          statementBuilder.IncreaseOffsetBy(StatementBuilder.SUGGESTED_PAGE_LIMIT);
        } while (statementBuilder.GetOffset() < page.totalResultSetSize);

        Console.WriteLine("Number of results found: {0}", page.totalResultSetSize);
      } catch (Exception e) {
        Console.WriteLine("Failed to get all users. Exception says \"{0}\"",
            e.Message);
      }
    }
  }
}
