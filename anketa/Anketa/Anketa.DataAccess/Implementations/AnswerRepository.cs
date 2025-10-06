using Anketa.DataAccess.Interfaces;
using Anketa.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Anketa.DataAccess.Implementations
{
    public class AnswerRepository : Repository<Answer>, IAnswerRepository
    {
        private readonly AppDbContext _context;

        public AnswerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        {
            Console.WriteLine($"=== REPOSITORY: Starting answer submission ===");
            Console.WriteLine($"User: {userId}, Form: {formId}, Answers: {answers.Count}");

            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var answer in answers)
                {
                    Console.WriteLine($"Processing answer - Question {answer.Key}: {answer.Value} (Type: {answer.Value.GetType()})");

                    var question = await _context.Questions
                        .Include(q => q.QuestionType)
                        .FirstOrDefaultAsync(q => q.Id == answer.Key);

                    if (question == null)
                    {
                        Console.WriteLine($"❌ Question {answer.Key} not found in database!");
                        continue;
                    }

                    Console.WriteLine($"Found question: {question.Text} (Type: {question.QuestionType.Name})");

                    // Check for existing answer
                    var existingAnswer = await _context.Answers
                        .FirstOrDefaultAsync(a => a.UserId == userId &&
                                                a.QuestionId == answer.Key &&
                                                a.QuestionFormId == formId);

                    if (existingAnswer != null)
                    {
                        Console.WriteLine($"Updating existing answer for question {answer.Key}");
                        UpdateAnswerValue(existingAnswer, question.QuestionType.Name, answer.Value);
                        Update(existingAnswer);
                    }
                    else
                    {
                        Console.WriteLine($"Creating new answer for question {answer.Key}");
                        var newAnswer = new Answer
                        {
                            UserId = userId,
                            QuestionId = answer.Key,
                            QuestionFormId = formId,
                            AnsweredDate = DateTime.UtcNow
                        };
                        UpdateAnswerValue(newAnswer, question.QuestionType.Name, answer.Value);
                        await AddAsync(newAnswer);
                    }
                }

                Console.WriteLine("Saving changes to database...");
                var saveResult = await _context.SaveChangesAsync();
                Console.WriteLine($"Save result: {saveResult} changes");

                await transaction.CommitAsync();
                Console.WriteLine("✅ Transaction committed successfully!");

                return saveResult > 0;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"❌ TRANSACTION ROLLED BACK: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");

                // Log inner exception if exists
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                return false;
            }
        }

        private void UpdateAnswerValue(Answer answer, string questionType, object value)
        {
            Console.WriteLine($"Updating answer value - Type: {questionType}, Value: {value}");

            if (questionType == "Scale")
            {
                if (value is int scaleValue)
                {
                    answer.ScaleValue = scaleValue;
                    answer.TextValue = null;
                    Console.WriteLine($"✅ Set scale value: {scaleValue}");
                }
                else if (value is string stringValue && int.TryParse(stringValue, out int parsedValue))
                {
                    answer.ScaleValue = parsedValue;
                    answer.TextValue = null;
                    Console.WriteLine($"✅ Parsed and set scale value: {parsedValue} from string '{stringValue}'");
                }
                else
                {
                    Console.WriteLine($"❌ Invalid scale value: {value} (Type: {value.GetType()})");
                }
            }
            else if (questionType == "Text")
            {
                answer.TextValue = value?.ToString();
                answer.ScaleValue = null;
                Console.WriteLine($"✅ Set text value: {value}");
            }
            else
            {
                Console.WriteLine($"❌ Unknown question type: {questionType}");
            }
        }
        //public async Task<bool> SubmitAnswersAsync(int userId, int formId, Dictionary<int, object> answers)
        //{
        //    using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();

        //    try
        //    {
        //        foreach (var answer in answers)
        //        {
        //            var question = await _context.Questions
        //                .Include(q => q.QuestionType)
        //                .FirstOrDefaultAsync(q => q.Id == answer.Key);

        //            if (question == null) continue;

        //            // ✅ IMPROVED: Better type handling
        //            object processedValue = ProcessAnswerValue(answer.Value, question.QuestionType.Name);

        //            if (processedValue == null)
        //            {
        //                System.Diagnostics.Debug.WriteLine($"Invalid value for question {answer.Key}: {answer.Value}");
        //                continue;
        //            }

        //            var existingAnswer = await _context.Answers
        //                .FirstOrDefaultAsync(a => a.UserId == userId &&
        //                                        a.QuestionId == answer.Key &&
        //                                        a.QuestionFormId == formId);

        //            if (existingAnswer != null)
        //            {
        //                // Update existing answer
        //                UpdateAnswerValue(existingAnswer, question.QuestionType.Name, processedValue);
        //                Update(existingAnswer);
        //                System.Diagnostics.Debug.WriteLine($"Updated answer for question {answer.Key}");
        //            }
        //            else
        //            {
        //                // Create new answer
        //                var newAnswer = new Answer
        //                {
        //                    UserId = userId,
        //                    QuestionId = answer.Key,
        //                    QuestionFormId = formId,
        //                    AnsweredDate = DateTime.UtcNow
        //                };
        //                UpdateAnswerValue(newAnswer, question.QuestionType.Name, processedValue);
        //                await AddAsync(newAnswer);
        //                System.Diagnostics.Debug.WriteLine($"Created new answer for question {answer.Key}");
        //            }
        //        }

        //        var saveResult = await _context.SaveChangesAsync();
        //        await transaction.CommitAsync();

        //        System.Diagnostics.Debug.WriteLine($"Saved {saveResult} changes to database");
        //        return saveResult > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        await transaction.RollbackAsync();
        //        System.Diagnostics.Debug.WriteLine($"Error in SubmitAnswersAsync: {ex.Message}");
        //        System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
        //        return false;
        //    }
        //}
        //// ✅ NEW: Process answer values with better type handling
        //private object ProcessAnswerValue(object value, string questionType)
        //{
        //    if (value == null) return null;

        //    if (questionType == "Scale")
        //    {
        //        // Handle string "5" → int 5 conversion
        //        if (value is string stringValue)
        //        {
        //            if (int.TryParse(stringValue, out int intValue))
        //            {
        //                return intValue;
        //            }
        //        }
        //        else if (value is int intValue)
        //        {
        //            return intValue;
        //        }
        //    }
        //    else if (questionType == "Text")
        //    {
        //        // Ensure text is properly handled
        //        return value.ToString();
        //    }

        //    return null;
        //}
        //private void UpdateAnswerValue(Answer answer, string questionType, object value)
        //{
        //    if (questionType == "Scale" && value is int scaleValue)
        //    {
        //        answer.ScaleValue = scaleValue;
        //        answer.TextValue = null;
        //        System.Diagnostics.Debug.WriteLine($"Set scale value {scaleValue} for answer {answer.QuestionId}");
        //    }
        //    else if (questionType == "Text" && value is string textValue)
        //    {
        //        answer.TextValue = textValue;
        //        answer.ScaleValue = null;
        //        System.Diagnostics.Debug.WriteLine($"Set text value for answer {answer.QuestionId}");
        //    }
        //    else
        //    {
        //        System.Diagnostics.Debug.WriteLine($"Failed to set value for answer {answer.QuestionId}, type: {questionType}, value: {value}");
        //    }
        //}


        public async Task<IEnumerable<Answer>> GetUserAnswersAsync(int userId, int formId)
        {
            return await _context.Answers
                .Where(a => a.UserId == userId && a.QuestionFormId == formId)
                .Include(a => a.Question)
                .ThenInclude(q => q.QuestionType)
                .ToListAsync();
        }

        public async Task<bool> HasUserCompletedFormAsync(int userId, int formId)
        {
            return await _context.Answers
                .AnyAsync(a => a.UserId == userId && a.QuestionFormId == formId);
        }
        public async Task<IEnumerable<Answer>> GetFormAnswersAsync(int formId)
        {
            return await _context.Answers
                .Where(a => a.QuestionFormId == formId)
                .Include(a => a.Question)
                .Include(a => a.User)
                .ToListAsync();
        }
    }
}
