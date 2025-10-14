# Answers Recording and Logout Fix (plus related updates)

Date: 2025-10-14

Summary
- Fixed the Thank You page “Return to Login” button to logout via POST with anti-forgery (prevents navigation errors and CSRF issues).
- Enabled recording multiple answers per user/question/form (keeps history of submissions with different AnsweredDate values).
- Fixed form submission reliability (anti-forgery and stale client state) and cleaned up unused assets.
- Added missing Results view to resolve view-not-found error.

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
  - Applied with dotnet-ef database update.
- File: GlasAnketa.DataAccess/Implementations/AnswerRepository.cs
  - SubmitAnswersAsync now inserts a new Answer row for every submission (keeps history).

3) Form submission robustness (anti-forgery + stale state)
- File: GlasAnketa/Views/Questionnaire/Form.cshtml
  - Removed duplicate @Html.AntiForgeryToken() (form tag helper already emits a token).
  - Enforced asp-antiforgery="true" on the form for clarity.
  - Removed client-side localStorage auto-save/restore and added a startup cleanup that deletes any formProgress_* keys to prevent stale answers across logins.

4) Results view not found (admin)
- Controller: ResultsController.ViewResults returns View() which searches Views/Results/ViewResults.cshtml.
- Added file: GlasAnketa/Views/Results/ViewResults.cshtml (includes POST logout form).
- Removed duplicate/old view: GlasAnketa/Views/Admin/ViewResults.cshtml.

5) Navigation: stop wrap-around at last active form (related)
- Repository: GlasAnketa.DataAccess/Implementations/QuestionFormRepository.cs
  - GetNextActiveFormAsync: select next active by Id > currentFormId (no wrap-around) and return null at end.
- Controller: GlasAnketa/Controllers/QuestionnaireController.cs
  - ShowForm builds ordered active list, computes NextFormId, sets ViewBag.CurrentFormNumber, ViewBag.TotalForms, ViewBag.IsLastForm, and ViewBag.NextFormId.
- View: Thank You shows once last form is submitted.
- See also Stop-QForm.md for a focused write-up on navigation changes.

6) Cleanup of unused assets
- Removed CSS not referenced by any view:
  - GlasAnketa/wwwroot/css/questionnaire.css
  - GlasAnketa/wwwroot/css/thankyou.css
- Consolidated to modern-questionnaire.css, viewresults.css, login.css, managequestions.css, admin.css, site.css, and logout-styles.css as actively referenced.

Behavior after fixes
- Thank You “Return to Login” reliably logs out and goes to login.
- Each questionnaire submission stores a new Answer row; historical answers remain with their original AnsweredDate.
- Questionnaire form submissions are stable (no 400 due to token collisions) and do not preload stale answers from previous sessions.
- Admin > Results > View Results works; the view renders instead of throwing view-not-found.
- Navigation proceeds form-by-form across active forms and ends on Thank You (no wrap-around).

Verification
- Logout: Reach Thank You, click “Return to Login” → back to /Account/Login.
- Multiple answers: Submit same form/questions multiple times → multiple rows exist in Answers for the same (UserId, QuestionId, QuestionFormId) with distinct AnsweredDate values.
- Results view: Login as admin, open https://localhost:7011/Results/ViewResults → page loads.
- Navigation: Start Questionnaire at first active, submit through to last → hits Thank You and stops.

Files touched (summary)
- Views: Questionnaire/Form.cshtml, Questionnaire/ThankYou.cshtml, Results/ViewResults.cshtml (new), removed Admin/ViewResults.cshtml.
- DataAccess: AppDbContext.cs (index), Implementations/AnswerRepository.cs (insert history), Implementations/QuestionFormRepository.cs (next form logic), Migrations/*AllowMultipleAnswersPerUserQuestion* (new), AppDbContextModelSnapshot.cs.
- Controllers: QuestionnaireController.cs (navigation/progress).
- Assets: Removed wwwroot/css/questionnaire.css and thankyou.css.

Notes
- Analytics that assumed one answer per user/question should aggregate (e.g., latest by AnsweredDate) if needed.
- For consistency and CSRF protection, consider ensuring all logout actions in views are POST with anti-forgery (many have already been updated).
