# Contribution evidence: Tlou (Test & CI lead)

Facts gathered from the Git history and test runs, as notes for the Team Contribution Report.
Write the report section itself in your own words from these notes.

## Role

- **Part 3:** Test & CI lead. Responsible for A.1 unit tests, A.2 integration tests, B.1/B.2 pipeline test integration, and the Team Contribution Report.
- **Parts 1–2:** developer. Every commit in the repository up to Part 3 is under Tlou's name.

## Parts 1–2 commits

| Commit | Date | Message | Size |
|---|---|---|---|
| `50777cb` | 2026-08-28 | Initial commit | 116 files |
| `7ee86ec` | 2026-09-25 | Added tax certificate function and donation helpers | 15 files, +525 |
| `d73f8fa` | 2026-09-25 | Shows the tax certificate number on the donation receipt. | 1 file |
| `f7d93c4` | 2026-09-25 | Clarify the volunteer application confirmation. | 1 file |
| `1cd4184` | 2026-09-25 | Merge branch 'volunteer-feature' | merge |
| `bf22fc4` | 2026-09-25 | Add build pipeline | 1 file, +39 |
| `0d6f091` | 2026-09-25 | Point the pipeline at the project feed. | 1 file |
| `cf670c5` | 2026-09-25 | Mention contact details on the volunteer confirmation. | 1 file |

## Part 3 commits (branch `tests/tlou`)

| Commit | Date | What it delivered | Rubric item |
|---|---|---|---|
| `b71834f` | 2026-10-28 | Visual redesign of all pages | (app) |
| `f4db90d` | 2026-10-28 | Azure publish profiles for the web app and function app | (deployment) |
| `b821508` | 2026-10-28 | Unit test project created, added to the solution; certificate number test | A.1 |
| `9864b61` | 2026-10-28 | Certificate number test updated | A.1 |
| `1e45aac` | 2026-10-28 | `MonetaryDonation` display-name and amount validation tests; `VolunteerApplication` validation tests | A.1 |
| `88ad945` | 2026-10-29 | Fix: tax certificate function URL and key read from configuration (was hard-coded to localhost, so donations failed on Azure); fallback certificate number if the function is down | A.1 (enables mocks), C.2 |
| `37969dd` | 2026-10-29 | `DonationsController` goods tests with in-memory database | A.1 |
| `9c35244` | 2026-10-29 | `CreateMoney` tests using Moq and a fake HTTP handler to stub the function | A.1 |
| `10243ed` | 2026-10-29 | `CustomWebApplicationFactory`: in-memory SQLite database and stubbed function for integration tests | A.2 |
| `44097e3` | 2026-10-29 | Integration tests: donor money donation through the form; zero-amount error case | A.2 |
| `78c8f4b` | 2026-10-29 | Integration tests: visitor blocked from the staff desk; staff sign-in opens the desk | A.2 |
| `711b8c9` | 2026-10-29 | Pipeline: Build and test stage, separate unit and integration test steps, test results and coverage published; triggers on `main` and `tests/*` | B.1, B.2 |

## Results

- **Unit tests (A.1):** 15 passing
  - `DonationHelperTests`: 3 (certificate number formatting)
  - `MonetaryDonationTests`: 5 (display name, amount validation)
  - `VolunteerApplicationTests`: 3 (required fields, email format)
  - `DonationsControllerTests`: 4 (goods valid and invalid; money with the function working and with it down), using Moq
- **Integration tests (A.2):** 4 passing in `DonationFlowIntegrationTests`
  - donor money flow form → function → database
  - zero-amount error case
  - visitor redirected to sign-in
  - staff sign-in opens the operations desk
- **Coverage:** about 53% of the app's own code (generated code excluded); `GiftOfTheGivers.Helpers` package 100%.
- **Pipeline (B.1/B.2):** run triggered by commit `711b8c9` on `tests/tlou`, passed.
  - Pipeline run ID: _fill in_
  - Coverage % shown on the Code Coverage tab: _fill in_
- **Pull request** `tests/tlou` → `main`: _fill in PR number once opened_

## Outline for your report section

1. **Role:** one sentence naming the role.
2. **Responsibilities:** the rubric items owned (A.1, A.2, B.1, B.2, contribution report).
3. **What was delivered:** a short summary of the tests, the pipeline stage and the configuration fix.
4. **Evidence:** a reference to the commit table, the PR number, the pipeline run ID and the screenshot figure numbers.
5. **Challenges** (optional): for example the hard-coded function URL found while writing tests.

## AI assistance to consider declaring

The brief requires AI use to stay within IIE guidelines. In these commits, Claude made most of the change rather than only debugging:

- `b71834f`: visual redesign
- `88ad945`: configuration fix in `DonationsController` and `Program.cs`
- `711b8c9`: the pipeline YAML and `coverlet.runsettings`

Claude also corrected and completed several test files (the controller test helpers, `CustomWebApplicationFactory`, `FakeHttpMessageHandler`). Check your module's declaration requirements and decide with your group how to disclose this.
