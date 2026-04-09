# 04-final-validation: Validate the upgraded solution and finalize workflow artifacts

## Objective

Run the final restore/build validation for the upgraded solution, confirm the remaining task statuses, and record the outcome in the workflow artifacts.

## Validation Results

- `dotnet build ContosoUniversity.sln -warnaserror` succeeded on the upgraded `net10.0` solution with Razor compilation enabled
- Build output contains 0 warnings and 0 errors
- No test projects exist in the repository, so there were no automated tests to execute for this sample

## Artifact Reconciliation

- `tasks.md` now reflects the completed migration subtasks and the intentionally failed SDK-style stabilization task
- Scenario task folders include `progress-details.md` for every executed task and subtask
- Notification polling now reads from the database-backed notification service without the old MSMQ wording in the dashboard view
