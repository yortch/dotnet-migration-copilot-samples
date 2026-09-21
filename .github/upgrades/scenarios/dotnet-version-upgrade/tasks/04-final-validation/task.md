# 04-final-validation: Build, smoke-test, and document follow-ups

Perform the full solution build after task 03 completes and confirm 0 build errors. Since Test Coverage was skipped, no automated regression suite exists — perform a basic manual/smoke check of core MVC flows (home, courses, students, instructors, departments, notifications) if feasible.

Document any deferred recommendations surfaced during the upgrade: the MSMQ replacement decision (if isolated rather than fully replaced), any binding-redirect entries that were kept rather than removed and why, and any System.Web Adapters limitations encountered.

**Done when**: The solution builds with 0 errors on `net10.0`, and deferred recommendations (MSMQ, binding redirects, adapter limitations) are documented for the user.
