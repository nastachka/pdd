using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PDD.Utility;
using SQLite;

namespace PDD.Models
{
    internal class TestQuestion
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PictureGroupId { get; set; }
        public string Question { get; set; }

        public static List<TestQuestion> CurrentQuestions;
        public static int CorrectAnswersNumber;

        public static List<TestQuestion> GetRandomQuestions(int n)
        {
            ObservableCollection<TestQuestion> allTests = ReadDataHelper.GetAll<TestQuestion>();
            IOrderedEnumerable<TestQuestion> shuffledTests = allTests.OrderBy(a => Guid.NewGuid());
            return shuffledTests.Take(n).ToList();
        }
    }
}
