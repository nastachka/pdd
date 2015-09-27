using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class TestsPage
    {
        private const int NumberOfQuestions = 10;
        private int _currentQuestionNumber;
        private bool _isLastQuestion;
        private bool _hasCorrectAnswer;
        private bool _hasIncorrectAnswer;
        private TestQuestion _currentQuestion;

        public TestsPage()
        {
            InitializeComponent();
        }

        private void AddButton()
        {
            var button = new Button
            {
                Content = _isLastQuestion ? "Посмотреть результат" : "Следующий вопрос",
                FontSize = 24,
                Margin = new Thickness(10, 30, 10, 10),
                HorizontalAlignment = HorizontalAlignment.Center,
                Width = 300,
                Height = 70,
            };
            if (_isLastQuestion)
            {
                button.Click += GetResultsClick;
            }
            else
            {
                button.Click += NextQuestionClick;
            }

            TestPanel.Children.Add(button);
        }

        private void Answer_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                var answer = ((TextBlock) sender);
                int isCorrect = Int32.Parse(answer.Tag.ToString());
                if (isCorrect == 1)
                {
                    if (_hasCorrectAnswer)
                    {
                        return;
                    }
                    if (!_hasIncorrectAnswer)
                    {
                        TestQuestion.CorrectAnswersNumber++;
                    }
                    answer.Foreground = new SolidColorBrush(Colors.Green);
                    AddButton();
                    _hasCorrectAnswer = true;
                }
                else
                {
                    answer.Foreground = new SolidColorBrush(Colors.Red);
                    _hasIncorrectAnswer = true;
                }
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void NextQuestionClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof (TestsPage), ++_currentQuestionNumber);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void GetResultsClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof (TestResultPage), NumberOfQuestions);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void AddProgressStatus()
        {
            var progress = new TextBlock
            {
                Text = (_currentQuestionNumber + 1) + " из " + NumberOfQuestions,
                Margin = new Thickness(10, 10, 10, 10),
                FontSize = 20,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center
            };

            TestPanel.Children.Add(progress);
        }

        private void AddQuestion()
        {
            var question = new TextBlock
            {
                Text = _currentQuestion.Question,
                Margin = new Thickness(10, 10, 10, 10),
                FontSize = 28,
                TextWrapping = TextWrapping.Wrap
            };

            TestPanel.Children.Add(question);
        }

        private void AddImage()
        {
            Picture.AddImageItems(TestPanel, Picture.GetPicturesByGroupId(_currentQuestion.PictureGroupId));
        }

        private void AddAnswers()
        {
            IEnumerable<TestAnswer> answers =
                ReadDataHelper.GetAll<TestAnswer>().Where(i => i.QuestionId == _currentQuestion.Id);
            foreach (TestAnswer answer in answers.OrderBy(a => Guid.NewGuid()))
            {
                var answerOption = new TextBlock
                {
                    Text = answer.Text,
                    Margin = new Thickness(10, 30, 10, 0),
                    FontSize = 20,
                    TextWrapping = TextWrapping.Wrap,
                    Tag = answer.IsCorrect
                };

                answerOption.Tapped += Answer_Tapped;

                TestPanel.Children.Add(answerOption);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                object param = e.Parameter;
                _currentQuestionNumber = param == null ? 0 : Int32.Parse(param.ToString());
                _isLastQuestion = _currentQuestionNumber + 1 == NumberOfQuestions;

                if (_currentQuestionNumber == 0)
                {
                    TestQuestion.CurrentQuestions = TestQuestion.GetRandomQuestions(NumberOfQuestions);
                    TestQuestion.CorrectAnswersNumber = 0;
                }

                _currentQuestion = TestQuestion.CurrentQuestions[_currentQuestionNumber];

                AddProgressStatus();
                AddQuestion();
                AddImage();
                AddAnswers();

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
