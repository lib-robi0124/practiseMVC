# Stop QForm

Date: 2025-10-14

Summary
- Issue: After submitting the last active questionnaire form, navigation wrapped back to the first form instead of stopping and showing the Thank You page.
- Root cause: The next-form lookup used modulo arithmetic and a hardcoded Id range (1..13), which forced wrap-around rather than stopping at the end.
- Fix: Determine the next active form strictly by Id > currentFormId without wrap-around. When no next active form exists, redirect to the Thank You page. Also expose progress counters and NextFormId to the view for correct UI behavior, and harden the Thank You page markup.

Changes
1) Data Access: Stop wrap-around in next-form selection
- File: GlasAnketa.DataAccess/Implementations/QuestionFormRepository.cs
- Method: GetNextActiveFormAsync(int currentFormId)
- Before: Used a hardcoded 1..13 range and modulo wrap-around logic to find the next active form.
- After: Select the next active form with Id > currentFormId; if none exists, return null.

2) Controller: Compute active sequence, NextFormId, and progress
- File: GlasAnketa/Controllers/QuestionnaireController.cs
- Action: ShowForm(int? formId)
- Before: Loaded a single active form without ordering/progress context; did not compute NextFormId.
- After: Builds ordered active forms list, picks the requested form (or first active), computes NextFormId, and sets ViewBag.CurrentFormNumber, ViewBag.TotalForms, ViewBag.IsLastForm, ViewBag.NextFormId. Populates FormSubmissionVM.NextFormId.

3) View: Thank You page resilient to missing model
- File: GlasAnketa/Views/Questionnaire/ThankYou.cshtml
- Change: Removed references to Model.SubmittedCount and Model.SubmissionDate so it renders cleanly without a model.

Detailed diffs (key excerpts)

- GlasAnketa.DataAccess/Implementations/QuestionFormRepository.cs
  - GetNextActiveFormAsync
  - Old (wrap-around with hardcoded 1..13):
    - Queried active forms within Id 1..13, then used modulo arithmetic to move through candidates and wrap back to 1.
  - New (no wrap-around):
    - Query next active form strictly by Id > currentFormId ordered by Id. Return null if none.

- GlasAnketa/Controllers/QuestionnaireController.cs
  - ShowForm
  - New logic:
    - activeForms = await GetAllActiveFormsAsync(), ordered by Id
    - If formId provided but inactive, choose first active form with Id > formId; else Thank You
    - idx = index of current form in sequence
    - nextFormId = idx < Count-1 ? activeForms[idx+1].Id : null
    - vm.NextFormId = nextFormId
    - ViewBag.CurrentFormNumber = idx + 1
    - ViewBag.TotalForms = activeForms.Count
    - ViewBag.IsLastForm = nextFormId == null
    - ViewBag.NextFormId = nextFormId

- GlasAnketa/Views/Questionnaire/ThankYou.cshtml
  - Removed the completion-stats block referencing Model.SubmittedCount/SubmissionDate to avoid null model exceptions.

Behavior after fix
- When submitting a form:
  - If there is a next active form (by Id order), the user is redirected to that form.
  - If the current form is the last active form, the next-form lookup returns null and the user is redirected to the Thank You page.
- The UI displays:
  - “Save & Continue →” when there is a next form.
  - “Submit All Answers” on the last form (NextFormId == null).
  - Progress indicator shows “Questionnaire X of Y”.

Files modified
- GlasAnketa.DataAccess/Implementations/QuestionFormRepository.cs
- GlasAnketa/Controllers/QuestionnaireController.cs
- GlasAnketa/Views/Questionnaire/ThankYou.cshtml

Verification steps
1) Build and run
- dotnet build GlasAnketa/GlasAnketa.csproj
- dotnet run --project GlasAnketa/GlasAnketa.csproj

2) Navigate through forms
- Login and start the questionnaire
- Submit any mid-sequence form → next active form opens
- Submit the last active form → redirected to Thank You page (no loop back)

3) Toggle active forms (optional)
- Deactivate the current last form and repeat: the new last active form should end on Thank You.

Impact
- Only the form navigation behavior was changed; no database schema changes.
- No wrap-around occurs, even if form Ids are sparse or outside 1..13.

Rollback
- Revert the three files listed above to their previous versions.

Notes
- The view already uses Model.NextFormId to change the submit button label. With the controller now populating NextFormId reliably, the button updates as expected.
- If you later want to show submission stats on the Thank You page, add a dedicated view model and pass it explicitly when redirecting.
