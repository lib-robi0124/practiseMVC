using GlasAnketa.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace GlasAnketa.DataAccess.Extensions
{
    public static class DataSeedExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
          
            // Seed Question Types
            modelBuilder.Entity<QuestionType>().HasData(
                new QuestionType { Id = 1, Name = "Scale", Description = "1-10 Scale" },
                new QuestionType { Id = 2, Name = "Text", Description = "Text Answer" }
            );
            // Seed Question Forms with OU
            modelBuilder.Entity<QuestionForm>().HasData(
                new QuestionForm { Id = 1, Title = "Општо задоволство", Description = "Overall Satisfaction" },
                new QuestionForm { Id = 2, Title = "Обврска кон компанијата", Description = "Commitment to the Company" },
                new QuestionForm { Id = 3, Title = "Професионален развој", Description = "Professional Development" },
                new QuestionForm { Id = 4, Title = "Рамнотежа помеѓу работата и животот", Description = "Work-Life Balance" },
                new QuestionForm { Id = 5, Title = "Комуникација и соработка", Description = "Communication and Collaboration" },
                new QuestionForm { Id = 6, Title = "Лидерство", Description = "Leadership" },
                new QuestionForm { Id = 7, Title = "Организациска култура", Description = "Organizational Culture" },
                new QuestionForm { Id = 8, Title = "Работна средина", Description = "Work Environment" },
                new QuestionForm { Id = 9, Title = "Награди и признанија", Description = "Rewards and Recognition" },
                new QuestionForm { Id = 10, Title = "Иновации и промени", Description = "Innovation and Changes" },
                new QuestionForm { Id = 11, Title = "Конечна евалуација", Description = "Final Evaluation" },
                new QuestionForm { Id = 12, Title = "Отворени прашања", Description = "Open Questions" }
            );
            // Seed Questions (UserId = 1 for Admin user, No OU)
            modelBuilder.Entity<Question>().HasData(
                // Form 1: Општо задоволство (Scale questions)
                new Question { Id = 1, Text = "Задоволен сум од мојата моментална работа", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
                new Question { Id = 2, Text = "Чувствувам дека мојата работа е ценета во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },
                new Question { Id = 3, Text = "Се чувствувам мотивиран да одам на работа секој ден", QuestionTypeId = 1, QuestionFormId = 1, UserId = 1 },

                // Form 2: Обврска кон компанијата (Scale questions)
                new Question { Id = 4, Text = "Се чувствувам горд што работам за оваа компанија", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },
                new Question { Id = 5, Text = "Со задоволство ја препорачувам оваа компанија како работно место на пријателите и семејството", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },
                new Question { Id = 6, Text = "Се гледам себеси како долгорочно работам во оваа компанија", QuestionTypeId = 1, QuestionFormId = 2, UserId = 1 },

                // Form 3: Професионален развој (Scale questions)
                new Question { Id = 7, Text = "Имам можности за професионален развој и напредување", QuestionTypeId = 1, QuestionFormId = 3, UserId = 1 },
                new Question { Id = 8, Text = "Добивам конструктивна повратна информација за мојата работа", QuestionTypeId = 1, QuestionFormId = 3, UserId = 1 },
                new Question { Id = 9, Text = "Компанијата обезбедува соодветна обука и ресурси за мојот развој", QuestionTypeId = 1, QuestionFormId = 3, UserId = 1 },

                // Form 4: "Рамнотежа помеѓу работата и животот" (Scale questions)
                new Question { Id = 10, Text = "Компанијата поддржува здрава рамнотежа помеѓу работата и личниот живот", QuestionTypeId = 1, QuestionFormId = 4, UserId = 1 },
                new Question { Id = 11, Text = "Можам ефикасно да управувам со стресот поврзан со работата", QuestionTypeId = 1, QuestionFormId = 4, UserId = 1 },
                new Question { Id = 12, Text = "Мојот работен распоред ми овозможува да ги исполнувам моите лични обврски", QuestionTypeId = 1, QuestionFormId = 4, UserId = 1 },

                // Form 5: "Комуникација и соработка" (Scale questions)
                new Question { Id = 13, Text = "Комуникацијата во мојот тим е ефикасна", QuestionTypeId = 1, QuestionFormId = 5, UserId = 1 },
                new Question { Id = 14, Text = "Се чувствувам удобно да ги искажувам моите идеи и мислења на работа", QuestionTypeId = 1, QuestionFormId = 5, UserId = 1 },
                new Question { Id = 15, Text = "Соработката помеѓу одделенијата е ефикасна", QuestionTypeId = 1, QuestionFormId = 5, UserId = 1 },

                // Form 6: "Лидерство" (Scale questions)
                new Question { Id = 16, Text = "Му верувам на раководството на компанијата", QuestionTypeId = 1, QuestionFormId = 6, UserId = 1 },
                new Question { Id = 17, Text = "Мојот директен менаџер ме поддржува во остварувањето на моите цели", QuestionTypeId = 1, QuestionFormId = 6, UserId = 1 },
                new Question { Id = 18, Text = "Важните одлуки на компанијата се пренесуваат транспарентно", QuestionTypeId = 1, QuestionFormId = 6, UserId = 1 },

                // Form 7: "Организациска култура" (Scale questions)
                new Question { Id = 19, Text = "Вредностите на компанијата се усогласуваат со моите лични вредности", QuestionTypeId = 1, QuestionFormId = 7, UserId = 1 },
                new Question { Id = 20, Text = "Се чувствувам вклучено и почитувано на работа", QuestionTypeId = 1, QuestionFormId = 7, UserId = 1 },
                new Question { Id = 21, Text = "Компанијата промовира различност и инклузија", QuestionTypeId = 1, QuestionFormId = 7, UserId = 1 },

                // Form 8: "Работна средина" (Scale questions)
                new Question { Id = 22, Text = "Ги имам сите ресурси потребни за ефикасно извршување на моите задачи", QuestionTypeId = 1, QuestionFormId = 8, UserId = 1 },
                new Question { Id = 23, Text = "Физичката работна средина е удобна и поволна за продуктивност", QuestionTypeId = 1, QuestionFormId = 8, UserId = 1 },
                new Question { Id = 24, Text = "Се чувствувам безбедно на работа", QuestionTypeId = 1, QuestionFormId = 8, UserId = 1 },

                // Form 9: "Награди и признанија" (Scale questions)
                new Question { Id = 25, Text = "Задоволен сум од мојот пакет компензации и бенефиции", QuestionTypeId = 1, QuestionFormId = 9, UserId = 1 },
                new Question { Id = 26, Text = "Моите напори и достигнувања се препознаени и ценети", QuestionTypeId = 1, QuestionFormId = 9, UserId = 1 },
                new Question { Id = 27, Text = "Постојат јасни можности за напредување во кариерата во рамките на компанијата", QuestionTypeId = 1, QuestionFormId = 9, UserId = 1 },

                // Form 10: "Иновации и промени" (Scale questions)
                new Question { Id = 28, Text = "Компанијата ги поттикнува иновациите и креативното размислување", QuestionTypeId = 1, QuestionFormId = 10, UserId = 1 },
                new Question { Id = 29, Text = "Подготвен сум да ги усвојам промените имплементирани во компанијата", QuestionTypeId = 1, QuestionFormId = 10, UserId = 1 },
                new Question { Id = 30, Text = "Идеите и предлозите на вработените се разгледуваат и се спроведуваат кога е соодветно", QuestionTypeId = 1, QuestionFormId = 10, UserId = 1 },

                // Form 11: "Конечна евалуација" (Scale and text questions)
                new Question { Id = 31, Text = "Kолку е веројатно да ја препорачате оваа компанија како работно место на пријател или колега", QuestionTypeId = 1, QuestionFormId = 11, UserId = 1 },
                new Question { Id = 32, Text = "Како ја гледате вашата иднина во оваа компанија во следните 2-3 години?", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },
                new Question { Id = 33, Text = "разно", QuestionTypeId = 2, QuestionFormId = 11, UserId = 1 },

                // Form 12: Отворени прашања (Text questions)
                new Question { Id = 34, Text = "Што најмногу ви се допаѓа на вашето сегашно работно место?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
                new Question { Id = 35, Text = "Кои се најголемите предизвици со кои се соочувате на работа?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 },
                new Question { Id = 36, Text = "Какви предлози имате за подобрување на работната средина или процесите на компанијата?", QuestionTypeId = 2, QuestionFormId = 12, UserId = 1 }
            );
           
        }
    }
}
