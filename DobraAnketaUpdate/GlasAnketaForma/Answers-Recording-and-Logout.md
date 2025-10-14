# Answers Recording and Logout Fix

Date: 2025-10-14

Summary
- Fixed the Thank You page “Return to Login” button to logout via POST with anti-forgery (prevents navigation errors and CSRF issues).
- Enabled recording multiple answers per user/question/form (keeps history of submissions with different AnsweredDate values).

Changes
1) Thank You page logout
- File: GlasAnketa/Views/Questionnaire/ThankYou.cshtml
- Before: Anchor tag to /Account/Logout (GET), which conflicted with your POST-only Logout action and anti-forgery requirement.
- After: Replaced with a POST form including @Html.AntiForgeryToken().

2) Allow multiple answers (history)
- File: GlasAnketa.DataAccess/DataContext/AppDbContext.cs
  - Removed .IsUnique() on the composite index (UserId, QuestionId, QuestionFormId). Retained a non-unique index for performance.
- New Migration: 20251014083550_AllowMultipleAnswersPerUserQuestion
  - Drops the unique index on Answers(UserId, QuestionId, QuestionFormId) and recreates it as non-unique.
- File: GlasAnketa.DataAccess/Implementations/AnswerRepository.cs
  - SubmitAnswersAsync now inserts a new Answer row for each submission instead of updating an existing row.

Behavior after fix
- Thank You “Return to Login” reliably logs out and redirects to login.
- Each time a user submits a questionnaire, new Answer rows are inserted. Previous answers remain in the table with their original AnsweredDate values.

Verification
- Logout: Reach Thank You page, click “Return to Login” → you return to /Account/Login without errors.
- Multiple answers: Submit answers for the same form/questions multiple times; verify multiple rows exist in Answers for the same (UserId, QuestionId, QuestionFormId) with distinct AnsweredDate values.

Notes
- Any analytics that assumed a single answer per user/question should be updated to aggregate by latest AnsweredDate if needed.
