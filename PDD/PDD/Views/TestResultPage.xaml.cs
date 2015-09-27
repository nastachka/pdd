using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class TestResultPage
    {
        public TestResultPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Title.Foreground = LayoutObjectFactory.GetThemeColor();

                object param = e.Parameter;

                if (param == null)
                {
                    Frame.Navigate(typeof (ErrorPage));
                }

                int numberOfQuestions = Int32.Parse(param.ToString());
                int numberOfCorrectAnswers = TestQuestion.CorrectAnswersNumber;

                string recomendation = numberOfCorrectAnswers >= numberOfQuestions - 1
                    ? "Отличный результат!"
                    : "Вам необходимо потренироваться еще.";
                var result = new TextBlock
                {
                    Text =
                        String.Format("Вы правильно ответили на {0} из {1} вопросов. {2}", numberOfCorrectAnswers,
                            numberOfQuestions, recomendation),
                    Margin = new Thickness(10, 10, 10, 20),
                    FontSize = 24,
                    TextWrapping = TextWrapping.Wrap
                };

                TestResultPanel.Children.Add(result);

                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception ex)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }

        private void StartTest(object sender, RoutedEventArgs e)
        {
            try
            {
                Frame.Navigate(typeof (TestsPage));
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
