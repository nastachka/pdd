using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.Calls;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PDD.Models;
using PDD.Utility;

namespace PDD.Views
{
    public sealed partial class ContactsPage
    {
        public ContactsPage()
        {
            InitializeComponent();
        }

        private static HyperlinkButton GetHyperlinkPropertyValue(string linkText)
        {
            return new HyperlinkButton
            {
                NavigateUri = new Uri(linkText),
                Content = linkText,
                Margin = new Thickness(10, 0, 0, 0),
                FontSize = 18,
                Width = Window.Current.Bounds.Width - 160 - 40,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Foreground = LayoutObjectFactory.GetThemeColor(),
                HorizontalContentAlignment = HorizontalAlignment.Left,
            };
        }

        private static TextBlock GetPropertyValue(string text)
        {
            return new TextBlock
            {
                Width = Window.Current.Bounds.Width - 160 - 40,
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 18,
                TextWrapping = TextWrapping.Wrap,
            };
        }

        private static StackPanel PrintContact(string labelText)
        {
            var block = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 10),
            };

            var label = new TextBlock
            {
                Width = 160,
                Text = labelText,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 5, 10, 5),
                FontSize = 18,
                TextWrapping = TextWrapping.Wrap,
                FontWeight = FontWeights.Bold
            };
            block.Children.Add(label);

            return block;
        }

        private static void MakeCall(object sender, RoutedEventArgs e)
        {
            object name = ((HyperlinkButton) sender).CommandParameter;
            string number = ((HyperlinkButton) sender).Tag.ToString();
            string displayName = name != null ? name.ToString() : "ГАИ";

            PhoneCallManager.ShowPhoneCallUI(number, displayName);
        }

        private static StackPanel GetPhonesPanel(IEnumerable<Phone> phones)
        {
            var phonesPanel = new StackPanel();

            foreach (Phone phone in phones)
            {
                TextBlock hyperlinkText = LayoutObjectFactory.CreateTextBlock(phone.FormattedPhone);
                hyperlinkText.FontSize = 18;
                hyperlinkText.TextWrapping = TextWrapping.Wrap;
                hyperlinkText.HorizontalAlignment = HorizontalAlignment.Left;
                hyperlinkText.VerticalAlignment = VerticalAlignment.Top;
                hyperlinkText.Margin = new Thickness(10, 0, 0, 0);
                hyperlinkText.Width = Window.Current.Bounds.Width - 160 - 40;

                var button = new HyperlinkButton
                {
                    Foreground = LayoutObjectFactory.GetThemeColor(),
                    Content = hyperlinkText,
                    Tag = phone.PhoneNumber,
                    CommandParameter = phone.DisplayName,
                };
                button.Click += MakeCall;

                phonesPanel.Children.Add(button);
            }

            return phonesPanel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                ObservableCollection<Phone> phones = ReadDataHelper.GetAll<Phone>();
                foreach (Contact contact in ReadDataHelper.GetAll<Contact>())
                {
                    var border = new Border
                    {
                        Background = LayoutObjectFactory.GetThemeColor(),
                        Margin = new Thickness(0, 20, 0, 10),
                    };
                    var hearder = new TextBlock
                    {
                        Text = contact.Title,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        VerticalAlignment = VerticalAlignment.Top,
                        Margin = new Thickness(10, 10, 10, 10),
                        FontSize = 18,
                        TextWrapping = TextWrapping.Wrap,
                    };
                    border.Child = hearder;

                    ContactsPanel.Children.Add(border);

                    if (contact.Address != null)
                    {
                        StackPanel block = PrintContact("Адрес:");
                        block.Children.Add(GetPropertyValue(contact.Address));
                        ContactsPanel.Children.Add(block);
                    }
                    int contactId = contact.Id;
                    List<Phone> contactPhones = phones.Where(i => i.ContactId == contactId && i.Type == 1).ToList();
                    List<Phone> contactPhonesSecretariat =
                        phones.Where(i => i.ContactId == contactId && i.Type == 2).ToList();
                    if (contactPhones != null && contactPhones.Count > 0)
                    {
                        StackPanel block = PrintContact("Дежурная часть:");
                        block.Children.Add(GetPhonesPanel(contactPhones));
                        ContactsPanel.Children.Add(block);
                    }

                    if (contactPhonesSecretariat != null && contactPhonesSecretariat.Count > 0)
                    {
                        StackPanel block = PrintContact("Секретариат:");
                        block.Children.Add(GetPhonesPanel(contactPhonesSecretariat));
                        ContactsPanel.Children.Add(block);
                    }
                    if (contact.Website != null)
                    {
                        StackPanel block = PrintContact("Сайт:");
                        block.Children.Add(GetHyperlinkPropertyValue(contact.Website));
                        ContactsPanel.Children.Add(block);
                    }
                }
                LayoutObjectFactory.AddBottomAppBar(this);
            }
            catch (Exception)
            {
                Frame.Navigate(typeof (ErrorPage));
            }
        }
    }
}
